using Server_Strategico.Gioco;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Server.Server;

namespace Server_Strategico.ServerData.Moduli
{
    internal class Descrizioni
    {
        public static async void DescUpdate(Player player)
        {
            var L = LocalizationManager.Get(player); // unica riga aggiunta

            double tempoBase = 0, tempoCalcolato = 0;
            tempoCalcolato = Strutture.Edifici.Fattoria.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.Fattoria.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.Fattoria.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Fattoria|[black]" +
                $"{L.Desc_Fattoria()}\n\n" +
                $"{L.Label_CostoCostruzione()}:\n" +
                $"{L.Label_Cibo()}: [icon:cibo]{Strutture.Edifici.Fattoria.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Strutture.Edifici.Fattoria.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Edifici.Fattoria.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Edifici.Fattoria.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Strutture.Edifici.Fattoria.Oro.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Strutture.Edifici.Fattoria.Popolazione.ToString("#,0")}\n" +
                $"{L.Label_Costruzione()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"{L.Label_Produzione()}: [icon:cibo]{(Strutture.Edifici.Fattoria.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Cibo * (1 + player.Bonus_Produzione_Risorse)).ToString("0.00")} s\n" +
                $"{L.Label_Limite_Magazzino()}: [icon:cibo][ferroScuro]{Strutture.Edifici.Fattoria.Limite.ToString()}");

