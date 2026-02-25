using static Server_Strategico.Gioco.Esercito;

namespace Server_Strategico.ServerData.Moduli
{
    internal class Tutorial
    {
        public class dati
        {
            public int StatoTutorial { get; set; }
            public string Obiettivo { get; set; }
            public string Descrizione { get; set; }
        }
        public class Parti
        {
            public static dati Introduzione_1 = new dati
            {
                StatoTutorial = 1,
                Obiettivo = "Premere click sinistro sul testo",
                Descrizione = "Non preoccuparti: in questo tutorial imparerai a muoverti in questo nuovo mondo e in tutte le schermate principali." +
                "Come puoi vedere, al momento non c'è molto da osservare… ma tra non molto tutto sarà più chiaro forse..." +
                "Quello che vedi adesso sono due schermate. La prima, quella più grande, è la schermata principale: ti permetterà di gestire praticamente tutto." +
                "La seconda, dove stai leggendo questo testo, è la schermata del tutorial." +
                "Rimarrà attiva fino al suo completamento. Concentriamoci un attimo su quest’ultima." +
                "Qui potrai vedere la progressione del tutorial, gli obiettivi che devi raggiungere per andare avanti… e anche una breve descrizione." +
                "In questo caso, per completare l’obiettivo, fai semplicemente click con il tasto sinistro su questo testo.Quando sei pronto, completa l’obiettivo e andiamo avanti."
            };
            public static dati Introduzione_2 = new dati
            {
                StatoTutorial = 2,
                Obiettivo = "Premere click sinistro sul testo",
                Descrizione = "Benvenuto su “[title]Warrior & Wealth[/title]”! " +
                "Vedo che sei un nuovo giocatore… ottimo. Questo tutorial sarà perfetto per te! Prima di iniziare, lasciami dire una cosa… " +
                "bevi responsabilmente! Non guidare mentre bevi, o potresti rovesciare la tua birra! Forse è meglio passare alle cose serie… " +
                "Posso finalmente guidarti, passo dopo passo, in questo nuovo mondo. Grandi avventure ti aspettano. " +
                "“[title]Warrior & Wealth[/title]” è diviso in due modalità. La prima parte ti permetterà di acquistare dei Feudi, che ti faranno ottenere i [icon:dollariVirtuali]Tributi del Feudo." +
                "Grazie all’immensità del regno e alla benevolenza dell’Imperatore, ogni secondo nuove ricchezze finiranno nelle casse del tuo villaggio… " +
                "Ma questo è un aspetto che vedremo al momento giusto.La seconda parte… come forse avrai capito… riguarda te. E se così non fosse, mi spiego meglio." +
                "Sei stato scelto. Sì, hai capito bene. Sei stato scelto per gestire questo piccolo villaggio. Mi raccomando… " +
                "le intemperie incombono su queste terre, e i pericoli sono ovunque. Fai molta attenzione. Quando sei pronto, possiamo andare avanti con il tutorial e mostrarti cosa puoi fare." +
                "Ah, già… ancora non ti ho spiegato cosa devi fare davvero. Dovrai gestire il villaggio che l’Imperatore ti ha assegnato." +
                "Le risorse saranno il tuo principale obiettivo: senza di esse, tutto è perduto. Ti serviranno per costruire edifici, strutture militari, per la ricerca… per riparare le difese, e molto altro ancora." +
                "Gli aspetti da imparare saranno molti… ma non riempiamo questo testo di troppe parole.Iniziamo! ...Quando sei pronto, completa l'obiettivo ed andiamo avanti."
            };
            public static dati Risorse = new dati
            {
                StatoTutorial = 3,
                Obiettivo = "Premere click sinistro sul testo",
                Descrizione = "In alto puoi vedere qualcosa, questa è la barra delle risorse, quello che dicevamo prima, queste sono indispensabili ed in ordine troviamo, '[icon:cibo]Cibo, [icon:legno]Legno, [icon:pietra]Pietra, [icon:ferro]Ferro," +
                "[icon:oro]Oro, [icon:popolazione]Popolazione, [icon:xp]Esperienza e [icon:lv]Livello', queste risorse servono per tutto, ti posso solo lasciare immaginare a cosa possano servire." +
                "Portando il puntatore del mouse sopra le singole icone, e lasciandolo su di esso per un breve istante, noterai una breve descrizione a tendina che apparirà, ti mostrerà cosa stai osservando, una breve descrizione e tutte le informazioni" +
                "che ti possono servire, le troverai al suo interno durante la gestione del tuo villaggio. Puoi fare questo, con quasi tutte le icone che incontrerai in questa schermata e nelle altre, ricordatelo." +
                "Passiamo con l'introdurre le risorse militari, premendo l'icona [icon:scambio], potrai vedere nel seguente ordine '[icon:spade]Spade, [icon:lance]Lancie, [icon:archi]Archi, [icon:scudi]Scudi, [icon:armature]Armature e [icon:frecce]Frecce', la raffigurazione delle singole icone dovrebbe essere già più che esaustivo, ma ti lascio immaginare " +
                "a cosa potrebbero servire, l'unica su cui mi vorrei sofferare meglio sono le [icon:frecce]frecce, esse essendo un componente da lancio, verranno utilizzate dai tuoi arceri e catapulte, quindi nei momenti" +
                "di maggiore tensione, consiglio di prestare attenzione alla loro quantità nei tuoi magazzini, dagli uno sguardo, per evitare di rinanerne sguarniti nel momento del bisogno." +
                "Tutte le risorse possono essere prodotte direttamente nel tuo villaggio, oppure ottenute attraverso altre vie..." +
                "Quando sei pronto, completa l'obiettivo ed andiamo avanti."
            };
            public static dati Diamanti_Viola = new dati
            {
                StatoTutorial = 4,
                Obiettivo = "Premere click sinistro sul testo",
                Descrizione = "Sempre in alto, sotto alla barra delle risorse puoi vedere che una seconda barra piu in basso è appena apparsa, ti permetterà di visualizzare, per il momento, " +
                "una sola icona i '[icon:diamanteBlu][viola]Diamanti Viola[/viola]', soffermiamoci un'istante. " +
                "I [icon:diamanteBlu][viola]Diamanti Viola[/viola] sono fondamentali per la realizzazione di 'feudi', questi li vedremo piu avanti non ti preoccupare, possono essere spesi nello 'Shop', " +
                "per ottenere vantaggi che accelereranno la progressione in questo regno, lo shop naturalmente sarà presto visibile, possono anche essere scambiati in Diamanti Blu, ma lo vedremo più avanti, " +
                "questa risorsa è al primo posto epr scarsità, quindi pondera bene la tua decisione prima di spenderli, non è facile ottenerli, ma lo è altrettanto spenderli... " +
                "Come puoi ottenere dei diamanti viola ti stai domandando? " +
                "Bene potrai ottenerli in numerosi modi, attraverso le quest mensili, nello shop, oppure vinti in battaglie contro 'Villaggi' e 'Città' barbare, " +
                "o addirittura attaccando e depredando le risorse di altri giocatori, ma per questo dovrai ancora aspettare, è troppo presto... " +
                "Prima concentramoci sul resto. " +
                "Quando sei pronto, completa l'obiettivo ed andiamo avanti.."
            };
            public static dati Diamanti_Blu = new dati
            {
                StatoTutorial = 5,
                Obiettivo = "Premere click sinistro sul testo. ",
                Descrizione = "Guarda… accanto ai Diamanti Viola è apparsa una nuova icona. " +
                "Raffigura i Diamanti Blu.Già… stavo quasi per dimenticarmene. Parliamo di loro. " +
                "I [blu]Diamanti Blu[/blu] [icon:diamanteBlu] sono molto simili alla loro controparte [viola]viola[/viola], ma con una piccola differenza: sono la seconda risorsa più rara del regno. " +
                "Questi diamanti, però, tendono a portare valore e vantaggi alla tua città.Vengono usati principalmente per accelerare: Costruzioni, l’addestramento delle truppe e le ricerche. " +
                "Tutte cose fondamentali per far crescere il tuo villaggio… e renderlo più forte. " +
                "Naturalmente, non sono indispensabili per progredire… ma possono darti un grande aiuto, soprattutto nei momenti più importanti. Inoltre, " +
                "puoi spenderli nello Shop per acquistare potenti incantesimi. Uno dei più utili è lo Scudo della Pace. " +
                "Grazie agli illustri maghi del regno, questo incantesimo creerà un enorme scudo blu attorno ai confini dei tuoi domìni… " +
                "e ti proteggerà dagli attacchi provenienti da altre città, sparse e lontane nel regno." +
                "Quando sei pronto, completa l’obiettivo… e andiamo avanti."
            };
            public static dati TributiFeudo = new dati
            {
                StatoTutorial = 6,
                Obiettivo = "Premere click sinistro sul testo.",
                Descrizione = "Sembra che sia appena apparsa un’ulteriore risorsa. Ora puoi visualizzare l’icona dei Tributi del Feudo. " +
                "La quantità di tributi che riceverai dipenderà dal numero di Feudi che amministri… e dal loro livello di rarità. " +
                "Ogni feudo genera tributi ogni secondo, e l’importo verrà accreditato automaticamente. " +
                "Potrai usare questi tributi nello Shop. Purtroppo per ora non puoi ancora vederlo… ma lo scopriremo più avanti. " +
                "In alternativa, potrai anche prelevarli, una volta raggiunta la soglia minima richiesta. " +
                "Il cambio attuale è di 1 Tributo = 1 USDT." +
                "Quando avrai raggiunto la soglia, dovrai inserire l’indirizzo USDT su cui desideri ricevere il prelievo." +
                "Quando sei pronto, completa l’obiettivo… e andiamo avanti."
            };
            public static dati Feudi = new dati
            {
                StatoTutorial = 7,
                Obiettivo = "Premere click sinistro sul testo. ",
                Descrizione = "Eccoci finalmente! Come puoi vedere, sulla sinistra è apparsa una nuova schermata. " +
                "Qui potrai visualizzare i feudi che possiedi… e anche acquistarne di nuovi. " +
                "Ogni feudo ha caratteristiche uniche, come i tributi generati e la loro rarità. " +
                "Per scoprire maggiori informazioni su un feudo, fai clic con il tasto sinistro sull’icona informativa in blu, in alto a destra, accanto ai feudi. " +
                "Quando sei pronto, completa l’obiettivo… e andiamo avanti.."
            };
            public static dati AcquistaFeudo = new dati
            {
                StatoTutorial = 8,
                Obiettivo = "Acquista un feudo.",
                Descrizione = "Perfetto… iniziamo tentando la fortuna! Come puoi vedere, in basso, sotto la casella dei feudi, c’è un bellissimo pulsante: “Acquista”. " +
                "L’Imperatore ti aiuterà durante questo tutorial, quindi ti sono già stati forniti 150 diamanti. " +
                "Premi il pulsante “Acquista” per iniziare la tua avventura su “Warrior & Wealth”… ed ottenere così il tuo primo feudo. " +
                "E finalmente, dopo tutta questa immensa fatica e dedizione…potrai goderti i guadagni provenienti direttamente dal feudo che hai appena annesso. " +
                "Noterai che il numero dei tuoi feudi sarà aumentato. E da questo momento, potrai vedere i Tributi crescere con il passare del tempo. " +
                "Come ti accennavo prima, i tributi possono anche essere spesi all’interno dello Shop, per ottenere vantaggi importanti… e competere contro gli altri villaggi del regno. " +
                "Quando sei pronto, completa l’obiettivo… e andiamo avanti."
            };
            public static dati Costruzione_1 = new dati
            {
                StatoTutorial = 9,
                Obiettivo = "Premere click sinistro sul testo.",
                Descrizione = "Ottimo! Hai appena ottenuto il tuo primo feudo!… adesso iniziamo a costruire qualcosa! Oltre ai feudi, esistono molte altre strutture che dovremo esaminare insieme. " +
                "Tra poco ti mostrerò come costruirle, utilizzando la tua popolazione e le risorse a tua disposizione… Beh… non proprio adesso. " +
                "Per il momento ti mancano ancora alcune risorse… ma non preoccuparti: presto tutto sarà possibile. " +
                "E, solo per questa volta, ti forniremo noi il necessario.Così potrai iniziare subito e conoscere le strutture disponibili. " +
                "Quando sei pronto, completa l’obiettivo… e andiamo avanti."

            };
            public static dati Civile_Militare = new dati
            {
                StatoTutorial = 10,
                Obiettivo = "Cambia la visualizzazione delle strutture da civili a militari [icon:scambio].",
                Descrizione = "Perfetto, ci siamo… parliamo delle strutture. Come puoi vedere, nella parte centrale della schermata di gioco è apparso un nuovo frammento dell’interfaccia: “Strutture Civili”. " +
                "Insieme a questo, è comparso anche un nuovo elemento dell’interfaccia. Queste sono le strutture principali, utilizzate per la produzione di risorse civili e militari. " +
                "Per il momento, però, ci concentreremo sulle risorse civili.Sono fondamentali per costruire, ricercare e mantenere il tuo villaggio. " +
                "E, con una buona strategia, ti permetteranno di crescere fino a raggiungere un potere degno dell’Imperatore. " +
                "Pensa quindi con attenzione alle tue scelte…e pianifica le tue prossime azioni per ambientarti al meglio in questo nuovo mondo. " +
                "Come puoi notare, nel riquadro delle strutture è presente un’icona [icon:scambio] che ti permette di passare agli edifici militari, dedicati alla produzione di risorse militari. " +
                "Ma di questo parleremo più avanti. " +
                "Quando sei pronto, completa l’obiettivo… e andiamo avanti."
            };
            public static dati Costruzione_2 = new dati
            {
                StatoTutorial = 11,
                Obiettivo = "Premi il pulsante Costruisci. ",
                Descrizione = "Finalmente ci siamo.Come puoi vedere, nella parte bassa della schermata di gioco è apparso un nuovo pulsante: “Costruisci”. " +
                "Per questa impresa ti verranno fornite tutte le risorse necessarie…ma fai attenzione: sarà l’unica volta. Se commetterai errori, le tue azioni saranno irreversibili.Mi raccomando. " +
                "Ora osserva la schermata principale. Accanto alla struttura “Fattoria”, oltre alla sua icona, puoi notare due indicatori. " +
                "Il primo mostra il numero di Edifici Disponibili: sono le strutture già costruite e attive. Il secondo indica il numero di Edifici in Coda: ovvero le strutture in attesa di essere costruite. " +
                "Il tempo rimanente per la costruzione lo potrai vedere in basso, nella schermata “Strutture Civili”. Questo indicatore comparirà non appena avvierai la tua prima costruzione. " +
                "Premendo il pulsante “Costruisci”, si aprirà una nuova schermata,da cui potrai gestire tutti questi aspetti. " +
                "Quando sei pronto, completa l’obiettivo… e andiamo avanti."
            };
            public static dati Costruisci_Fattoria = new dati
            {
                StatoTutorial = 12,
                Obiettivo = "Costruisci fattoria",
                Descrizione = "Lascia aperta la schermata “Costruisci”: i prossimi passaggi ti guideranno nella realizzazione di tutte le strutture necessarie. " +
                "Per comodità, sposta la finestra appena aperta in una posizione dello schermo che ti permetta di tenere tutto sotto controllo e di vedere entrambe le schermate. " +
                "Accanto a ogni struttura puoi osservare la quantità che stai per costruire. Al momento è disponibile solo la Fattoria, e il valore iniziale è impostato su zero. " +
                "Vicino a questo numero trovi i comandi che ti permettono di modificarlo. Con un semplice clic sinistro puoi aumentare la quantità di una unità; " +
                "usando la combinazione Contròl più clic, l’incremento sarà di cinque, mentre con Shift più clic potrai aumentare il valore di dieci. " +
                "Regola la quantità che desideri… Costruisci una Fattoria. " +
                "Quando sei pronto, completa l’obiettivo… e andiamo avanti."
            };
            public static dati Scambia = new dati
            {
                StatoTutorial = 13,
                Obiettivo = "Scambia diamanti viola per diamanti blu.",
                Descrizione = "Hai appena messo in costruzione la tua prima Fattoria." +
                "Ricorda che puoi assegnare la costruzione di più strutture contemporaneamente, ma una nuova costruzione potrà iniziare solo quando un costruttore sarà di nuovo libero. " +
                "Il numero di costruttori disponibili lo puoi vedere in alto, nella schermata “Strutture Civili”, sopra la prima struttura, la Fattoria. " +
                "Qui è indicato sia il numero massimo di costruttori disponibili… sia quanti di loro sono attualmente liberi. " +
                "Dato che hai già avviato una costruzione, noterai ora il tempo rimanente, che scorre man mano che il lavoro procede. Prima che la Fattoria sia completata, servirà un po’ di pazienza. " +
                "In qualsiasi momento, però, puoi velocizzare la costruzione utilizzando i Diamanti Blu. E infatti… utilizziamone qualcuno. Ma prima dobbiamo ottenerli. Vediamo come. " +
                "Torna alla schermata principale e guarda nella sezione Feudi.Sotto il pulsante “Acquista Feudo” è appena apparso un nuovo pulsante: “Scambia”. " +
                "Premilo, e potrai convertire alcuni Diamanti Viola in Diamanti Blu. " +
                "Per questa costruzione, 50 Diamanti Blu dovrebbero essere più che sufficienti." +
                "Quando sei pronto, completa l’obiettivo… e andiamo avanti."
            };
            public static dati Velocizza = new dati
            {
                StatoTutorial = 14,
                Obiettivo = "Velocizza la costruzione. Completa la fattoria",
                Descrizione = "Ottimo, sei riuscito a ottenere i tuoi primi Diamanti Blu tramite lo scambio." +
                "In questo modo, se dovessi rimanerne sprovvisto e ne avessi bisogno, potrai ottenerli in qualsiasi momento… a patto di avere Diamanti Viola a disposizione. " +
                "In questo tutorial ti mostrerò solo come velocizzare la costruzione della Fattoria,ma più avanti potrai fare la stessa cosa anche con le altre strutture, con le unità militari in addestramento… " +
                "e persino con la ricerca. Ora fai clic con il tasto sinistro sull’icona accanto al tempo di costruzione rimanente. " +
                "Dovrebbe avere l’aspetto di un velocizzatore.Prèmila e aumenta il numero di Diamanti Blu da utilizzare fino a 50. " +
                "Anche qui valgono le stesse regole di prima: puoi cliccare normalmente, oppure usare le combinazioni di tasti per raggiungere più rapidamente il valore desiderato. " +
                "E non preoccuparti: non serve chiederti se stai usando troppi diamanti. " +
                "Verranno consumati solo quelli realmente necessari… tutti gli altri ti verranno restituiti. " +
                "Quando sei pronto, completa l’obiettivo… e andiamo avanti."
            };
            public static dati Costruisci_Segheria = new dati
            {
                StatoTutorial = 15,
                Obiettivo = "Costruisci una segheria.",
                Descrizione = "Ottimo lavoro, la fattoria è costruita e il tuo insediamento comincia a prendere forma.\n" +
                "Ora dobbiamo assicurarci di avere i materiali giusti per crescere, il legno è tra questi.\n" +
                "Costruisci una Segheria e rendi stabile la tua economia.\n\n" +
                "Quando sei pronto, completa l'obiettivo ed andiamo avanti."
            };
            public static dati Costruisci_Cava = new dati
            {
                StatoTutorial = 16,
                Obiettivo = "Costruisci una cava di pietra.",
                Descrizione = "Ottimo lavoro! Vedo che hai costruito la tua prima segheria, e questo ti permetterà di ottenere legno in modo costante.\n" +
                "Adesso è il momento di assicurarti un’altra risorsa fondamentale per la crescita del tuo insediamento: Costruisci una Cava di pietra per iniziare a estrarre questa risorsa, " +
                "indispensabile per nuove strutture, le riparazioni e la ricerca\n\n. " +
                "Quando sei pronto, completa l'obiettivo ed andiamo avanti."
            };
            public static dati Costruisci_Miniera_Ferro = new dati
            {
                StatoTutorial = 17,
                Obiettivo = "Costruisci una miniera di ferro.",
                Descrizione = "Perfetto! Vedo che hai costruito la tua prima cava di pietra, ottimo lavoro.\n" +
                "Ora il tuo insediamento sta iniziando ad avere basi solide, ma per crescere davvero ti serve una risorsa ancora più importante: il ferro.\n" +
                "Costruisci una Miniera di ferro per avviare l’estrazione e ottenere materiali indispensabili per l'esercito e lo sviluppo futuro.\n\n" +
                "Quando sei pronto, completa l'obiettivo ed andiamo avanti."
            };
            public static dati Costruisci_Miniera_Oro = new dati
            {
                StatoTutorial = 18,
                Obiettivo = "Costruisci una miniera d'oro.",
                Descrizione = "Perfetto! Vedo che hai costruito la tua prima miniera di ferro, ottimo lavoro.\n" +
                "Adesso hai iniziato ad estrarre risorse essenziali per lo sviluppo, ma manca ancora qualcosa di fondamentale: una fonte di ricchezza stabile.\n" +
                "L'oro è fondamentale anche per il mantenimento di strutture e unità militari, prima di una grande espansione, controlla sempre le tue produzioni, " +
                "i tuoi consumi non dovrebbero mai superare le tue capacità produttive.\n" +
                "Costruisci una Miniera d’oro per iniziare a estrarre una risorsa preziosa, utile per sostenere l’economia del tuo insediamento.\n\n" +
                "Quando sei pronto, completa l'obiettivo ed andiamo avanti."
            };
            public static dati Costruisci_Abitazioni = new dati
            {
                StatoTutorial = 19,
                Obiettivo = "Costruisci una casa.",
                Descrizione = "La tua miniera d’oro è pronta e il villaggio comincia a prendere vita.\n" +
                "Per farlo crescere davvero servono abitanti che possano viverci e lavorare.\n" +
                "Costruisci una Casa per accogliere nuovi cittadini e dare al tuo insediamento i suoi primi abitanti.\n" +
                "Costruisci una 'Casa'.\n\n" +
                "Quando sei pronto, completa l'obiettivo ed andiamo avanti."
            };
            public static dati Strutture_Militari = new dati
            {
                StatoTutorial = 20,
                Obiettivo = "Premere click sinistro sul testo.",
                Descrizione = "Adesso è il momento di presentarti le Strutture Militari." +
                "Anche per loro vale lo stesso principio delle costruzioni civili, ma queste produrranno risorse indispensabili per le tue unità, come Guerrieri, Lancieri, Arcieri e Catapulte." +
                "Le risorse prodotte servono principalmente ad addestrare l’esercito. In base al tipo di unità e al tier selezionato, le quantità richieste aumenteranno insieme alla qualità delle truppe." +
                "Approfondiremo questi aspetti più avanti, quando parleremo direttamente delle unità… e del loro addestramento." +
                "In questo tutorial non costruiremo strutture militari, ma è importante conoscerle fin da subito." +
                "Il loro costo è significativo e tutte richiedono un consumo costante di risorse per produrre equipaggiamento." +
                "Sarai tu a decidere quando iniziare la produzione, al termine di questo tutorial… quando ti sentirai davvero pronto. " +
                "Quando sei pronto, completa l’obiettivo… e andiamo avanti."
            };
            public static dati Unita_Militari = new dati
            {
                StatoTutorial = 21,
                Obiettivo = "Clicca sull'icona del Guerriero.",
                Descrizione = "Passiamo ora alle Unità Militari. " +
                "Fino a questo momento non erano visibili, ma ora puoi trovarle sia nella schermata “Costruisci”, sia direttamente nella schermata di gioco, accanto alle strutture civili." +
                "In entrambi i casi, l’interfaccia è molto simile.Sopra alle unità militari puoi notare i livelli, che vanno da uno a sei." +
                "Per ora potrai addestrare solamente le unità di livello uno, ma in futuro, salendo di livello, sbloccherai anche quelle più avanzate." +
                "Le unità sono suddivise in Guerrieri, Lancieri, Arcieri e Catapulte." +
                "Tutte richiedono un mantenimento costante di cibo e oro, e necessitano di risorse ed equipaggiamento militare per essere addestrate." +
                "Al momento, però, anche se disponessi delle risorse necessarie, non potresti ancora addestrarle. Non hai infatti accesso alle Caserme, che verranno sbloccate più avanti nel gioco." +
                "Quando sei pronto, completa l’obiettivo… e andiamo avanti."
            };
            public static dati Caserme = new dati
            {
                StatoTutorial = 22,
                Obiettivo = "Clicca sull'icona Caserma delle Catapulte.",
                Descrizione = "Passiamo ora alle Caserme." +
                "Come anticipato, queste strutture sono fondamentali per fondare il tuo esercito: senza di esse non puoi formare alcuna forza militare." +
                "La loro costruzione consente di alloggiare un certo numero di uomini, che varia in base al tipo di struttura." +
                "Esistono caserme dedicate ai Guerrieri, ai Lancieri, agli Arcieri e alle Catapulte, e ognuna è pensata per supportare una specifica tipologia di unità." +
                "Tuttavia, il loro utilizzo comporta un costo: richiedono una manutenzione costante in cibo e oro per poter funzionare correttamente." +
                "Prima di costruirle, assicurati che la tua produzione sia in grado di sostenere queste spese nel tempo." +
                "Una gestione attenta delle risorse sarà fondamentale per mantenere efficiente il tuo esercito." +
                "Quando sei pronto, completa l’obiettivo… e andiamo avanti."
            };
            public static dati Addestramento = new dati
            {
                StatoTutorial = 23,
                Obiettivo = "Chiudi la scermata 'Costruisci'.",
                Descrizione = "Perfetto, ci siamo. Abbiamo finalmente concluso con le costruzioni, quindi lasciami introdurre brevemente l’addestramento, giusto per restare in tema con questa schermata." +
                "Come puoi vedere, sulla destra è apparsa la sezione dedicata all’addestramento. Qui potrai osservare le diverse unità disponibili." +
                "Come accennato in precedenza, nella parte superiore è visibile il tier selezionato: al momento è disponibile solo il tier 1, mentre i successivi verranno sbloccati più avanti." +
                "Di questo parleremo nel dettaglio quando affronteremo direttamente le unità. Per ora è tutto. Chiudi la schermata Costruisci." +
                "Quando sei pronto, completa l’obiettivo e andiamo avanti."
            };
            public static dati Citta = new dati
            {
                StatoTutorial = 24,
                Obiettivo = "Apri la schermata della 'Città'.",
                Descrizione = "Ottimo, non ci resta che introdurre la mappa del villaggio. Come puoi notare, in basso è appena apparso un nuovo pulsante: “Città”." +
                "Premilo e lasciati mostrare come sono composti i villaggi di questo regno.Iniziamo da una struttura fondamentale: le Mura." +
                "Come puoi vedere, quasi tutte le strutture condividono la stessa impostazione." +
                "In cima è presente il nome della struttura; subito alla sua destra trovi un numero tra parentesi quadre, che indica l’ordine delle strutture." +
                "Accanto è visibile il livello della struttura. Subito sotto sono presenti tre barre: Salute, Difesa e Guarnigione.Queste sono le tue strutture difensive. In totale sono sei." +
                "Tre di esse, oltre alle Mura — Cancello, Torri e Castello — condividono le stesse statistiche." +
                "La guarnigione rappresenta il numero massimo di unità che la struttura può ospitare e quante ne sono effettivamente assegnate al suo interno. Le restanti due strutture, Ingresso e Centro, sono simili ma dispongono solo dello spazio per la guarnigione, senza barre di salute e difesa." +
                "È importante mantenere le strutture in buono stato. In caso di attacco al villaggio, una difesa ben organizzata può fare la differenza e ridurre le perdite." +
                "Fai però attenzione: se una struttura dovesse perdere tutta la salute e arrivare a zero, crollerà. In quel caso, tutte le unità presenti al suo interno perderanno la vita durante il crollo." +
                "L’assegnazione delle unità è quindi cruciale. " +
                "Facciamo un esempio: se nell’Ingresso sono presenti unità in guarnigione, pur essendo una struttura priva di salute e difesa, il loro posizionamento, insieme a unità assegnate a Mura e Cancello, " +
                "può fornire un bonus difensivo alle unità presenti all’ingresso, in base al numero schierato.In generale, è sempre consigliabile mantenere ben rifornite le guarnigioni in questa schermata. " +
                "Spesso è meglio avere più uomini in difesa piuttosto che rischiare una sconfitta rovinosa. Se le unità attaccanti dovessero superare il Castello, il settimo e ultimo attacco colpirà direttamente te. " +
                "In caso di sconfitta, l’attaccante potrà saccheggiare le tue risorse, inclusi diamanti viola e diamanti blu." +
                "Quando sei pronto, completa l’obiettivo e andiamo avanti."
            };
            public static dati Riparazione = new dati
            {
                StatoTutorial = 25,
                Obiettivo = "Ripara la struttura danneggiata.",
                Descrizione = "Oh no, guarda… un sabotatore ha appena danneggiato una tua struttura." +
                "Vedi? La salute e la difesa delle Mura sono diminuite. Questo non va bene, ma è l’occasione perfetta per introdurre le riparazioni." +
                "Quando una struttura viene danneggiata a seguito di un assedio, necessita di essere riparata. Per farlo, puoi notare che accanto alle sue statistiche è comparsa un’icona raffigurante la “Riparazione”." +
                "Questa icona è visibile solo quando la struttura ha subito dei danni. Premila e vedrai come i tuoi uomini si affretteranno a sistemarla." +
                "Presta attenzione: le riparazioni richiedono tempo e risorse. Se le risorse dovessero terminare, il processo di riparazione si fermerà automaticamente, quindi assicurati di averne a sufficienza." +
                "Come già visto per le altre icone, puoi portare il puntatore del mouse sopra l’icona di riparazione per visualizzare il menu a tendina." +
                "Qui potrai controllare le risorse necessarie per ripristinare ogni punto di salute e il tempo richiesto per completare il lavoro." +
                "Quando sei pronto, completa l’obiettivo e andiamo avanti."
            };
            public static dati Guranigione = new dati
            {
                StatoTutorial = 26,
                Obiettivo = "Premi il pulsante 'Guarnigione' del 'Castello'.",
                Descrizione = "Ottimo, siamo giunti alla fine di questa schermata." +
                "Per assegnare uomini a una struttura è sufficiente premere il pulsante “Guarnigione” presente sotto ciascuna di esse." +
                "Da qui potrai selezionare le unità e il loro livello, assegnandole alla struttura scelta." +
                "Questo passaggio può essere ripetuto per tutte le strutture disponibili, non appena disporrai di un esercito al tuo servizio.Ricorda inoltre che Salute, Difesa e capacità di guarnigione possono essere potenziate tramite la ricerca." +
                "Il livello del tuo personaggio avrà un ruolo fondamentale nello sviluppo della città e nella sua capacità di resistere agli attacchi." +
                "Quando sei pronto, completa l’obiettivo e andiamo avanti."
            };
            public static dati Statistiche = new dati
            {
                StatoTutorial = 27,
                Obiettivo = "Apri la schermata delle 'Statistiche'.",
                Descrizione = "Puoi chiudere la schermata “Città”, non ci servirà più." +
                "Nota però che, nella schermata di gioco, accanto ai diamanti viola è comparsa una nuova icona: il tuo personaggio, con il suo nome." +
                "Cliccando con il tasto sinistro sulla tua icona si aprirà una nuova schermata dedicata alle “Statistiche”.Questa schermata non è fondamentale, ma vale la pena soffermarsi un momento." +
                "Troverai due colonne: quella di destra mostra le tue statistiche personali, dove potrai osservare informazioni interessanti che potrebbero tornarti utili in futuro." +
                "Sulla sinistra, invece, è indicato lo stato attuale del tuo villaggio, con i tempi rimanenti prima del reset delle varie attività, la potenza complessiva e i bonus applicati, sia temporanei che permanenti." +
                "Quando sei pronto, completa l’obiettivo e andiamo avanti."
            };
            public static dati Shop = new dati
            {
                StatoTutorial = 28,
                Obiettivo = "Apri la schermata dello 'Shop'.",
                Descrizione = "Puoi chiudere la schermata “Statistiche”, non ci servirà più." +
                "Nota però che ora è disponibile un nuovo pulsante: lo “Shop”. È il luogo in cui potrai ottenere diamanti viola e molto altro." +
                "All’interno dello shop potrai aumentare il numero massimo di costruttori e di addestratori, acquistare lo Scudo della Pace per una difesa aggiuntiva delle tue terre, attivare il VIP, " +
                "che garantisce vantaggi esclusivi e l’accesso a premi extra nelle quest, e scegliere tra due varianti di GamePass, Base e Avanzato." +
                "Come per le altre interfacce, puoi spostare il puntatore del mouse sulle icone per visualizzare informazioni più dettagliate su ogni elemento disponibile." +
                "Prenditi un momento per esplorare liberamente lo shop." +
                "Quando sei pronto, completa l’obiettivo e andiamo avanti."
            };
            public static dati Ricerca = new dati
            {
                StatoTutorial = 29,
                Obiettivo = "Apri la schermata della 'Ricerca'.",
                Descrizione = "Puoi chiudere la schermata “Shop”, non ci servirà più. Nota però che nella schermata principale è comparso un nuovo pulsante: “Ricerca”." +
                "Questo luogo è fondamentale se vuoi tentare di raggiungere anche solo una minima frazione della potenza dell’Imperatore." +
                "Le ricerche disponibili sono numerose, e altre potranno essere aggiunte in futuro." +
                "Come ormai sarai abituato, puoi spostare il puntatore del mouse su ciascuna ricerca per ottenere informazioni dettagliate sui costi e sui tempi necessari al completamento." +
                "Ricorda però che è possibile effettuare una sola ricerca alla volta, quindi scegli con attenzione cosa sviluppare per primo." +
                "Potrai migliorare quasi tutto: dalla produzione di risorse, all’implementazione di strategie per attrarre nuovi abitanti, fino al potenziamento dell’addestramento, sia della città che delle singole unità militari, e molto altro ancora." +
                "Ti lascio qualche momento per familiarizzare con la schermata della ricerca, ma potrai esplorarla più approfonditamente anche in seguito." +
                "Quando sei pronto, completa l’obiettivo e andiamo avanti."
            };
            public static dati Quest_Mensili = new dati
            {
                StatoTutorial = 30,
                Obiettivo = "Apri la schermata delle 'Quest Mensili'.",
                Descrizione = "Puoi chiudere la schermata “Ricerca”, non ci servirà più." +
                "Ora, nella schermata principale, è comparso un nuovo pulsante: “Quest Mensili”.Qui in realtà non c’è molto da spiegare." +
                "Si tratta di una schermata in cui puoi vedere le quest disponibili: il loro requisito per essere completate e l’esperienza che forniranno singolarmente." +
                "Più in basso troverai un pulsante che ti permette di scorrere tra le quest disponibili. Alcune quest possono essere completate più volte, aumentando progressivamente il loro requisito." +
                "Infine, potrai osservare la barra del progresso, che mostra quanta esperienza hai accumulato. I premi collegati possono essere reclamati una volta raggiunte le soglie indicate: " +
                "quelli sopra la barra sono disponibili per tutti, mentre quelli sotto possono essere raccolti solo se possiedi il VIP." +
                "Quando sei pronto, completa l’obiettivo e andiamo avanti."
            };
            public static dati Battaglia = new dati
            {
                StatoTutorial = 31,
                Obiettivo = "Apri la schermata battaglie 'PVP/PVE'.",
                Descrizione = "Puoi chiudere la schermata “Quest Mensili”, non ci servirà più. Ora, nella schermata principale, è comparso un nuovo pulsante: “Battaglie”." +
                "Qui potrai trovare i Villaggi Barbari, le Città Barbare e anche i villaggi di altri giocatori." +
                "Al momento potrebbe sembrarti poco rilevante, ma la battaglia è l’unico modo per ottenere esperienza e salire di livello." +
                "Non sarà il tuo obiettivo principale, perché le cose da fare sono molte, ma non trascurarlo: farlo ti permetterà di progredire nel gioco senza restare indietro." +
                "Fin dall’inizio saranno disponibili i Villaggi Barbari. Non sottovalutarli: possono riservare spiacevoli sorprese, e ricorda che in ogni scontro tu sarai l’attaccante." +
                "Le Città Barbare sono più difficili: contengono più uomini e sono meglio organizzate. Fai attenzione quando ti troverai ad affrontarle." +
                "Quando sei pronto, completa l’obiettivo e andiamo avanti."
            };
            public static dati Finale = new dati
            {
                StatoTutorial = 32,
                Obiettivo = "Premere click sinistro sul testo.",
                Descrizione = "Sei giunto alla fine di questo splendido viaggio." +
                "A questo punto credo tu sia pronto: ti ho raccontato tutto ciò che sapevo sulle meccaniche di questo nuovo mondo, o almeno spero di non aver dimenticato nulla." +
                "In caso contrario, lascio a te il piacere di scoprirlo con il tempo." +
                "Dopotutto, sei arrivato fin qui con successo, dimostrando di saper affrontare ogni sfida che ti è stata posta davanti." +
                "Non mi resta che salutarti. Io mi allontanerò da queste terre per rifugiarmi in un luogo sicuro, lasciandoti il regno nelle tue mani." +
                "Da ora in poi, il tuo destino dipenderà solo dalle tue scelte."
            };
        }
    }
}
