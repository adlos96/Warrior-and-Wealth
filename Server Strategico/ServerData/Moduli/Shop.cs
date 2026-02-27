using Server_Strategico.Gioco;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Manager.QuestManager;

namespace Server_Strategico.ServerData.Moduli
{
    internal class Shop
    {
        public static void Shop_Call(Guid guid, Player player, string comando)
        {
            int diamanti_Viola = player.Diamanti_Viola;
            int diamanti_Blu = player.Diamanti_Blu;
            decimal Dollari_Virtuali = player.Dollari_Virtuali;
            bool payment_Status_Blockchain = false;

            int numero_Code_Base = Variabili_Server.numero_Code_Base;
            int numero_Code_Vip = Variabili_Server.numero_Code_Base_Vip;
            int coda_Costr_Player = player.Code_Costruzione;
            int coda_Reclut_Player = player.Code_Reclutamento;

            bool conferma_Transazione = false; //Impostare via funzioni

            switch (comando)
            {
                case "Vip_1":
                    if (diamanti_Viola < Variabili_Server.Shop.Vip_1.Costo)
                    {
                        Console.WriteLine($"[Shop] Diamanti Viola insufficienti per vip 24H... Richiesta annullata..");
                        Server.Server.Send(player.guid_Player, $"Log_Server|[Shop] Sono necessari [icon:diamanteViola][warning]{Variabili_Server.Shop.Vip_1.Costo} [viola]Diamanti Viola[/viola] per l'acquisto del VIP");
                        return;
                    }
                    if (player.Vip_Tempo + Variabili_Server.Shop.Vip_1.Reward > 2 * 24 * 60 * 60) //Max 2gg di accumolo
                    {
                        Console.WriteLine($"[Shop]Tempo VIP oltre il limite massimo di 2 giorni... Richiesta annullata..");
                        Server.Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Tempo [highlight]{player.Vip_Tempo}[/highlight] [highlight]VIP[/highlight] [error]oltre il limite massimo di [highlight]2 giorni[/highlight]... Richiesta annullata..");
                        return;
                    }

                    player.Diamanti_Viola -= (int)Variabili_Server.Shop.Vip_1.Costo;
                    OnEvent(player, QuestEventType.Risorse, "Diamanti Viola", (int)Variabili_Server.Shop.Vip_1.Costo);
                    player.Vip_Tempo += Variabili_Server.Shop.Vip_1.Reward;
                    //player.Vip_Tempo += 2 * 60; //Test
                    player.Vip = true;
                    Server.Server.Send(player.guid_Player, $"Log_Server|[Shop] Hai usato [warning][icon:diamanteViola]{Variabili_Server.Shop.Vip_1.Costo} [viola]Diamanti Viola[/viola] per l'acquisto del [highlight]VIP 24H[/highlight], Tempo disponibile: {player.FormatTime(player.Vip_Tempo)}");
                    player.SetupVillaggioGiocatore(player);
                    break;
                case "Vip_2":
                    if (player.Vip_Tempo + Variabili_Server.Shop.Vip_2.Reward > 2 * 24 * 60 * 60) //Max 2gg di accumolo
                    {
                        Server.Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Tempo [highlight]{player.Vip_Tempo}[/highlight] [highlight]VIP[/highlight] [error]oltre il limite massimo di [highlight]2 giorni[/highlight]... Richiesta annullata..");
                        return;
                    }

                    //payment_Status_Blockchain = BlockchainManager.Verifica_Pagamento_Vip(player, 2);
                    if (payment_Status_Blockchain == true)
                    {
                        player.Vip_Tempo += Variabili_Server.Shop.Vip_2.Reward;
                        player.Vip = true;
                    }
                    break;

                case "GamePass_Base":
                    if (player.GamePass_Base_Tempo + Variabili_Server.Shop.GamePass_Base.Reward > 6 * Variabili_Server.Shop.GamePass_Base.Reward) //Max 6 mesi di accumolo
                    {
                        Server.Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Tempo [highlight]{player.Vip_Tempo}[/highlight] [highlight]VIP[/highlight] [error]oltre il limite massimo di [highlight]6 mesi[/highlight]... Richiesta annullata..");
                        return;
                    }
                    Server.Server.Send(player.guid_Player, $"Log_Server|[Shop]Hai acquistato l'accesso al 'GamePass Base' per 30 giorni, fanne buon uso.");
                    player.GamePass_Base_Tempo += Variabili_Server.Shop.GamePass_Base.Reward;
                    player.GamePass_Base = true;
                    Descrizioni.DescUpdate(player);
                    player.SetupVillaggioGiocatore(player);
                    break;
                case "GamePass_Avanzato":
                    if (player.GamePass_Avanzato_Tempo + Variabili_Server.Shop.GamePass_Avanzato.Reward > 6 * Variabili_Server.Shop.GamePass_Avanzato.Reward) //Max 6 mesi di accumulo
                    {
                        Server.Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Tempo [highlight]{player.Vip_Tempo}[/highlight] [highlight]VIP[/highlight] [error]oltre il limite massimo di [highlight]6 mesi[/highlight]... Richiesta annullata..");
                        return;
                    }
                    Server.Server.Send(player.guid_Player, $"Log_Server|[Shop]Hai acquistato l'accesso al 'GamePass Base' per 30 giorni, fanne buon uso.");
                    player.GamePass_Avanzato_Tempo += Variabili_Server.Shop.GamePass_Avanzato.Reward;
                    player.GamePass_Avanzato = true;
                    Descrizioni.DescUpdate(player);
                    player.SetupVillaggioGiocatore(player);
                    break;

                case "Costruttori_24H":
                    if (diamanti_Blu < Variabili_Server.Shop.Costruttore_24h.Costo)
                    {
                        Server.Server.Send(player.guid_Player, $"Log_Server|[Shop] Sono necessari [icon:diamanteBlu][warning]{Variabili_Server.Shop.Costruttore_24h.Costo} [blu]Diamanti Blu[/blu] per l'acquisto di '[highlight]Costruttori 24H[/highlight]'... Richiesta annullata..");
                        return;
                    }
                    if ((coda_Costr_Player == numero_Code_Base && player.Vip == false) ||
                        (coda_Costr_Player == numero_Code_Base + numero_Code_Vip && player.Vip == true)) // 
                    {
                        if (player.Costruttori + Variabili_Server.Shop.Costruttore_24h.Reward > 3 * 24 * 60 * 60) //Max 3gg di accumulo
                        {
                            Server.Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Tempo [highlight]{player.Costruttori}[/highlight] [highlight]costruttori[/highlight] [error]oltre il limite massimo di [highlight]4 giorni[/highlight]... Richiesta annullata..");
                            return;
                        }

                        player.Diamanti_Blu -= (int)Variabili_Server.Shop.Costruttore_24h.Costo;
                        player.Costruttori += Variabili_Server.Shop.Costruttore_24h.Reward;
                        OnEvent(player, QuestEventType.Risorse, "Diamanti Blu", (int)Variabili_Server.Shop.Costruttore_24h.Costo);

                        if (player.Costruttori > 0)
                            Server.Server.Send(player.guid_Player, $"Log_Server|[Shop] Acquisto 'Costruttori 24H' completato [icon:diamanteBlu]{Variabili_Server.Shop.Costruttore_24h.Costo} [blu]Diamanti Blu[/blu] spesi " +
                                $"Tempo costruttori è stato esteso di: {player.FormatTime(Variabili_Server.Shop.Costruttore_24h.Reward)}");
                        else
                        {
                            if (player.Costruttori == 0 || player.Vip == false) player.Code_Costruzione += 1;
                                Server.Server.Send(player.guid_Player, $"Log_Server|[Shop] Acquisto 'Costruttori 24H' completato [icon:diamanteBlu][warning]{Variabili_Server.Shop.Vip_1.Costo} [blu]Diamanti Blu[/blu] spesi " +
                                    $"Coda costruzioni aumentato a: {player.Code_Costruzione} per: {player.FormatTime(player.Costruttori)}");
                        }
                    }
                    break;
                case "Costruttori_48H":
                    if (diamanti_Blu < Variabili_Server.Shop.Costruttore_48h.Costo)
                    {
                        Server.Server.Send(player.guid_Player, $"Log_Server|[Shop] Sono necessari [icon:diamanteBlu][warning]{Variabili_Server.Shop.Costruttore_48h.Costo} [blu]Diamanti Blu[/blu] per l'acquisto di '[highlight]Costruttori 24H[/highlight]'... Richiesta annullata..");
                        return;
                    }
                    if ((coda_Costr_Player == numero_Code_Base && player.Vip == false) ||
                        (coda_Costr_Player == numero_Code_Base + numero_Code_Vip && player.Vip == true)) // 
                    {
                        if (player.Costruttori + Variabili_Server.Shop.Costruttore_48h.Reward > 3 * 24 * 60 * 60) //Max 3gg di accumulo
                        {
                            Server.Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Tempo [highlight]{player.Costruttori}[/highlight] [highlight]costruttori[/highlight] [error]oltre il limite massimo di [highlight]4 giorni[/highlight]... Richiesta");
                            return;
                        }

                        player.Diamanti_Blu -= (int)Variabili_Server.Shop.Costruttore_48h.Costo;
                        player.Costruttori += Variabili_Server.Shop.Costruttore_48h.Reward;
                        OnEvent(player, QuestEventType.Risorse, "Diamanti Blu", (int)Variabili_Server.Shop.Costruttore_48h.Costo);
                        if (player.Costruttori > 0)
                            Server.Server.Send(player.guid_Player, $"Log_Server|[Shop]Acquisto '[highlight]Costruttori 48H[/highlight]' completato, [icon:diamanteBlu][warning]{Variabili_Server.Shop.Costruttore_48h.Costo} [blu]Diamanti Blu[/blu] spesi, " +
                                $"Tempo costruttori è stato esteso di: {player.FormatTime(Variabili_Server.Shop.Costruttore_48h.Reward)}");
                        else
                        {
                            if (player.Costruttori == 0 || player.Vip == false) player.Code_Costruzione += 1;
                                Server.Server.Send(player.guid_Player, $"Log_Server|[Shop]Acquisto '[highlight]Costruttori 48H[/highlight]' completato, [icon:diamanteBlu][warning]{Variabili_Server.Shop.Costruttore_48h.Costo} [blu]Diamanti Blu[/blu] spesi, " +
                                    $"Coda costruzioni aumentato a: {player.Code_Costruzione} per: {player.FormatTime(player.Costruttori)}");
                        }
                    }
                    break;
                case "Reclutatori_24H":
                    if (diamanti_Blu < Variabili_Server.Shop.Reclutatore_24h.Costo)
                    {
                        Server.Server.Send(player.guid_Player, $"Log_Server|[Shop] Sono necessari [icon:diamanteBlu][warning]{Variabili_Server.Shop.Reclutatore_24h.Costo} [blu]Diamanti Blu[/blu] per 'Reclutatori 24H'... Richiesta annullata..");
                        return;
                    }
                    if ((coda_Reclut_Player == numero_Code_Base && player.Vip == false) ||
                        (coda_Reclut_Player == numero_Code_Base + numero_Code_Vip && player.Vip == true)) // 
                    {
                        if (player.Reclutatori + Variabili_Server.Shop.Reclutatore_24h.Reward > 2 * 24 * 60 * 60) //Max 2gg di accumolo
                        {
                            Server.Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Tempo [highlight]{player.Reclutatori}[/highlight] [highlight]reclutatori[/highlight] [error]oltre il limite massimo di [highlight]4 giorni[/highlight]... Richiesta");
                            return;
                        }

                        player.Diamanti_Blu -= (int)Variabili_Server.Shop.Reclutatore_24h.Costo;
                        player.Reclutatori += Variabili_Server.Shop.Reclutatore_24h.Reward;
                        OnEvent(player, QuestEventType.Risorse, "Diamanti Blu", (int)Variabili_Server.Shop.Reclutatore_24h.Costo);

                        if (player.Reclutatori > 0)
                            Server.Server.Send(player.guid_Player, $"Log_Server|[Shop] Acquisto '[highlight]Reclutatori 24H[/highlight]' completato, [icon:diamanteBlu][warning]{Variabili_Server.Shop.Reclutatore_24h.Costo} [blu]Diamanti Blu[/blu] spesi " +
                                $"Tempo reclutatori è stato esteso di: {player.FormatTime(Variabili_Server.Shop.Reclutatore_24h.Reward)}");
                        else
                        {
                            if (player.Reclutatori == 0 || player.GamePass_Base == false) player.Code_Reclutamento += 1;
                                Server.Server.Send(player.guid_Player, $"Log_Server|[Shop] Acquisto '[highlight]Reclutatori 24H[/highlight]' completato, [icon:diamanteBlu][warning]{Variabili_Server.Shop.Reclutatore_24h.Costo} [blu]Diamanti Blu[/blu] spesi " +
                                    $"Coda reclutatori aumentato a: {player.Code_Reclutamento} per: {player.FormatTime(player.Reclutatori)}");
                        }
                    }
                    break;
                case "Reclutatori_48H":
                    if (diamanti_Blu < Variabili_Server.Shop.Reclutatore_48h.Costo)
                    {
                        Server.Server.Send(player.guid_Player, $"Log_Server|[Shop] Sono necessari [icon:diamanteBlu][warning]{Variabili_Server.Shop.Reclutatore_48h.Costo} [blu]Diamanti Blu[/blu] per 'Reclutatori 48H'... Richiesta annullata..");
                        return;
                    }
                    if ((coda_Reclut_Player == numero_Code_Base && player.Vip == false) ||
                        (coda_Reclut_Player == numero_Code_Base + numero_Code_Vip && player.Vip == true)) // 
                    {
                        if (player.Reclutatori + Variabili_Server.Shop.Reclutatore_48h.Reward > 2 * 24 * 60 * 60)
                        {
                            Server.Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Tempo [highlight]{player.Reclutatori}[/highlight] [highlight]reclutatori[/highlight] [error]oltre il limite massimo di [highlight]4 giorni[/highlight]... Richiesta");
                            return;
                        }

                        player.Diamanti_Blu -= (int)Variabili_Server.Shop.Reclutatore_48h.Costo;
                        player.Reclutatori += Variabili_Server.Shop.Reclutatore_48h.Reward;
                        OnEvent(player, QuestEventType.Risorse, "Diamanti Blu", (int)Variabili_Server.Shop.Reclutatore_48h.Costo);

                        if (player.Reclutatori > 0)
                            Server.Server.Send(player.guid_Player, $"Log_Server|[Shop] Acquisto '[highlight]Reclutatori 48H[/highlight]' completato, [icon:diamanteBlu][warning]{Variabili_Server.Shop.Reclutatore_48h.Costo} [blu]Diamanti Blu[/blu] spesi " +
                                $"Tempo reclutatori aumentato a: {player.FormatTime(player.Reclutatori)}");
                        else
                        {
                            if (player.Reclutatori == 0 || player.GamePass_Base == false) player.Code_Reclutamento += 1;
                            Server.Server.Send(player.guid_Player, $"Log_Server|[Shop] Acquisto '[highlight]Reclutatori 48H[/highlight]' completato, [icon:diamanteBlu][warning]{Variabili_Server.Shop.Reclutatore_48h.Costo} [blu]Diamanti Blu[/blu] spesi " +
                                $"Coda reclutatori aumentato a: [highlight]{player.Code_Reclutamento}[/highlight] per: [highlight]{player.FormatTime(player.Reclutatori)}[/highlight]");
                        }
                    }
                    break;

                case "Scudo_Pace_8H":
                    if (diamanti_Blu >= Variabili_Server.Shop.Scudo_Pace_8h.Costo)
                    {
                        if (player.ScudoDellaPace + Variabili_Server.Shop.Scudo_Pace_8h.Reward > 7 * 24 * 60 * 60) //Max 5gg di accumolo
                        {
                            Server.Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Lo scudo della pace non può superare i: [warning]7[/warning][error] giorni. Il tuo scudo sarà attivo per: [highlight]{player.FormatTime(player.ScudoDellaPace)}[/highlight][error]. Richiesta annullata...");
                            return;
                        }

                        player.Diamanti_Blu -= (int)Variabili_Server.Shop.Scudo_Pace_8h.Costo;
                        OnEvent(player, QuestEventType.Risorse, "Diamanti Blu", (int)Variabili_Server.Shop.Scudo_Pace_8h.Costo);
                        player.ScudoDellaPace += Variabili_Server.Shop.Scudo_Pace_8h.Reward; //Riduzione ogni tick (1000 = 1s) --> potrebbe essere simile anche per gli altri due timer (costr. addestr.)
                        Server.Server.Send(player.guid_Player, $"Log_Server|[Shop] Scudo della pace attivo, utilizzati [icon:diamanteBlu][warning]{Variabili_Server.Shop.Scudo_Pace_8h.Costo} [blu]Diamanti Blu[/blu], per estendere lo scudo di: {player.FormatTime(Variabili_Server.Shop.Scudo_Pace_8h.Reward)}. Scudo disponibile per: {player.FormatTime(player.ScudoDellaPace)}");
                    }
                    break;
                case "Scudo_Pace_24H":
                    if (diamanti_Blu >= Variabili_Server.Shop.Scudo_Pace_24h.Costo)
                    {
                        if (player.ScudoDellaPace + Variabili_Server.Shop.Scudo_Pace_24h.Reward > 7 * 24 * 60 * 60) //Max 5gg di accumolo
                        {
                            Server.Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Lo scudo della pace non può superare i: [warning]7[/warning][error] giorni. Il tuo scudo sarà attivo per: [highlight]{player.FormatTime(player.ScudoDellaPace)}[/highlight][error]. Richiesta annullata...");
                            return;
                        }

                        player.Diamanti_Blu -= (int)Variabili_Server.Shop.Scudo_Pace_24h.Costo;
                        OnEvent(player, QuestEventType.Risorse, "Diamanti Blu", (int)Variabili_Server.Shop.Scudo_Pace_24h.Costo);
                        player.ScudoDellaPace += Variabili_Server.Shop.Scudo_Pace_24h.Reward;
                        Server.Server.Send(player.guid_Player, $"Log_Server|[Shop] Scudo della pace attivo, utilizzati [icon:diamanteBlu][warning]{Variabili_Server.Shop.Scudo_Pace_24h.Costo} [blu]Diamanti Blu[/blu], per estendere lo scudo di: {player.FormatTime(Variabili_Server.Shop.Scudo_Pace_24h.Reward)}. Scudo disponibile per: [{player.FormatTime(player.ScudoDellaPace)}]");
                    }
                    break;
                case "Scudo_Pace_72H":
                    if (diamanti_Blu >= Variabili_Server.Shop.Scudo_Pace_72h.Costo)
                    {
                        if (player.ScudoDellaPace + Variabili_Server.Shop.Scudo_Pace_72h.Reward > 7 * 24 * 60 * 60) //Max 5gg di accumolo
                        {
                            Server.Server.Send(player.guid_Player, $"Log_Server|[Shop][error]Lo scudo della pace non può superare i: [warning]7[/warning][error] giorni. Il tuo scudo sarà attivo per: [highlight]{player.FormatTime(player.ScudoDellaPace)}[/highlight][error]. Richiesta annullata...");
                            return;
                        }

                        player.Diamanti_Blu -= (int)Variabili_Server.Shop.Scudo_Pace_72h.Costo;
                        OnEvent(player, QuestEventType.Risorse, "Diamanti Blu", (int)Variabili_Server.Shop.Scudo_Pace_72h.Costo);
                        player.ScudoDellaPace += Variabili_Server.Shop.Scudo_Pace_72h.Reward;
                        Server.Server.Send(player.guid_Player, $"Log_Server|[Shop] Scudo della pace attivo, utilizzati [icon:diamanteBlu][warning]{Variabili_Server.Shop.Scudo_Pace_72h.Costo} [blu]Diamanti Blu[/blu], per estendere lo scudo di: {player.FormatTime(Variabili_Server.Shop.Scudo_Pace_72h.Reward)}. Scudo disponibile per: {player.FormatTime(player.ScudoDellaPace)}");
                    }
                    break;
                case "Pacchetto_1":

                    // Procedere alla richiesta transazione USDT da parte dell'utente

                    //Confermare l'acquisto
                    //Accreditare i diamanti
                    if (conferma_Transazione == true)
                        player.Diamanti_Viola += Variabili_Server.Shop.Pacchetto_Diamanti_1.Reward;

                    break;
            }
        }
    }
}
