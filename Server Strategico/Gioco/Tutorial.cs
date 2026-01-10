using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server_Strategico.Gioco.Esercito;

namespace Server_Strategico.Gioco
{
    internal class Tutorial
    {
        public class dati
        {
            public int StatoTutorial { get; set; }
            public string Descrizione { get; set; }
        }
        public class Parti
        {
            public static dati Introduzione = new dati
            {
                StatoTutorial = 1,
                Descrizione = "Benvenuto su 'Warrior & Wealth', vedo che sei un nuovo giocatore perfetto, questo tutorial sarà perfetto per te!\n" +
                "Prima di inziare lasciami dire... Bevi responsabilmente! Non guidare mentre bevi o potresti rovesciare la tua birra!\n" +
                "Forse è meglio pasasre alle cose serie, posso finalmente guidarti passo passo verso questo nuovo mondo. Grandi avventure ti aspettano.\n\n" +
                "'Warrior & Wealth', è diviso in due sotto giochi; La prima parte ti permetterà di acquistare dei 'Feudi' che ti permetteranno di ricevere i 'Tributi del Feudo'," +
                "grazie all'immensità del regno e della benevolenza dell'imperatore, ogni secondo porterà verso le casse del vostro villaggio, ma questo è un aspetto che vedremo a momento debito.\n\n" +
                "La seconda parte, come forse avrai capito... Ma se non lo avessi già capito mi spiego meglio... Sei stato scelto, si hai capito bene, sei stato scelto per gestire questo piccolo villaggio," +
                "miraccomando, imperbie imcombono su queste terre, ti scongiuro fai molta attenzione. Prenditi cura della tua gente, e vedrai che i tuoi sudditi ti seguiranno ovunque. Quando sei pronto" +
                "possiamo andare avanti con il tutorial e mostrarti cosa puoi fare. Ah ah già ancora non ti ho spuegato cosa devi fare ^.^ benissimo, dovrai gestire il villaggio che l'imeratore ti ha assegnato," +
                "le risorse saranno il tuo principale obiettivo senza di esse tutto è perduto, ti serviranno per la costruzione di edifici, strutture militari, per la ricerca, la riparazione di strutture difensive" +
                "ed altro ancora.\n" +
                "Molti saranno gli aspetti, ma evitiamo di accumulare caratteri in questo testo.\n\n" +
                "Inziamo!"
                
            };
            public static dati Risorse_1 = new dati
            {
                StatoTutorial = 2,
                Descrizione = "Non preoccuparti, al momento è tutto vuoto, ma a breve le cose ti verranno mostrate..."
            };
            public static dati Risorse_2 = new dati
            {
                StatoTutorial = 2,
                Descrizione = "In alto puoi vedere qualcosa è la barra delle risorse, queste era ciò di cui parlavamo prima, queste sono indispensabili in ordine troviamo, 'Cibo, Legno, Pietra, Ferro," +
                "Oro, Popolazione, Esperienza e Livello', queste risorse servono per tutto, ti posso solo lasciare immaginare.\n" +
                "Portando il puntatore del mouse sulle varie icone e lasciandolo, una breve descrizione a tendina apparirà, ti mostrerà cosa stai vedendo, una breve descrizione e tutte le informazioni" +
                "che ti possono essere molto utili durante la gestione del tuo villaggio. Puoi fare questo con quasi tutte le icone che incontrerai in questa schermata e nelle altre, ricordatelo," +
                "oltre a questo, passiamo con l'introdurre le risorse militari premendo l'icona 'ICONA' potrai vedere in ordine 'Spade, Lance, Archi, Scudi, Armature, Frecce', ti lascio immaginare " +
                "a cosa possano servire, l'unica su cui mi vorrei leggermente sofferare sono le frecce, esse essendo un componente da lancio verranno utilizzate dai tuoi arceri e dalle catapulte quindi nei momenti" +
                "di tensione, consiglio di dargli uno sguardo, per evitare di rinanerne sguarniti.\n" +
                "Tutte le risorse possono essere prodotte direttamente nel tuo villaggio oppure ottenute attraverso altre vie... "
            };
            public static dati Risorse_3 = new dati
            {
                StatoTutorial = 3,
                Descrizione = "La seconda barra piu in basso che è appena apparsa ti permetterà di visualizzare, per il momento una sola icona i 'Tributi del feudo', i tributi sono legati ai feudi di cui sei amministratore" +
                $"permmetteranno di generare ogni secondo dei tributi che ti verranno accreditati e potrai utilizzarli nello shop, che ancora non puoi vedere, oppure una volta raggiunta la soglia minima di " +
                $"{Variabili_Server.prelievo_Minimo} Tributi, il cambio è di 1 Tributo per 1 USDT, dovrai fornire l'indirizzo USDT a cui inviare quando sarai pronto."
            };
            public static dati TerreniVirtuali = new dati
            {
                StatoTutorial = 4,
                Descrizione = "Eccoci finalmente, come puoi vedere una nuova schermata è apparsa sulla sinistra, qui potrai vedere i terreni virtuali che possiedi, oltre a poterne acquistare di nuove" +
                "ogni terreno dispone di caratteristiche uniche come i tributi generati e la loro rarità, potrai scoprire maggiori informazioni sui terreni virtuali premendo con il tasto sinistro del mouse" +
                "sull'icona 'ICONA'"
            };
            public static dati AcquistaTerreno = new dati
            {
                StatoTutorial = 5,
                Descrizione = "Perfetto, iniziamo tentando la fortuna, come puoi ben vedere altre cose sono appena apparse, arricchendo la tua interfaccia, possiamo vedere, in alto tra le risorse, una nuova icona," +
                $"i 'Diamanti Viola [icon:diamantiViola]' ed in basso, sotto i terreni virtuali vedra un belissimo pulsante, 'Acquista {Strutture.Edifici.Terreni_Virtuali.Diamanti_Viola}[icon:diamantiViola]'.\n" +
                $"I [icon:diamantiViola] possono essere ottenuti tramite lo 'Shop', il completamento delle 'Quest' e tramite il combattimento e possono essere scambiati per [icon:diamantiBlu], " +
                $"vedremo piu avanti nel dettaglio le varie sezioni, ma al momento fermiamoci qui non vorrei osare troppo.\n Per questo tutorial ti sono già stati forniti 150[icon:diamantiViola]" +
                $"premi il pulsante per inziare la tua avventura su 'Warrior & Wealth' ed acuista il tuo primo terreno virtuale, goditi finalmente dopo tutta quaesta immensa fatica e dedizione i proventi," +
                $"provenenti direttamente dal feudo annesso. Potrai vedere come i tributi aumentano con il passare del tempo. come già accennavo i tributi possono anche essere spesi all'interno dello 'Shop'," +
                $"per ottenere grandi vantaggi contro gli altri villaggi circostanti."
            };
            public static dati Costruzione_1 = new dati
            {
                StatoTutorial = 6,
                Descrizione = "Perfetto, iniziamo a costruire qualcosa oltre ai terreni virtuali, tra poco ti verrà mostrato come fare, utilizzando la tua popolazione e le risorse che hai, ah no non ancora," +
                "che in futuro avrai, tutto è possibile.\n" +
                "Naturalmente solo per questa volta, ti forniremo le risorse per farlo, introducendoti le varie strutture disponibili. Quando sei pronto possiamo andare avanti."
            };
            public static dati Costruzione = new dati
            {
                StatoTutorial = 6,
                Descrizione = "Perfetto, ci siamo, inizamo a costruire, come puoi vedere un nuvo punsate 'Costruisci' è apparso, insieme ad un nuovo frammento dell'interfaccia. Queste sono le strutture principali" +
                "per la produzione di risorse civili, fondamentali per mantenere e rendere il proprio villaggio a capo del regno, quindi pensa bene alla tua strategia e ragiona su come muoverti al meglio." +
                "Come puoi vedere nel riquadro delle strutture è situata unìicona 'ICONA', tramite questa sarà possibile passare agli edifici militari, atti alla produzione di risorse militari, lo vedremo più avanti."
                
            };
            public static dati Costruisci_Fattoria = new dati
            {
                StatoTutorial = 7,
                Descrizione = "Finalmente ci siamo, ti verrano fornite tutte le risorse per questa impresa, ma sarà l'unica volta, fuori da questo tutorial, se sbaglierai, le tue azioni saranno" +
                "irreversibili, miraccomando. Come puoi vedere accanto alla struttura 'Fattoria' oltre alla sua icona raffigurativa troviamo il numero di 'Edifici Disponibili', (strutture" +
                "già create e produttive) ed il numero di 'Edifici in coda' (numero di strutture in attesa di spazio per essere costruite), puoi vedere il tempo rimanente in fondo alla schermata 'Strutture Civili'" +
                "vedrai il tempo necessario apparire non appena inizierai la prima costruzione.\n\n" +
                "Premi il pulsante 'Costruzione' una nuova schermata si aprirà dove potrai gestire tutto. Lascia aperta la schermata 'Costruisci', i prossimi passi ti guideranno nella costruzione di" +
                "tutte le strutture necessare. Per una maggiore comodità, sposta la schermata 'Costruisci'in una posizione comoda così puoi avere la situazione sotto controllo.\n\n" +
                "Costruisci una 'Fattoria[icon:fattoria]'."
            };
            public static dati Costruisci_Segheria = new dati
            {
                StatoTutorial = 8,
                Descrizione = "Perfetto!\n\nCostruisci una 'Segheria[icon:segheria]'"
            };
            public static dati Costruisci_Cava = new dati
            {
                StatoTutorial = 9,
                Descrizione = "Perfetto!\n\nCostruisci una 'Cava di pietra[icon:cava]'"
            };
            public static dati Costruisci_Miniera_Ferro = new dati
            {
                StatoTutorial = 10,
                Descrizione = "Perfetto!\n\nCostruisci una 'Miniera di ferro[icon:minieraFerro]'"
            };
            public static dati Costruisci_Miniera_Oro = new dati
            {
                StatoTutorial = 11,
                Descrizione = "Perfetto!\n\nCostruisci una 'Miniera d'oro[icon:minieraOro]'"
            };
            public static dati Costruisci_Abitazioni = new dati
            {
                StatoTutorial = 12,
                Descrizione = "Perfetto!\n\nCostruisci un 'Abitazione[icon:abitazione]'"
            };
            public static dati Costruisci_Militare_Spade = new dati
            {
                StatoTutorial = 13,
                Descrizione = "Perfetto, ora possiamo passare alle 'Strutture Militari', vale lo stesso discorso anche per loro, solo che queste produrranno risorse militari, fondamentali per le tue " +
                "unità come 'Guerrieri, Lanceri, Arceri e Catapulte'. Successivamente parleremo anche di loro più nel dettaglio più avanti.\n\n" +
                "Costruisci un 'Worshop Spade[icon:workshopSpade]'"
            };
            public static dati Costruisci_Militare_Lance = new dati
            {
                StatoTutorial = 14,
                Descrizione = "Perfetto\n\n" +
                "Costruisci un 'Worshop Lance[icon:workshopLance]'"
            };
            public static dati Costruisci_Militare_Archi = new dati
            {
                StatoTutorial = 15,
                Descrizione = "Perfetto\n\n" +
                "Costruisci un 'Worshop Scudi[icon:workshopArchi]'"
            };
            public static dati Costruisci_Militare_Scudi = new dati
            {
                StatoTutorial = 16,
                Descrizione = "Perfetto\n\n" +
                "Costruisci un 'Worshop Scudi[icon:workshopScudi]'"
            };
            public static dati Costruisci_Militare_Frecce = new dati
            {
                StatoTutorial = 17,
                Descrizione = "Perfetto\n\n" +
                "Costruisci un 'Worshop Frecce[icon:workshopFrecce]'"
            };
            public static dati Caserme = new dati
            {
                StatoTutorial = 18,
                Descrizione = "Benissimo, hai completato la maggior parte delle strutture disponibili, miraccomando, non sottovalutare la loro importanza.\n" +
                "Ora passiamo alle 'Caserme', sono simili alle strutture civili, ma servono a mantenere una forza militare attiva, pronta per ogni evenienza e perchè no, anche per la difesa," +
                "QUesto aspetto lo vedremo più avanti quando parleremo della città.\n\n" +
                "Cosa devi sapere delle caserme? Sono presenti 4 tipi 'Caserma dei guerrieri, Lanceri, Arceri e Catapulte'. Ognuna di esse può ospitare un certo numero di uomini e consuma delle" +
                "risorse giornaliere quindi cerca di mantenere sempre d'occhio la tua produzione prima della loro costruzione in massa.\n\n" +
                "Per questo motivo non ti farò costruire delle caserme, lo farai tu stesso con il progredire del gioco."
            };
            public static dati Unità_Militari = new dati
            {
                StatoTutorial = 19,
                Descrizione = "Ottimo, siamo alla fine della schermata costruisci, come hai ben visto questa schermata racchiude tutto, anche le unità militari, per quanto riguarda l'esercito è" +
                "fondamentale per proteggere i tuoi confini. Dopo questa fase passeremo alla 'Città', così potrai renderti conto di come è strutturato il tuo possedimento.\n\n" +
                "Queste sono le 4 'Unità Militari', Specularmente per quanto accade per le caserme abbiamo a disposizione 'Guerrieri, Lanceri, Arceri e Catappulte' di vario livello. Come per le " +
                "caserme anche le unità hanno le loro caratteristiche, che possono essere osservate nella schermata precedente, ma lo vederemo dopo questo, come 'Salute, Difesa, Attacco, Trasporto'" +
                "ed anche un mantenimento in 'Cibo ed Oro'.\n\n" +
                "Anche se fondamentali per il successo nelle campagne militari, consigli pruntenza quando ci si cimenta in un'addestramento di massa, cercando di garantire una produzione al loro" +
                "sostenimento, altrimenti potresti rimanere a corto di risorse. Hai le risorse necessarie, prima di chiudere costruisci una 'Fattoria'.\n\n" +
                "A questo punto sei pronto confermare e chiudere la schermata di costruzione per ritornare nell'interfaccia principale."
            };
            public static dati Scambia_Diamanti = new dati
            {
                StatoTutorial = 20,
                Descrizione = "Accolgo l'occasione della fattoria in costruzione per parlarti delle velocizzazioni, come funzionano? ti serviranno dei 'Diamanti Blu[icon:diamantiBlu]'," +
                "tranquillo ti darò dei diamanti viola per iniziare. Guarda, sopra tra le risorse, accanto hai diamanti viola abbiamo una nuova icona, i diamanti blu! oltre a questo, sotto i" +
                "terreni virtuali è apparto un nuovo pulsante, 'Scambia' premilo e scambia tutti i diamanti viola che hai!\n\n"
            };
            public static dati Velocizza = new dati
            {
                StatoTutorial = 21,
                Descrizione = "Usa questi diamanti appena ottenuti per velocizzare la costruzione, certo non la completerai, ma sarà un aiuto in più.\n" +
                "Sotto l'ultima icona delle strutture civili e reciprocamente per le unità militari anche se non visibili al momento, puoi vedere un timer ed un'icona," +
                "questo è il tempo necessario alla fine della costruzione o delle costruzioni se più di una è presente,l'icona affianco invece, se premuta permette di scegliere quanti " +
                "diamanti utilizzare per perlocizzare le strutture.\n\n" +
                "Fallo accelera la tua prima struttura!"
            };
            public static dati Città = new dati
            {
                StatoTutorial = 22,
                Descrizione = ""
            };
        }
    }
}