            tempoCalcolato = Strutture.Edifici.Segheria.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.Segheria.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.Segheria.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Segheria|[black]" +
               $"{L.Desc_Segheria()}\n\n" +
               $"{L.Label_CostoCostruzione()}\n" +
               $"{L.Label_Cibo()} [icon:cibo]{Strutture.Edifici.Segheria.Cibo.ToString("#,0")}\n" +
               $"{L.Label_Legno()}: [icon:legno]{Strutture.Edifici.Segheria.Legno.ToString("#,0")}\n" +
               $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Edifici.Segheria.Pietra.ToString("#,0")}\n" +
               $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Edifici.Segheria.Ferro.ToString("#,0")}\n" +
               $"{L.Label_Oro()}: [icon:oro]{Strutture.Edifici.Segheria.Oro.ToString("#,0")}\n" +
               $"{L.Label_Popolazione()}: [icon:popolazione]{Strutture.Edifici.Segheria.Popolazione.ToString("#,0")}\n" +
               $"{L.Label_Costruzione()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
               $"{L.Label_Produzione()}: [icon:legno]{(Strutture.Edifici.Segheria.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Legno * (1 + player.Bonus_Produzione_Risorse)).ToString("0.00")} s\n" +
               $"{L.Label_Limite_Magazzino()}: [icon:legno][ferroScuro]{Strutture.Edifici.Segheria.Limite.ToString()}\n");

            tempoCalcolato = Strutture.Edifici.CavaPietra.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.CavaPietra.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.CavaPietra.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Cava di Pietra|[black]" +
                $"{L.Desc_CavaPietra()}\n\n" +
                $"{L.Label_CostoCostruzione()}\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Strutture.Edifici.CavaPietra.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Strutture.Edifici.CavaPietra.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Edifici.CavaPietra.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Edifici.CavaPietra.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Strutture.Edifici.CavaPietra.Oro.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Strutture.Edifici.CavaPietra.Popolazione.ToString("#,0")}\n" +
                $"{L.Label_Costruzione()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"{L.Label_Produzione()}: [icon:pietra]{(Strutture.Edifici.CavaPietra.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Pietra * (1 + player.Bonus_Produzione_Risorse)).ToString("0.00")} s\n" +
                $"{L.Label_Limite_Magazzino()}: [icon:pietra][ferroScuro]{Strutture.Edifici.CavaPietra.Limite.ToString()}\n");

            tempoCalcolato = Strutture.Edifici.MinieraFerro.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.MinieraFerro.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.MinieraFerro.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Miniera di Ferro|[black]" +
                $"{L.Desc_MinieraFerro()}\n\n" +
                $"{L.Label_CostoCostruzione()}\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Strutture.Edifici.MinieraFerro.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Strutture.Edifici.MinieraFerro.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Edifici.MinieraFerro.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Edifici.MinieraFerro.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Strutture.Edifici.MinieraFerro.Oro.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Strutture.Edifici.MinieraFerro.Popolazione.ToString("#,0")}\n" +
                $"{L.Label_Costruzione()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"{L.Label_Produzione()}: [icon:ferro]{(Strutture.Edifici.MinieraFerro.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Ferro * (1 + player.Bonus_Produzione_Risorse)).ToString("0.00")} s\n" +
                $"{L.Label_Limite_Magazzino()}: [icon:ferro][ferroScuro]{Strutture.Edifici.MinieraFerro.Limite.ToString()}\n");

            tempoCalcolato = Strutture.Edifici.MinieraOro.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.MinieraOro.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.MinieraOro.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Miniera d'Oro|[black]" +
                $"{L.Desc_MinieraOro()}\n\n" +
                $"{L.Label_CostoCostruzione()}\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Strutture.Edifici.MinieraOro.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Strutture.Edifici.MinieraOro.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Edifici.MinieraOro.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Edifici.MinieraOro.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Strutture.Edifici.MinieraOro.Oro.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Strutture.Edifici.MinieraOro.Popolazione.ToString("#,0")}\n" +
                $"{L.Label_Costruzione()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"{L.Label_Produzione()}: [icon:oro]{(Strutture.Edifici.MinieraOro.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Oro * (1 + player.Bonus_Produzione_Risorse)).ToString("0.00")} s\n" +
                $"{L.Label_Limite_Magazzino()}: [icon:oro][ferroScuro]{Strutture.Edifici.MinieraOro.Limite.ToString()}\n");

            tempoCalcolato = Strutture.Edifici.Case.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.Case.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.Case.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Case|[black]" +
                $"{L.Desc_Case()}\n\n" +
                $"{L.Label_CostoCostruzione()}\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Strutture.Edifici.Case.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Strutture.Edifici.Case.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Edifici.Case.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Edifici.Case.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Strutture.Edifici.Case.Oro.ToString("#,0")}\n" +
                $"{L.Label_Costruzione()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"{L.Label_Produzione()}: [icon:popolazione]{(Strutture.Edifici.Case.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Popolazione).ToString("0.0000")} s\n" +
                $"Limite abitanti: [icon:popolazione][ferroScuro]{Strutture.Edifici.Case.Limite.ToString()}");

            tempoCalcolato = Strutture.Edifici.ProduzioneSpade.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.ProduzioneSpade.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.ProduzioneSpade.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Produzione Spade|[black]" +
                $"{L.Desc_ProduzioneSpade()}\n\n" +
                $"{L.Label_CostoCostruzione()}\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Strutture.Edifici.ProduzioneSpade.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Strutture.Edifici.ProduzioneSpade.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Edifici.ProduzioneSpade.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Edifici.ProduzioneSpade.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Strutture.Edifici.ProduzioneSpade.Oro.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Strutture.Edifici.ProduzioneSpade.Popolazione.ToString("#,0")}\n" +
                $"{L.Label_Costruzione()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"{L.Label_Produzione()}: [icon:spade]{(Strutture.Edifici.ProduzioneSpade.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Spade).ToString("0.00")} s\n" +
                $"Limite spade [icon:spade][ferroScuro]{Strutture.Edifici.ProduzioneSpade.Limite.ToString()}[black]\n" +
                $"{L.Label_Mantenimento_Legno()}: [icon:legno][rosso]-{Strutture.Edifici.ProduzioneSpade.Consumo_Legno}[black] s\n" +
                $"{L.Label_Mantenimento_Ferro()}: [icon:ferro][rosso]-{Strutture.Edifici.ProduzioneSpade.Consumo_Ferro}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Strutture.Edifici.ProduzioneSpade.Consumo_Oro}[black] s\n");

            tempoCalcolato = Strutture.Edifici.ProduzioneLance.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.ProduzioneLance.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.ProduzioneLance.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Produzione Lance|[black]" +
                $"{L.Label_Lancie()}\n\n" +
                $"{L.Label_CostoCostruzione()}\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Strutture.Edifici.ProduzioneLance.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Strutture.Edifici.ProduzioneLance.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Edifici.ProduzioneLance.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Edifici.ProduzioneLance.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Strutture.Edifici.ProduzioneLance.Oro.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Strutture.Edifici.ProduzioneLance.Popolazione.ToString("#,0")}\n" +
                $"{L.Label_Costruzione()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"{L.Label_Produzione()}: [icon:lance]{(Strutture.Edifici.ProduzioneLance.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Lance).ToString("0.00")} s\n" +
                $"Limite lancie [icon:lance][ferroScuro]{Strutture.Edifici.ProduzioneLance.Limite.ToString()}[black]\n" +
                $"{L.Label_Mantenimento_Legno()}: [icon:legno][rosso]-{Strutture.Edifici.ProduzioneLance.Consumo_Legno}[black] s\n" +
                $"{L.Label_Mantenimento_Ferro()}: [icon:ferro][rosso]-{Strutture.Edifici.ProduzioneLance.Consumo_Ferro}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Strutture.Edifici.ProduzioneLance.Consumo_Oro}[black] s\n");

            tempoCalcolato = Strutture.Edifici.ProduzioneArchi.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.ProduzioneArchi.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.ProduzioneArchi.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Produzione Archi|[black]" +
                $"{L.Desc_ProduzioneArchi()}\n\n" +
                $"{L.Label_CostoCostruzione()}\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Strutture.Edifici.ProduzioneArchi.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Strutture.Edifici.ProduzioneArchi.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Edifici.ProduzioneArchi.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Edifici.ProduzioneArchi.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Strutture.Edifici.ProduzioneArchi.Oro.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:archi]{Strutture.Edifici.ProduzioneArchi.Popolazione.ToString("#,0")}\n" +
                $"{L.Label_Costruzione()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"{L.Label_Produzione()}: [icon:archi]{(Strutture.Edifici.ProduzioneArchi.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Archi).ToString("0.00")} s\n" +
                $"Limite archi [icon:archi][ferroScuro]{Strutture.Edifici.ProduzioneArchi.Limite.ToString()}[black]\n" +
                $"{L.Label_Mantenimento_Legno()}: [icon:legno][rosso]-{Strutture.Edifici.ProduzioneArchi.Consumo_Legno}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Strutture.Edifici.ProduzioneArchi.Consumo_Oro}[black] s\n");

            tempoCalcolato = Strutture.Edifici.ProduzioneScudi.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.ProduzioneScudi.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.ProduzioneScudi.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Produzione Scudi|[black]" +
                $"{L.Desc_ProduzioneScudi()}\n\n" +
                $"{L.Label_CostoCostruzione()}\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Strutture.Edifici.ProduzioneScudi.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Strutture.Edifici.ProduzioneScudi.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Edifici.ProduzioneScudi.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Edifici.ProduzioneScudi.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Strutture.Edifici.ProduzioneScudi.Oro.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Strutture.Edifici.ProduzioneScudi.Popolazione.ToString("#,0")}\n" +
                $"{L.Label_Costruzione()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"{L.Label_Produzione()}: [icon:scudi]{(Strutture.Edifici.ProduzioneScudi.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Scudi).ToString("0.00")} s\n" +
                $"Limite scudi [icon:scudi][ferroScuro]{Strutture.Edifici.ProduzioneScudi.Limite.ToString()}[black]\n" +
                $"{L.Label_Mantenimento_Legno()}: [icon:legno][rosso]-{Strutture.Edifici.ProduzioneScudi.Consumo_Legno}[black] s\n" +
                $"{L.Label_Mantenimento_Ferro()}: [icon:ferro][rosso]-{Strutture.Edifici.ProduzioneScudi.Consumo_Ferro}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Strutture.Edifici.ProduzioneScudi.Consumo_Oro}[black] s\n");

            tempoCalcolato = Strutture.Edifici.ProduzioneArmature.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.ProduzioneArmature.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.ProduzioneArmature.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Produzione Armature|[black]" +
                $"{L.Desc_ProduzioneArmature()}\n\n" +
                $"{L.Label_CostoCostruzione()}\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Strutture.Edifici.ProduzioneArmature.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Strutture.Edifici.ProduzioneArmature.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Edifici.ProduzioneArmature.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Edifici.ProduzioneArmature.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Strutture.Edifici.ProduzioneArmature.Oro.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Strutture.Edifici.ProduzioneArmature.Popolazione.ToString("#,0")}\n" +
                $"{L.Label_Costruzione()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"{L.Label_Produzione()}: [icon:armature]{(Strutture.Edifici.ProduzioneArmature.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Armature).ToString("0.00")} s\n" +
                $"Limite {L.Label_Armature()}: [icon:armature][ferroScuro]{Strutture.Edifici.ProduzioneArmature.Limite.ToString()}[black]\n" +
                $"{L.Label_Mantenimento_Ferro()}: [icon:ferro][rosso]-{Strutture.Edifici.ProduzioneArmature.Consumo_Ferro}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Strutture.Edifici.ProduzioneArmature.Consumo_Oro}[black] s");

            tempoCalcolato = Strutture.Edifici.ProduzioneFrecce.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.ProduzioneFrecce.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.ProduzioneFrecce.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Produzione Frecce|[black]" +
                $"{L.Desc_ProduzioneFrecce()}\n\n" +
                $"{L.Label_CostoCostruzione()}\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Strutture.Edifici.ProduzioneFrecce.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Strutture.Edifici.ProduzioneFrecce.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Edifici.ProduzioneFrecce.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Edifici.ProduzioneFrecce.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Strutture.Edifici.ProduzioneFrecce.Oro.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Strutture.Edifici.ProduzioneFrecce.Popolazione.ToString("#,0")}\n" +
                $"{L.Label_Costruzione()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"{L.Label_Produzione()}: [icon:frecce]{(Strutture.Edifici.ProduzioneFrecce.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Oro).ToString("0.00")} s\n" +
                $"Limite frecce [icon:frecce][ferroScuro]{Strutture.Edifici.ProduzioneFrecce.Limite.ToString()}[black]\n" +
                $"{L.Label_Mantenimento_Legno()}: [icon:legno][rosso]-{Strutture.Edifici.ProduzioneFrecce.Consumo_Legno.ToString()}[black] s\n" +
                $"{L.Label_Mantenimento_Pietra()}: [icon:pietra][rosso]-{Strutture.Edifici.ProduzioneFrecce.Consumo_Pietra.ToString()}[black] s\n" +
                $"{L.Label_Mantenimento_Ferro()}: [icon:ferro][rosso]-{Strutture.Edifici.ProduzioneFrecce.Consumo_Ferro.ToString()}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Strutture.Edifici.ProduzioneFrecce.Consumo_Ferro.ToString()}[black] s\n");

            //Caserme
            tempoCalcolato = Strutture.Edifici.CasermaGuerrieri.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.CasermaGuerrieri.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.CasermaGuerrieri.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Caserma Guerrieri|[black]" +
                 $"{L.Desc_CasermaGuerrieri()}\n\n" +
                 $"{L.Label_CostoCostruzione()}\n" +
                 $"{L.Label_Cibo()} [icon:cibo]{Strutture.Edifici.CasermaGuerrieri.Cibo.ToString("#,0")}\n" +
                 $"{L.Label_Legno()}: [icon:legno]{Strutture.Edifici.CasermaGuerrieri.Legno.ToString("#,0")}\n" +
                 $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Edifici.CasermaGuerrieri.Pietra.ToString("#,0")}\n" +
                 $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Edifici.CasermaGuerrieri.Ferro.ToString("#,0")}\n" +
                 $"{L.Label_Oro()}: [icon:oro]{Strutture.Edifici.CasermaGuerrieri.Oro.ToString("#,0")}\n" +
                 $"{L.Label_Popolazione()}: [icon:popolazione]{Strutture.Edifici.CasermaGuerrieri.Popolazione.ToString("#,0")}\n" +
                 $"{L.Label_Costruzione()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                 $"Limite Guerrieri: [icon:guerrieri]{Strutture.Edifici.CasermaGuerrieri.Limite.ToString("#,0")}\n" +
                 $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Strutture.Edifici.CasermaGuerrieri.Consumo_Cibo}[black] s\n" +
                 $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Strutture.Edifici.CasermaGuerrieri.Consumo_Oro}[black] s\n\n");

            tempoCalcolato = Strutture.Edifici.CasermaLanceri.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.CasermaLanceri.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.CasermaLanceri.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Caserma Lanceri|[black]" +
                 $"{L.Desc_CasermaLanceri()}\n\n" +
                 $"{L.Label_CostoCostruzione()}\n" +
                 $"{L.Label_Cibo()} [icon:cibo]{Strutture.Edifici.CasermaLanceri.Cibo.ToString("#,0")}\n" +
                 $"{L.Label_Legno()}: [icon:legno]{Strutture.Edifici.CasermaLanceri.Legno.ToString("#,0")}\n" +
                 $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Edifici.CasermaLanceri.Pietra.ToString("#,0")}\n" +
                 $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Edifici.CasermaLanceri.Ferro.ToString("#,0")}\n" +
                 $"{L.Label_Oro()}: [icon:oro]{Strutture.Edifici.CasermaLanceri.Oro.ToString("#,0")}\n" +
                 $"{L.Label_Popolazione()}: [icon:popolazione]{Strutture.Edifici.CasermaLanceri.Popolazione.ToString("#,0")}\n" +
                 $"{L.Label_Costruzione()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                 $"Limite Lancieri: [icon:lanceri]{Strutture.Edifici.CasermaLanceri.Limite.ToString("#,0")}\n" +
                 $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Strutture.Edifici.CasermaLanceri.Consumo_Cibo}[black] s\n" +
                 $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Strutture.Edifici.CasermaLanceri.Consumo_Oro}[black] s\n\n");

            tempoCalcolato = Strutture.Edifici.CasermaArceri.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.CasermaArceri.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.CasermaArceri.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Caserma Arceri|[black]" +
                 $"{L.Desc_CasermaArceri()}\n\n" +
                 $"{L.Label_CostoCostruzione()}\n" +
                 $"{L.Label_Cibo()} [icon:cibo]{Strutture.Edifici.CasermaArceri.Cibo.ToString("#,0")}\n" +
                 $"{L.Label_Legno()}: [icon:legno]{Strutture.Edifici.CasermaArceri.Legno.ToString("#,0")}\n" +
                 $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Edifici.CasermaArceri.Pietra.ToString("#,0")}\n" +
                 $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Edifici.CasermaArceri.Ferro.ToString("#,0")}\n" +
                 $"{L.Label_Oro()}: [icon:oro]{Strutture.Edifici.CasermaArceri.Oro.ToString("#,0")}\n" +
                 $"{L.Label_Popolazione()}: [icon:popolazione]{Strutture.Edifici.CasermaArceri.Popolazione.ToString("#,0")}\n" +
                 $"{L.Label_Costruzione()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                 $"Limite Arceri: [icon:arceri]{Strutture.Edifici.CasermaArceri.Limite.ToString("#,0")}\n" +
                 $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Strutture.Edifici.CasermaArceri.Consumo_Cibo}[black] s\n" +
                 $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Strutture.Edifici.CasermaArceri.Consumo_Oro}[black] s\n\n");

            tempoCalcolato = Strutture.Edifici.CasermaCatapulte.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.CasermaCatapulte.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.CasermaCatapulte.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Caserma Catapulte|[black]" +
                 $"{L.Desc_CasermaCatapulte()}\n\n" +
                 $"{L.Label_CostoCostruzione()}\n" +
                 $"{L.Label_Cibo()} [icon:cibo]{Strutture.Edifici.CasermaCatapulte.Cibo.ToString("#,0")}\n" +
                 $"{L.Label_Legno()}: [icon:legno]{Strutture.Edifici.CasermaCatapulte.Legno.ToString("#,0")}\n" +
                 $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Edifici.CasermaCatapulte.Pietra.ToString("#,0")}\n" +
                 $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Edifici.CasermaCatapulte.Ferro.ToString("#,0")}\n" +
                 $"{L.Label_Oro()}: [icon:oro]{Strutture.Edifici.CasermaCatapulte.Oro.ToString("#,0")}\n" +
                 $"{L.Label_Popolazione()}: [icon:popolazione]{Strutture.Edifici.CasermaCatapulte.Popolazione.ToString("#,0")}\n" +
                 $"{L.Label_Costruzione()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                 $"Limite Catapulte: [icon:catapulte]{Strutture.Edifici.CasermaCatapulte.Limite.ToString("#,0")}\n" +
                 $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Strutture.Edifici.CasermaCatapulte.Consumo_Cibo}[black] s\n" +
                 $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Strutture.Edifici.CasermaCatapulte.Consumo_Oro}[black] s\n\n");

            //Esercito
            tempoCalcolato = Esercito.CostoReclutamento.Guerriero_1.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Guerrieri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Guerriero_1.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Guerrieri 1|[black]" +
                $"{L.Desc_Guerriero(1)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Guerriero_1.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Esercito.CostoReclutamento.Guerriero_1.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Esercito.CostoReclutamento.Guerriero_1.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Guerriero_1.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Guerriero_1.Oro.ToString("#,0")}\n" +
                $"{L.Label_Spade()}: [icon:spade]{Esercito.CostoReclutamento.Guerriero_1.Spade.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Guerriero_1.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Guerriero_1.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Guerriero_1.Cibo.ToString()}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Guerriero_1.Salario.ToString()}[black] s\n\n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Guerriero_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Guerriero_1.Salute.ToString("#,0")}[black]{(player.Guerriero_Salute == 0 ? "" : $" [black]+ [verde]{player.Guerriero_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Guerriero_1.Difesa.ToString("#,0")}[black]{(player.Guerriero_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Guerriero_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Guerriero_1.Attacco.ToString("#,0")}[black]{(player.Guerriero_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Guerriero_Attacco}[black]")}");

            tempoCalcolato = Esercito.CostoReclutamento.Guerriero_2.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Guerrieri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Guerriero_2.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Guerrieri 2|[black]" +
                $"{L.Desc_Guerriero(2)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Guerriero_2.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Esercito.CostoReclutamento.Guerriero_2.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Esercito.CostoReclutamento.Guerriero_2.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Guerriero_2.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Guerriero_2.Oro.ToString("#,0")}\n" +
                $"{L.Label_Spade()}: [icon:spade]{Esercito.CostoReclutamento.Guerriero_2.Spade.ToString("#,0")}\n" +
                $"{L.Label_Lancie()}: [icon:lance]{Esercito.CostoReclutamento.Guerriero_2.Lance.ToString("#,0")}\n" +
                $"{L.Label_Archi()}: [icon:archi]{Esercito.CostoReclutamento.Guerriero_2.Archi.ToString("#,0")}\n" +
                $"{L.Label_Scudi()}: [icon:scudi]{Esercito.CostoReclutamento.Guerriero_2.Scudi.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Guerriero_2.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Guerriero_2.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Guerriero_2.Cibo.ToString()}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Guerriero_2.Salario.ToString()}[black] s\n\n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Guerriero_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Guerriero_2.Salute.ToString("#,0")}[black]{(player.Guerriero_Salute == 0 ? "" : $" [black]+ [verde]{player.Guerriero_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Guerriero_2.Difesa.ToString("#,0")}[black]{(player.Guerriero_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Guerriero_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Guerriero_2.Attacco.ToString("#,0")}[black]{(player.Guerriero_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Guerriero_Attacco}[black]")}");

            tempoCalcolato = Esercito.CostoReclutamento.Guerriero_3.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Guerrieri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Guerriero_3.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Guerrieri 3|[black]" +
                $"{L.Desc_Guerriero(3)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Guerriero_3.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Esercito.CostoReclutamento.Guerriero_3.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Esercito.CostoReclutamento.Guerriero_3.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Guerriero_3.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Guerriero_3.Oro.ToString("#,0")}\n" +
                $"{L.Label_Spade()}: [icon:spade]{Esercito.CostoReclutamento.Guerriero_3.Spade.ToString("#,0")}\n" +
                $"{L.Label_Lancie()}: [icon:lance]{Esercito.CostoReclutamento.Guerriero_3.Lance.ToString("#,0")}\n" +
                $"{L.Label_Archi()}: [icon:archi]{Esercito.CostoReclutamento.Guerriero_3.Archi.ToString("#,0")}\n" +
                $"{L.Label_Scudi()}: [icon:scudi]{Esercito.CostoReclutamento.Guerriero_3.Scudi.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Guerriero_3.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Guerriero_3.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Guerriero_3.Cibo.ToString()}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Guerriero_3.Salario.ToString()}[black] s\n\n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Guerriero_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Guerriero_3.Salute.ToString("#,0")}[black]{(player.Guerriero_Salute == 0 ? "" : $" [black]+ [verde]{player.Guerriero_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Guerriero_3.Difesa.ToString("#,0")}[black]{(player.Guerriero_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Guerriero_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Guerriero_3.Attacco.ToString("#,0")}[black]{(player.Guerriero_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Guerriero_Attacco}[black]")}");

            tempoCalcolato = Esercito.CostoReclutamento.Guerriero_4.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Guerrieri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Guerriero_4.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Guerrieri 4|[black]" +
                $"{L.Desc_Guerriero(4)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Guerriero_4.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Esercito.CostoReclutamento.Guerriero_4.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Esercito.CostoReclutamento.Guerriero_4.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Guerriero_4.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Guerriero_4.Oro.ToString("#,0")}\n" +
                $"{L.Label_Spade()}: [icon:spade]{Esercito.CostoReclutamento.Guerriero_4.Spade.ToString("#,0")}\n" +
                $"{L.Label_Lancie()}: [icon:lance]{Esercito.CostoReclutamento.Guerriero_4.Lance.ToString("#,0")}\n" +
                $"{L.Label_Archi()}: [icon:archi]{Esercito.CostoReclutamento.Guerriero_4.Archi.ToString("#,0")}\n" +
                $"{L.Label_Scudi()}: [icon:scudi]{Esercito.CostoReclutamento.Guerriero_4.Scudi.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Guerriero_4.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Guerriero_4.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Guerriero_4.Cibo.ToString()}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Guerriero_4.Salario.ToString()}[black] s\n\n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Guerriero_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Guerriero_4.Salute.ToString("#,0")}[black]{(player.Guerriero_Salute == 0 ? "" : $" [black]+ [verde]{player.Guerriero_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Guerriero_4.Difesa.ToString("#,0")}[black]{(player.Guerriero_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Guerriero_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Guerriero_4.Attacco.ToString("#,0")}[black]{(player.Guerriero_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Guerriero_Attacco}[black]")}");

            tempoCalcolato = Esercito.CostoReclutamento.Guerriero_5.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Guerrieri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Guerriero_5.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Guerrieri 5|[black]" +
                $"{L.Desc_Guerriero(5)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Guerriero_5.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Esercito.CostoReclutamento.Guerriero_5.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Esercito.CostoReclutamento.Guerriero_5.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Guerriero_5.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Guerriero_5.Oro.ToString("#,0")}\n" +
                $"{L.Label_Spade()}: [icon:spade]{Esercito.CostoReclutamento.Guerriero_5.Spade.ToString("#,0")}\n" +
                $"{L.Label_Lancie()}: [icon:lance]{Esercito.CostoReclutamento.Guerriero_5.Lance.ToString("#,0")}\n" +
                $"{L.Label_Archi()}: [icon:archi]{Esercito.CostoReclutamento.Guerriero_5.Archi.ToString("#,0")}\n" +
                $"{L.Label_Scudi()}: [icon:scudi]{Esercito.CostoReclutamento.Guerriero_5.Scudi.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Guerriero_5.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Guerriero_5.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Guerriero_5.Cibo.ToString()}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Guerriero_5.Salario.ToString()}[black] s\n\n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Guerriero_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Guerriero_5.Salute.ToString("#,0")}[black]{(player.Guerriero_Salute == 0 ? "" : $" [black]+ [verde]{player.Guerriero_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Guerriero_5.Difesa.ToString("#,0")}[black]{(player.Guerriero_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Guerriero_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Guerriero_5.Attacco.ToString("#,0")}[black]{(player.Guerriero_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Guerriero_Attacco}[black]")}");

            tempoCalcolato = Esercito.CostoReclutamento.Lancere_1.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Lanceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Lancere_1.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Lanceri 1|[black]" +
                $"{L.Desc_Lancere(1)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Lancere_1.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Esercito.CostoReclutamento.Lancere_1.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Esercito.CostoReclutamento.Lancere_1.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Lancere_1.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Lancere_1.Oro.ToString("#,0")}\n" +
                $"{L.Label_Lancie()}: [icon:lance]{Esercito.CostoReclutamento.Lancere_1.Lance.ToString("#,0")}\n" +
                $"{L.Label_Scudi()}: [icon:scudi]{Esercito.CostoReclutamento.Lancere_1.Scudi.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Lancere_1.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Lancere_1.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Lancere_1.Cibo.ToString()}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Lancere_1.Salario.ToString()}[black] s\n \n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Lancere_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Lancere_1.Salute.ToString("#,0")}[black]{(player.Lancere_Salute == 0 ? "" : $" [black]+ [verde]{player.Lancere_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Lancere_1.Difesa.ToString("#,0")}[black]{(player.Lancere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Lancere_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Lancere_1.Attacco.ToString("#,0")}[black]{(player.Lancere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Lancere_Attacco}[black]")}");

            tempoCalcolato = Esercito.CostoReclutamento.Lancere_2.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Lanceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Lancere_2.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Lanceri 2|[black]" +
                $"{L.Desc_Lancere(2)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Lancere_2.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:cilegnobo]{Esercito.CostoReclutamento.Lancere_2.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Esercito.CostoReclutamento.Lancere_2.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Lancere_2.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Lancere_2.Oro.ToString("#,0")}\n" +
                $"{L.Label_Spade()}: [icon:spade]{Esercito.CostoReclutamento.Lancere_2.Spade.ToString("#,0")}\n" +
                $"{L.Label_Lancie()}: [icon:lance]{Esercito.CostoReclutamento.Lancere_2.Lance.ToString("#,0")}\n" +
                $"{L.Label_Archi()}: [icon:archi]{Esercito.CostoReclutamento.Lancere_2.Archi.ToString("#,0")}\n" +
                $"{L.Label_Scudi()}: [icon:scudi]{Esercito.CostoReclutamento.Lancere_2.Scudi.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Lancere_2.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Lancere_2.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Lancere_2.Cibo.ToString()}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Lancere_2.Salario.ToString()}[black] s\n \n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Lancere_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Lancere_2.Salute.ToString("#,0")}[black]{(player.Lancere_Salute == 0 ? "" : $" [black]+ [verde]{player.Lancere_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Lancere_2.Difesa.ToString("#,0")}[black]{(player.Lancere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Lancere_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Lancere_2.Attacco.ToString("#,0")}[black]{(player.Lancere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Lancere_Attacco}[black]")}");

            tempoCalcolato = Esercito.CostoReclutamento.Lancere_3.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Lanceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Lancere_3.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Lanceri 3|[black]" +
                $"{L.Desc_Lancere(3)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Lancere_3.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:cilegnobo]{Esercito.CostoReclutamento.Lancere_3.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Esercito.CostoReclutamento.Lancere_3.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Lancere_3.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Lancere_3.Oro.ToString("#,0")}\n" +
                $"{L.Label_Spade()}: [icon:spade]{Esercito.CostoReclutamento.Lancere_3.Spade.ToString("#,0")}\n" +
                $"{L.Label_Lancie()}: [icon:lance]{Esercito.CostoReclutamento.Lancere_3.Lance.ToString("#,0")}\n" +
                $"{L.Label_Archi()}: [icon:archi]{Esercito.CostoReclutamento.Lancere_3.Archi.ToString("#,0")}\n" +
                $"{L.Label_Scudi()}: [icon:scudi]{Esercito.CostoReclutamento.Lancere_3.Scudi.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Lancere_3.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Lancere_3.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Lancere_3.Cibo.ToString()}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Lancere_3.Salario.ToString()}[black] s\n \n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Lancere_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Lancere_3.Salute.ToString("#,0")}[black]{(player.Lancere_Salute == 0 ? "" : $" [black]+ [verde]{player.Lancere_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Lancere_3.Difesa.ToString("#,0")}[black]{(player.Lancere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Lancere_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Lancere_3.Attacco.ToString("#,0")}[black]{(player.Lancere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Lancere_Attacco}[black]")}");

            tempoCalcolato = Esercito.CostoReclutamento.Lancere_4.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Lanceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Lancere_4.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Lanceri 4|[black]" +
                $"{L.Desc_Lancere(4)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Lancere_4.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:cilegnobo]{Esercito.CostoReclutamento.Lancere_4.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Esercito.CostoReclutamento.Lancere_4.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Lancere_4.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Lancere_4.Oro.ToString("#,0")}\n" +
                $"{L.Label_Spade()}: [icon:spade]{Esercito.CostoReclutamento.Lancere_4.Spade.ToString("#,0")}\n" +
                $"{L.Label_Lancie()}: [icon:lance]{Esercito.CostoReclutamento.Lancere_4.Lance.ToString("#,0")}\n" +
                $"{L.Label_Archi()}: [icon:archi]{Esercito.CostoReclutamento.Lancere_4.Archi.ToString("#,0")}\n" +
                $"{L.Label_Scudi()}: [icon:scudi]{Esercito.CostoReclutamento.Lancere_4.Scudi.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Lancere_4.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Lancere_1.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Lancere_4.Cibo.ToString()}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Lancere_4.Salario.ToString()}[black] s\n \n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Lancere_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Lancere_4.Salute.ToString("#,0")}[black]{(player.Lancere_Salute == 0 ? "" : $" [black]+ [verde]{player.Lancere_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Lancere_4.Difesa.ToString("#,0")}[black]{(player.Lancere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Lancere_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Lancere_4.Attacco.ToString("#,0")}[black]{(player.Lancere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Lancere_Attacco}[black]")}");

            tempoCalcolato = Esercito.CostoReclutamento.Lancere_5.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Lanceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Lancere_5.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Lanceri 5|[black]" +
                $"{L.Desc_Lancere(5)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Lancere_5.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:cilegnobo]{Esercito.CostoReclutamento.Lancere_5.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Esercito.CostoReclutamento.Lancere_5.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Lancere_5.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Lancere_5.Oro.ToString("#,0")}\n" +
                $"{L.Label_Spade()}: [icon:spade]{Esercito.CostoReclutamento.Lancere_5.Spade.ToString("#,0")}\n" +
                $"{L.Label_Lancie()}: [icon:lance]{Esercito.CostoReclutamento.Lancere_5.Lance.ToString("#,0")}\n" +
                $"{L.Label_Archi()}: [icon:archi]{Esercito.CostoReclutamento.Lancere_5.Archi.ToString("#,0")}\n" +
                $"{L.Label_Scudi()}: [icon:scudi]{Esercito.CostoReclutamento.Lancere_5.Scudi.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Lancere_5.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Lancere_5.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Lancere_5.Cibo.ToString()}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Lancere_5.Salario.ToString()}[black] s\n \n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Lancere_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Lancere_5.Salute.ToString("#,0")}[black]{(player.Lancere_Salute == 0 ? "" : $" [black]+ [verde]{player.Lancere_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Lancere_5.Difesa.ToString("#,0")}[black]{(player.Lancere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Lancere_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Lancere_5.Attacco.ToString("#,0")}[black]{(player.Lancere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Lancere_Attacco}[black]")}");

            tempoCalcolato = Esercito.CostoReclutamento.Arcere_1.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Arceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Arcere_1.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Arceri 1|[black]" +
                $"{L.Desc_Arcere(1)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Arcere_1.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Esercito.CostoReclutamento.Arcere_1.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Esercito.CostoReclutamento.Arcere_1.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Arcere_1.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Arcere_1.Oro.ToString("#,0")}\n" +
                $"{L.Label_Archi()}: [icon:archi]{Esercito.CostoReclutamento.Arcere_1.Archi.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Arcere_1.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Arcere_1.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Arcere_1.Cibo.ToString()}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Arcere_1.Salario.ToString()}[black] s\n\n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Arcere_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Arcere_1.Salute.ToString("#,0")}[black]{(player.Arcere_Salute == 0 ? "" : $" [black]+ [verde]{player.Arcere_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Arcere_1.Difesa.ToString("#,0")}[black]{(player.Arcere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Arcere_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Arcere_1.Attacco.ToString("#,0")}[black]{(player.Arcere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Arcere_Attacco}[black]")}");

            tempoCalcolato = Esercito.CostoReclutamento.Arcere_2.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Arceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Arcere_2.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Arceri 2|[black]" +
                $"{L.Desc_Arcere(2)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Arcere_2.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Esercito.CostoReclutamento.Arcere_2.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Esercito.CostoReclutamento.Arcere_2.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Arcere_2.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Arcere_2.Oro.ToString("#,0")}\n" +
                $"{L.Label_Spade()}: [icon:spade]{Esercito.CostoReclutamento.Arcere_2.Spade.ToString("#,0")}\n" +
                $"{L.Label_Lancie()}: [icon:lance]{Esercito.CostoReclutamento.Arcere_2.Lance.ToString("#,0")}\n" +
                $"{L.Label_Archi()}: [icon:archi]{Esercito.CostoReclutamento.Arcere_2.Archi.ToString("#,0")}\n" +
                $"{L.Label_Scudi()}: [icon:scudi]{Esercito.CostoReclutamento.Arcere_2.Scudi.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Arcere_2.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Arcere_2.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Arcere_2.Cibo.ToString()}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Arcere_2.Salario.ToString()}[black] s\n\n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Arcere_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Arcere_2.Salute.ToString("#,0")}[black]{(player.Arcere_Salute == 0 ? "" : $" [black]+ [verde]{player.Arcere_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Arcere_2.Difesa.ToString("#,0")}[black]{(player.Arcere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Arcere_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Arcere_2.Attacco.ToString("#,0")}[black]{(player.Arcere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Arcere_Attacco}[black]")}");

            tempoCalcolato = Esercito.CostoReclutamento.Arcere_3.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Arceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Arcere_3.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Arceri 3|[black]" +
                $"{L.Desc_Arcere(3)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Arcere_3.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Esercito.CostoReclutamento.Arcere_3.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Esercito.CostoReclutamento.Arcere_3.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Arcere_3.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Arcere_3.Oro.ToString("#,0")}" +
                $"{L.Label_Spade()}: [icon:spade]{Esercito.CostoReclutamento.Arcere_3.Spade.ToString("#,0")}\n" +
                $"{L.Label_Lancie()}: [icon:lance]{Esercito.CostoReclutamento.Arcere_3.Lance.ToString("#,0")}\n" +
                $"{L.Label_Archi()}: [icon:archi]{Esercito.CostoReclutamento.Arcere_3.Archi.ToString("#,0")}\n" +
                $"{L.Label_Scudi()}: [icon:scudi]{Esercito.CostoReclutamento.Arcere_3.Scudi.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Arcere_3.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Arcere_3.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"M{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Arcere_3.Cibo.ToString()}[black] s\n" +
                $"M{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Arcere_3.Salario.ToString()}[black] s\n\n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Arcere_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Arcere_3.Salute.ToString("#,0")}[black]{(player.Arcere_Salute == 0 ? "" : $" [black]+ [verde]{player.Arcere_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Arcere_3.Difesa.ToString("#,0")}[black]{(player.Arcere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Arcere_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Arcere_3.Attacco.ToString("#,0")}[black]{(player.Arcere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Arcere_Attacco}[black]")}");

            tempoCalcolato = Esercito.CostoReclutamento.Arcere_4.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Arceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Arcere_4.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Arceri 4|[black]" +
                $"{L.Desc_Arcere(4)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Arcere_4.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Esercito.CostoReclutamento.Arcere_4.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Esercito.CostoReclutamento.Arcere_4.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Arcere_4.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Arcere_4.Oro.ToString("#,0")}\n" +
                $"{L.Label_Spade()}: [icon:spade]{Esercito.CostoReclutamento.Arcere_4.Spade.ToString("#,0")}\n" +
                $"{L.Label_Lancie()}: [icon:lance]{Esercito.CostoReclutamento.Arcere_4.Lance.ToString("#,0")}\n" +
                $"{L.Label_Archi()}: [icon:archi]{Esercito.CostoReclutamento.Arcere_4.Archi.ToString("#,0")}\n" +
                $"{L.Label_Scudi()}: [icon:scudi]{Esercito.CostoReclutamento.Arcere_4.Scudi.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Arcere_4.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Arcere_4.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Arcere_4.Cibo.ToString()}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Arcere_4.Salario.ToString()}[black] s\n\n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Arcere_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Arcere_4.Salute.ToString("#,0")}[black]{(player.Arcere_Salute == 0 ? "" : $" [black]+ [verde]{player.Arcere_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Arcere_4.Difesa.ToString("#,0")}[black]{(player.Arcere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Arcere_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Arcere_4.Attacco.ToString("#,0")}[black]{(player.Arcere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Arcere_Attacco}[black]")}");

            tempoCalcolato = Esercito.CostoReclutamento.Arcere_5.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Arceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Arcere_5.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Arceri 5|[black]" +
                $"{L.Desc_Arcere(5)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Arcere_5.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Esercito.CostoReclutamento.Arcere_5.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Esercito.CostoReclutamento.Arcere_5.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Arcere_5.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Arcere_5.Oro.ToString("#,0")}\n" +
                $"{L.Label_Spade()}: [icon:spade]{Esercito.CostoReclutamento.Arcere_5.Spade.ToString("#,0")}\n" +
                $"{L.Label_Lancie()}: [icon:lance]{Esercito.CostoReclutamento.Arcere_5.Lance.ToString("#,0")}\n" +
                $"{L.Label_Archi()}: [icon:archi]{Esercito.CostoReclutamento.Arcere_5.Archi.ToString("#,0")}\n" +
                $"{L.Label_Scudi()}: [icon:scudi]{Esercito.CostoReclutamento.Arcere_5.Scudi.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Arcere_5.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Arcere_5.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Arcere_5.Cibo.ToString()}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Arcere_5.Salario.ToString()}[black] s\n\n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Arcere_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Arcere_5.Salute.ToString("#,0")}[black]{(player.Arcere_Salute == 0 ? "" : $" [black]+ [verde]{player.Arcere_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Arcere_5.Difesa.ToString("#,0")}[black]{(player.Arcere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Arcere_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Arcere_5.Attacco.ToString("#,0")}[black]{(player.Arcere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Arcere_Attacco}[black]")}");

            tempoCalcolato = Esercito.CostoReclutamento.Catapulta_1.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Catapulte_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Catapulta_1.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Catapulte 1|[black]" +
                $"{L.Desc_Catapulta(1)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Catapulta_1.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Esercito.CostoReclutamento.Catapulta_1.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:cipietrabo]{Esercito.CostoReclutamento.Catapulta_1.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Catapulta_1.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Catapulta_1.Oro.ToString("#,0")}\n" +
                $"{L.Label_Spade()}: [icon:spade]{Esercito.CostoReclutamento.Catapulta_1.Spade.ToString("#,0")}\n" +
                $"{L.Label_Lancie()}: [icon:lance]{Esercito.CostoReclutamento.Catapulta_1.Lance.ToString("#,0")}\n" +
                $"{L.Label_Scudi()}: [icon:scudi]{Esercito.CostoReclutamento.Catapulta_1.Scudi.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Catapulta_1.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Catapulta_1.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Catapulta_1.Cibo.ToString("0.00")}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Catapulta_1.Salario.ToString("0.00")}[black] s\n\n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Catapulta_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Catapulta_1.Salute.ToString("#,0")}[black]{(player.Catapulta_Salute == 0 ? "" : $" [black]+ [verde]{player.Catapulta_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Catapulta_1.Difesa.ToString("#,0")}[black]{(player.Catapulta_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Catapulta_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Catapulta_1.Attacco.ToString("#,0")}[black]{(player.Catapulta_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Catapulta_Attacco}[black]")}");

            tempoCalcolato = Esercito.CostoReclutamento.Catapulta_2.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Catapulte_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Catapulta_2.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Catapulte 2|[black]" +
                $"{L.Desc_Catapulta(2)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Catapulta_2.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Esercito.CostoReclutamento.Catapulta_2.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:cipietrabo]{Esercito.CostoReclutamento.Catapulta_2.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Catapulta_2.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Catapulta_2.Oro.ToString("#,0")}\n" +
                $"{L.Label_Spade()}: [icon:spade]{Esercito.CostoReclutamento.Catapulta_2.Spade.ToString("#,0")}\n" +
                $"{L.Label_Lancie()}: [icon:lance]{Esercito.CostoReclutamento.Catapulta_2.Lance.ToString("#,0")}\n" +
                $"{L.Label_Scudi()}: [icon:scudi]{Esercito.CostoReclutamento.Catapulta_2.Scudi.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Catapulta_2.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Catapulta_2.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Catapulta_2.Cibo.ToString("0.00")}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Catapulta_2.Salario.ToString("0.00")}[black] s\n\n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Catapulta_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Catapulta_2.Salute.ToString("#,0")}[black]{(player.Catapulta_Salute == 0 ? "" : $" [black]+ [verde]{player.Catapulta_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Catapulta_2.Difesa.ToString("#,0")}[black]{(player.Catapulta_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Catapulta_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Catapulta_2.Attacco.ToString("#,0")}[black]{(player.Catapulta_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Catapulta_Attacco}[black]")}");

            tempoCalcolato = Esercito.CostoReclutamento.Catapulta_3.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Catapulte_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Catapulta_3.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Catapulte 3|[black]" +
                $"{L.Desc_Catapulta(3)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Catapulta_3.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Esercito.CostoReclutamento.Catapulta_3.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:cipietrabo]{Esercito.CostoReclutamento.Catapulta_3.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Catapulta_3.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Catapulta_3.Oro.ToString("#,0")}\n" +
                $"{L.Label_Spade()}: [icon:spade]{Esercito.CostoReclutamento.Catapulta_3.Spade.ToString("#,0")}\n" +
                $"{L.Label_Lancie()}: [icon:lance]{Esercito.CostoReclutamento.Catapulta_3.Lance.ToString("#,0")}\n" +
                $"{L.Label_Scudi()}: [icon:scudi]{Esercito.CostoReclutamento.Catapulta_3.Scudi.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Catapulta_3.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Catapulta_3.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Catapulta_3.Cibo.ToString("0.00")}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Catapulta_3.Salario.ToString("0.00")}[black] s\n\n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Catapulta_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Catapulta_3.Salute.ToString("#,0")}[black]{(player.Catapulta_Salute == 0 ? "" : $" [black]+ [verde]{player.Catapulta_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Catapulta_3.Difesa.ToString("#,0")}[black]{(player.Catapulta_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Catapulta_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Catapulta_3.Attacco.ToString("#,0")}[black]{(player.Catapulta_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Catapulta_Attacco}[black]")}");

            tempoCalcolato = Esercito.CostoReclutamento.Catapulta_4.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Catapulte_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Catapulta_4.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Catapulte 4|[black]" +
                $"{L.Desc_Catapulta(4)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Catapulta_4.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Esercito.CostoReclutamento.Catapulta_4.Legno.ToString("#,0")}" +
                $"{L.Label_Pietra()}: [icon:pietra]{Esercito.CostoReclutamento.Catapulta_4.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Catapulta_4.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Catapulta_4.Oro.ToString("#,0")}\n" +
                $"{L.Label_Spade()}: [icon:spade]{Esercito.CostoReclutamento.Catapulta_4.Spade.ToString("#,0")}\n" +
                $"{L.Label_Lancie()}: [icon:lance]{Esercito.CostoReclutamento.Catapulta_4.Lance.ToString("#,0")}\n" +
                $"{L.Label_Scudi()}: [icon:scudi]{Esercito.CostoReclutamento.Catapulta_4.Scudi.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Catapulta_4.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Catapulta_4.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Catapulta_4.Cibo.ToString("0.00")}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Catapulta_4.Salario.ToString("0.00")}[black] s\n\n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Catapulta_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Catapulta_4.Salute.ToString("#,0")}[black]{(player.Catapulta_Salute == 0 ? "" : $" [black]+ [verde]{player.Catapulta_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Catapulta_4.Difesa.ToString("#,0")}[black]{(player.Catapulta_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Catapulta_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Catapulta_4.Attacco.ToString("#,0")}[black]{(player.Catapulta_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Catapulta_Attacco}[black]")}");

            tempoCalcolato = Esercito.CostoReclutamento.Catapulta_5.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Catapulte_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Catapulta_5.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Catapulte 5|[black]" +
                $"{L.Desc_Catapulta(5)}\n\n" +
                $"{L.Label_CostoAddestramento()}:\n" +
                $"{L.Label_Cibo()} [icon:cibo]{Esercito.CostoReclutamento.Catapulta_5.Cibo.ToString("#,0")}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Esercito.CostoReclutamento.Catapulta_5.Legno.ToString("#,0")}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Esercito.CostoReclutamento.Catapulta_5.Pietra.ToString("#,0")}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Esercito.CostoReclutamento.Catapulta_5.Ferro.ToString("#,0")}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Esercito.CostoReclutamento.Catapulta_5.Oro.ToString("#,0")}\n" +
                $"{L.Label_Spade()}: [icon:spade]{Esercito.CostoReclutamento.Catapulta_5.Spade.ToString("#,0")}\n" +
                $"{L.Label_Lancie()}: [icon:lance]{Esercito.CostoReclutamento.Catapulta_5.Lance.ToString("#,0")}\n" +
                $"{L.Label_Scudi()}: [icon:scudi]{Esercito.CostoReclutamento.Catapulta_5.Scudi.ToString("#,0")}\n" +
                $"{L.Label_Armature()}: [icon:armature]{Esercito.CostoReclutamento.Catapulta_5.Armature.ToString("#,0")}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Esercito.CostoReclutamento.Catapulta_5.Popolazione}\n" +
                $"{L.Label_Addestramento()}: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n\n" +
                $"{L.Label_Mantenimento_Cibo()} [icon:cibo][rosso]-{Esercito.Unità.Catapulta_5.Cibo.ToString("0.00")}[black] s\n" +
                $"{L.Label_Mantenimento_Oro()}: [icon:oro][rosso]-{Esercito.Unità.Catapulta_5.Salario.ToString("0.00")}[black] s\n\n" +
                $"{L.Label_Statistiche()}:\n" +
                $"{L.Label_Livello()}: [arancione]{player.Catapulta_Livello.ToString("#,0")}[black]\n" +
                $"{L.Label_Salute()}: [verde]{Esercito.Unità.Catapulta_5.Salute.ToString("#,0")}[black]{(player.Catapulta_Salute == 0 ? "" : $" [black]+ [verde]{player.Catapulta_Salute}[black]")}\n" +
                $"{L.Label_Difesa()}: [bluGotico]{Esercito.Unità.Catapulta_5.Difesa.ToString("#,0")}[black]{(player.Catapulta_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Catapulta_Difesa}[black]")}\n" +
                $"{L.Label_Attacco()}: [rosso]{Esercito.Unità.Catapulta_5.Attacco.ToString("#,0")}[black]{(player.Catapulta_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Catapulta_Attacco}[black]")}");

            Send(player.guid_Player, $"Descrizione|Esperienza|[black]{L.Desc_Esperienza(Esperienza.LevelUp(player).ToString())}");
            Send(player.guid_Player, $"Descrizione|Livello|[black]{L.Desc_Livello()}");
            Send(player.guid_Player, $"Descrizione|Giocatore|[black]{L.Desc_Statistiche()}");
            Send(player.guid_Player, $"Descrizione|Diamanti Blu|[black]{L.Desc_DiamantiBlu()}");
            Send(player.guid_Player, $"Descrizione|Diamanti Viola|[black]{L.Desc_DiamantiViola()}");
            Send(player.guid_Player, $"Descrizione|Dollari Virtuali|[black]{L.Desc_DollariVirtuali()}");
            Send(player.guid_Player, $"Descrizione|Cibo|[black]{L.Desc_Cibo()}");
            Send(player.guid_Player, $"Descrizione|Legno|[black]{L.Desc_Legno()}");
            Send(player.guid_Player, $"Descrizione|Pietra|[black]{L.Desc_Pietra()}");
            Send(player.guid_Player, $"Descrizione|Ferro|[black]{L.Desc_Ferro()}");
            Send(player.guid_Player, $"Descrizione|Oro|[black]{L.Desc_Oro()}");
            Send(player.guid_Player, $"Descrizione|Popolazione|[black]{L.Desc_Popolazione()}");
            Send(player.guid_Player, $"Descrizione|Spade|{L.Desc_Spade()}");
            Send(player.guid_Player, $"Descrizione|Lance|{L.Desc_Lance()}");
            Send(player.guid_Player, $"Descrizione|Archi|{L.Desc_Archi()}");
            Send(player.guid_Player, $"Descrizione|Scudi|{L.Desc_Scudi()}");
            Send(player.guid_Player, $"Descrizione|Armature|{L.Desc_Armature()}");
            Send(player.guid_Player, $"Descrizione|Frecce|{L.Desc_Frecce()}");

            Send(player.guid_Player, $"Descrizione|Mura Salute|{L.Desc_SaluteMura()}" +
                $"{L.Label_Cibo()} [icon:cibo]{Strutture.Riparazione.Mura.Consumo_Cibo}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Strutture.Riparazione.Mura.Consumo_Legno}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Riparazione.Mura.Consumo_Pietra}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Riparazione.Mura.Consumo_Ferro}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Strutture.Riparazione.Mura.Consumo_Oro}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");
            Send(player.guid_Player, $"Descrizione|Mura Difesa|{L.Desc_DifesaMura()}" +
                $"{L.Label_Cibo()} [icon:cibo]{Strutture.Riparazione.Mura.Consumo_Cibo}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Strutture.Riparazione.Mura.Consumo_Legno}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Riparazione.Mura.Consumo_Pietra}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Riparazione.Mura.Consumo_Ferro}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Strutture.Riparazione.Mura.Consumo_Oro}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");

            Send(player.guid_Player, $"Descrizione|Cancello Salute|{L.Desc_SaluteCancello()}" +
                $"{L.Label_Cibo()} [icon:cibo]{Strutture.Riparazione.Cancello.Consumo_Cibo}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Strutture.Riparazione.Cancello.Consumo_Legno}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Riparazione.Cancello.Consumo_Pietra}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Riparazione.Cancello.Consumo_Ferro}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Strutture.Riparazione.Cancello.Consumo_Oro}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");
            Send(player.guid_Player, $"Descrizione|Cancello Difesa|{L.Desc_DifesaCancello()}" +
                $"{L.Label_Cibo()} [icon:cibo]{Strutture.Riparazione.Cancello.Consumo_Cibo}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Strutture.Riparazione.Cancello.Consumo_Legno}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Riparazione.Cancello.Consumo_Pietra}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Riparazione.Cancello.Consumo_Ferro}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Strutture.Riparazione.Cancello.Consumo_Oro}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");

            Send(player.guid_Player, $"Descrizione|Torri Salute|{L.Desc_SaluteTorri()}" +
                $"{L.Label_Cibo()} [icon:cibo]{Strutture.Riparazione.Torri.Consumo_Cibo}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Strutture.Riparazione.Torri.Consumo_Legno}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Riparazione.Torri.Consumo_Pietra}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Riparazione.Torri.Consumo_Ferro}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Strutture.Riparazione.Torri.Consumo_Oro}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");
            Send(player.guid_Player, $"Descrizione|Torri Difesa|{L.Desc_DifesaTorri()}" +
                $"{L.Label_Cibo()} [icon:cibo]{Strutture.Riparazione.Torri.Consumo_Cibo}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Strutture.Riparazione.Torri.Consumo_Legno}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Riparazione.Torri.Consumo_Pietra}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Riparazione.Torri.Consumo_Ferro}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Strutture.Riparazione.Torri.Consumo_Oro}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");

            Send(player.guid_Player, $"Descrizione|Castello Salute|{L.Desc_CastelloSalute()}" +
                $"{L.Label_Cibo()} [icon:cibo]{Strutture.Riparazione.Castello.Consumo_Cibo}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Strutture.Riparazione.Castello.Consumo_Legno}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Riparazione.Castello.Consumo_Pietra}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Riparazione.Castello.Consumo_Ferro}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Strutture.Riparazione.Castello.Consumo_Oro}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");
            Send(player.guid_Player, $"Descrizione|Castello Difesa|{L.Desc_SaluteCancello()}" +
                $"{L.Label_Cibo()} [icon:cibo]{Strutture.Riparazione.Castello.Consumo_Cibo}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Strutture.Riparazione.Castello.Consumo_Legno}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Strutture.Riparazione.Castello.Consumo_Pietra}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Strutture.Riparazione.Castello.Consumo_Ferro}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Strutture.Riparazione.Castello.Consumo_Oro}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Addestramento|{L.Desc_RicercaAddestramento(player.Ricerca_Addestramento + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Tipi.Addestramento.Cibo * (player.Ricerca_Addestramento + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Tipi.Addestramento.Legno * (player.Ricerca_Addestramento + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Tipi.Addestramento.Pietra * (player.Ricerca_Addestramento + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Tipi.Addestramento.Ferro * (player.Ricerca_Addestramento + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Tipi.Addestramento.Oro * (player.Ricerca_Addestramento + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Tipi.Addestramento.TempoRicerca * (player.Ricerca_Addestramento + 1))}\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Costruzione|{L.Desc_RicercaCostruzione(player.Ricerca_Costruzione + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Tipi.Costruzione.Cibo * (player.Ricerca_Costruzione + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Tipi.Costruzione.Legno * (player.Ricerca_Costruzione + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Tipi.Costruzione.Pietra * (player.Ricerca_Costruzione + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Tipi.Costruzione.Ferro * (player.Ricerca_Costruzione + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Tipi.Costruzione.Oro * (player.Ricerca_Costruzione + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Tipi.Costruzione.TempoRicerca * (player.Ricerca_Costruzione + 1))}\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Produzione|{L.Desc_RicercaProduzione(player.Ricerca_Produzione + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Tipi.Produzione.Cibo * (player.Ricerca_Produzione + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Tipi.Produzione.Legno * (player.Ricerca_Produzione + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Tipi.Produzione.Pietra * (player.Ricerca_Produzione + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Tipi.Produzione.Ferro * (player.Ricerca_Produzione + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Tipi.Produzione.Oro * (player.Ricerca_Produzione + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Tipi.Produzione.TempoRicerca * (player.Ricerca_Produzione + 1))}");

            Send(player.guid_Player, $"Descrizione|Ricerca Popolazione|{L.Desc_RicercaPopolazione(player.Ricerca_Popolazione + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Tipi.Popolazione.Cibo * (player.Ricerca_Popolazione + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Tipi.Popolazione.Legno * (player.Ricerca_Popolazione + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Tipi.Popolazione.Pietra * (player.Ricerca_Popolazione + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Tipi.Popolazione.Ferro * (player.Ricerca_Popolazione + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Tipi.Popolazione.Oro * (player.Ricerca_Popolazione + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Tipi.Popolazione.TempoRicerca * (player.Ricerca_Popolazione + 1))}");

            Send(player.guid_Player, $"Descrizione|Ricerca Trasporto|{L.Desc_RicercaTrasporto(player.Ricerca_Trasporto + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Tipi.Trasporto.Cibo * (player.Ricerca_Trasporto + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Tipi.Trasporto.Legno * (player.Ricerca_Trasporto + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Tipi.Trasporto.Pietra * (player.Ricerca_Trasporto + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Tipi.Trasporto.Ferro * (player.Ricerca_Trasporto + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Tipi.Trasporto.Oro * (player.Ricerca_Trasporto + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Tipi.Trasporto.TempoRicerca * (player.Ricerca_Trasporto + 1))}");

            Send(player.guid_Player, $"Descrizione|Ricerca Riparazione|{L.Desc_RicercaRiparazione(player.Ricerca_Riparazione + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Tipi.Riparazione.Cibo * (player.Ricerca_Riparazione + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Tipi.Riparazione.Legno * (player.Ricerca_Riparazione + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Tipi.Riparazione.Pietra * (player.Ricerca_Riparazione + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Tipi.Riparazione.Ferro * (player.Ricerca_Riparazione + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Tipi.Riparazione.Oro * (player.Ricerca_Riparazione + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Tipi.Riparazione.TempoRicerca * (player.Ricerca_Riparazione + 1))}\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Guerriero Livello|{L.Desc_RicercaGuerrieroLivello(player.Guerriero_Livello + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Soldati.Livello.Cibo * (player.Guerriero_Livello + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Soldati.Livello.Legno * (player.Guerriero_Livello + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Soldati.Livello.Pietra * (player.Guerriero_Livello + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Soldati.Livello.Ferro * (player.Guerriero_Livello + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Soldati.Livello.Oro * (player.Guerriero_Livello + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Livello.TempoRicerca * (player.Guerriero_Livello + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Guerriero Salute|{L.Desc_RicercaGuerrieroSalute(player.Guerriero_Salute + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Soldati.Salute.Cibo * (player.Guerriero_Salute + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Soldati.Salute.Legno * (player.Guerriero_Salute + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Soldati.Salute.Pietra * (player.Guerriero_Salute + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Soldati.Salute.Ferro * (player.Guerriero_Salute + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Soldati.Salute.Oro * (player.Guerriero_Salute + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Salute.TempoRicerca * (player.Guerriero_Salute + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Guerriero Attacco|{L.Desc_RicercaGuerrieroAttacco(player.Guerriero_Attacco + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Soldati.Attacco.Cibo * (player.Guerriero_Attacco + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Soldati.Attacco.Legno * (player.Guerriero_Attacco + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Soldati.Attacco.Pietra * (player.Guerriero_Attacco + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Soldati.Attacco.Ferro * (player.Guerriero_Attacco + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Soldati.Attacco.Oro * (player.Guerriero_Attacco + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Attacco.TempoRicerca * (player.Guerriero_Attacco + 1))}\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Guerriero Difesa|{L.Desc_RicercaGuerrieroDifesa(player.Guerriero_Difesa + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Soldati.Difesa.Cibo * (player.Guerriero_Difesa + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Soldati.Difesa.Legno * (player.Guerriero_Difesa + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Soldati.Difesa.Pietra * (player.Guerriero_Difesa + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Soldati.Difesa.Ferro * (player.Guerriero_Difesa + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Soldati.Difesa.Oro * (player.Guerriero_Difesa + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Difesa.TempoRicerca * (player.Guerriero_Difesa + 1))}\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Lancere Livello|{L.Desc_RicercaLancereLivello(player.Lancere_Livello + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Soldati.Livello.Cibo * (player.Lancere_Livello + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Soldati.Livello.Legno * (player.Lancere_Livello + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Soldati.Livello.Pietra * (player.Lancere_Livello + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Soldati.Livello.Ferro * (player.Lancere_Livello + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Soldati.Livello.Oro * (player.Lancere_Livello + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Livello.TempoRicerca * (player.Lancere_Livello + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Lancere Salute|{L.Desc_RicercaLancereSalute(player.Lancere_Salute + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Soldati.Salute.Cibo * (player.Lancere_Salute + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Soldati.Salute.Legno * (player.Lancere_Salute + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Soldati.Salute.Pietra * (player.Lancere_Salute + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Soldati.Salute.Ferro * (player.Lancere_Salute + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Soldati.Salute.Oro * (player.Lancere_Salute + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Salute.TempoRicerca * (player.Lancere_Salute + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Lancere Attacco|{L.Desc_RicercaLancereAttacco(player.Lancere_Attacco + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Soldati.Attacco.Cibo * (player.Lancere_Attacco + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Soldati.Attacco.Legno * (player.Lancere_Attacco + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Soldati.Attacco.Pietra * (player.Lancere_Attacco + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Soldati.Attacco.Ferro * (player.Lancere_Attacco + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Soldati.Attacco.Oro * (player.Lancere_Attacco + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Attacco.TempoRicerca * (player.Lancere_Attacco + 1))}\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Lancere Difesa|{L.Desc_RicercaLancereDifesa(player.Lancere_Difesa + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Soldati.Difesa.Cibo * (player.Lancere_Difesa + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Soldati.Difesa.Legno * (player.Lancere_Difesa + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Soldati.Difesa.Pietra * (player.Lancere_Difesa + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Soldati.Difesa.Ferro * (player.Lancere_Difesa + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Soldati.Difesa.Oro * (player.Lancere_Difesa + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Difesa.TempoRicerca * (player.Lancere_Difesa + 1))}\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Arcere Livello|{L.Desc_RicercaArcereLivelloe(player.Arcere_Livello + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Soldati.Livello.Cibo * (player.Arcere_Livello + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Soldati.Livello.Legno * (player.Arcere_Livello + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Soldati.Livello.Pietra * (player.Arcere_Livello + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Soldati.Livello.Ferro * (player.Arcere_Livello + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Soldati.Livello.Oro * (player.Arcere_Livello + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Livello.TempoRicerca * (player.Arcere_Livello + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Arcere Salute|{L.Desc_RicercaArcereSalute(player.Arcere_Salute + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Soldati.Salute.Cibo * (player.Arcere_Salute + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Soldati.Salute.Legno * (player.Arcere_Salute + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Soldati.Salute.Pietra * (player.Arcere_Salute + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Soldati.Salute.Ferro * (player.Arcere_Salute + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Soldati.Salute.Oro * (player.Arcere_Salute + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Salute.TempoRicerca * (player.Arcere_Salute + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Arcere Attacco|{L.Desc_RicercaArcereAttacco(player.Arcere_Attacco + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Soldati.Attacco.Cibo * (player.Arcere_Attacco + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Soldati.Attacco.Legno * (player.Arcere_Attacco + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Soldati.Attacco.Pietra * (player.Arcere_Attacco + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Soldati.Attacco.Ferro * (player.Arcere_Attacco + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Soldati.Attacco.Oro * (player.Arcere_Attacco + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Attacco.TempoRicerca * (player.Arcere_Attacco + 1))}\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Arcere Difesa|{L.Desc_RicercaArcereDifesa(player.Arcere_Difesa + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Soldati.Difesa.Cibo * (player.Arcere_Difesa + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Soldati.Difesa.Legno * (player.Arcere_Difesa + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Soldati.Difesa.Pietra * (player.Arcere_Difesa + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Soldati.Difesa.Ferro * (player.Arcere_Difesa + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Soldati.Difesa.Oro * (player.Arcere_Difesa + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Difesa.TempoRicerca * (player.Arcere_Difesa + 1))}\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Catapulta Livello|{L.Desc_RicercaCatapultaLivello(player.Catapulta_Livello + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Soldati.Livello.Cibo * (player.Catapulta_Livello + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Soldati.Livello.Legno * (player.Catapulta_Livello + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Soldati.Livello.Pietra * (player.Catapulta_Livello + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Soldati.Livello.Ferro * (player.Catapulta_Livello + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Soldati.Livello.Oro * (player.Catapulta_Livello + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Livello.TempoRicerca * (player.Catapulta_Livello + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Catapulta Salute|{L.Desc_RicercaCatapultaSalute(player.Catapulta_Salute + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Soldati.Salute.Cibo * (player.Catapulta_Salute + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Soldati.Salute.Legno * (player.Catapulta_Salute + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Soldati.Salute.Pietra * (player.Catapulta_Salute + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Soldati.Salute.Ferro * (player.Catapulta_Salute + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Soldati.Salute.Oro * (player.Catapulta_Salute + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Salute.TempoRicerca * (player.Catapulta_Salute + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Catapulta Attacco|{L.Desc_RicercaCatapultaAttacco(player.Catapulta_Attacco + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Soldati.Attacco.Cibo * (player.Catapulta_Attacco + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Soldati.Attacco.Legno * (player.Catapulta_Attacco + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Soldati.Attacco.Pietra * (player.Catapulta_Attacco + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Soldati.Attacco.Ferro * (player.Catapulta_Attacco + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Soldati.Attacco.Oro * (player.Catapulta_Attacco + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Attacco.TempoRicerca * (player.Catapulta_Attacco + 1))}\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Catapulta Difesa|{L.Desc_RicercaCatapultaDifesa(player.Catapulta_Difesa + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Soldati.Difesa.Cibo * (player.Catapulta_Difesa + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Soldati.Difesa.Legno * (player.Catapulta_Difesa + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Soldati.Difesa.Pietra * (player.Catapulta_Difesa + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Soldati.Difesa.Ferro * (player.Catapulta_Difesa + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Soldati.Difesa.Oro * (player.Catapulta_Difesa + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Difesa.TempoRicerca * (player.Catapulta_Difesa + 1))}\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Ingresso Guarnigione|{L.Desc_RicercaIngressoGuarnigione(player.Ricerca_Ingresso_Guarnigione + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Citta.Ingresso.Cibo * (player.Ricerca_Ingresso_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Citta.Ingresso.Legno * (player.Ricerca_Ingresso_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Citta.Ingresso.Pietra * (player.Ricerca_Ingresso_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Citta.Ingresso.Ferro * (player.Ricerca_Ingresso_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Citta.Ingresso.Oro * (player.Ricerca_Ingresso_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Ricerca.Citta.Ingresso.Popolazione * (player.Ricerca_Ingresso_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Citta.Ingresso.TempoRicerca * (player.Ricerca_Ingresso_Guarnigione + 1))}");
            Send(player.guid_Player, $"Descrizione|Ricerca Citta Guarnigione|{L.Desc_RicercaCittaGuarnigione(player.Ricerca_Citta_Guarnigione + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Citta.Città.Cibo * (player.Ricerca_Citta_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Citta.Città.Legno * (player.Ricerca_Citta_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Citta.Città.Pietra * (player.Ricerca_Citta_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Citta.Città.Ferro * (player.Ricerca_Citta_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Citta.Città.Oro * (player.Ricerca_Citta_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Ricerca.Citta.Città.Popolazione * (player.Ricerca_Citta_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Citta.Città.TempoRicerca * (player.Ricerca_Citta_Guarnigione + 1))}");

            Send(player.guid_Player, $"Descrizione|Ricerca Mura Livello|{L.Desc_RicercaMuraLivello(player.Ricerca_Mura_Livello + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Citta.Mura_Livello.Cibo * (player.Ricerca_Mura_Livello + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Citta.Mura_Livello.Legno * (player.Ricerca_Mura_Livello + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Citta.Mura_Livello.Pietra * (player.Ricerca_Mura_Livello + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Citta.Mura_Livello.Ferro * (player.Ricerca_Mura_Livello + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Citta.Mura_Livello.Oro * (player.Ricerca_Mura_Livello + 1):#,0}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Ricerca.Citta.Mura_Livello.Popolazione * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Citta.Mura_Livello.TempoRicerca * (player.Ricerca_Mura_Livello + 1))}");
            Send(player.guid_Player, $"Descrizione|Ricerca Mura Guarnigione|{L.Desc_RicercaMuraGuarnigione(player.Ricerca_Mura_Guarnigione + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Citta.Mura_Guarnigione.Cibo * (player.Ricerca_Mura_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Citta.Mura_Guarnigione.Legno * (player.Ricerca_Mura_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Citta.Mura_Guarnigione.Pietra * (player.Ricerca_Mura_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Citta.Mura_Guarnigione.Ferro * (player.Ricerca_Mura_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Citta.Mura_Guarnigione.Oro * (player.Ricerca_Mura_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Ricerca.Citta.Mura_Guarnigione.Popolazione * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Citta.Mura_Guarnigione.TempoRicerca * (player.Ricerca_Mura_Guarnigione + 1))}");
            Send(player.guid_Player, $"Descrizione|Ricerca Mura Salute|{L.Desc_RicercaMuraSalute(player.Ricerca_Mura_Salute + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Citta.Mura_Salute.Cibo * (player.Ricerca_Mura_Salute + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Citta.Mura_Salute.Legno * (player.Ricerca_Mura_Salute + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Citta.Mura_Salute.Pietra * (player.Ricerca_Mura_Salute + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Citta.Mura_Salute.Ferro * (player.Ricerca_Mura_Salute + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Citta.Mura_Salute.Oro * (player.Ricerca_Mura_Salute + 1):#,0}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Ricerca.Citta.Mura_Salute.Popolazione * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Citta.Mura_Salute.TempoRicerca * (player.Ricerca_Mura_Salute + 1))}\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Mura Difesa|{L.Desc_RicercaMuraDifesa(player.Ricerca_Mura_Difesa + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Citta.Mura_Difesa.Cibo * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Citta.Mura_Difesa.Legno * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Citta.Mura_Difesa.Pietra * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Citta.Mura_Difesa.Ferro * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Citta.Mura_Difesa.Oro * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Ricerca.Citta.Mura_Difesa.Popolazione * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Citta.Mura_Difesa.TempoRicerca * (player.Ricerca_Mura_Difesa + 1))}\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Cancello Livello|{L.Desc_RicercaCancelloLivello(player.Ricerca_Cancello_Guarnigione + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Citta.Cancello_Livello.Cibo * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Citta.Cancello_Livello.Legno * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Citta.Cancello_Livello.Pietra * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Citta.Cancello_Livello.Ferro * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Citta.Cancello_Livello.Oro * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Ricerca.Citta.Cancello_Livello.Popolazione * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Citta.Cancello_Livello.TempoRicerca * (player.Ricerca_Cancello_Guarnigione + 1))}");
            Send(player.guid_Player, $"Descrizione|Ricerca Cancello Guarnigione|{L.Desc_RicercaCancelloGuarnigione(player.Ricerca_Cancello_Guarnigione + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Citta.Cancello_Guarnigione.Cibo * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Citta.Cancello_Guarnigione.Legno * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Citta.Cancello_Guarnigione.Pietra * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Citta.Cancello_Guarnigione.Ferro * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Citta.Cancello_Guarnigione.Oro * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Ricerca.Citta.Cancello_Guarnigione.Popolazione * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Citta.Cancello_Guarnigione.TempoRicerca * (player.Ricerca_Cancello_Guarnigione + 1))}");
            Send(player.guid_Player, $"Descrizione|Ricerca Cancello Salute|{L.Desc_RicercaCancelloSalute(player.Ricerca_Cancello_Salute + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Citta.Cancello_Salute.Cibo * (player.Ricerca_Cancello_Salute + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Citta.Cancello_Salute.Legno * (player.Ricerca_Cancello_Salute + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Citta.Cancello_Salute.Pietra * (player.Ricerca_Cancello_Salute + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Citta.Cancello_Salute.Ferro * (player.Ricerca_Cancello_Salute + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Citta.Cancello_Salute.Oro * (player.Ricerca_Cancello_Salute + 1):#,0}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Ricerca.Citta.Cancello_Salute.Popolazione * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Citta.Cancello_Salute.TempoRicerca * (player.Ricerca_Cancello_Salute + 1))}\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Cancello Difesa|{L.Desc_RicercaCancelloDifesa(player.Ricerca_Cancello_Difesa + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Citta.Cancello_Difesa.Cibo * (player.Ricerca_Cancello_Difesa + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Citta.Cancello_Difesa.Legno * (player.Ricerca_Cancello_Difesa + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Citta.Cancello_Difesa.Pietra * (player.Ricerca_Cancello_Difesa + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Citta.Cancello_Difesa.Ferro * (player.Ricerca_Cancello_Difesa + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Citta.Cancello_Difesa.Oro * (player.Ricerca_Cancello_Difesa + 1):#,0}\n" +
                $"{L.Label_Popolazione()}: [icon:popolazione]{Ricerca.Citta.Cancello_Difesa.Popolazione * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Citta.Cancello_Difesa.TempoRicerca * (player.Ricerca_Cancello_Difesa + 1))}\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Torri Livello|{L.Desc_RicercaTorriLivello(player.Ricerca_Torri_Guarnigione + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Citta.Torri_Livello.Cibo * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Citta.Torri_Livello.Legno * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Citta.Torri_Livello.Pietra * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Citta.Torri_Livello.Ferro * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Citta.Torri_Livello.Oro * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Citta.Torri_Livello.TempoRicerca * (player.Ricerca_Torri_Guarnigione + 1))}");
            Send(player.guid_Player, $"Descrizione|Ricerca Torri Guarnigione|{L.Desc_RicercaTorriGuarnigione(player.Ricerca_Torri_Guarnigione + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Citta.Torri_Guarnigione.Cibo * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Citta.Torri_Guarnigione.Legno * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Citta.Torri_Guarnigione.Pietra * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Citta.Torri_Guarnigione.Ferro * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Citta.Torri_Guarnigione.Oro * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Citta.Torri_Guarnigione.TempoRicerca * (player.Ricerca_Torri_Guarnigione + 1))}");
            Send(player.guid_Player, $"Descrizione|Ricerca Torri Salute|{L.Desc_RicercaTorriSalute(player.Ricerca_Torri_Salute + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Citta.Torri_Salute.Cibo * (player.Ricerca_Torri_Salute + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Citta.Torri_Salute.Legno * (player.Ricerca_Torri_Salute + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Citta.Torri_Salute.Pietra * (player.Ricerca_Torri_Salute + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Citta.Torri_Salute.Ferro * (player.Ricerca_Torri_Salute + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Citta.Torri_Salute.Oro * (player.Ricerca_Torri_Salute + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Citta.Torri_Salute.TempoRicerca * (player.Ricerca_Torri_Salute + 1))}\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Torri Difesa|{L.Desc_RicercaTorriDifesa(player.Ricerca_Torri_Difesa + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Citta.Torri_Difesa.Cibo * (player.Ricerca_Torri_Difesa + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Citta.Torri_Difesa.Legno * (player.Ricerca_Torri_Difesa + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Citta.Torri_Difesa.Pietra * (player.Ricerca_Torri_Difesa + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Citta.Torri_Difesa.Ferro * (player.Ricerca_Torri_Difesa + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Citta.Torri_Difesa.Oro * (player.Ricerca_Torri_Difesa + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Citta.Torri_Difesa.TempoRicerca * (player.Ricerca_Torri_Difesa + 1))}\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Castello Livello|{L.Desc_RicercaCastelloLivello(player.Ricerca_Castello_Guarnigione + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Citta.Castello_Livello.Cibo * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Citta.Castello_Livello.Legno * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Citta.Castello_Livello.Pietra * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Citta.Castello_Livello.Ferro * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Citta.Castello_Livello.Oro * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Citta.Castello_Livello.TempoRicerca * (player.Ricerca_Castello_Guarnigione + 1))}");
            Send(player.guid_Player, $"Descrizione|Ricerca Castello Guarnigione|{L.Desc_RicercaCastelloGuarnigione(player.Ricerca_Castello_Guarnigione + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Citta.Castello_Guarnigione.Cibo * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Citta.Castello_Guarnigione.Legno * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Citta.Castello_Guarnigione.Pietra * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Citta.Castello_Guarnigione.Ferro * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Citta.Castello_Guarnigione.Oro * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Citta.Castello_Guarnigione.TempoRicerca * (player.Ricerca_Castello_Guarnigione + 1))}");
            Send(player.guid_Player, $"Descrizione|Ricerca Castello Salute|{L.Desc_RicercaCastelloSalute(player.Ricerca_Castello_Salute + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Citta.Castello_Salute.Cibo * (player.Ricerca_Castello_Salute + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Citta.Castello_Salute.Legno * (player.Ricerca_Castello_Salute + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Citta.Castello_Salute.Pietra * (player.Ricerca_Castello_Salute + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Citta.Castello_Salute.Ferro * (player.Ricerca_Castello_Salute + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Citta.Castello_Salute.Oro * (player.Ricerca_Castello_Salute + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Citta.Castello_Salute.TempoRicerca * (player.Ricerca_Castello_Salute + 1))}\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Castello Difesa|{L.Desc_RicercaCastelloDifesa(player.Ricerca_Castello_Difesa + 1)}" +
                $"{L.Label_Cibo()} [icon:cibo]{Ricerca.Citta.Castello_Difesa.Cibo * (player.Ricerca_Castello_Difesa + 1):#,0}\n" +
                $"{L.Label_Legno()}: [icon:legno]{Ricerca.Citta.Castello_Difesa.Legno * (player.Ricerca_Castello_Difesa + 1):#,0}\n" +
                $"{L.Label_Pietra()}: [icon:pietra]{Ricerca.Citta.Castello_Difesa.Pietra * (player.Ricerca_Castello_Difesa + 1):#,0}\n" +
                $"{L.Label_Ferro()}: [icon:ferro]{Ricerca.Citta.Castello_Difesa.Ferro * (player.Ricerca_Castello_Difesa + 1):#,0}\n" +
                $"{L.Label_Oro()}: [icon:oro]{Ricerca.Citta.Castello_Difesa.Oro * (player.Ricerca_Castello_Difesa + 1):#,0}\n" +
                $"{L.Label_Ricerca()}: [icon:tempo]{player.FormatTime(Ricerca.Citta.Castello_Difesa.TempoRicerca * (player.Ricerca_Castello_Difesa + 1))}\n");

            //Shop Descrizioni
            Send(player.guid_Player, $"Descrizione|Shop GamePass Base|{L.Desc_Shop_GamePassBase()}");
            Send(player.guid_Player, $"Descrizione|Shop GamePass Avanzato|{L.Desc_Shop_GamePassAvanzato()}");
            Send(player.guid_Player, $"Descrizione|Shop Vip 1|{L.Desc_Shop_Vip1()}");
            Send(player.guid_Player, $"Descrizione|Shop Vip 2|{L.Desc_Shop_Vip2()}");
            Send(player.guid_Player, $"Descrizione|Shop Costruttore 24h|{L.Desc_Shop_Costruttore(player.FormatTime(Variabili_Server.Shop.Costruttore_48h.Reward))}");
            Send(player.guid_Player, $"Descrizione|Shop Costruttore 48h|{L.Desc_Shop_Costruttore(player.FormatTime(Variabili_Server.Shop.Costruttore_48h.Reward))}");
            Send(player.guid_Player, $"Descrizione|Shop Reclutatore 24h|{L.Desc_Shop_Reclutatore(player.FormatTime(Variabili_Server.Shop.Reclutatore_24h.Reward))}");
            Send(player.guid_Player, $"Descrizione|Shop Reclutatore 48h|{L.Desc_Shop_Reclutatore(player.FormatTime(Variabili_Server.Shop.Reclutatore_48h.Reward))}");
            Send(player.guid_Player, $"Descrizione|Shop Scudo Pace 8h|{L.Desc_Shop_ScudoPace(player.FormatTime(Variabili_Server.Shop.Scudo_Pace_8h.Reward))}");
            Send(player.guid_Player, $"Descrizione|Shop Scudo Pace 24h|{L.Desc_Shop_ScudoPace(player.FormatTime(Variabili_Server.Shop.Scudo_Pace_24h.Reward))}");
            Send(player.guid_Player, $"Descrizione|Shop Scudo Pace 72h|{L.Desc_Shop_ScudoPace(player.FormatTime(Variabili_Server.Shop.Scudo_Pace_72h.Reward))}");

            Send(player.guid_Player, $"Descrizione|Feudi Info|{L.Desc_Feudi_Testo()}");
        }
    }
}
