using Strategico_V2;

namespace CriptoGame_Online.GUI
{
    public partial class Statistiche : Form
    {
        private CancellationTokenSource cts = new CancellationTokenSource();
        public Statistiche()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void Giocatore_Load(object sender, EventArgs e)
        {
            Task.Run(() => Gui_Update(cts.Token), cts.Token);
        }
        async void Gui_Update(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (groupBox1.IsHandleCreated && !groupBox1.IsDisposed)
                {
                    lbl_Giocatore_Testo.Invoke(new Action(() =>
                    {
                        lbl_Giocatore_Testo.Text = 
                        $"VIP: \n" +
                        $"GamepPass A: \n" +
                        $"GamepPass B: \n" +
                        $"Scudo della pace: \n" +
                        $"Costruttori: \n" +
                        $"Reclutatori: \n" +
                        $"Quest Mensile: \n" +
                        $"Barbari: \n\n" +
                        $"------- Potenza -------\n\n" +
                        $"Edifici: \n" +
                        $"Ricerca: \n" +
                        $"Esercito: \n" +
                        $"Totale: \n" +
                        $"------- Bonus -------\n\n" +
                        $"Costruzione: \n" +
                        $"Addestramento: \n" +
                        $"Ricerca: \n" +
                        $"Riparazione: \n" +
                        $"Produzione Risorse: \n" +
                        $"Capacità Trasporto: \n\n" +

                        $"Salute Strutture: \n" +
                        $"Difesa Strutture: \n" +
                        $"Guarnigione Strutture: \n\n" +

                        $"Attacco Guerrieri: \n" +
                        $"Attacco Lanceri: \n" +
                        $"Attacco Arceri: \n" +
                        $"Attacco Catapulte: \n\n" +

                        $"Salute Guerrieri: \n" +
                        $"Salute Lanceri: \n" +
                        $"Salute Arceri: \n" +
                        $"Salute Catapulte: \n\n" +

                        $"Difesa Guerrieri: \n" +
                        $"Difesa Lanceri: \n" +
                        $"Difesa Arceri: \n" +
                        $"Difesa Catapulte: \n";

                        lbl_Giocatore_Valore.Text =
                        $"{Variabili_Client.Utente.User_Vip_Tempo}\n" +
                        $"{Variabili_Client.Utente.User_GamePass_Base_Tempo}\n" +
                        $"{Variabili_Client.Utente.User_GamePass_Avanzato_Tempo}\n" +
                        $"{Variabili_Client.Utente.Scudo_Pace_Tempo}\n" +
                        $"{Variabili_Client.Utente.Costruttori_Tempo}\n" +
                        $"{Variabili_Client.Utente.Reclutatori_Tempo}\n" +
                        $"{Variabili_Client.Utente.Montly_Quest_Tempo}\n" +
                        $"{Variabili_Client.Utente.Barbari_Tempo}\n\n\n\n" +

                        $"{Variabili_Client.Statistiche.Potenza_Edifici}\n" +
                        $"{Variabili_Client.Statistiche.Potenza_Ricerca}\n" +
                        $"{Variabili_Client.Statistiche.Potenza_Esercito}\n" +
                        $"{Variabili_Client.Statistiche.Potenza_Totale}\n\n\n\n" +

                        $"{Variabili_Client.Bonus.Bonus_Costruzione} \n" +
                        $"{Variabili_Client.Bonus.Bonus_Addestramento} \n" +
                        $"{Variabili_Client.Bonus.Bonus_Ricerca} \n" +
                        $"{Variabili_Client.Bonus.Bonus_Riparazione} \n" +
                        $"{Variabili_Client.Bonus.Bonus_Produzione_Risorse} \n" +
                        $"{Variabili_Client.Bonus.Bonus_Capacità_Trasporto} \n\n" +

                        $"{Variabili_Client.Bonus.Bonus_Salute_Strutture} \n" +
                        $"{Variabili_Client.Bonus.Bonus_Difesa_Strutture} \n" +
                        $"{Variabili_Client.Bonus.Bonus_Guarnigione_Strutture} \n\n" +

                        $"{Variabili_Client.Bonus.Bonus_Attacco_Guerrieri} \n" +
                        $"{Variabili_Client.Bonus.Bonus_Attacco_Lanceri} \n" +
                        $"{Variabili_Client.Bonus.Bonus_Attacco_Arceri} \n" +
                        $"{Variabili_Client.Bonus.Bonus_Attacco_Catapulte} \n\n" +

                        $"{Variabili_Client.Bonus.Bonus_Salute_Guerrieri} \n" +
                        $"{Variabili_Client.Bonus.Bonus_Salute_Lanceri} \n" +
                        $"{Variabili_Client.Bonus.Bonus_Salute_Arceri} \n" +
                        $"{Variabili_Client.Bonus.Bonus_Salute_Catapulte} \n\n" +

                        $"{Variabili_Client.Bonus.Bonus_Difesa_Guerrieri} \n" +
                        $"{Variabili_Client.Bonus.Bonus_Difesa_Lanceri} \n" +
                        $"{Variabili_Client.Bonus.Bonus_Difesa_Arceri} \n" +
                        $"{Variabili_Client.Bonus.Bonus_Difesa_Catapulte} \n";

                        lbl_Statistiche.Text =
                        $"Strutture civili costruite: \r\n" +
                        $"Strutture militari costruite: \r\n" +
                        $"Caserme costruite: \r\n" +
                        $"Risorse utilizzate: \r\n" +
                        $"Tempo addestramento: \r\n" +
                        $"Tempo costruzione: \r\n" +
                        $"Tempo ricerca: \r\n" +
                        $"Tempo sottratto Diamanti: \r\n" +
                        $"Frecce utilizzate: \r\n" +
                        $"Danno HP Barbari: \r\n" +
                        $"Danno DEF Barbari: \r\n" +
                        $"Quest completate: \r\n\r\n" +

                        $"Risorse razziate: \r\n" +
                        $"Barbari sconfitti: \r\n" +
                        $"Battaglie Vinte: \r\n" +
                        $"Battaglie Perse: \r\n" +
                        $"Attacchi Effettuati (PVP): \r\n" +
                        $"Attacchi Subiti (PVP): \r\n" +
                        $"Accampamenti sconfitti: \r\n" +
                        $"Città sconfitte: \r\n\r\n" +

                        $"Unità addestrate: \r\n" +
                        $"Unità Eliminate: \r\n" +
                        $"Guerrieri eliminati: \r\n" +
                        $"Lanceri eliminati: \r\n" +
                        $"Arceri eliminati: \r\n" +
                        $"Catapulte eliminate: \r\n" +
                        $"Unità Perse: \r\n" +
                        $"Guerrieri persi: \r\n" +
                        $"Lanceri persi: \r\n" +
                        $"Arceri persi: \r\n" +
                        $"Catapulte perse: \r\n";

                        lbl_Statistiche_Valore.Text =
                        $"{Variabili_Client.Statistiche.Strutture_Civili_Costruite}\r\n" +
                        $"{Variabili_Client.Statistiche.Strutture_Militari_Costruite}\r\n" +
                        $"{Variabili_Client.Statistiche.Caserme_Costruite}\r\n" +
                        $"{Variabili_Client.Statistiche.Risorse_Utilizzate}\r\n" +

                        $"{Variabili_Client.Statistiche.Tempo_Addestramento_Totale}\r\n" +
                        $"{Variabili_Client.Statistiche.Tempo_Costruzione_Totale}\r\n" +
                        $"{Variabili_Client.Statistiche.Tempo_Ricerca_Totale}\r\n" +
                        $"{Variabili_Client.Statistiche.Tempo_Sottratto_Diamanti}\r\n" +
                        $"{Variabili_Client.Statistiche.Frecce_Utilizzate}\r\n" +
                        $"{Variabili_Client.Statistiche.Danno_HP_Barbaro}\r\n" +
                        $"{Variabili_Client.Statistiche.Danno_DEF_Barbaro}\r\n" +
                        $"{Variabili_Client.Statistiche.Quest_Completate}\r\n\r\n" +

                        $"{Variabili_Client.Statistiche.Risorse_Razziate}\r\n" +
                        $"{Variabili_Client.Statistiche.Barbari_Sconfitti}\r\n" +
                        $"{Variabili_Client.Statistiche.Battaglie_Vinte}\r\n" +
                        $"{Variabili_Client.Statistiche.Battaglie_Perse}\r\n" +
                        $"{Variabili_Client.Statistiche.Attacchi_Effettuati_PVP}\r\n" +
                        $"{Variabili_Client.Statistiche.Attacchi_Subiti_PVP}\r\n" +
                        $"{Variabili_Client.Statistiche.Accampamenti_Barbari_Sconfitti}\r\n" +
                        $"{Variabili_Client.Statistiche.Città_Barbare_Sconfitte}\r\n\r\n" +

                        $"{Variabili_Client.Statistiche.Unità_Addestrate}\r\n" +
                        $"{Variabili_Client.Statistiche.Unità_Eliminate}\r\n" +
                        $"{Variabili_Client.Statistiche.Guerrieri_Eliminate}\r\n" +
                        $"{Variabili_Client.Statistiche.Lanceri_Eliminate}\r\n" +
                        $"{Variabili_Client.Statistiche.Arceri_Eliminate}\r\n" +
                        $"{Variabili_Client.Statistiche.Catapulte_Eliminate}\r\n" +
                        $"{Variabili_Client.Statistiche.Unità_Perse}\r\n" +
                        $"{Variabili_Client.Statistiche.Guerrieri_Persi}\r\n" +
                        $"{Variabili_Client.Statistiche.Lanceri_Persi}\r\n" +
                        $"{Variabili_Client.Statistiche.Arceri_Persi}\r\n" +
                        $"{Variabili_Client.Statistiche.Catapulte_Perse}\r\n";

                    }));
                }
                await Task.Delay(1000); // meglio di Thread.Sleep
            }

        }
    }
}
