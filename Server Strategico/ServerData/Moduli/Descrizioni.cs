using Server_Strategico.Gioco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server_Strategico.Gioco.Giocatori;
using static Server_Strategico.Server.Server;

namespace Server_Strategico.ServerData.Moduli
{
    internal class Descrizioni
    {
        public static async void DescUpdate(Player player)
        {
            double tempoBase = 0, tempoCalcolato = 0;
            tempoCalcolato = Strutture.Edifici.Fattoria.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.Fattoria.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.Fattoria.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Fattoria|[black]" +
                $"La fattoria è la struttura principale per la produzione di [icon:cibo]Cibo, fondamentale per la costruzione di edifici militari e civili, " +
                $"l'addestramento delle unità militari ed il loro mantenimento. Indispensabile per la ricerca tecnologica e di componenti militari\n\n" +
                $"Costo Costruzione:\n" +
                $"Cibo: [icon:cibo]{Strutture.Edifici.Fattoria.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Strutture.Edifici.Fattoria.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Strutture.Edifici.Fattoria.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Strutture.Edifici.Fattoria.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Strutture.Edifici.Fattoria.Oro.ToString("#,0")}\n" +
                $"Popolazione: [icon:popolazione]{Strutture.Edifici.Fattoria.Popolazione.ToString("#,0")}\n" +
                $"Costruzione: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Produzione risorse: [icon:cibo]{(Strutture.Edifici.Fattoria.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Cibo * (1 + player.Bonus_Produzione_Risorse)).ToString("0.00")} s\n" +
                $"Limite stoccaggio: [icon:cibo][ferroScuro]{Strutture.Edifici.Fattoria.Limite.ToString()}");

            tempoCalcolato = Strutture.Edifici.Segheria.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.Segheria.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.Segheria.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Segheria|[black]" +
               $"La Segheria è la struttura principale per la produzione di [icon:legno]Legna, fondamentale per la costruzione di strutture militari, civili e " +
               $"l'addestramento delle unità militari. Indispensabile per la ricerca tecnologica e di componenti militari\n\n" +
               $"Costo Costruzione:\n" +
               $"Cibo: [icon:cibo]{Strutture.Edifici.Segheria.Cibo.ToString("#,0")}\n" +
               $"Legno: [icon:legno]{Strutture.Edifici.Segheria.Legno.ToString("#,0")}\n" +
               $"Pietra: [icon:pietra]{Strutture.Edifici.Segheria.Pietra.ToString("#,0")}\n" +
               $"Ferro: [icon:ferro]{Strutture.Edifici.Segheria.Ferro.ToString("#,0")}\n" +
               $"Oro: [icon:oro]{Strutture.Edifici.Segheria.Oro.ToString("#,0")}\n" +
               $"Popolazione: [icon:popolazione]{Strutture.Edifici.Segheria.Popolazione.ToString("#,0")}\n" +
               $"Costruzione: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
               $"Produzione risorse: [icon:legno]{(Strutture.Edifici.Segheria.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Legno * (1 + player.Bonus_Produzione_Risorse)).ToString("0.00")} s\n" +
               $"Limite stoccaggio: [icon:legno][ferroScuro]{Strutture.Edifici.Segheria.Limite.ToString()}");

