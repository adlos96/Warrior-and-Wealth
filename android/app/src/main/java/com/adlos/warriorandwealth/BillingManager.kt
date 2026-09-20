package com.adlos.warriorandwealth

import android.app.Activity
import android.util.Log
import com.android.billingclient.api.*

/**
 * Simple wrapper around Google Play Billing.
 *
 * @param activity          The Activity that will host the billing UI.
 * @param onPurchaseSuccess Called when a purchase is successfully acknowledged.
 *                          Parameters are (productId, purchaseToken).
 */
class BillingManager(
    private val activity: Activity,
    private val onPurchaseSuccess: (productId: String, purchaseToken: String) -> Unit
) : PurchasesUpdatedListener {

    // BillingClient – built once and reused
    private val billingClient: BillingClient = BillingClient.newBuilder(activity)
        .setListener(this)
        .enablePendingPurchases(
            // Enables one‑time in‑app products; keep this even if you later add subscriptions
            PendingPurchasesParams.newBuilder()
                .enableOneTimeProducts()
                .build()
        )
        .build()

    init {
        startConnection()
    }

    // Connection handling (re‑connect automatically if service is lost)
    private fun startConnection() {
        billingClient.startConnection(object : BillingClientStateListener {
            override fun onBillingSetupFinished(billingResult: BillingResult) {
                if (billingResult.responseCode == BillingClient.BillingResponseCode.OK) {
                    Log.d("Billing", "✅ BillingClient ready")
                } else {
                    Log.e(
                        "Billing",
                        "❌ Billing setup failed: ${billingResult.responseCode} – ${billingResult.debugMessage}"
                    )
                }
            }

            override fun onBillingServiceDisconnected() {
                Log.w("Billing", "🔌 Service disconnected – retrying")
                startConnection()
            }
        })
    }

    // Public API – launch the Google‑Play purchase flow for a given productId
    fun launchPurchaseFlow(productId: String, type: String) {
        val productType = if (type.lowercase() == "subs") {
            BillingClient.ProductType.SUBS
        } else {
            BillingClient.ProductType.INAPP
        }

        val product = QueryProductDetailsParams.Product.newBuilder()
            .setProductId(productId)
            .setProductType(productType)
            .build()

        val queryParams = QueryProductDetailsParams.newBuilder()
            .setProductList(listOf(product))
            .build()

        billingClient.queryProductDetailsAsync(queryParams) { billingResult, result ->
            val productDetailsList = result.productDetailsList

            if (billingResult.responseCode != BillingClient.BillingResponseCode.OK || productDetailsList.isNullOrEmpty()) {
                Log.e("Billing", "❌ Error fetching product details: ${billingResult.debugMessage}")
                return@queryProductDetailsAsync
            }

            val productDetails = productDetailsList[0]

            val productDetailsParams = BillingFlowParams.ProductDetailsParams.newBuilder()
                .setProductDetails(productDetails)
                .apply {
                    if (productType == BillingClient.ProductType.SUBS) {
                        val offerToken = productDetails.subscriptionOfferDetails?.firstOrNull()?.offerToken
                        if (offerToken != null) {
                            setOfferToken(offerToken)
                        } else {
                            Log.e("Billing", "❌ No offer token found for subscription $productId")
                            return@apply
                        }
                    }
                }
                .build()
            val flowParams = BillingFlowParams.newBuilder()
                .setProductDetailsParamsList(listOf(productDetailsParams))
                .build()

            activity.runOnUiThread {
                val launchResult = billingClient.launchBillingFlow(activity, flowParams)
                Log.d("Billing", "🛒 launchBillingFlow result: ${launchResult.responseCode}")
            }
        }
    }

    // PurchasesUpdatedListener – receives the result of the UI flow
    override fun onPurchasesUpdated(billingResult: BillingResult, purchases: MutableList<Purchase>?) {
        if (billingResult.responseCode == BillingClient.BillingResponseCode.OK && !purchases.isNullOrEmpty()) {
            purchases.forEach { handlePurchase(it) }
        } else {
            Log.w(
                "Billing",
                "Purchase not completed: ${billingResult.responseCode} – ${billingResult.debugMessage}"
            )
        }
    }

    // Acknowledge the purchase (required for non‑consumables/in‑apps) and forward success
    private fun handlePurchase(purchase: Purchase) {
        if (purchase.purchaseState != Purchase.PurchaseState.PURCHASED) {
            Log.w("Billing", "Purchase not in PURCHASED state; ignoring")
            return
        }

        if (!purchase.isAcknowledged) {
            val ackParams = AcknowledgePurchaseParams.newBuilder()
                .setPurchaseToken(purchase.purchaseToken)
                .build()

            billingClient.acknowledgePurchase(ackParams) { ackResult ->
                if (ackResult.responseCode == BillingClient.BillingResponseCode.OK) {
                    onPurchaseSuccess(purchase.products.firstOrNull() ?: "unknown", purchase.purchaseToken)
                } else {
                    Log.e(
                        "Billing",
                        "❌ Acknowledge failed: ${ackResult.debugMessage}"
                    )
                }
            }
        } else {
            // Already acknowledged – just forward the success
            onPurchaseSuccess(purchase.products.firstOrNull() ?: "unknown", purchase.purchaseToken)
        }
    }

    // Optional helper – can be called from Activity.onDestroy()
    fun endConnection() {
        if (billingClient.isReady) {
            billingClient.endConnection()
        }
    }
}
