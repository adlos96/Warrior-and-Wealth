package com.adlos.warriorandwealth

import android.net.Uri
import android.os.Build
import android.os.Bundle
import android.webkit.*
import androidx.activity.OnBackPressedCallback
import androidx.appcompat.app.AppCompatActivity

class MainActivity : AppCompatActivity() {

    private lateinit var webView: WebView
    private lateinit var billingManager: BillingManager
    private val trustedDomain = "warriorsandwealth.com"
    private val startUrl = "https://warriorsandwealth.com:8448/"

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        webView = findViewById(R.id.webView)

        // ----- Billing -------------------------------------------------
        billingManager = BillingManager(this) { productId, purchaseToken ->
            val js = "javascript:verifyPurchaseWithServer('$productId', '$purchaseToken')"
            webView.evaluateJavascript(js, null)
        }

        // ----- WebView setup -----------------------------------------
        configureWebView()
        webView.loadUrl(startUrl)

        // ----- Back handling ------------------------------------------
        onBackPressedDispatcher.addCallback(this,
            object : OnBackPressedCallback(true) {
                override fun handleOnBackPressed() {
                    if (webView.canGoBack()) webView.goBack() else finish()
                }
            })
    }

    override fun onPause() {
        super.onPause()
        webView.onPause()
    }

    override fun onResume() {
        super.onResume()
        webView.onResume()
    }

    override fun onDestroy() {
        billingManager.endConnection()
        webView.destroy()
        super.onDestroy()
    }

    private fun configureWebView() {
        webView.webViewClient = object : WebViewClient() {
            override fun shouldOverrideUrlLoading(view: WebView, request: WebResourceRequest): Boolean {
                if (request.isForMainFrame) {
                    val host = request.url.host ?: ""
                    return !host.endsWith(trustedDomain, ignoreCase = true)
                }
                return false
            }

            override fun onPageFinished(view: WebView, url: String) {
                super.onPageFinished(view, url)
                if (!url.contains(trustedDomain)) view.loadUrl("about:blank")
            }

            @Deprecated("Deprecated in newer APIs, kept for compatibility")
            override fun onReceivedError(
                view: WebView,
                errorCode: Int,
                description: String?,
                failingUrl: String?
            ) {
                if (view.url?.let { Uri.parse(it).host?.endsWith(trustedDomain, true) } == true) {
                view.loadUrl("file:///android_asset/error.html")
                }
            }
        }

        webView.webChromeClient = WebChromeClient()

        webView.settings.apply {
            javaScriptEnabled = true
            domStorageEnabled = true
            cacheMode = WebSettings.LOAD_NO_CACHE
            mixedContentMode = WebSettings.MIXED_CONTENT_NEVER_ALLOW
            mediaPlaybackRequiresUserGesture = true
            safeBrowsingEnabled = true

            setAllowFileAccess(false)
            setAllowContentAccess(false)
        }
        webView.addJavascriptInterface(WebAppInterface(), "AndroidBridge")
    }

    // JavaScript → Kotlin bridge
    inner class WebAppInterface {
        /**
        * Called from JavaScript.
        * @param productId The ID of the product.
        * @param type Use "inapp" for one-time purchases and "subs" for   subscriptions.
        */
        @JavascriptInterface
        fun purchaseProduct(productId: String, type: String) {
            if (!productId.matches(Regex("^[a-zA-Z0-9_\\-]+$"))) return

            val host = Uri.parse(webView.url).host ?: ""
            if (host.endsWith(trustedDomain, ignoreCase = true)) {
                runOnUiThread {
                // Pass the type to the billing manager
                    billingManager.launchPurchaseFlow(productId, type)
                }
            }
        }
    }
}