            tempoCalcolato = Strutture.Edifici.CavaPietra.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.CavaPietra.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.CavaPietra.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Cava di Pietra|[black]" +
                $"La cava di pietra è la struttura principale per la produzione di [icon:pietra]Pietra, fondamentale per la costruzione di strutture militari, cilivi e " +
                $"l'addestramento delle unità militari. Indispensabile per la ricerca tecnologica e di componenti militari\n\n" +
                $"Costo Costruzione:\n" +
                $"Cibo: [icon:cibo]{Strutture.Edifici.CavaPietra.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Strutture.Edifici.CavaPietra.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Strutture.Edifici.CavaPietra.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Strutture.Edifici.CavaPietra.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Strutture.Edifici.CavaPietra.Oro.ToString("#,0")}\n" +
                $"Popolazione: [icon:popolazione]{Strutture.Edifici.CavaPietra.Popolazione.ToString("#,0")}\n" +
                $"Costruzione: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Produzione risorse: [icon:pietra]{(Strutture.Edifici.CavaPietra.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Pietra * (1 + player.Bonus_Produzione_Risorse)).ToString("0.00")} s\n" +
                $"Limite stoccaggio: [icon:pietra][ferroScuro]{Strutture.Edifici.CavaPietra.Limite.ToString()}\n");

            tempoCalcolato = Strutture.Edifici.MinieraFerro.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.MinieraFerro.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.MinieraFerro.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Miniera di Ferro|[black]" +
                $"La Miniera di ferro è la struttura principale per la produzione di [icon:ferro]Ferro, fondamentale per la costruzione di strutture militari, cilivi e " +
                $"l'addestramento delle unità militari. Indispensabile per la ricerca tecnologica e di componenti militari\n\n" +
                $"Costo Costruzione:\n" +
                $"Cibo: [icon:cibo]{Strutture.Edifici.MinieraFerro.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Strutture.Edifici.MinieraFerro.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Strutture.Edifici.MinieraFerro.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Strutture.Edifici.MinieraFerro.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Strutture.Edifici.MinieraFerro.Oro.ToString("#,0")}\n" +
                $"Popolazione: [icon:popolazione]{Strutture.Edifici.MinieraFerro.Popolazione.ToString("#,0")}\n" +
                $"Costruzione: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Produzione risorse: [icon:ferro]{(Strutture.Edifici.MinieraFerro.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Ferro * (1 + player.Bonus_Produzione_Risorse)).ToString("0.00")} s\n" +
                $"Limite stoccaggio: [icon:ferro][ferroScuro]{Strutture.Edifici.MinieraFerro.Limite.ToString()}");

            tempoCalcolato = Strutture.Edifici.MinieraOro.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.MinieraOro.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.MinieraOro.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Miniera d'Oro|[black]" +
                $"La miniera d'oro è la struttura principale per la produzione di [icon:oro]oro, fondamentale per la costruzione di strutture militari, civili e " +
                $"l'addestramento delle unità militari. Indispensabile per la ricerca tecnologica e di componenti militari\n\n" +
                $"Costo Costruzione:\n" +
                $"Cibo: [icon:cibo]{Strutture.Edifici.MinieraOro.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Strutture.Edifici.MinieraOro.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Strutture.Edifici.MinieraOro.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Strutture.Edifici.MinieraOro.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Strutture.Edifici.MinieraOro.Oro.ToString("#,0")}\n" +
                $"Popolazione: [icon:popolazione]{Strutture.Edifici.MinieraOro.Popolazione.ToString("#,0")}\n" +
                $"Costruzione: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Produzione risorse: [icon:oro]{(Strutture.Edifici.MinieraOro.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Oro * (1 + player.Bonus_Produzione_Risorse)).ToString("0.00")} s\n" +
                $"Limite stoccaggio: [icon:oro][ferroScuro]{Strutture.Edifici.MinieraOro.Limite.ToString()}\n");

            tempoCalcolato = Strutture.Edifici.Case.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.Case.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.Case.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Case|[black]" +
                $"Le Case sono necessarie per attirare sempre più [icon:popolazione]cittadini, verso il vostro villaggio, " +
                $"sono fondamentali per la costruzione di strutture militari e civili, oltre che per addestrare le unità militari.\n\n" +
                $"Costo Costruzione:\n" +
                $"Cibo: [icon:cibo]{Strutture.Edifici.Case.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Strutture.Edifici.Case.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Strutture.Edifici.Case.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Strutture.Edifici.Case.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Strutture.Edifici.Case.Oro.ToString("#,0")}\n" +
                $"Costruzione: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Produzione risorse: [icon:popolazione]{(Strutture.Edifici.Case.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Popolazione).ToString("0.0000")} s\n" +
                $"Limite abitanti: [icon:popolazione][ferroScuro]{Strutture.Edifici.Case.Limite.ToString()}\n");

            tempoCalcolato = Strutture.Edifici.ProduzioneSpade.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.ProduzioneSpade.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.ProduzioneSpade.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Produzione Spade|[black]" +
                $"Workshop Spade questa struttura produce equipaggiamento militare specifico, " +
                $"essenziali per l'addestramento di unità militari, questa struttura produce [icon:spade]Spade.\n\n" +
                $"Costo Costruzione:\n" +
                $"Cibo: [icon:cibo]{Strutture.Edifici.ProduzioneSpade.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Strutture.Edifici.ProduzioneSpade.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Strutture.Edifici.ProduzioneSpade.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Strutture.Edifici.ProduzioneSpade.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Strutture.Edifici.ProduzioneSpade.Oro.ToString("#,0")}\n" +
                $"Popolazione: [icon:popolazione]{Strutture.Edifici.ProduzioneSpade.Popolazione.ToString("#,0")}\n" +
                $"Costruzione: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Produzione risorse: [icon:spade]{(Strutture.Edifici.ProduzioneSpade.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Spade).ToString("0.00")} s\n" +
                $"Limite spade [icon:spade][ferroScuro]{Strutture.Edifici.ProduzioneSpade.Limite.ToString()}[black]\n" +
                $"Consumo legno: [icon:legno][rosso]-{Strutture.Edifici.ProduzioneSpade.Consumo_Legno}[black] s\n" +
                $"Consumo ferro: [icon:ferro][rosso]-{Strutture.Edifici.ProduzioneSpade.Consumo_Ferro}[black] s\n" +
                $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.ProduzioneSpade.Consumo_Oro}[black] s\n");

            tempoCalcolato = Strutture.Edifici.ProduzioneLance.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.ProduzioneLance.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.ProduzioneLance.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Produzione Lance|[black]" +
                $"Workshop Lancie questa struttura produce equipaggiamento militare specifico, " +
                $"essenziali per l'addestramento di unità militari, questa struttura produce [icon:lance]Lancie.\n\n" +
                $"Costo Costruzione:\n" +
                $"Cibo: [icon:cibo]{Strutture.Edifici.ProduzioneLance.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Strutture.Edifici.ProduzioneLance.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Strutture.Edifici.ProduzioneLance.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Strutture.Edifici.ProduzioneLance.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Strutture.Edifici.ProduzioneLance.Oro.ToString("#,0")}\n" +
                $"Popolazione: [icon:popolazione]{Strutture.Edifici.ProduzioneLance.Popolazione.ToString("#,0")}\n" +
                $"Costruzione: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Produzione risorse: [icon:lance]{(Strutture.Edifici.ProduzioneLance.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Lance).ToString("0.00")} s\n" +
                $"Limite lancie [icon:lance][ferroScuro]{Strutture.Edifici.ProduzioneLance.Limite.ToString()}[black]\n" +
                $"Consumo legno: [icon:legno][rosso]-{Strutture.Edifici.ProduzioneLance.Consumo_Legno}[black] s\n" +
                $"Consumo ferro: [icon:ferro][rosso]-{Strutture.Edifici.ProduzioneLance.Consumo_Ferro}[black] s\n" +
                $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.ProduzioneLance.Consumo_Oro}[black] s\n");

            tempoCalcolato = Strutture.Edifici.ProduzioneArchi.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.ProduzioneArchi.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.ProduzioneArchi.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Produzione Archi|[black]" +
                $"Workshop Archi questa struttura produce equipaggiamento militare specifico, " +
                $"essenziali per l'addestramento di unità militari, questa struttura produce [icon:archi]Archi.\n\n" +
                $"Costo Costruzione:\n" +
                $"Cibo: [icon:cibo]{Strutture.Edifici.ProduzioneArchi.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Strutture.Edifici.ProduzioneArchi.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Strutture.Edifici.ProduzioneArchi.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Strutture.Edifici.ProduzioneArchi.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Strutture.Edifici.ProduzioneArchi.Oro.ToString("#,0")}\n" +
                $"Popolazione: [icon:archi]{Strutture.Edifici.ProduzioneArchi.Popolazione.ToString("#,0")}\n" +
                $"Costruzione: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Produzione risorse: [icon:archi]{(Strutture.Edifici.ProduzioneArchi.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Archi).ToString("0.00")} s\n" +
                $"Limite archi [icon:archi][ferroScuro]{Strutture.Edifici.ProduzioneArchi.Limite.ToString()}[black]\n" +
                $"Consumo legno: [icon:legno][rosso]-{Strutture.Edifici.ProduzioneArchi.Consumo_Legno}[black] s\n" +
                $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.ProduzioneArchi.Consumo_Oro}[black] s\n");

            tempoCalcolato = Strutture.Edifici.ProduzioneScudi.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.ProduzioneScudi.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.ProduzioneScudi.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Produzione Scudi|[black]" +
                $"Workshop Scudi questa struttura produce equipaggiamento militare specifico, " +
                $"essenziali per l'addestramento di unità militari, questa struttura produce [icon:scudi]Scudi.\n\n" +
                $"Costo Costruzione:\n" +
                $"Cibo: [icon:cibo]{Strutture.Edifici.ProduzioneScudi.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Strutture.Edifici.ProduzioneScudi.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Strutture.Edifici.ProduzioneScudi.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Strutture.Edifici.ProduzioneScudi.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Strutture.Edifici.ProduzioneScudi.Oro.ToString("#,0")}\n" +
                $"Popolazione: [icon:popolazione]{Strutture.Edifici.ProduzioneScudi.Popolazione.ToString("#,0")}\n" +
                $"Costruzione: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Produzione risorse: [icon:scudi]{(Strutture.Edifici.ProduzioneScudi.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Scudi).ToString("0.00")} s\n" +
                $"Limite scudi [icon:scudi][ferroScuro]{Strutture.Edifici.ProduzioneScudi.Limite.ToString()}[black]\n" +
                $"Consumo legno: [icon:legno][rosso]-{Strutture.Edifici.ProduzioneScudi.Consumo_Legno}[black] s\n" +
                $"Consumo ferro: [icon:ferro][rosso]-{Strutture.Edifici.ProduzioneScudi.Consumo_Ferro}[black] s\n" +
                $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.ProduzioneScudi.Consumo_Oro}[black] s\n");

            tempoCalcolato = Strutture.Edifici.ProduzioneArmature.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.ProduzioneArmature.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.ProduzioneArmature.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Produzione Armature|[black]" +
                $"Workshop Armature questa struttura produce equipaggiamento militare specifico, " +
                $"essenziali per l'addestramento di unità militari, questa struttura produce [icon:armature]Armature.\n\n" +
                $"Costo Costruzione:\n" +
                $"Cibo: [icon:cibo]{Strutture.Edifici.ProduzioneArmature.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Strutture.Edifici.ProduzioneArmature.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Strutture.Edifici.ProduzioneArmature.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Strutture.Edifici.ProduzioneArmature.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Strutture.Edifici.ProduzioneArmature.Oro.ToString("#,0")}\n" +
                $"Popolazione: [icon:popolazione]{Strutture.Edifici.ProduzioneArmature.Popolazione.ToString("#,0")}\n" +
                $"Costruzione: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Produzione risorse: [icon:armature]{(Strutture.Edifici.ProduzioneArmature.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Armature).ToString("0.00")} s\n" +
                $"Limite armature: [icon:armature][ferroScuro]{Strutture.Edifici.ProduzioneArmature.Limite.ToString()}[black]\n" +
                $"Consumo ferro: [icon:ferro][rosso]-{Strutture.Edifici.ProduzioneArmature.Consumo_Ferro}[black] s\n" +
                $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.ProduzioneArmature.Consumo_Oro}[black] s");

            tempoCalcolato = Strutture.Edifici.ProduzioneFrecce.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.ProduzioneFrecce.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.ProduzioneFrecce.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Produzione Frecce|[black]" +
                $"Workshop Frecce questa struttura produce equipaggiamento militare specifico, " +
                $"essenziali per l'addestramento di unità militari, questa struttura produce [icon:frecce]Frecce.\n\n" +
                $"Costo Costruzione:\n" +
                $"Cibo: [icon:cibo]{Strutture.Edifici.ProduzioneFrecce.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Strutture.Edifici.ProduzioneFrecce.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Strutture.Edifici.ProduzioneFrecce.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Strutture.Edifici.ProduzioneFrecce.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Strutture.Edifici.ProduzioneFrecce.Oro.ToString("#,0")}\n" +
                $"Popolazione: [icon:popolazione]{Strutture.Edifici.ProduzioneFrecce.Popolazione.ToString("#,0")}\n" +
                $"Costruzione: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Produzione risorse: [icon:frecce]{(Strutture.Edifici.ProduzioneFrecce.Produzione + player.Ricerca_Produzione * Ricerca.Tipi.Incremento.Oro).ToString("0.00")} s\n" +
                $"Limite frecce [icon:frecce][ferroScuro]{Strutture.Edifici.ProduzioneFrecce.Limite.ToString()}[black]\n" +
                $"Consumo legno: [icon:legno][rosso]-{Strutture.Edifici.ProduzioneFrecce.Consumo_Legno.ToString()}[black] s\n" +
                $"Consumo pietra: [icon:pietra][rosso]-{Strutture.Edifici.ProduzioneFrecce.Consumo_Pietra.ToString()}[black] s\n" +
                $"Consumo ferro: [icon:ferro][rosso]-{Strutture.Edifici.ProduzioneFrecce.Consumo_Ferro.ToString()}[black] s\n" +
                $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.ProduzioneFrecce.Consumo_Ferro.ToString()}[black] s\n");

            //Caserme
            tempoCalcolato = Strutture.Edifici.CasermaGuerrieri.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.CasermaGuerrieri.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.CasermaGuerrieri.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Caserma Guerrieri|[black]" +
                 $"Caserma guerrieri questa struttura militare di fondamentale presenza per ogni villaggio, permette l'addestramento ed il mantenimento di unità militari specifiche.\n\n" +
                 $"Ogni caserma è attrezzata per ospitare un certo numero di uomini, raccomandati di averne un numero sufficiente.\n\n" +
                 $"Costo Costruzione:\n" +
                 $"Cibo: [icon:cibo]{Strutture.Edifici.CasermaGuerrieri.Cibo.ToString("#,0")}\n" +
                 $"Legno: [icon:legno]{Strutture.Edifici.CasermaGuerrieri.Legno.ToString("#,0")}\n" +
                 $"Pietra: [icon:pietra]{Strutture.Edifici.CasermaGuerrieri.Pietra.ToString("#,0")}\n" +
                 $"Ferro: [icon:ferro]{Strutture.Edifici.CasermaGuerrieri.Ferro.ToString("#,0")}\n" +
                 $"Oro: [icon:oro]{Strutture.Edifici.CasermaGuerrieri.Oro.ToString("#,0")}\n" +
                 $"Popolazione: [icon:popolazione]{Strutture.Edifici.CasermaGuerrieri.Popolazione.ToString("#,0")}\n" +
                 $"Costruzione: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                 $"Limite Guerrieri: [icon:guerrieri]{Strutture.Edifici.CasermaGuerrieri.Limite.ToString("#,0")}\n" +
                 $"Consumo cibo: [icon:cibo][rosso]-{Strutture.Edifici.CasermaGuerrieri.Consumo_Cibo}[black] s\n" +
                 $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.CasermaGuerrieri.Consumo_Oro}[black] s\n\n");

            tempoCalcolato = Strutture.Edifici.CasermaLanceri.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.CasermaLanceri.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.CasermaLanceri.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Caserma Lanceri|[black]" +
                 $"Caserma lancieri questa struttura militare di fondamentale presenza per ogni villaggio, permette l'addestramento ed il mantenimento di unità militari specifiche.\n\n" +
                 $"Ogni caserma è attrezzata per ospitare un certo numero di uomini, raccomandati di averne un numero sufficiente.\n\n" +
                 $"Costo Costruzione:\n" +
                 $"Cibo: [icon:cibo]{Strutture.Edifici.CasermaLanceri.Cibo.ToString("#,0")}\n" +
                 $"Legno: [icon:legno]{Strutture.Edifici.CasermaLanceri.Legno.ToString("#,0")}\n" +
                 $"Pietra: [icon:pietra]{Strutture.Edifici.CasermaLanceri.Pietra.ToString("#,0")}\n" +
                 $"Ferro: [icon:ferro]{Strutture.Edifici.CasermaLanceri.Ferro.ToString("#,0")}\n" +
                 $"Oro: [icon:oro]{Strutture.Edifici.CasermaLanceri.Oro.ToString("#,0")}\n" +
                 $"Popolazione: [icon:popolazione]{Strutture.Edifici.CasermaLanceri.Popolazione.ToString("#,0")}\n" +
                 $"Costruzione: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                 $"Limite Lancieri: [icon:lanceri]{Strutture.Edifici.CasermaLanceri.Limite.ToString("#,0")}\n" +
                 $"Consumo cibo: [icon:cibo][rosso]-{Strutture.Edifici.CasermaLanceri.Consumo_Cibo}[black] s\n" +
                 $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.CasermaLanceri.Consumo_Oro}[black] s\n\n");

            tempoCalcolato = Strutture.Edifici.CasermaArceri.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.CasermaArceri.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.CasermaArceri.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Caserma Arceri|[black]" +
                 $"Caserma arcieri questa struttura militare di fondamentale presenza per ogni villaggio permette l'addestramento ed il mantenimento di unità militari specifiche.\n\n" +
                 $"Ogni caserma è attrezzata per ospitare un certo numero di uomini, raccomandati di averne un numero sufficiente.\n\n" +
                 $"Costo Costruzione:\n" +
                 $"Cibo: [icon:cibo]{Strutture.Edifici.CasermaArceri.Cibo.ToString("#,0")}\n" +
                 $"Legno: [icon:legno]{Strutture.Edifici.CasermaArceri.Legno.ToString("#,0")}\n" +
                 $"Pietra: [icon:pietra]{Strutture.Edifici.CasermaArceri.Pietra.ToString("#,0")}\n" +
                 $"Ferro: [icon:ferro]{Strutture.Edifici.CasermaArceri.Ferro.ToString("#,0")}\n" +
                 $"Oro: [icon:oro]{Strutture.Edifici.CasermaArceri.Oro.ToString("#,0")}\n" +
                 $"Popolazione: [icon:popolazione]{Strutture.Edifici.CasermaArceri.Popolazione.ToString("#,0")}\n" +
                 $"Costruzione: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                 $"Limite Arceri: [icon:arceri]{Strutture.Edifici.CasermaArceri.Limite.ToString("#,0")}\n" +
                 $"Consumo cibo: [icon:cibo][rosso]-{Strutture.Edifici.CasermaArceri.Consumo_Cibo}[black] s\n" +
                 $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.CasermaArceri.Consumo_Oro}[black] s\n\n");

            tempoCalcolato = Strutture.Edifici.CasermaCatapulte.TempoCostruzione - player.Ricerca_Costruzione - (Strutture.Edifici.CasermaCatapulte.TempoCostruzione * player.Bonus_Costruzione);
            tempoBase = Strutture.Edifici.CasermaCatapulte.TempoCostruzione;
            Send(player.guid_Player, $"Descrizione|Caserma Catapulte|[black]" +
                 $"Caserma catapulte questa struttura militare di fondamentale presenza per ogni villaggio, permette l'addestramento ed il mantenimento di unità militari specifiche.\n\n" +
                 $"Ogni caserma è attrezzata per ospitare un certo numero di uomini, raccomandati di averne un numero sufficiente.\n\n" +
                 $"Costo Costruzione:\n" +
                 $"Cibo: [icon:cibo]{Strutture.Edifici.CasermaCatapulte.Cibo.ToString("#,0")}\n" +
                 $"Legno: [icon:legno]{Strutture.Edifici.CasermaCatapulte.Legno.ToString("#,0")}\n" +
                 $"Pietra: [icon:pietra]{Strutture.Edifici.CasermaCatapulte.Pietra.ToString("#,0")}\n" +
                 $"Ferro: [icon:ferro]{Strutture.Edifici.CasermaCatapulte.Ferro.ToString("#,0")}\n" +
                 $"Oro: [icon:oro]{Strutture.Edifici.CasermaCatapulte.Oro.ToString("#,0")}\n" +
                 $"Popolazione: [icon:popolazione]{Strutture.Edifici.CasermaCatapulte.Popolazione.ToString("#,0")}\n" +
                 $"Costruzione: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                 $"Limite Catapulte: [icon:catapulte]{Strutture.Edifici.CasermaCatapulte.Limite.ToString("#,0")}\n" +
                 $"Consumo cibo: [icon:cibo][rosso]-{Strutture.Edifici.CasermaCatapulte.Consumo_Cibo}[black] s\n" +
                 $"Consumo oro: [icon:oro][rosso]-{Strutture.Edifici.CasermaCatapulte.Consumo_Oro}[black] s\n\n");

            //Esercito
            tempoCalcolato = Esercito.CostoReclutamento.Guerriero_1.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Guerrieri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Guerriero_1.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Guerrieri 1|[black]" +
                $"I guerrieri I sono la spina dorsale dell'esercito, anche se sprovvisti di scudo sono, " +
                $"sono facili da reclutare e non sono molto dispendiosi in cibo ed oro.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Guerriero_1.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Guerriero_1.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Guerriero_1.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Guerriero_1.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Guerriero_1.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Guerriero_1.Spade.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Guerriero_1.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Guerriero_1.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento cibo: [icon:cibo][rosso]-{Esercito.Unità.Guerriero_1.Cibo.ToString()}[black] s\n" +
                $"Mantenimento oro: [icon:oro][rosso]-{Esercito.Unità.Guerriero_1.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Guerriero_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Guerriero_1.Salute.ToString("#,0")}[black]{(player.Guerriero_Salute == 0 ? "" : $" [black]+ [verde]{player.Guerriero_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Guerriero_1.Difesa.ToString("#,0")}[black]{(player.Guerriero_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Guerriero_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Guerriero_1.Attacco.ToString("#,0")}[black]{(player.Guerriero_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Guerriero_Attacco}[black]")}\n");

            tempoCalcolato = Esercito.CostoReclutamento.Guerriero_2.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Guerrieri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Guerriero_2.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Guerrieri 2|[black]" +
                $"I guerrieri II sono la spina dorsale dell'esercito, anche se sprovvisti di scudo sono, " +
                $"sono facili da reclutare e non sono molto dispendiosi in cibo ed oro.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Guerriero_2.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Guerriero_2.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Guerriero_2.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Guerriero_2.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Guerriero_2.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Guerriero_2.Spade.ToString("#,0")}\n" +
                $"Lancie: [icon:lance]{Esercito.CostoReclutamento.Guerriero_2.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Guerriero_2.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Guerriero_2.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Guerriero_2.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Guerriero_2.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento cibo: [icon:cibo][rosso]-{Esercito.Unità.Guerriero_2.Cibo.ToString()}[black] s\n" +
                $"Mantenimento oro: [icon:oro][rosso]-{Esercito.Unità.Guerriero_2.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Guerriero_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Guerriero_2.Salute.ToString("#,0")}[black]{(player.Guerriero_Salute == 0 ? "" : $" [black]+ [verde]{player.Guerriero_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Guerriero_2.Difesa.ToString("#,0")}[black]{(player.Guerriero_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Guerriero_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Guerriero_2.Attacco.ToString("#,0")}[black]{(player.Guerriero_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Guerriero_Attacco}[black]")}\n");

            tempoCalcolato = Esercito.CostoReclutamento.Guerriero_3.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Guerrieri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Guerriero_3.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Guerrieri 3|[black]" +
                $"I guerrieri III sono la spina dorsale dell'esercito, anche se sprovvisti di scudo sono, " +
                $"sono facili da reclutare e non sono molto dispendiosi in cibo ed oro.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Guerriero_3.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Guerriero_3.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Guerriero_3.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Guerriero_3.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Guerriero_3.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Guerriero_3.Spade.ToString("#,0")}\n" +
                $"Lancie: [icon:lance]{Esercito.CostoReclutamento.Guerriero_3.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Guerriero_3.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Guerriero_3.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Guerriero_3.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Guerriero_3.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento cibo: [icon:cibo][rosso]-{Esercito.Unità.Guerriero_3.Cibo.ToString()}[black] s\n" +
                $"Mantenimento oro: [icon:oro][rosso]-{Esercito.Unità.Guerriero_3.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Guerriero_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Guerriero_3.Salute.ToString("#,0")}[black]{(player.Guerriero_Salute == 0 ? "" : $" [black]+ [verde]{player.Guerriero_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Guerriero_3.Difesa.ToString("#,0")}[black]{(player.Guerriero_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Guerriero_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Guerriero_3.Attacco.ToString("#,0")}[black]{(player.Guerriero_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Guerriero_Attacco}[black]")}\n");

            tempoCalcolato = Esercito.CostoReclutamento.Guerriero_4.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Guerrieri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Guerriero_4.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Guerrieri 4|[black]" +
                $"I guerrieri IV sono la spina dorsale dell'esercito, anche se sprovvisti di scudo sono, " +
                $"sono facili da reclutare e non sono molto dispendiosi in cibo ed oro.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Guerriero_4.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Guerriero_4.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Guerriero_4.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Guerriero_4.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Guerriero_4.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Guerriero_4.Spade.ToString("#,0")}\n" +
                $"Lancie: [icon:lance]{Esercito.CostoReclutamento.Guerriero_4.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Guerriero_4.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Guerriero_4.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Guerriero_4.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Guerriero_4.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento cibo: [icon:cibo][rosso]-{Esercito.Unità.Guerriero_4.Cibo.ToString()}[black] s\n" +
                $"Mantenimento oro: [icon:oro][rosso]-{Esercito.Unità.Guerriero_4.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Guerriero_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Guerriero_4.Salute.ToString("#,0")}[black]{(player.Guerriero_Salute == 0 ? "" : $" [black]+ [verde]{player.Guerriero_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Guerriero_4.Difesa.ToString("#,0")}[black]{(player.Guerriero_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Guerriero_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Guerriero_4.Attacco.ToString("#,0")}[black]{(player.Guerriero_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Guerriero_Attacco}[black]")}\n");

            tempoCalcolato = Esercito.CostoReclutamento.Guerriero_5.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Guerrieri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Guerriero_5.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Guerrieri 5|[black]" +
                $"I guerrieri V sono la spina dorsale dell'esercito, anche se sprovvisti di scudo sono, " +
                $"sono facili da reclutare e non sono molto dispendiosi in cibo ed oro.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Guerriero_5.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Guerriero_5.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Guerriero_5.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Guerriero_5.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Guerriero_5.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Guerriero_5.Spade.ToString("#,0")}\n" +
                $"Lancie: [icon:lance]{Esercito.CostoReclutamento.Guerriero_5.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Guerriero_5.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Guerriero_5.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Guerriero_5.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Guerriero_5.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento cibo: [icon:cibo][rosso]-{Esercito.Unità.Guerriero_5.Cibo.ToString()}[black] s\n" +
                $"Mantenimento oro: [icon:oro][rosso]-{Esercito.Unità.Guerriero_5.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Guerriero_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Guerriero_5.Salute.ToString("#,0")}[black]{(player.Guerriero_Salute == 0 ? "" : $" [black]+ [verde]{player.Guerriero_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Guerriero_5.Difesa.ToString("#,0")}[black]{(player.Guerriero_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Guerriero_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Guerriero_5.Attacco.ToString("#,0")}[black]{(player.Guerriero_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Guerriero_Attacco}[black]")}\n");

            tempoCalcolato = Esercito.CostoReclutamento.Lancere_1.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Lanceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Lancere_1.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Lanceri 1|[black]" +
                $"I lancieri I sono la spina dorsale di ogni esercito ben organizzato. Armati di lance, " +
                $"questi soldati costituiscono un baluardo formidabile contro gli assalti nemici.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Lancere_1.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Lancere_1.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Lancere_1.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Lancere_1.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Lancere_1.Oro.ToString("#,0")}\n" +
                $"Lancie: [icon:lance]{Esercito.CostoReclutamento.Lancere_1.Lance.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Lancere_1.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Lancere_1.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Lancere_1.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Lancere_1.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Lancere_1.Salario.ToString()}[black] s\n \n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Lancere_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Lancere_1.Salute.ToString("#,0")}[black]{(player.Lancere_Salute == 0 ? "" : $" [black]+ [verde]{player.Lancere_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Lancere_1.Difesa.ToString("#,0")}[black]{(player.Lancere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Lancere_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Lancere_1.Attacco.ToString("#,0")}[black]{(player.Lancere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Lancere_Attacco}[black]")}\n");

            tempoCalcolato = Esercito.CostoReclutamento.Lancere_2.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Lanceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Lancere_2.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Lanceri 2|[black]" +
                $"I lancieri II sono la spina dorsale di ogni esercito ben organizzato. Armati di lancie, " +
                $"questi soldati costituiscono un baluardo formidabile contro gli assalti nemici.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Lancere_2.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:cilegnobo]{Esercito.CostoReclutamento.Lancere_2.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Lancere_2.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Lancere_2.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Lancere_2.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Lancere_2.Spade.ToString("#,0")}\n" +
                $"Lancie: [icon:lance]{Esercito.CostoReclutamento.Lancere_2.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Lancere_2.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Lancere_2.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Lancere_2.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Lancere_2.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Lancere_2.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Lancere_2.Salario.ToString()}[black] s\n \n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Lancere_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Lancere_2.Salute.ToString("#,0")}[black]{(player.Lancere_Salute == 0 ? "" : $" [black]+ [verde]{player.Lancere_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Lancere_2.Difesa.ToString("#,0")}[black]{(player.Lancere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Lancere_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Lancere_2.Attacco.ToString("#,0")}[black]{(player.Lancere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Lancere_Attacco}[black]")}\n");

            tempoCalcolato = Esercito.CostoReclutamento.Lancere_3.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Lanceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Lancere_3.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Lanceri 3|[black]" +
                $"I lancieri III sono la spina dorsale di ogni esercito ben organizzato. Armati di lance, " +
                $"questi soldati costituiscono un baluardo formidabile contro gli assalti nemici.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Lancere_3.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:cilegnobo]{Esercito.CostoReclutamento.Lancere_3.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Lancere_3.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Lancere_3.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Lancere_3.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Lancere_3.Spade.ToString("#,0")}\n" +
                $"Lancie: [icon:lance]{Esercito.CostoReclutamento.Lancere_3.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Lancere_3.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Lancere_3.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Lancere_3.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Lancere_3.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Lancere_3.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Lancere_3.Salario.ToString()}[black] s\n \n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Lancere_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Lancere_3.Salute.ToString("#,0")}[black]{(player.Lancere_Salute == 0 ? "" : $" [black]+ [verde]{player.Lancere_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Lancere_3.Difesa.ToString("#,0")}[black]{(player.Lancere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Lancere_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Lancere_3.Attacco.ToString("#,0")}[black]{(player.Lancere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Lancere_Attacco}[black]")}\n");

            tempoCalcolato = Esercito.CostoReclutamento.Lancere_4.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Lanceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Lancere_4.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Lanceri 4|[black]" +
                $"I lancieri IV sono la spina dorsale di ogni esercito ben organizzato. Armati di lance, " +
                $"questi soldati costituiscono un baluardo formidabile contro gli assalti nemici.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Lancere_4.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:cilegnobo]{Esercito.CostoReclutamento.Lancere_4.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Lancere_4.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Lancere_4.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Lancere_4.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Lancere_4.Spade.ToString("#,0")}\n" +
                $"Lancie: [icon:lance]{Esercito.CostoReclutamento.Lancere_4.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Lancere_4.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Lancere_4.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Lancere_4.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Lancere_1.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Lancere_4.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Lancere_4.Salario.ToString()}[black] s\n \n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Lancere_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Lancere_4.Salute.ToString("#,0")}[black]{(player.Lancere_Salute == 0 ? "" : $" [black]+ [verde]{player.Lancere_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Lancere_4.Difesa.ToString("#,0")}[black]{(player.Lancere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Lancere_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Lancere_4.Attacco.ToString("#,0")}[black]{(player.Lancere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Lancere_Attacco}[black]")}\n");

            tempoCalcolato = Esercito.CostoReclutamento.Lancere_5.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Lanceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Lancere_5.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Lanceri 5|[black]" +
                $"I lancieri V sono la spina dorsale di ogni esercito ben organizzato. Armati di lance, " +
                $"questi soldati costituiscono un baluardo formidabile contro gli assalti nemici.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Lancere_5.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:cilegnobo]{Esercito.CostoReclutamento.Lancere_5.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Lancere_5.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Lancere_5.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Lancere_5.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Lancere_5.Spade.ToString("#,0")}\n" +
                $"Lancie: [icon:lance]{Esercito.CostoReclutamento.Lancere_5.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Lancere_5.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Lancere_5.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Lancere_5.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Lancere_5.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Lancere_5.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Lancere_5.Salario.ToString()}[black] s\n \n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Lancere_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Lancere_5.Salute.ToString("#,0")}[black]{(player.Lancere_Salute == 0 ? "" : $" [black]+ [verde]{player.Lancere_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Lancere_5.Difesa.ToString("#,0")}[black]{(player.Lancere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Lancere_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Lancere_5.Attacco.ToString("#,0")}[black]{(player.Lancere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Lancere_Attacco}[black]")}\n");

            tempoCalcolato = Esercito.CostoReclutamento.Arcere_1.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Arceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Arcere_1.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Arceri 1|[black]" +
                $"Gli arcieri I armati di arco e faretra, sono soldati specializzati, dominano il campo di battaglia dalla distanza, " +
                $"scagliando frecce mortali sulle linee nemiche, prima che possano avvicinarsi.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Arcere_1.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Arcere_1.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Arcere_1.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Arcere_1.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Arcere_1.Oro.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Arcere_1.Archi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Arcere_1.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Arcere_1.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Arcere_1.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Arcere_1.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Arcere_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Arcere_1.Salute.ToString("#,0")}[black]{(player.Arcere_Salute == 0 ? "" : $" [black]+ [verde]{player.Arcere_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Arcere_1.Difesa.ToString("#,0")}[black]{(player.Arcere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Arcere_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Arcere_1.Attacco.ToString("#,0")}[black]{(player.Arcere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Arcere_Attacco}[black]")}\n");

            tempoCalcolato = Esercito.CostoReclutamento.Arcere_2.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Arceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Arcere_2.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Arceri 2|[black]" +
                $"Gli arcieri II armati di arco e faretra, sono soldati specializzati, dominano il campo di battaglia dalla distanza, " +
                $"scagliando frecce mortali sulle linee nemiche, prima che possano avvicinarsi.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Arcere_2.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Arcere_2.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Arcere_2.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Arcere_2.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Arcere_2.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Arcere_2.Spade.ToString("#,0")}\n" +
                $"Lancie: [icon:lance]{Esercito.CostoReclutamento.Arcere_2.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Arcere_2.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Arcere_2.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Arcere_2.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Arcere_2.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Arcere_2.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Arcere_2.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Arcere_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Arcere_2.Salute.ToString("#,0")}[black]{(player.Arcere_Salute == 0 ? "" : $" [black]+ [verde]{player.Arcere_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Arcere_2.Difesa.ToString("#,0")}[black]{(player.Arcere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Arcere_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Arcere_2.Attacco.ToString("#,0")}[black]{(player.Arcere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Arcere_Attacco}[black]")}\n");

            tempoCalcolato = Esercito.CostoReclutamento.Arcere_3.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Arceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Arcere_3.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Arceri 3|[black]" +
                $"Gli arcieri III armati di arco e faretra, sono soldati specializzati, dominano il campo di battaglia dalla distanza, " +
                $"scagliando frecce mortali sulle linee nemiche, prima che possano avvicinarsi.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Arcere_3.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Arcere_3.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Arcere_3.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Arcere_3.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Arcere_3.Oro.ToString("#,0")}" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Arcere_3.Spade.ToString("#,0")}\n" +
                $"Lancie: [icon:lance]{Esercito.CostoReclutamento.Arcere_3.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Arcere_3.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Arcere_3.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Arcere_3.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Arcere_3.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Arcere_3.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Arcere_3.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Arcere_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Arcere_3.Salute.ToString("#,0")}[black]{(player.Arcere_Salute == 0 ? "" : $" [black]+ [verde]{player.Arcere_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Arcere_3.Difesa.ToString("#,0")}[black]{(player.Arcere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Arcere_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Arcere_3.Attacco.ToString("#,0")}[black]{(player.Arcere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Arcere_Attacco}[black]")}\n");

            tempoCalcolato = Esercito.CostoReclutamento.Arcere_4.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Arceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Arcere_4.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Arceri 4|[black]" +
                $"Gli arcieri IV armati di arco e faretra, sono soldati specializzati, dominano il campo di battaglia dalla distanza, " +
                $"scagliando frecce mortali sulle linee nemiche, prima che possano avvicinarsi.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Arcere_4.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Arcere_4.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Arcere_4.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Arcere_4.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Arcere_4.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Arcere_4.Spade.ToString("#,0")}\n" +
                $"Lancie: [icon:lance]{Esercito.CostoReclutamento.Arcere_4.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Arcere_4.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Arcere_4.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Arcere_4.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Arcere_4.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Arcere_4.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Arcere_4.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Arcere_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Arcere_4.Salute.ToString("#,0")}[black]{(player.Arcere_Salute == 0 ? "" : $" [black]+ [verde]{player.Arcere_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Arcere_4.Difesa.ToString("#,0")}[black]{(player.Arcere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Arcere_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Arcere_4.Attacco.ToString("#,0")}[black]{(player.Arcere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Arcere_Attacco}[black]")}\n");

            tempoCalcolato = Esercito.CostoReclutamento.Arcere_5.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Arceri_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Arcere_5.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Arceri 5|[black]" +
                $"Gli arcieri V armati di arco e faretra, sono soldati specializzati, dominano il campo di battaglia dalla distanza, " +
                $"scagliando frecce mortali sulle linee nemiche, prima che possano avvicinarsi.\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Arcere_5.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Arcere_5.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Arcere_5.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Arcere_5.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Arcere_5.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Arcere_5.Spade.ToString("#,0")}\n" +
                $"Lancie: [icon:lance]{Esercito.CostoReclutamento.Arcere_5.Lance.ToString("#,0")}\n" +
                $"Archi: [icon:archi]{Esercito.CostoReclutamento.Arcere_5.Archi.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Arcere_5.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Arcere_5.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Arcere_5.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Arcere_5.Cibo.ToString()}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Arcere_5.Salario.ToString()}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Arcere_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Arcere_5.Salute.ToString("#,0")}[black]{(player.Arcere_Salute == 0 ? "" : $" [black]+ [verde]{player.Arcere_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Arcere_5.Difesa.ToString("#,0")}[black]{(player.Arcere_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Arcere_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Arcere_5.Attacco.ToString("#,0")}[black]{(player.Arcere_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Arcere_Attacco}[black]")}\n");

            tempoCalcolato = Esercito.CostoReclutamento.Catapulta_1.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Catapulte_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Catapulta_1.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Catapulte 1|[black]" +
                $"Le catapulte I sono potenti macchine d'assedio che cambiano le sorti delle battaglie, " +
                $"scagliano enormi proiettili distruggendo mura e seminando il terrore tra le fila nemiche\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Catapulta_1.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Catapulta_1.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:cipietrabo]{Esercito.CostoReclutamento.Catapulta_1.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Catapulta_1.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Catapulta_1.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Catapulta_1.Spade.ToString("#,0")}\n" +
                $"Lancie: [icon:lance]{Esercito.CostoReclutamento.Catapulta_1.Lance.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Catapulta_1.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Catapulta_1.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Catapulta_1.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Catapulta_1.Cibo.ToString("0.00")}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Catapulta_1.Salario.ToString("0.00")}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Catapulta_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Catapulta_1.Salute.ToString("#,0")}[black]{(player.Catapulta_Salute == 0 ? "" : $" [black]+ [verde]{player.Catapulta_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Catapulta_1.Difesa.ToString("#,0")}[black]{(player.Catapulta_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Catapulta_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Catapulta_1.Attacco.ToString("#,0")}[black]{(player.Catapulta_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Catapulta_Attacco}[black]")}\n");

            tempoCalcolato = Esercito.CostoReclutamento.Catapulta_2.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Catapulte_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Catapulta_2.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Catapulte 2|[black]" +
                $"Le catapulte II sono potenti macchine d'assedio che cambiano le sorti delle battaglie, " +
                $"scagliano enormi proiettili distruggendo mura e seminando il terrore tra le fila nemiche\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Catapulta_2.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Catapulta_2.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:cipietrabo]{Esercito.CostoReclutamento.Catapulta_2.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Catapulta_2.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Catapulta_2.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Catapulta_2.Spade.ToString("#,0")}\n" +
                $"Lancie: [icon:lance]{Esercito.CostoReclutamento.Catapulta_2.Lance.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Catapulta_2.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Catapulta_2.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Catapulta_2.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Catapulta_2.Cibo.ToString("0.00")}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Catapulta_2.Salario.ToString("0.00")}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Catapulta_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Catapulta_2.Salute.ToString("#,0")}[black]{(player.Catapulta_Salute == 0 ? "" : $" [black]+ [verde]{player.Catapulta_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Catapulta_2.Difesa.ToString("#,0")}[black]{(player.Catapulta_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Catapulta_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Catapulta_2.Attacco.ToString("#,0")}[black]{(player.Catapulta_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Catapulta_Attacco}[black]")}\n");

            tempoCalcolato = Esercito.CostoReclutamento.Catapulta_3.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Catapulte_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Catapulta_3.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Catapulte 3|[black]" +
                $"Le catapulte III sono potenti macchine d'assedio che cambiano le sorti delle battaglie, " +
                $"scagliano enormi proiettili distruggendo mura e seminando il terrore tra le fila nemiche\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Catapulta_3.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Catapulta_3.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:cipietrabo]{Esercito.CostoReclutamento.Catapulta_3.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Catapulta_3.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Catapulta_3.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Catapulta_3.Spade.ToString("#,0")}\n" +
                $"Lancie: [icon:lance]{Esercito.CostoReclutamento.Catapulta_3.Lance.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Catapulta_3.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Catapulta_3.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Catapulta_3.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Catapulta_3.Cibo.ToString("0.00")}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Catapulta_3.Salario.ToString("0.00")}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Catapulta_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Catapulta_3.Salute.ToString("#,0")}[black]{(player.Catapulta_Salute == 0 ? "" : $" [black]+ [verde]{player.Catapulta_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Catapulta_3.Difesa.ToString("#,0")}[black]{(player.Catapulta_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Catapulta_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Catapulta_3.Attacco.ToString("#,0")}[black]{(player.Catapulta_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Catapulta_Attacco}[black]")}\n");

            tempoCalcolato = Esercito.CostoReclutamento.Catapulta_4.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Catapulte_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Catapulta_4.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Catapulte 4|[black]" +
                $"Le catapulte IV sono potenti macchine d'assedio che cambiano le sorti delle battaglie, " +
                $"scagliano enormi proiettili distruggendo mura e seminando il terrore tra le fila nemiche\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Catapulta_4.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Catapulta_4.Legno.ToString("#,0")}" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Catapulta_4.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Catapulta_4.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Catapulta_4.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Catapulta_4.Spade.ToString("#,0")}\n" +
                $"Lancie: [icon:lance]{Esercito.CostoReclutamento.Catapulta_4.Lance.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Catapulta_4.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Catapulta_4.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Catapulta_4.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Catapulta_4.Cibo.ToString("0.00")}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Catapulta_4.Salario.ToString("0.00")}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Catapulta_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Catapulta_4.Salute.ToString("#,0")}[black]{(player.Catapulta_Salute == 0 ? "" : $" [black]+ [verde]{player.Catapulta_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Catapulta_4.Difesa.ToString("#,0")}[black]{(player.Catapulta_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Catapulta_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Catapulta_4.Attacco.ToString("#,0")}[black]{(player.Catapulta_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Catapulta_Attacco}[black]")}\n");

            tempoCalcolato = Esercito.CostoReclutamento.Catapulta_5.TempoReclutamento - player.Ricerca_Addestramento * Ricerca.Catapulte_Riduzione * (player.Bonus_Addestramento + 1);
            tempoBase = Esercito.CostoReclutamento.Catapulta_5.TempoReclutamento;
            Send(player.guid_Player, $"Descrizione|Catapulte 5|[black]" +
                $"Le catapulte V sono potenti macchine d'assedio che cambiano le sorti delle battaglie, " +
                $"scagliano enormi proiettili distruggendo mura e seminando il terrore tra le fila nemiche\n\n" +
                $"Costo Addestramento:\n" +
                $"Cibo: [icon:cibo]{Esercito.CostoReclutamento.Catapulta_5.Cibo.ToString("#,0")}\n" +
                $"Legno: [icon:legno]{Esercito.CostoReclutamento.Catapulta_5.Legno.ToString("#,0")}\n" +
                $"Pietra: [icon:pietra]{Esercito.CostoReclutamento.Catapulta_5.Pietra.ToString("#,0")}\n" +
                $"Ferro: [icon:ferro]{Esercito.CostoReclutamento.Catapulta_5.Ferro.ToString("#,0")}\n" +
                $"Oro: [icon:oro]{Esercito.CostoReclutamento.Catapulta_5.Oro.ToString("#,0")}\n" +
                $"Spade: [icon:spade]{Esercito.CostoReclutamento.Catapulta_5.Spade.ToString("#,0")}\n" +
                $"Lancie: [icon:lance]{Esercito.CostoReclutamento.Catapulta_5.Lance.ToString("#,0")}\n" +
                $"Scudi: [icon:scudi]{Esercito.CostoReclutamento.Catapulta_5.Scudi.ToString("#,0")}\n" +
                $"Armature: [icon:armature]{Esercito.CostoReclutamento.Catapulta_5.Armature.ToString("#,0")}\n \n" +
                $"Popolazione: [icon:popolazione]{Esercito.CostoReclutamento.Catapulta_5.Popolazione}\n" +
                $"Addestramento: [icon:tempo]{player.FormatTime(tempoCalcolato)}{(tempoCalcolato == tempoBase ? "" : $" ({player.FormatTime(tempoBase)})")}\n" +
                $"Mantenimento Cibo: [icon:cibo][rosso]-{Esercito.Unità.Catapulta_5.Cibo.ToString("0.00")}[black] s\n" +
                $"Mantenimento Oro: [icon:oro][rosso]-{Esercito.Unità.Catapulta_5.Salario.ToString("0.00")}[black] s\n\n" +
                $"Statistiche:\n" +
                $"Livello: [arancione]{player.Catapulta_Livello.ToString("#,0")}[black]\n" +
                $"Salute: [verde]{Esercito.Unità.Catapulta_5.Salute.ToString("#,0")}[black]{(player.Catapulta_Salute == 0 ? "" : $" [black]+ [verde]{player.Catapulta_Salute}[black]")}\n" +
                $"Difesa: [bluGotico]{Esercito.Unità.Catapulta_5.Difesa.ToString("#,0")}[black]{(player.Catapulta_Difesa == 0 ? "" : $" [black]+ [bluGotico]{player.Catapulta_Difesa}[black]")}\n" +
                $"Attacco: [rosso]{Esercito.Unità.Catapulta_5.Attacco.ToString("#,0")}[black]{(player.Catapulta_Attacco == 0 ? "" : $" [black]+ [rosso]{player.Catapulta_Attacco}[black]")}\n");

            Send(player.guid_Player, $"Descrizione|Esperienza|[black]" +
                $"L’esperienza[icon:xp] rappresenta la crescita del giocatore nel tempo.\nAccumulare esperienza permette di salire di livello.\n\n Esperienza prossimo livello: [icon:xp][acciaioBlu]{Esperienza.LevelUp(player)}[black]xp");

            Send(player.guid_Player, $"Descrizione|Livello|[black]" +
                $"Il livello[icon:lv] indica il grado di avanzamento del giocatore, fondamentale per raggiungere le vette nella ricerca, poter migliorare unità e strutture.\n\n" +
                $"Necessaria per avanzare nel 'PVP/PVE', migliorare le strutture difensive del proprio villaggio, oltre che per lo sblocco di unità militari avanzate.\n " +
                $"Attualmente non è presente un limite al livello.");

            Send(player.guid_Player, $"Descrizione|Giocatore|[black]" +
                $"Scheda statistiche, qui è possibile visualizzare le proprie statistiche di gioco cliccando l'icona, insieme a ulteriori informazioni molto utili durante l'avanzamento.\n");

            Send(player.guid_Player, $"Descrizione|Diamanti Blu|[black]" +
                $"I [blu]Diamanti Blu[/blu][black][icon:diamanteBlu] possono essere utilizzati all'interno dello shop per l'acquisto di pacchetti, per una migliore gestione della città.\n\n" +
                $"Inoltre possono essere richiesti in alcune quest, velocizzare i tempi d'attesa per strutture e unità militari.\n");

            Send(player.guid_Player, $"Descrizione|Diamanti Viola|[black]" +
                $"I [viola]Diamanti Viola[/viola][black][icon:diamanteViola] fondamentali per l'acquisto di feudi[leggendario], sono alla base dell'economia.\n\nPossono essere scambiati per [blu]Diamanti Blu[/blu][black][icon:diamanteBlu] ed utilizzati " +
                $"all'interno dello shop per l'acquisto di pacchetti o per una migliore gestione della città.\n\nOltre ad essere richiesti in alcune quest, " +
                $"dovrebbero essere sempre presenti nelle casse della città.");

            Send(player.guid_Player, $"Descrizione|Dollari Virtuali|[black]" +
                $"I [icon:dollariVirtuali]Tributi dei feudi vengono generati tramite i feudi posseduti dal giocatore.\n\nPossono essere prelevati raggiunta la soglia di [verde]{Variabili_Server.prelievo_Minimo}$[/verde][black][icon:dollariVirtuali] " +
                $"oppure utilizzati all'interno dello shop per l'acquisto di pacchetti.\nAttualmente [icon:dollariVirtuali]1 tributo equivale ad [icon:usdt]1 USDT");

            Send(player.guid_Player, $"Descrizione|Cibo|[black]" +
                $"Il [icon:cibo]Cibo, è fondamentale per il mantenimento delle unità militari, per il loro addestramento e la costruzione di strutture.\n\n" +
                $"Necessario anche per la ricerca.\n\n");

            Send(player.guid_Player, $"Descrizione|Legno|[black]" +
                $"Il [icon:legno]Legno, è necessario per l'addestramento di unità militari e la costruzione di strutture. Necessario per la ricerca.\n\n");

            Send(player.guid_Player, $"Descrizione|Pietra|[black]" +
                $"La [icon:pietra]Pietra, fondamentale per la riparazione delle strutture difensive. Necessario per la ricerca.\n\n");

            Send(player.guid_Player, $"Descrizione|Ferro|[black]" +
                $"Il [icon:ferro]Ferro, fondamentale per la costruzione di edifici e la riparazione delle strutture difensive.\n\n" +
                $"E' necessiamo per la produzione di armamento militare e per la ricerca.\n\n");

            Send(player.guid_Player, $"Descrizione|Oro|[black]" +
                $"L [icon:oro]Oro è una risorsa primaria per le casse della città, necessario per la costruzione di edifici civili e militari, oltre che per il reclutamento delle unità.\n\n" +
                $"E' fondamentale per il mantenimento di unità e strutture belliche oltre alla ricerca.\n\n");

            Send(player.guid_Player, $"Descrizione|Popolazione|[black]" +
                $"La [icon:popolazione]Popolazione è fondamentale per la costruzione di strutture civili e militari, oltre al reclutamento di unità.\n\n");

            Send(player.guid_Player, $"Descrizione|Spade|[black]Le Spade[icon:spade] sono necessarie per l'addestramento dei [cuoioScuro]guerrieri[icon:guerriero][black].\n\n");
            Send(player.guid_Player, $"Descrizione|Lance|[black]Le Lancie[icon:lance] sono necessarie per l'addestramento dei [cuoioScuro]lancieri[icon:lancere][black].\n\n");
            Send(player.guid_Player, $"Descrizione|Archi|[black]Gli Archi[icon:archi] sono necessari per l'addestramento degli [cuoioScuro]arcieri[icon:arcere][black].\n\n");
            Send(player.guid_Player, $"Descrizione|Scudi|[black]Gli Scudi[icon:scudi] sono necessari per l'addestramento delle unità militari.\n\n");
            Send(player.guid_Player, $"Descrizione|Armature|[black]Le Armature[icon:armature] sono necessarie per l'addestramento delle unità militari.\n\n");
            Send(player.guid_Player, $"Descrizione|Frecce|[black]Le Frecce[icon:frecce] sono fondamentali per le [cuoioScuro]unità a distanza[black], senza di esse sono praticamente inutili.\n\n");

            Send(player.guid_Player, $"Descrizione|Mura Salute|[black]Ripara la [verdeF]salute[black] delle [porporaReale]mura[black] al massimo.\nCosto riparazioni per [verdeF]1 HP[/verdeF][black] per ogni ciclo.\n\n" +
                $"Cibo: [icon:cibo]{Strutture.Riparazione.Mura.Consumo_Cibo}\n" +
                $"Legno: [icon:legno]{Strutture.Riparazione.Mura.Consumo_Legno}\n" +
                $"Pietra: [icon:pietra]{Strutture.Riparazione.Mura.Consumo_Pietra}\n" +
                $"Ferro: [icon:ferro]{Strutture.Riparazione.Mura.Consumo_Ferro}\n" +
                $"Oro: [icon:oro]{Strutture.Riparazione.Mura.Consumo_Oro}\n" +
                $"Tempo: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");
            Send(player.guid_Player, $"Descrizione|Mura Difesa|[black]Ripara la [blu]difesa[black] delle [porporaReale]mura[black] al massimo.\nCosto riparazioni per [blu]1 DEF[/blu][black] per ogni ciclo.\n\n" +
                $"Cibo: [icon:cibo]{Strutture.Riparazione.Mura.Consumo_Cibo}\n" +
                $"Legno: [icon:legno]{Strutture.Riparazione.Mura.Consumo_Legno}\n" +
                $"Pietra: [icon:pietra]{Strutture.Riparazione.Mura.Consumo_Pietra}\n" +
                $"Ferro: [icon:ferro]{Strutture.Riparazione.Mura.Consumo_Ferro}\n" +
                $"Oro: [icon:oro]{Strutture.Riparazione.Mura.Consumo_Oro}\n" +
                $"Tempo: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");

            Send(player.guid_Player, $"Descrizione|Cancello Salute|[black]Ripara la [verdeF]salute[black] del [porporaReale]cancello[black] al massimo.\nCosto riparazioni per [verdeF]1 HP[/verdeF][black] per ogni ciclo.\n\n" +
                $"Cibo: [icon:cibo]{Strutture.Riparazione.Cancello.Consumo_Cibo}\n" +
                $"Legno: [icon:legno]{Strutture.Riparazione.Cancello.Consumo_Legno}\n" +
                $"Pietra: [icon:pietra]{Strutture.Riparazione.Cancello.Consumo_Pietra}\n" +
                $"Ferro: [icon:ferro]{Strutture.Riparazione.Cancello.Consumo_Ferro}\n" +
                $"Oro: [icon:oro]{Strutture.Riparazione.Cancello.Consumo_Oro}\n" +
                $"Tempo: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");
            Send(player.guid_Player, $"Descrizione|Cancello Difesa|[black]Ripara la [blu]difesa[black] del [porporaReale]cancello[black] al massimo.\nCosto riparazioni per [blu]1 DEF[/blu][black] per ogni ciclo.\n\n" +
                $"Cibo: [icon:cibo]{Strutture.Riparazione.Cancello.Consumo_Cibo}\n" +
                $"Legno: [icon:legno]{Strutture.Riparazione.Cancello.Consumo_Legno}\n" +
                $"Pietra: [icon:pietra]{Strutture.Riparazione.Cancello.Consumo_Pietra}\n" +
                $"Ferro: [icon:ferro]{Strutture.Riparazione.Cancello.Consumo_Ferro}\n" +
                $"Oro: [icon:oro]{Strutture.Riparazione.Cancello.Consumo_Oro}\n" +
                $"Tempo: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");

            Send(player.guid_Player, $"Descrizione|Torri Salute|[black]Ripara la [verdeF]salute[black] delle [porporaReale]torri[black] al massimo.\nCosto riparazioni per [verdeF]1 HP[/verdeF][black] per ogni ciclo.\n\n" +
                $"Cibo: [icon:cibo]{Strutture.Riparazione.Torri.Consumo_Cibo}\n" +
                $"Legno: [icon:legno]{Strutture.Riparazione.Torri.Consumo_Legno}\n" +
                $"Pietra: [icon:pietra]{Strutture.Riparazione.Torri.Consumo_Pietra}\n" +
                $"Ferro: [icon:ferro]{Strutture.Riparazione.Torri.Consumo_Ferro}\n" +
                $"Oro: [icon:oro]{Strutture.Riparazione.Torri.Consumo_Oro}\n" +
                $"Tempo: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");
            Send(player.guid_Player, $"Descrizione|Torri Difesa|[black]Ripara la [blu]difesa[black] delle [porporaReale]torri[black]al massimo.\nCosto riparazioni per [blu]1 DEF[/blu][black] per ogni ciclo.\n\n" +
                $"Cibo: [icon:cibo]{Strutture.Riparazione.Torri.Consumo_Cibo}\n" +
                $"Legno: [icon:legno]{Strutture.Riparazione.Torri.Consumo_Legno}\n" +
                $"Pietra: [icon:pietra]{Strutture.Riparazione.Torri.Consumo_Pietra}\n" +
                $"Ferro: [icon:ferro]{Strutture.Riparazione.Torri.Consumo_Ferro}\n" +
                $"Oro: [icon:oro]{Strutture.Riparazione.Torri.Consumo_Oro}\n" +
                $"Tempo: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");

            Send(player.guid_Player, $"Descrizione|Castello Salute|[black]Ripara la [verdeF]salute[black] del [porporaReale]castello[black]al massimo.\nCosto riparazioni per [verdeF]1 HP[/verdeF][black] per ogni ciclo.\n\n" +
                $"Cibo: [icon:cibo]{Strutture.Riparazione.Castello.Consumo_Cibo}\n" +
                $"Legno: [icon:legno]{Strutture.Riparazione.Castello.Consumo_Legno}\n" +
                $"Pietra: [icon:pietra]{Strutture.Riparazione.Castello.Consumo_Pietra}\n" +
                $"Ferro: [icon:ferro]{Strutture.Riparazione.Castello.Consumo_Ferro}\n" +
                $"Oro: [icon:oro]{Strutture.Riparazione.Castello.Consumo_Oro}\n" +
                $"Tempo: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");
            Send(player.guid_Player, $"Descrizione|Castello Difesa|[black]Ripara la [blu]difesa[black] del [porporaReale]castello[black]al massimo.\nCosto riparazioni per [blu]1 DEF[/blu][black] per ogni ciclo.\n\n" +
                $"Cibo: [icon:cibo]{Strutture.Riparazione.Castello.Consumo_Cibo}\n" +
                $"Legno: [icon:legno]{Strutture.Riparazione.Castello.Consumo_Legno}\n" +
                $"Pietra: [icon:pietra]{Strutture.Riparazione.Castello.Consumo_Pietra}\n" +
                $"Ferro: [icon:ferro]{Strutture.Riparazione.Castello.Consumo_Ferro}\n" +
                $"Oro: [icon:oro]{Strutture.Riparazione.Castello.Consumo_Oro}\n" +
                $"Tempo: [icon:tempo]{Variabili_Server.tempo_Riparazione}s\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Addestramento|[black]Permette di diminuire il tempo necessario per l'addestramento di ogni unità.\n" +
                $"Costo ricerca addestramento lv {player.Ricerca_Addestramento + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Tipi.Addestramento.Cibo * (player.Ricerca_Addestramento + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Tipi.Addestramento.Legno * (player.Ricerca_Addestramento + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Tipi.Addestramento.Pietra * (player.Ricerca_Addestramento + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Tipi.Addestramento.Ferro * (player.Ricerca_Addestramento + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Tipi.Addestramento.Oro * (player.Ricerca_Addestramento + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Tipi.Addestramento.TempoRicerca * (player.Ricerca_Addestramento + 1))}\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Costruzione|[black]Permette di diminuire il tempo necessario per la costruzione di ogni edificio.\n" +
                $"Costo ricerca costruzione lv {player.Ricerca_Costruzione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Tipi.Costruzione.Cibo * (player.Ricerca_Costruzione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Tipi.Costruzione.Legno * (player.Ricerca_Costruzione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Tipi.Costruzione.Pietra * (player.Ricerca_Costruzione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Tipi.Costruzione.Ferro * (player.Ricerca_Costruzione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Tipi.Costruzione.Oro * (player.Ricerca_Costruzione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Tipi.Costruzione.TempoRicerca * (player.Ricerca_Costruzione + 1))}\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Produzione|[black]Permette di incrementare la quantità di risorse prodotte da ogni struttura produttiva.\n" +
                $"Costo ricerca produzione lv {player.Ricerca_Produzione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Tipi.Produzione.Cibo * (player.Ricerca_Produzione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Tipi.Produzione.Legno * (player.Ricerca_Produzione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Tipi.Produzione.Pietra * (player.Ricerca_Produzione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Tipi.Produzione.Ferro * (player.Ricerca_Produzione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Tipi.Produzione.Oro * (player.Ricerca_Produzione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Tipi.Produzione.TempoRicerca * (player.Ricerca_Produzione + 1))}\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Popolazione|[black]Permette di implementaremigliori strategie per aumentare il numero di cittadini che giungono nel tuo villaggio.\n" +
                $"Costo ricerca popolazione lv {player.Ricerca_Popolazione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Tipi.Popolazione.Cibo * (player.Ricerca_Popolazione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Tipi.Popolazione.Legno * (player.Ricerca_Popolazione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Tipi.Popolazione.Pietra * (player.Ricerca_Popolazione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Tipi.Popolazione.Ferro * (player.Ricerca_Popolazione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Tipi.Popolazione.Oro * (player.Ricerca_Popolazione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Tipi.Popolazione.TempoRicerca * (player.Ricerca_Popolazione + 1))}\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Trasporto|[black]Permette di aumentare la capacità di trasporto delle singole unità militari.\n" +
                $"Costo ricerca trasporto lv {player.Ricerca_Trasporto + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Tipi.Trasporto.Cibo * (player.Ricerca_Trasporto + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Tipi.Trasporto.Legno * (player.Ricerca_Trasporto + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Tipi.Trasporto.Pietra * (player.Ricerca_Trasporto + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Tipi.Trasporto.Ferro * (player.Ricerca_Trasporto + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Tipi.Trasporto.Oro * (player.Ricerca_Trasporto + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Tipi.Trasporto.TempoRicerca * (player.Ricerca_Trasporto + 1))}\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Riparazione|[black]Permette di migliorare la riparazione delle singole strutture.\n" +
                $"Costo ricerca riparazione lv {player.Ricerca_Riparazione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Tipi.Riparazione.Cibo * (player.Ricerca_Riparazione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Tipi.Riparazione.Legno * (player.Ricerca_Riparazione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Tipi.Riparazione.Pietra * (player.Ricerca_Riparazione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Tipi.Riparazione.Ferro * (player.Ricerca_Riparazione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Tipi.Riparazione.Oro * (player.Ricerca_Riparazione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Tipi.Riparazione.TempoRicerca * (player.Ricerca_Riparazione + 1))}\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Guerriero Livello|[black]Aumenta il livello dei guerrieri.\n" +
                $"Costo ricerca livello guerriero lv {player.Guerriero_Livello + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Livello.Cibo * (player.Guerriero_Livello + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Livello.Legno * (player.Guerriero_Livello + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Livello.Pietra * (player.Guerriero_Livello + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Livello.Ferro * (player.Guerriero_Livello + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Livello.Oro * (player.Guerriero_Livello + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Livello.TempoRicerca * (player.Guerriero_Livello + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Guerriero Salute|[black]Aumenta la salute dei guerrieri.\n" +
                $"Costo ricerca salute guerriero lv {player.Guerriero_Salute + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Salute.Cibo * (player.Guerriero_Salute + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Salute.Legno * (player.Guerriero_Salute + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Salute.Pietra * (player.Guerriero_Salute + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Salute.Ferro * (player.Guerriero_Salute + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Salute.Oro * (player.Guerriero_Salute + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Salute.TempoRicerca * (player.Guerriero_Salute + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Guerriero Attacco|[black]Aumenta l'attacco dei guerrieri.\n" +
                $"Costo ricerca difesa guerriero lv {player.Guerriero_Attacco + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Attacco.Cibo * (player.Guerriero_Attacco + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Attacco.Legno * (player.Guerriero_Attacco + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Attacco.Pietra * (player.Guerriero_Attacco + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Attacco.Ferro * (player.Guerriero_Attacco + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Attacco.Oro * (player.Guerriero_Attacco + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Attacco.TempoRicerca * (player.Guerriero_Attacco + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Guerriero Difesa|[black]Aumenta la difesa dei guerrieri.\n" +
                $"Costo ricerca difesa guerriero lv {player.Guerriero_Difesa + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Difesa.Cibo * (player.Guerriero_Difesa + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Difesa.Legno * (player.Guerriero_Difesa + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Difesa.Pietra * (player.Guerriero_Difesa + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Difesa.Ferro * (player.Guerriero_Difesa + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Difesa.Oro * (player.Guerriero_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Difesa.TempoRicerca * (player.Guerriero_Difesa + 1))}\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Lancere Livello|[black]Aumenta il livello dei Lancieri.\n" +
                $"Costo ricerca livello Lanciere lv {player.Lancere_Livello + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Livello.Cibo * (player.Lancere_Livello + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Livello.Legno * (player.Lancere_Livello + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Livello.Pietra * (player.Lancere_Livello + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Livello.Ferro * (player.Lancere_Livello + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Livello.Oro * (player.Lancere_Livello + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Livello.TempoRicerca * (player.Lancere_Livello + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Lancere Salute|[black]Aumenta la salute dei Lancieri.\n" +
                $"Costo ricerca salute Lanciere lv {player.Lancere_Salute + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Salute.Cibo * (player.Lancere_Salute + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Salute.Legno * (player.Lancere_Salute + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Salute.Pietra * (player.Lancere_Salute + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Salute.Ferro * (player.Lancere_Salute + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Salute.Oro * (player.Lancere_Salute + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Salute.TempoRicerca * (player.Lancere_Salute + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Lancere Attacco|[black]Aumenta l'attacco dei Lancieri.\n" +
                $"Costo ricerca difesa Lanciere lv {player.Lancere_Attacco + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Attacco.Cibo * (player.Lancere_Attacco + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Attacco.Legno * (player.Lancere_Attacco + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Attacco.Pietra * (player.Lancere_Attacco + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Attacco.Ferro * (player.Lancere_Attacco + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Attacco.Oro * (player.Lancere_Attacco + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Attacco.TempoRicerca * (player.Lancere_Attacco + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Lancere Difesa|[black]Aumenta la difesa dei Lancieri.\n" +
                $"Costo ricerca difesa Lanciere lv {player.Lancere_Difesa + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Difesa.Cibo * (player.Lancere_Difesa + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Difesa.Legno * (player.Lancere_Difesa + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Difesa.Pietra * (player.Lancere_Difesa + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Difesa.Ferro * (player.Lancere_Difesa + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Difesa.Oro * (player.Lancere_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Difesa.TempoRicerca * (player.Lancere_Difesa + 1))}\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Arcere Livello|[black]Aumenta il livello degli Arcieri.\n" +
                $"Costo ricerca livello Arciere lv {player.Arcere_Livello + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Livello.Cibo * (player.Arcere_Livello + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Livello.Legno * (player.Arcere_Livello + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Livello.Pietra * (player.Arcere_Livello + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Livello.Ferro * (player.Arcere_Livello + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Livello.Oro * (player.Arcere_Livello + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Livello.TempoRicerca * (player.Arcere_Livello + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Arcere Salute|[black]Aumenta la salute degli Arcieri.\n" +
                $"Costo ricerca salute Arciere lv {player.Arcere_Salute + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Salute.Cibo * (player.Arcere_Salute + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Salute.Legno * (player.Arcere_Salute + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Salute.Pietra * (player.Arcere_Salute + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Salute.Ferro * (player.Arcere_Salute + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Salute.Oro * (player.Arcere_Salute + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Salute.TempoRicerca * (player.Arcere_Salute + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Arcere Attacco|[black]Aumenta l'attacco degli Arcieri.\n" +
                $"Costo ricerca difesa Arciere lv {player.Arcere_Attacco + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Attacco.Cibo * (player.Arcere_Attacco + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Attacco.Legno * (player.Arcere_Attacco + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Attacco.Pietra * (player.Arcere_Attacco + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Attacco.Ferro * (player.Arcere_Attacco + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Attacco.Oro * (player.Arcere_Attacco + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Attacco.TempoRicerca * (player.Arcere_Attacco + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Arcere Difesa|[black]Aumenta la difesa degli Arcieri.\n" +
                $"Costo ricerca difesa Arciere lv {player.Arcere_Difesa + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Difesa.Cibo * (player.Arcere_Difesa + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Difesa.Legno * (player.Arcere_Difesa + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Difesa.Pietra * (player.Arcere_Difesa + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Difesa.Ferro * (player.Arcere_Difesa + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Difesa.Oro * (player.Arcere_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Difesa.TempoRicerca * (player.Arcere_Difesa + 1))}\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Catapulta Livello|[black]Aumenta il livello delle Catapulte.\n" +
                $"Costo ricerca livello catapulte lv {player.Catapulta_Livello + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Livello.Cibo * (player.Catapulta_Livello + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Livello.Legno * (player.Catapulta_Livello + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Livello.Pietra * (player.Catapulta_Livello + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Livello.Ferro * (player.Catapulta_Livello + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Livello.Oro * (player.Catapulta_Livello + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Livello.TempoRicerca * (player.Catapulta_Livello + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Catapulta Salute|[black]Aumenta la salute delle Catapulte.\n" +
                $"Costo ricerca salute catapulta lv {player.Catapulta_Salute + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Salute.Cibo * (player.Catapulta_Salute + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Salute.Legno * (player.Catapulta_Salute + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Salute.Pietra * (player.Catapulta_Salute + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Salute.Ferro * (player.Catapulta_Salute + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Salute.Oro * (player.Catapulta_Salute + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Salute.TempoRicerca * (player.Catapulta_Salute + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Catapulta Attacco|[black]Aumenta l'attacco delle Catapulte.\n" +
                $"Costo ricerca difesa catapulta lv {player.Catapulta_Attacco + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Attacco.Cibo * (player.Catapulta_Attacco + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Attacco.Legno * (player.Catapulta_Attacco + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Attacco.Pietra * (player.Catapulta_Attacco + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Attacco.Ferro * (player.Catapulta_Attacco + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Attacco.Oro * (player.Catapulta_Attacco + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Attacco.TempoRicerca * (player.Catapulta_Attacco + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Catapulta Difesa|[black]Aumenta la difesa delle Catapulte.\n" +
                $"Costo ricerca difesa catapulta lv {player.Catapulta_Difesa + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Soldati.Difesa.Cibo * (player.Catapulta_Difesa + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Soldati.Difesa.Legno * (player.Catapulta_Difesa + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Soldati.Difesa.Pietra * (player.Catapulta_Difesa + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Soldati.Difesa.Ferro * (player.Catapulta_Difesa + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Soldati.Difesa.Oro * (player.Catapulta_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Soldati.Difesa.TempoRicerca * (player.Catapulta_Difesa + 1))}\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Ingresso Guarnigione|[black]Aumenta il numero massimo di unità che può ospitare la struttura.\n" +
                $"Costo ricerca ingresso guarnigione lv {player.Ricerca_Ingresso_Guarnigione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Ingresso.Cibo * (player.Ricerca_Ingresso_Guarnigione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Ingresso.Legno * (player.Ricerca_Ingresso_Guarnigione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Ingresso.Pietra * (player.Ricerca_Ingresso_Guarnigione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Ingresso.Ferro * (player.Ricerca_Ingresso_Guarnigione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Ingresso.Oro * (player.Ricerca_Ingresso_Guarnigione + 1):#,0}\n" +
                $"Popolazione: [icon:popolazione]{Ricerca.Citta.Ingresso.Popolazione * (player.Ricerca_Ingresso_Guarnigione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Ingresso.TempoRicerca * (player.Ricerca_Ingresso_Guarnigione + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Citta Guarnigione|[black]Aumenta il numero massimo di unità che può ospitare la struttura.\n" +
                $"Costo ricerca citta guarnigione lv {player.Ricerca_Citta_Guarnigione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Città.Cibo * (player.Ricerca_Citta_Guarnigione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Città.Legno * (player.Ricerca_Citta_Guarnigione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Città.Pietra * (player.Ricerca_Citta_Guarnigione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Città.Ferro * (player.Ricerca_Citta_Guarnigione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Città.Oro * (player.Ricerca_Citta_Guarnigione + 1):#,0}\n" +
                $"Popolazione: [icon:popolazione]{Ricerca.Citta.Città.Popolazione * (player.Ricerca_Citta_Guarnigione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Città.TempoRicerca * (player.Ricerca_Citta_Guarnigione + 1))}\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Mura Livello|[black]Aumenta il livello della struttura.\n" +
                $"Costo ricerca livello mura: {player.Ricerca_Mura_Livello + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Mura_Livello.Cibo * (player.Ricerca_Mura_Livello + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Mura_Livello.Legno * (player.Ricerca_Mura_Livello + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Mura_Livello.Pietra * (player.Ricerca_Mura_Livello + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Mura_Livello.Ferro * (player.Ricerca_Mura_Livello + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Mura_Livello.Oro * (player.Ricerca_Mura_Livello + 1):#,0}\n" +
                $"Popolazione: [icon:popolazione]{Ricerca.Citta.Mura_Livello.Popolazione * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Mura_Livello.TempoRicerca * (player.Ricerca_Mura_Livello + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Mura Guarnigione|[black]Aumenta il numero massimo di unità che può ospitare la struttura.\n" +
                $"Costo ricerca guarnigione mura lv: {player.Ricerca_Mura_Guarnigione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Mura_Guarnigione.Cibo * (player.Ricerca_Mura_Guarnigione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Mura_Guarnigione.Legno * (player.Ricerca_Mura_Guarnigione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Mura_Guarnigione.Pietra * (player.Ricerca_Mura_Guarnigione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Mura_Guarnigione.Ferro * (player.Ricerca_Mura_Guarnigione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Mura_Guarnigione.Oro * (player.Ricerca_Mura_Guarnigione + 1):#,0}\n" +
                $"Popolazione: [icon:popolazione]{Ricerca.Citta.Mura_Guarnigione.Popolazione * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Mura_Guarnigione.TempoRicerca * (player.Ricerca_Mura_Guarnigione + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Mura Salute|[black]Aumenta il numero massimo di punti salute della struttura.\n" +
                $"Costo ricerca salute mura lv: {player.Ricerca_Mura_Salute + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Mura_Salute.Cibo * (player.Ricerca_Mura_Salute + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Mura_Salute.Legno * (player.Ricerca_Mura_Salute + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Mura_Salute.Pietra * (player.Ricerca_Mura_Salute + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Mura_Salute.Ferro * (player.Ricerca_Mura_Salute + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Mura_Salute.Oro * (player.Ricerca_Mura_Salute + 1):#,0}\n" +
                $"Popolazione: [icon:popolazione]{Ricerca.Citta.Mura_Salute.Popolazione * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Mura_Salute.TempoRicerca * (player.Ricerca_Mura_Salute + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Mura Difesa|[black]Aumenta il numero massimo di punti difesa della struttura.\n" +
                $"Costo ricerca difesa mura lv: {player.Ricerca_Mura_Difesa + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Mura_Difesa.Cibo * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Mura_Difesa.Legno * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Mura_Difesa.Pietra * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Mura_Difesa.Ferro * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Mura_Difesa.Oro * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"Popolazione: [icon:popolazione]{Ricerca.Citta.Mura_Difesa.Popolazione * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Mura_Difesa.TempoRicerca * (player.Ricerca_Mura_Difesa + 1))}\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Cancello Livello|[black]Aumenta il numero massimo di unità che può ospitare la struttura.\n" +
                $"Costo ricerca livello cancello: {player.Ricerca_Cancello_Guarnigione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Cancello_Livello.Cibo * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Cancello_Livello.Legno * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Cancello_Livello.Pietra * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Cancello_Livello.Ferro * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Cancello_Livello.Oro * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"Popolazione: [icon:popolazione]{Ricerca.Citta.Cancello_Livello.Popolazione * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Cancello_Livello.TempoRicerca * (player.Ricerca_Cancello_Guarnigione + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Cancello Guarnigione|[black]Aumenta il numero massimo di unità che può ospitare la struttura.\n" +
                $"Costo ricerca guarnigione cancello lv: {player.Ricerca_Cancello_Guarnigione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Cancello_Guarnigione.Cibo * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Cancello_Guarnigione.Legno * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Cancello_Guarnigione.Pietra * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Cancello_Guarnigione.Ferro * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Cancello_Guarnigione.Oro * (player.Ricerca_Cancello_Guarnigione + 1):#,0}\n" +
                $"Popolazione: [icon:popolazione]{Ricerca.Citta.Cancello_Guarnigione.Popolazione * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Cancello_Guarnigione.TempoRicerca * (player.Ricerca_Cancello_Guarnigione + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Cancello Salute|[black]Aumenta il numero massimo di punti salute della struttura.\n" +
                $"Costo ricerca salute cancello lv: {player.Ricerca_Cancello_Salute + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Cancello_Salute.Cibo * (player.Ricerca_Cancello_Salute + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Cancello_Salute.Legno * (player.Ricerca_Cancello_Salute + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Cancello_Salute.Pietra * (player.Ricerca_Cancello_Salute + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Cancello_Salute.Ferro * (player.Ricerca_Cancello_Salute + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Cancello_Salute.Oro * (player.Ricerca_Cancello_Salute + 1):#,0}\n" +
                $"Popolazione: [icon:popolazione]{Ricerca.Citta.Cancello_Salute.Popolazione * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Cancello_Salute.TempoRicerca * (player.Ricerca_Cancello_Salute + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Cancello Difesa|[black]Aumenta il numero massimo di punti difesa della struttura.\n" +
                $"Costo ricerca difesa cancello lv: {player.Ricerca_Cancello_Difesa + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Cancello_Difesa.Cibo * (player.Ricerca_Cancello_Difesa + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Cancello_Difesa.Legno * (player.Ricerca_Cancello_Difesa + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Cancello_Difesa.Pietra * (player.Ricerca_Cancello_Difesa + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Cancello_Difesa.Ferro * (player.Ricerca_Cancello_Difesa + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Cancello_Difesa.Oro * (player.Ricerca_Cancello_Difesa + 1):#,0}\n" +
                $"Popolazione: [icon:popolazione]{Ricerca.Citta.Cancello_Difesa.Popolazione * (player.Ricerca_Mura_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Cancello_Difesa.TempoRicerca * (player.Ricerca_Cancello_Difesa + 1))}\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Torri Livello|[black]Aumenta il numero massimo di unità che può ospitare la struttura.\n" +
                $"Costo ricerca livello torri: {player.Ricerca_Torri_Guarnigione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Torri_Livello.Cibo * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Torri_Livello.Legno * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Torri_Livello.Pietra * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Torri_Livello.Ferro * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Torri_Livello.Oro * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Torri_Livello.TempoRicerca * (player.Ricerca_Torri_Guarnigione + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Torri Guarnigione|[black]Aumenta il numero massimo di unità che può ospitare la struttura.\n" +
                $"Costo ricerca guarnigione torri lv: {player.Ricerca_Torri_Guarnigione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Torri_Guarnigione.Cibo * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Torri_Guarnigione.Legno * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Torri_Guarnigione.Pietra * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Torri_Guarnigione.Ferro * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Torri_Guarnigione.Oro * (player.Ricerca_Torri_Guarnigione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Torri_Guarnigione.TempoRicerca * (player.Ricerca_Torri_Guarnigione + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Torri Salute|[black]Aumenta il numero massimo di punti salute della struttura.\n" +
                $"Costo ricerca salute torri lv: {player.Ricerca_Torri_Salute + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Torri_Salute.Cibo * (player.Ricerca_Torri_Salute + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Torri_Salute.Legno * (player.Ricerca_Torri_Salute + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Torri_Salute.Pietra * (player.Ricerca_Torri_Salute + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Torri_Salute.Ferro * (player.Ricerca_Torri_Salute + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Torri_Salute.Oro * (player.Ricerca_Torri_Salute + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Torri_Salute.TempoRicerca * (player.Ricerca_Torri_Salute + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Torri Difesa|[black]Aumenta il numero massimo di punti difesa della struttura.\n" +
                $"Costo ricerca difesa torri lv: {player.Ricerca_Torri_Difesa + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Torri_Difesa.Cibo * (player.Ricerca_Torri_Difesa + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Torri_Difesa.Legno * (player.Ricerca_Torri_Difesa + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Torri_Difesa.Pietra * (player.Ricerca_Torri_Difesa + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Torri_Difesa.Ferro * (player.Ricerca_Torri_Difesa + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Torri_Difesa.Oro * (player.Ricerca_Torri_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Torri_Difesa.TempoRicerca * (player.Ricerca_Torri_Difesa + 1))}\n\n");

            Send(player.guid_Player, $"Descrizione|Ricerca Castello Livello|[black]Aumenta il numero massimo di unità che può ospitare la struttura.\n" +
                $"Costo ricerca livello castello: {player.Ricerca_Castello_Guarnigione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Castello_Livello.Cibo * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Castello_Livello.Legno * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Castello_Livello.Pietra * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Castello_Livello.Ferro * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Castello_Livello.Oro * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Castello_Livello.TempoRicerca * (player.Ricerca_Castello_Guarnigione + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Castello Guarnigione|[black]Aumenta il numero massimo di unità che può ospitare la struttura.\n" +
                $"Costo ricerca guarnigione castello lv: {player.Ricerca_Castello_Guarnigione + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Castello_Guarnigione.Cibo * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Castello_Guarnigione.Legno * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Castello_Guarnigione.Pietra * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Castello_Guarnigione.Ferro * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Castello_Guarnigione.Oro * (player.Ricerca_Castello_Guarnigione + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Castello_Guarnigione.TempoRicerca * (player.Ricerca_Castello_Guarnigione + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Castello Salute|[black]Aumenta il numero massimo di punti salute della struttura.\n" +
                $"Costo ricerca salute castello lv: {player.Ricerca_Castello_Salute + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Castello_Salute.Cibo * (player.Ricerca_Castello_Salute + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Castello_Salute.Legno * (player.Ricerca_Castello_Salute + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Castello_Salute.Pietra * (player.Ricerca_Castello_Salute + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Castello_Salute.Ferro * (player.Ricerca_Castello_Salute + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Castello_Salute.Oro * (player.Ricerca_Castello_Salute + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Castello_Salute.TempoRicerca * (player.Ricerca_Castello_Salute + 1))}\n\n");
            Send(player.guid_Player, $"Descrizione|Ricerca Castello Difesa|[black]Aumenta il numero massimo di punti difesa della struttura.\n" +
                $"Costo ricerca difesa castello lv: {player.Ricerca_Castello_Difesa + 1}\n\n" +
                $"Cibo: [icon:cibo]{Ricerca.Citta.Castello_Difesa.Cibo * (player.Ricerca_Castello_Difesa + 1):#,0}\n" +
                $"Legno: [icon:legno]{Ricerca.Citta.Castello_Difesa.Legno * (player.Ricerca_Castello_Difesa + 1):#,0}\n" +
                $"Pietra: [icon:pietra]{Ricerca.Citta.Castello_Difesa.Pietra * (player.Ricerca_Castello_Difesa + 1):#,0}\n" +
                $"Ferro: [icon:ferro]{Ricerca.Citta.Castello_Difesa.Ferro * (player.Ricerca_Castello_Difesa + 1):#,0}\n" +
                $"Oro: [icon:oro]{Ricerca.Citta.Castello_Difesa.Oro * (player.Ricerca_Castello_Difesa + 1):#,0}\n" +
                $"Tempo: [icon:tempo]{player.FormatTime(Ricerca.Citta.Castello_Difesa.TempoRicerca * (player.Ricerca_Castello_Difesa + 1))}\n\n");

            //Shop Descrizioni
            Send(player.guid_Player, $"Descrizione|Shop GamePass Base|[black]Tramite l'acquito di questo pacchetto 'GamePass Base'...\n" +
                $"Attualmente non disponibili vantaggi. Ha una durata di [title]30 [black]giorni.\n");
            Send(player.guid_Player, $"Descrizione|Shop GamePass Avanzato|[black]Tramite l'acquito di questo pacchetto 'GamePass Avanzato' ...\n" +
                $"Attualmente non disponibili vantaggi. Ha una durata di [title]30 [black]giorni.\n");

            Send(player.guid_Player, $"Descrizione|Shop Vip 1|[black]Tramite l'acquito di questo pacchetto 'Vip 1'," +
                $"Il tempo del vip verrà incrementato di: [black]{player.FormatTime(Variabili_Server.Shop.Vip_1.Reward)}.\n Un massimo di [ferroScuro]2 [black]giorni può essere accumulato, acquistando più pacchetti\n");
            Send(player.guid_Player, $"Descrizione|Shop Vip 2|[black]Tramite l'acquito di questo pacchetto 'Vip 2'," +
                $"Il tempo del vip verrà incrementato di: [black]{player.FormatTime(Variabili_Server.Shop.Vip_2.Reward)}. Una volta comfermata la transazione dalla blockchain i diamanti verranno accreditati immediatamente.\n Un massimo di [ferroScuro]2 [black]giorni può essere accumulato, acquistando più pacchetti\n");

            Send(player.guid_Player, $"Descrizione|Shop Costruttore 24h|[black]Tramite l'acquito di questo pacchetto 'Costruttore 24H' è possibile richiedere un costruttore aggiuntivo.\n" +
                $"Il tempo di costruzione verrà incrementato di: [black]{player.FormatTime(Variabili_Server.Shop.Costruttore_24h.Reward)}.\n Un massimo di [ferroScuro]3 [black]giorni può essere accumulato, acquistando più pacchetti\n");
            Send(player.guid_Player, $"Descrizione|Shop Costruttore 48h|[black]Tramite l'acquito di questo pacchetto 'Costruttore 48H' è possibile richiedere un costruttore aggiuntivo.\n" +
                $"Il tempo di costruzione verrà incrementato di: [black]{player.FormatTime(Variabili_Server.Shop.Costruttore_48h.Reward)}.\n Un massimo di [ferroScuro]3 [black]giorni può essere accumulato, acquistando più pacchetti\n");

            Send(player.guid_Player, $"Descrizione|Shop Reclutatore 24h|[black]Tramite l'acquito di questo pacchetto 'Reclutatore 24h' è possibile richiedere un reclutatore aggiuntivo.\n" +
                $"Il tempo di reclutamento verrà incrementato di: [black]{player.FormatTime(Variabili_Server.Shop.Reclutatore_24h.Reward)}.\n Un massimo di [ferroScuro]2 [black]giorni può essere accumulato, acquistando più pacchetti\n");
            Send(player.guid_Player, $"Descrizione|Shop Reclutatore 48h|[black]Tramite l'acquito di questo pacchetto 'Reclutatore 48h' è possibile richiedere un reclutatore aggiuntivo.\n" +
                $"Il tempo di reclutamento verrà incrementato di: [black]{player.FormatTime(Variabili_Server.Shop.Reclutatore_48h.Reward)}.\n Un massimo di [ferroScuro]2 [black]giorni può essere accumulato, acquistando più pacchetti\n");

            Send(player.guid_Player, $"Descrizione|Shop Scudo Pace 8h|[black]Tramite l'acquito di questo pacchetto 'scudo della pace 8h', si otterrà una protezione dagli attacchi degli altri giocatori.\n" +
                    $"Il tempo dello scudo verrà incrementato di: [black]{player.FormatTime(Variabili_Server.Shop.Scudo_Pace_8h.Reward)} al tempo disponibile dello scudo.\n Un massimo di [ferroScuro]7 [black]giorni può essere accumulato, acquistando più pacchetti\n");
            Send(player.guid_Player, $"Descrizione|Shop Scudo Pace 24h|[black]Tramite l'acquito di questo pacchetto 'scudo della pace 24h', si otterrà una protezione dagli attacchi degli altri giocatori.\n" +
                $"Il tempo dello scudo verrà incrementato di: [black]{player.FormatTime(Variabili_Server.Shop.Scudo_Pace_24h.Reward)} al tempo disponibile dello scudo.\n Un massimo di [ferroScuro]7 [black]giorni può essere accumulato, acquistando più pacchetti\n");
            Send(player.guid_Player, $"Descrizione|Shop Scudo Pace 72h|[black]Tramite l'acquito di questo pacchetto 'scudo della pace 72h', si otterrà una protezione dagli attacchi degli altri giocatori.\n" +
                $"Il tempo dello scudo verrà incrementato di: [black]{player.FormatTime(Variabili_Server.Shop.Scudo_Pace_72h.Reward)} al tempo disponibile dello scudo.\n Un massimo di [ferroScuro]7 [black]giorni può essere accumulato, acquistando più pacchetti\n");
        }
    }
}
