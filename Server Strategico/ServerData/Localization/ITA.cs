using Server_Strategico.Gioco;

namespace Server_Strategico.ServerData.Localization
{
    internal class ITA : ILocalization
    {
        public string GetQuestDescription(int questId) => questId switch
        {
            0 => "Acquista un feudo",
            1 => "Addestra guerrieri",
            2 => "Addestra lancieri",
            3 => "Addestra arcieri",
            4 => "Addestra catapulte",
            5 => "Elimina guerrieri",
            6 => "Elimina lancieri",
            7 => "Elimina arcieri",
            8 => "Elimina catapulte",
            9 => "Elimina qualsiasi unità",
            10 => "Costruisci qualsiasi struttura civile",
            11 => "Costruisci fattorie",
            12 => "Costruisci segherie",
            13 => "Costruisci cave di pietra",
            14 => "Costruisci miniere di ferro",
            15 => "Costruisci miniere d'oro",
            16 => "Costruisci case",
            17 => "Costruisci workshop spade",
            18 => "Costruisci workshop lancie",
            19 => "Costruisci workshop archi",
            20 => "Costruisci workshop scudi",
            21 => "Costruisci workshop armature",
            22 => "Costruisci workshop frecce",
            23 => "Costruisci caserme guerrieri",
            24 => "Costruisci caserme lancieri",
            25 => "Costruisci caserme arcieri",
            26 => "Costruisci caserme catapulte",
            27 => "Utilizza diamanti blu",
            28 => "Utilizza diamanti viola",
            29 => "Velocizza costruzioni",
            30 => "Velocizza addestramento",
            31 => "Velocizza qualsiasi cosa",
            32 => "Attacca giocatori",
            33 => "Attacca villaggi barbari",
            34 => "Attacca città barbare",
            35 => "Costruisci qualsiasi struttura militare",
            36 => "Addestra qualsiasi unità militare",
            37 => "Utilizza cibo",
            38 => "Utilizza legno",
            39 => "Utilizza pietra",
            40 => "Utilizza ferro",
            41 => "Utilizza oro",
            42 => "Utilizza popolazione",
            43 => "Utilizza spade",
            44 => "Utilizza lancie",
            45 => "Utilizza archi",
            46 => "Utilizza scudi",
            47 => "Utilizza armature",
            48 => "Utilizza frecce",
            49 => "Completa tutte le quest iniziali",
            50 => "Esplora villaggio barbaro",
            51 => "Esplora città barbare",
            52 => "Esplora giocatori",
            53 => "Esegui qualsiasi esplorazione",
            54 => "Raggiungi il livello 5",
            55 => "Raggiungi il livello 10",
            56 => "Raggiungi il livello 25",
            57 => "Raggiungi il livello 50",
            58 => "Raggiungi il livello 75",
            59 => "Raggiungi il livello 100",
            _ => $"Unknown quest"
        };
        public string GetTutorialObiettivo(int statoTutorial) => statoTutorial switch
        {
            1 => "Premere click sinistro sul testo",
            2 => "Premere click sinistro sul testo",
            3 => "Premere click sinistro sul testo",
            4 => "Premere click sinistro sul testo",
            5 => "Premere click sinistro sul testo.",
            6 => "Premere click sinistro sul testo.",
            7 => "Premere click sinistro sul testo.",
            8 => "Acquista un feudo.",
            9 => "Premere click sinistro sul testo.",
            10 => "Cambia la visualizzazione delle strutture da civili a militari [icon:scambio].",
            11 => "Premi il pulsante Costruisci.",
            12 => "Costruisci fattoria",
            13 => "Scambia [icon:diamanteViola][viola]diamanti viola[/viola] per [icon:diamanteBlu][blu]diamanti blu[/blu].",
            14 => "Velocizza la costruzione. [warning]Completa la fattoria[/warning]",
            15 => "Costruisci una segheria.",
            16 => "Costruisci una cava di pietra.",
            17 => "Costruisci una miniera di ferro.",
            18 => "Costruisci una miniera d'oro.",
            19 => "Costruisci una casa.",
            20 => "Premere click sinistro sul testo.",
            21 => "Clicca sull'icona del Guerriero.",
            22 => "Clicca sull'icona Caserma delle Catapulte.",
            23 => "Chiudi la schermata 'Costruisci'.",
            24 => "Apri la schermata della 'Città'.",
            25 => "Ripara la struttura danneggiata.",
            26 => "Premi il pulsante 'Guarnigione' del 'Castello'.",
            27 => "Apri la schermata delle '[warning]Statistiche[/warning]'.",
            28 => "Apri la schermata dello '[warning]Shop[/warning]'.",
            29 => "Apri la schermata della 'Ricerca'.",
            30 => "Apri la schermata delle 'Quest Mensili'.",
            31 => "Apri la schermata battaglie 'PVP/PVE'.",
            32 => "Premere click sinistro sul testo.",
            _ => ""
        };
        public string GetTutorialDescrizione(int statoTutorial) => statoTutorial switch
        {
            1 =>
                "Non preoccuparti: in questo tutorial imparerai a muoverti in questo nuovo mondo e in tutte le schermate principali." +
                "Come puoi vedere, al momento non c'è molto da osservare… ma tra non molto tutto sarà più chiaro forse..." +
                "Quello che vedi adesso sono due schermate. La prima, quella più grande, è la schermata principale: ti permetterà di gestire praticamente tutto." +
                "La seconda, dove stai leggendo questo testo, è la schermata del tutorial." +
                "Rimarrà attiva fino al suo completamento. Concentriamoci un attimo su quest'ultima." +
                "Qui potrai vedere la progressione del tutorial, gli obiettivi che devi raggiungere per andare avanti… e anche una breve descrizione." +
                "In questo caso, per completare l'obiettivo, fai semplicemente click con il tasto sinistro su questo testo. Quando sei pronto, completa l'obiettivo e andiamo avanti.",

            2 =>
                "Benvenuto su \"[warning]Warrior & Wealth[/warning]\"! " +
                "Vedo che sei un nuovo giocatore… ottimo. Questo tutorial sarà perfetto per te! Prima di iniziare, lasciami dire una cosa… " +
                "bevi responsabilmente! Non guidare mentre bevi, o potresti rovesciare la tua birra! Forse è meglio passare alle cose serie… " +
                "Posso finalmente guidarti, passo dopo passo, in questo nuovo mondo. Grandi avventure ti aspettano. " +
                "\"[warning]Warrior & Wealth[/warning]\" è diviso in due modalità. La prima parte ti permetterà di acquistare dei Feudi, che ti faranno ottenere i [icon:dollariVirtuali]Tributi del Feudo." +
                "Grazie all'immensità del regno e alla benevolenza dell'Imperatore, ogni secondo nuove ricchezze finiranno nelle casse del tuo villaggio… " +
                "Ma questo è un aspetto che vedremo al momento giusto. La seconda parte… come forse avrai capito… riguarda te. E se così non fosse, mi spiego meglio." +
                "Sei stato scelto. Sì, hai capito bene. Sei stato scelto per gestire questo piccolo villaggio. Mi raccomando… " +
                "le intemperie incombono su queste terre, e i pericoli sono ovunque. Fai molta attenzione. Quando sei pronto, possiamo andare avanti con il tutorial e mostrarti cosa puoi fare." +
                "Ah, già… ancora non ti ho spiegato cosa devi fare davvero. Dovrai gestire il villaggio che l'Imperatore ti ha assegnato." +
                "Le risorse saranno il tuo principale obiettivo: senza di esse, tutto è perduto. Ti serviranno per costruire edifici, strutture militari, per la ricerca… per riparare le difese, e molto altro ancora." +
                "Gli aspetti da imparare saranno molti… ma non riempiamo questo testo di troppe parole. Iniziamo! ...Quando sei pronto, completa l'obiettivo ed andiamo avanti.",

            3 =>
                "In alto puoi vedere qualcosa, questa è la barra delle risorse, quello che dicevamo prima, queste sono indispensabili ed in ordine troviamo, '[icon:cibo]Cibo, [icon:legno]Legno, [icon:pietra]Pietra, [icon:ferro]Ferro," +
                "[icon:oro]Oro, [icon:popolazione]Popolazione, [icon:xp]Esperienza e [icon:lv]Livello', queste risorse servono per tutto, ti posso solo lasciare immaginare a cosa possano servire." +
                "Portando il puntatore del mouse sopra le singole icone, e lasciandolo su di esso per un breve istante, noterai una breve descrizione a tendina che apparirà, ti mostrerà cosa stai osservando, una breve descrizione e tutte le informazioni" +
                "che ti possono servire, le troverai al suo interno durante la gestione del tuo villaggio. Puoi fare questo, con quasi tutte le icone che incontrerai in questa schermata e nelle altre, ricordatelo." +
                "Passiamo con l'introdurre le risorse militari, premendo l'icona [icon:scambio], potrai vedere nel seguente ordine '[icon:spade]Spade, [icon:lance]Lancie, [icon:archi]Archi, [icon:scudi]Scudi, [icon:armature]Armature e [icon:frecce]Frecce', la raffigurazione delle singole icone dovrebbe essere già più che esaustivo, ma ti lascio immaginare " +
                "a cosa potrebbero servire, l'unica su cui mi vorrei sofferare meglio sono le [icon:frecce]frecce, esse essendo un componente da lancio, verranno utilizzate dai tuoi arceri e catapulte, quindi nei momenti" +
                "di maggiore tensione, consiglio di prestare attenzione alla loro quantità nei tuoi magazzini, dagli uno sguardo, per evitare di rinanerne sguarniti nel momento del bisogno." +
                "Tutte le risorse possono essere prodotte direttamente nel tuo villaggio, oppure ottenute attraverso altre vie..." +
                "Quando sei pronto, completa l'obiettivo ed andiamo avanti.",

            4 =>
                "Sempre in alto, sotto alla barra delle risorse puoi vedere che una seconda barra piu in basso è appena apparsa, ti permetterà di visualizzare, per il momento, " +
                "una sola icona i '[icon:diamanteViola][viola]Diamanti Viola[/viola]', soffermiamoci un'istante. " +
                "I [icon:diamanteViola][viola]Diamanti Viola[/viola] sono fondamentali per la realizzazione di 'feudi', questi li vedremo piu avanti non ti preoccupare, possono essere spesi nello 'Shop', " +
                "per ottenere vantaggi che accelereranno la progressione in questo regno, lo shop naturalmente sarà presto visibile, possono anche essere scambiati in Diamanti Blu, ma lo vedremo più avanti, " +
                "questa risorsa è al primo posto per scarsità, quindi pondera bene la tua decisione prima di spenderli, non è facile ottenerli, ma lo è altrettanto spenderli... " +
                "Come puoi ottenere dei [icon:diamanteViola][viola]Diamanti Viola[/viola] ti stai domandando? " +
                "Bene potrai ottenerli in numerosi modi, attraverso le quest mensili, nello shop, oppure vinti in battaglie contro 'Villaggi' e 'Città' barbare, " +
                "o addirittura attaccando e depredando le risorse di altri giocatori, ma per questo dovrai ancora aspettare, è troppo presto... " +
                "Prima concentramoci sul resto. " +
                "Quando sei pronto, completa l'obiettivo ed andiamo avanti..",

            5 =>
                "Guarda… accanto ai [icon:diamanteViola][viola]Diamanti Viola[/viola] è apparsa una nuova icona. " +
                "Raffigura i [icon:diamanteBlu][blu]diamanti blu[/blu]. Già… stavo quasi per dimenticarmene. Parliamo di loro. " +
                "I [icon:diamanteBlu][blu]diamanti blu[/blu] sono molto simili alla loro controparte [icon:diamanteViola][viola]diamanti viola[/viola], ma con una piccola differenza: sono la seconda risorsa più rara del regno. " +
                "Questi diamanti, però, tendono a portare valore e vantaggi alla tua città. Vengono usati principalmente per accelerare: Costruzioni, l'addestramento delle truppe e le ricerche. " +
                "Tutte cose fondamentali per far crescere il tuo villaggio… e renderlo più forte. " +
                "Naturalmente, non sono indispensabili per progredire… ma possono darti un grande aiuto, soprattutto nei momenti più importanti. Inoltre, " +
                "puoi spenderli nello Shop per acquistare potenti incantesimi. Uno dei più utili è lo Scudo della Pace. " +
                "Grazie agli illustri maghi del regno, questo incantesimo creerà un enorme scudo blu attorno ai confini dei tuoi domìni… " +
                "e ti proteggerà dagli attacchi provenienti da altre città, sparse e lontane nel regno." +
                "Quando sei pronto, completa l'obiettivo… e andiamo avanti.",

            6 =>
                "Sembra che sia appena apparsa un'ulteriore risorsa. Ora puoi visualizzare l'icona dei Tributi del Feudo. " +
                "La quantità di tributi che riceverai dipenderà dal numero di Feudi che amministri… e dal loro livello di rarità. " +
                "Ogni feudo genera tributi ogni secondo, e l'importo verrà accreditato automaticamente. " +
                "Potrai usare questi tributi nello Shop. Purtroppo per ora non puoi ancora vederlo… ma lo scopriremo più avanti. " +
                "In alternativa, potrai anche prelevarli, una volta raggiunta la soglia minima richiesta. " +
                "Il cambio attuale è di 1 Tributo = 1 USDT." +
                "Quando avrai raggiunto la soglia, dovrai inserire l'indirizzo USDT su cui desideri ricevere il prelievo." +
                "Quando sei pronto, completa l'obiettivo… e andiamo avanti.",

            7 =>
                "Eccoci finalmente! Come puoi vedere, sulla sinistra è apparsa una nuova schermata. " +
                "Qui potrai visualizzare i feudi che possiedi… e anche acquistarne di nuovi. " +
                "Ogni feudo ha caratteristiche uniche, come i tributi generati e la loro rarità. " +
                "Per scoprire maggiori informazioni su un feudo, fai clic con il tasto sinistro sull'icona informativa in blu, in alto a destra, accanto ai feudi. " +
                "Quando sei pronto, completa l'obiettivo… e andiamo avanti..",

            8 =>
                "Perfetto… iniziamo tentando la fortuna! Come puoi vedere, in basso, sotto la casella dei feudi, c'è un bellissimo pulsante: ”Acquista”. " +
                "L'Imperatore ti aiuterà durante questo tutorial, quindi ti sono già stati forniti [icon:diamanteViola][warning]150 [viola]diamanti[/viola]. " +
                "Premi il pulsante ”Acquista” per iniziare la tua avventura su \"[warning]Warrior & Wealth[/warning]\"… ed ottenere così il tuo primo feudo. " +
                "E finalmente, dopo tutta questa immensa fatica e dedizione… potrai goderti i guadagni provenienti direttamente dal feudo che hai appena annesso. " +
                "Noterai che il numero dei tuoi feudi sarà aumentato. E da questo momento, potrai vedere i Tributi crescere con il passare del tempo. " +
                "Come ti accennavo prima, i tributi possono anche essere spesi all'interno dello Shop, per ottenere vantaggi importanti… e competere contro gli altri villaggi del regno. " +
                "Quando sei pronto, completa l'obiettivo… e andiamo avanti.",

            9 =>
                "Ottimo! Hai appena ottenuto il tuo primo feudo!… adesso iniziamo a costruire qualcosa! Oltre ai feudi, esistono molte altre strutture che dovremo esaminare insieme. " +
                "Tra poco ti mostrerò come costruirle, utilizzando la tua popolazione e le risorse a tua disposizione… Beh… non proprio adesso. " +
                "Per il momento ti mancano ancora alcune risorse… ma non preoccuparti: presto tutto sarà possibile. " +
                "E, solo per questa volta, ti forniremo noi il necessario. Così potrai iniziare subito e conoscere le strutture disponibili. " +
                "Quando sei pronto, completa l'obiettivo… e andiamo avanti.",

            10 =>
                "Perfetto, ci siamo… parliamo delle strutture. Come puoi vedere, nella parte centrale della schermata di gioco è apparso un nuovo frammento dell'interfaccia: ”Strutture Civili”. " +
                "Insieme a questo, è comparso anche un nuovo elemento dell'interfaccia. Queste sono le strutture principali, utilizzate per la produzione di risorse civili e militari. " +
                "Per il momento, però, ci concentreremo sulle risorse civili. Sono fondamentali per costruire, ricercare e mantenere il tuo villaggio. " +
                "E, con una buona strategia, ti permetteranno di crescere fino a raggiungere un potere degno dell'Imperatore. " +
                "Pensa quindi con attenzione alle tue scelte… e pianifica le tue prossime azioni per ambientarti al meglio in questo nuovo mondo. " +
                "Come puoi notare, nel riquadro delle strutture è presente un'icona [icon:scambio] che ti permette di passare agli edifici militari, dedicati alla produzione di risorse militari. " +
                "Ma di questo parleremo più avanti. " +
                "Quando sei pronto, completa l'obiettivo… e andiamo avanti.",

            11 =>
                "Finalmente ci siamo. Come puoi vedere, nella parte bassa della schermata di gioco è apparso un nuovo pulsante: ”Costruisci”. " +
                "Per questa impresa ti verranno fornite tutte le risorse necessarie… ma fai attenzione: sarà l'unica volta. Se commetterai errori, le tue azioni saranno irreversibili. Mi raccomando. " +
                "Ora osserva la schermata principale. Accanto alla struttura ”Fattoria”, oltre alla sua icona, puoi notare due indicatori. " +
                "Il primo mostra il numero di Edifici Disponibili: sono le strutture già costruite e attive. Il secondo indica il numero di Edifici in Coda: ovvero le strutture in attesa di essere costruite. " +
                "Il tempo rimanente per la costruzione lo potrai vedere in basso, nella schermata ”Strutture Civili”. Questo indicatore comparirà non appena avvierai la tua prima costruzione. " +
                "Premendo il pulsante ”Costruisci”, si aprirà una nuova schermata, da cui potrai gestire tutti questi aspetti. " +
                "Quando sei pronto, completa l'obiettivo… e andiamo avanti.",

            12 =>
                "Lascia aperta la schermata \"[warning]Costruisci[/warning]\": i prossimi passaggi ti guideranno nella realizzazione di tutte le strutture necessarie. " +
                "Per comodità, sposta la finestra appena aperta in una posizione dello schermo che ti permetta di tenere tutto sotto controllo e di vedere entrambe le schermate. " +
                "Accanto a ogni struttura puoi osservare la quantità che stai per costruire. Al momento è disponibile solo la Fattoria, e il valore iniziale è impostato su zero. " +
                "Vicino a questo numero trovi i comandi che ti permettono di modificarlo. Con un semplice clic sinistro puoi aumentare la quantità di [warning]1[/warning] unità; " +
                "usando la combinazione Contròl più clic, l'incremento sarà di [warning]5[/warning], mentre con Shift più clic potrai aumentare il valore di [warning]10[/warning]. " +
                "Regola la quantità che desideri… Costruisci una [warning]Fattoria[/warning]. " +
                "Quando sei pronto, completa l'obiettivo… e andiamo avanti.",

            13 =>
                "Hai appena messo in costruzione la tua prima Fattoria." +
                "Ricorda che puoi assegnare la costruzione di più strutture contemporaneamente, ma una nuova costruzione potrà iniziare solo quando un costruttore sarà di nuovo libero. " +
                "Il numero di costruttori disponibili lo puoi vedere in alto, nella schermata ”Strutture Civili”, sopra la prima struttura, la Fattoria. " +
                "Qui è indicato sia il numero massimo di costruttori disponibili… sia quanti di loro sono attualmente liberi. " +
                "Dato che hai già avviato una costruzione, noterai ora il tempo rimanente, che scorre man mano che il lavoro procede. Prima che la Fattoria sia completata, servirà un po' di pazienza. " +
                "In qualsiasi momento, però, puoi velocizzare la costruzione utilizzando i [icon:diamanteBlu][blu]diamanti blu[/blu]. E infatti… utilizziamone qualcuno. Ma prima dobbiamo ottenerli. Vediamo come. " +
                "Torna alla schermata principale e guarda nella sezione Feudi. Sotto il pulsante ”Acquista Feudo” è appena apparso un nuovo pulsante: ”Scambia”. " +
                "Premilo, e potrai convertire alcuni [icon:diamanteViola][viola]diamanti viola[/viola] in [icon:diamanteBlu][blu]diamanti blu[/blu]. " +
                "Per questa costruzione, [icon:diamanteBlu][warning]50 [blu]diamanti blu[/blu] dovrebbero essere più che sufficienti." +
                "Quando sei pronto, completa l'obiettivo… e andiamo avanti.",

            14 =>
                "Ottimo, sei riuscito a ottenere i tuoi primi [icon:diamanteBlu][blu]diamanti blu[/blu] tramite lo scambio." +
                "In questo modo, se dovessi rimanerne sprovvisto e ne avessi bisogno, potrai ottenerli in qualsiasi momento… a patto di avere [icon:diamanteViola][viola]diamanti viola[/viola] a disposizione. " +
                "In questo tutorial ti mostrerò solo come velocizzare la costruzione della Fattoria, ma più avanti potrai fare la stessa cosa anche con le altre strutture, con le unità militari in addestramento… " +
                "e persino con la ricerca. Ora fai clic con il tasto sinistro sull'icona accanto al tempo di costruzione rimanente. " +
                "Dovrebbe avere l'aspetto di un velocizzatore. Prèmila e aumenta il numero di [icon:diamanteBlu][blu]diamanti blu[/blu] da utilizzare fino a [warning]50[/warning]. " +
                "Anche qui valgono le stesse regole di prima: puoi cliccare normalmente, oppure usare le combinazioni di tasti per raggiungere più rapidamente il valore desiderato. " +
                "E non preoccuparti: non serve chiederti se stai usando troppi diamanti. " +
                "Verranno consumati solo quelli realmente necessari… tutti gli altri ti verranno restituiti. " +
                "Quando sei pronto, completa l'obiettivo… e andiamo avanti.",

            15 =>
                "Ottimo lavoro, la fattoria è costruita e il tuo insediamento comincia a prendere forma.\n" +
                "Ora dobbiamo assicurarci di avere i materiali giusti per crescere, il [warning]legno[/warning] è tra questi.\n" +
                "[warning]Costruisci una Segheria[/warning] e rendi stabile la tua economia.\n\n" +
                "Quando sei pronto, completa l'obiettivo ed andiamo avanti.",

            16 =>
                "Ottimo lavoro! Vedo che hai costruito la tua prima segheria, e questo ti permetterà di ottenere legno in modo costante.\n" +
                "Adesso è il momento di assicurarti un'altra risorsa fondamentale per la crescita del tuo insediamento: [warning]Costruisci una Cava di pietra[/warning] per iniziare a estrarre questa risorsa, " +
                "indispensabile per nuove strutture, le riparazioni e la ricerca.\n\n" +
                "Quando sei pronto, completa l'obiettivo ed andiamo avanti.",

            17 =>
                "Perfetto! Vedo che hai costruito la tua prima cava di pietra, ottimo lavoro.\n" +
                "Ora il tuo insediamento sta iniziando ad avere basi solide, ma per crescere davvero ti serve una risorsa ancora più importante: il [warning]ferro[/warning].\n" +
                "Costruisci una [warning]Miniera di ferro[/warning] per avviare l'estrazione e ottenere materiali indispensabili per l'esercito e lo sviluppo futuro.\n\n" +
                "Quando sei pronto, completa l'obiettivo ed andiamo avanti.",

            18 =>
                "Perfetto! Vedo che hai costruito la tua prima miniera di ferro, ottimo lavoro.\n" +
                "Adesso hai iniziato ad estrarre risorse essenziali per lo sviluppo, ma manca ancora qualcosa di fondamentale: una fonte di ricchezza stabile.\n" +
                "L'[warning]oro[/warning] è fondamentale anche per il mantenimento di strutture e unità militari, prima di una grande espansione, controlla sempre le tue produzioni, " +
                "i tuoi consumi non dovrebbero mai superare le tue capacità produttive.\n" +
                "Costruisci una [warning]Miniera d'oro[/warning] per iniziare a estrarre una risorsa preziosa, utile per sostenere l'economia del tuo insediamento.\n\n" +
                "Quando sei pronto, completa l'obiettivo ed andiamo avanti.",

            19 =>
                "La tua miniera d'oro è pronta e il villaggio comincia a prendere vita.\n" +
                "Per farlo crescere davvero servono abitanti che possano viverci e lavorare.\n" +
                "Costruisci una [warning]Casa[/warning] per accogliere nuovi cittadini e dare al tuo insediamento i suoi primi abitanti.\n\n" +
                "Quando sei pronto, completa l'obiettivo ed andiamo avanti.",

            20 =>
                "Adesso è il momento di presentarti le Strutture Militari." +
                "Anche per loro vale lo stesso principio delle costruzioni civili, ma queste produrranno risorse indispensabili per le tue unità, come [warning]Guerrieri[/warning], [warning]Lancieri[/warning], [warning]Arcieri[/warning] e [warning]Catapulte[/warning]." +
                "Le risorse prodotte servono principalmente ad addestrare l'esercito. In base al tipo di unità e al tier selezionato, le quantità richieste aumenteranno insieme alla qualità delle truppe." +
                "Approfondiremo questi aspetti più avanti, quando parleremo direttamente delle unità… e del loro addestramento." +
                "In questo tutorial non costruiremo strutture militari, ma è importante conoscerle fin da subito." +
                "Il loro costo è significativo e tutte richiedono un consumo costante di risorse per produrre equipaggiamento." +
                "Sarai tu a decidere quando iniziare la produzione, al termine di questo tutorial… quando ti sentirai davvero pronto. " +
                "Quando sei pronto, completa l'obiettivo… e andiamo avanti.",

            21 =>
                "Passiamo ora alle Unità Militari. " +
                "Fino a questo momento non erano visibili, ma ora puoi trovarle sia nella schermata ”Costruisci”, sia direttamente nella schermata di gioco, accanto alle strutture civili." +
                "In entrambi i casi, l'interfaccia è molto simile. Sopra alle unità militari puoi notare i livelli, che vanno da uno a sei." +
                "Per ora potrai addestrare solamente le unità di livello uno, ma in futuro, salendo di livello, sbloccherai anche quelle più avanzate." +
                "Le unità sono suddivise in Guerrieri, Lancieri, Arcieri e Catapulte." +
                "Tutte richiedono un mantenimento costante di cibo e oro, e necessitano di risorse ed equipaggiamento militare per essere addestrate." +
                "Al momento, però, anche se disponessi delle risorse necessarie, non potresti ancora addestrarle. Non hai infatti accesso alle Caserme, che verranno sbloccate più avanti nel gioco." +
                "Quando sei pronto, completa l'obiettivo… e andiamo avanti.",

            22 =>
                "Passiamo ora alle Caserme." +
                "Come anticipato, queste strutture sono fondamentali per fondare il tuo esercito: senza di esse non puoi formare alcuna forza militare." +
                "La loro costruzione consente di alloggiare un certo numero di uomini, che varia in base al tipo di struttura." +
                "Esistono caserme dedicate ai Guerrieri, ai Lancieri, agli Arcieri e alle Catapulte, e ognuna è pensata per supportare una specifica tipologia di unità." +
                "Tuttavia, il loro utilizzo comporta un costo: richiedono una manutenzione costante in cibo e oro per poter funzionare correttamente." +
                "Prima di costruirle, assicurati che la tua produzione sia in grado di sostenere queste spese nel tempo." +
                "Una gestione attenta delle risorse sarà fondamentale per mantenere efficiente il tuo esercito." +
                "Quando sei pronto, completa l'obiettivo… e andiamo avanti.",

            23 =>
                "Perfetto, ci siamo. Abbiamo finalmente concluso con le costruzioni, quindi lasciami introdurre brevemente l'addestramento, giusto per restare in tema con questa schermata." +
                "Come puoi vedere, sulla destra è apparsa la sezione dedicata all'addestramento. Qui potrai osservare le diverse unità disponibili." +
                "Come accennato in precedenza, nella parte superiore è visibile il tier selezionato: al momento è disponibile solo il tier 1, mentre i successivi verranno sbloccati più avanti." +
                "Di questo parleremo nel dettaglio quando affronteremo direttamente le unità. Per ora è tutto. Chiudi la schermata Costruisci." +
                "Quando sei pronto, completa l'obiettivo e andiamo avanti.",

            24 =>
                "Ottimo, non ci resta che introdurre la mappa del villaggio. Come puoi notare, in basso è appena apparso un nuovo pulsante: ”Città”." +
                "Premilo e lasciati mostrare come sono composti i villaggi di questo regno. Iniziamo da una struttura fondamentale: le Mura." +
                "Come puoi vedere, quasi tutte le strutture condividono la stessa impostazione." +
                "In cima è presente il nome della struttura; subito alla sua destra trovi un numero tra parentesi quadre, che indica l'ordine delle strutture." +
                "Accanto è visibile il livello della struttura. Subito sotto sono presenti tre barre: [warning]Salute[/warning], [warning]Difesa[/warning] e [warning]Guarnigione[/warning]. Queste sono le tue strutture difensive. In totale sono sei." +
                "Tre di esse, oltre alle Mura — Cancello, Torri e Castello — condividono le stesse statistiche." +
                "La guarnigione rappresenta il numero massimo di unità che la struttura può ospitare e quante ne sono effettivamente assegnate al suo interno. Le restanti due strutture, Ingresso e Centro, sono simili ma dispongono solo dello spazio per la guarnigione, senza barre di salute e difesa." +
                "È importante mantenere le strutture in buono stato. In caso di attacco al villaggio, una difesa ben organizzata può fare la differenza e ridurre le perdite." +
                "Fai però attenzione: se una struttura dovesse perdere tutta la salute e arrivare a zero, crollerà. In quel caso, tutte le unità presenti al suo interno perderanno la vita durante il crollo." +
                "L'assegnazione delle unità è quindi cruciale. " +
                "Facciamo un esempio: se nell'Ingresso sono presenti unità in guarnigione, pur essendo una struttura priva di salute e difesa, il loro posizionamento, insieme a unità assegnate a Mura e Cancello, " +
                "può fornire un bonus difensivo alle unità presenti all'ingresso, in base al numero schierato. In generale, è sempre consigliabile mantenere ben rifornite le guarnigioni in questa schermata. " +
                "Spesso è meglio avere più uomini in difesa piuttosto che rischiare una sconfitta rovinosa. Se le unità attaccanti dovessero superare il Castello, il settimo e ultimo attacco colpirà direttamente te. " +
                "In caso di sconfitta, l'attaccante potrà saccheggiare le tue risorse, inclusi diamanti viola e diamanti blu." +
                "Quando sei pronto, completa l'obiettivo e andiamo avanti.",

            25 =>
                "Oh no, guarda… un sabotatore ha appena danneggiato una tua struttura." +
                "Vedi? La salute e la difesa delle Mura sono diminuite. Questo non va bene, ma è l'occasione perfetta per introdurre le riparazioni." +
                "Quando una struttura viene danneggiata a seguito di un assedio, necessita di essere riparata. Per farlo, puoi notare che accanto alle sue statistiche è comparsa un'icona raffigurante la ”Riparazione”." +
                "Questa icona è visibile solo quando la struttura ha subito dei danni. Premila e vedrai come i tuoi uomini si affretteranno a sistemarla." +
                "Presta attenzione: le riparazioni richiedono tempo e risorse. Se le risorse dovessero terminare, il processo di riparazione si fermerà automaticamente, quindi assicurati di averne a sufficienza." +
                "Come già visto per le altre icone, puoi portare il puntatore del mouse sopra l'icona di riparazione per visualizzare il menu a tendina." +
                "Qui potrai controllare le risorse necessarie per ripristinare ogni punto di salute e il tempo richiesto per completare il lavoro." +
                "Quando sei pronto, completa l'obiettivo e andiamo avanti.",

            26 =>
                "Ottimo, siamo giunti alla fine di questa schermata." +
                "Per assegnare uomini a una struttura è sufficiente premere il pulsante ”Guarnigione” presente sotto ciascuna di esse." +
                "Da qui potrai selezionare le unità e il loro livello, assegnandole alla struttura scelta." +
                "Questo passaggio può essere ripetuto per tutte le strutture disponibili, non appena disporrai di un esercito al tuo servizio. Ricorda inoltre che Salute, Difesa e capacità di guarnigione possono essere potenziate tramite la ricerca." +
                "Il livello del tuo personaggio avrà un ruolo fondamentale nello sviluppo della città e nella sua capacità di resistere agli attacchi." +
                "Quando sei pronto, completa l'obiettivo e andiamo avanti.",

            27 =>
                "Puoi chiudere la schermata ”Città”, non ci servirà più." +
                "Nota però che, nella schermata di gioco, accanto ai diamanti viola è comparsa una nuova icona: il tuo personaggio, con il suo nome." +
                "Cliccando con il tasto sinistro sulla tua icona si aprirà una nuova schermata dedicata alle ”Statistiche”. Questa schermata non è fondamentale, ma vale la pena soffermarsi un momento." +
                "Troverai due colonne: quella di destra mostra le tue statistiche personali, dove potrai osservare informazioni interessanti che potrebbero tornarti utili in futuro." +
                "Sulla sinistra, invece, è indicato lo stato attuale del tuo villaggio, con i tempi rimanenti prima del reset delle varie attività, la potenza complessiva e i bonus applicati, sia temporanei che permanenti." +
                "Quando sei pronto, completa l'obiettivo e andiamo avanti.",

            28 =>
                "Puoi chiudere la schermata ”Statistiche”, non ci servirà più." +
                "Nota però che ora è disponibile un nuovo pulsante: lo \"[warning]Shop[/warning]\". È il luogo in cui potrai ottenere diamanti viola e molto altro." +
                "All'interno dello shop potrai aumentare il numero massimo di costruttori e di addestratori, acquistare lo Scudo della Pace per una difesa aggiuntiva delle tue terre, attivare il VIP, " +
                "che garantisce vantaggi esclusivi e l'accesso a premi extra nelle quest, e scegliere tra due varianti di GamePass, Base e Avanzato." +
                "Come per le altre interfacce, puoi spostare il puntatore del mouse sulle icone per visualizzare informazioni più dettagliate su ogni elemento disponibile." +
                "Prenditi un momento per esplorare liberamente lo shop." +
                "Quando sei pronto, completa l'obiettivo e andiamo avanti.",

            29 =>
                "Puoi chiudere la schermata ”Shop”, non ci servirà più. Nota però che nella schermata principale è comparso un nuovo pulsante: ”[warning]Ricerca[/warning]”." +
                "Questo luogo è fondamentale se vuoi tentare di raggiungere anche solo una minima frazione della potenza dell'Imperatore." +
                "Le ricerche disponibili sono numerose, e altre potranno essere aggiunte in futuro." +
                "Come ormai sarai abituato, puoi spostare il puntatore del mouse su ciascuna ricerca per ottenere informazioni dettagliate sui costi e sui tempi necessari al completamento." +
                "Ricorda però che è possibile effettuare una sola ricerca alla volta, quindi scegli con attenzione cosa sviluppare per primo." +
                "Potrai migliorare quasi tutto: dalla produzione di risorse, all'implementazione di strategie per attrarre nuovi abitanti, fino al potenziamento dell'addestramento, sia della città che delle singole unità militari, e molto altro ancora." +
                "Ti lascio qualche momento per familiarizzare con la schermata della ricerca, ma potrai esplorarla più approfonditamente anche in seguito." +
                "Quando sei pronto, completa l'obiettivo e andiamo avanti.",

            30 =>
                "Puoi chiudere la schermata ”Ricerca”, non ci servirà più." +
                "Ora, nella schermata principale, è comparso un nuovo pulsante: \"[warning]Quest Mensili[/warning]\". Qui in realtà non c'è molto da spiegare." +
                "Si tratta di una schermata in cui puoi vedere le quest disponibili: il loro requisito per essere completate e l'esperienza che forniranno singolarmente." +
                "Più in basso troverai un pulsante che ti permette di scorrere tra le quest disponibili. Alcune quest possono essere completate più volte, aumentando progressivamente il loro requisito." +
                "Infine, potrai osservare la barra del progresso, che mostra quanta esperienza hai accumulato. I premi collegati possono essere reclamati una volta raggiunte le soglie indicate: " +
                "quelli sopra la barra sono disponibili per tutti, mentre quelli sotto possono essere raccolti solo se possiedi il [warning]Gamepass Silver[/warning]." +
                "Quando sei pronto, completa l'obiettivo e andiamo avanti.",

            31 =>
                "Puoi chiudere la schermata ”Quest Mensili”, non ci servirà più. Ora, nella schermata principale, è comparso un nuovo pulsante: \"[warning]Battaglie[/warning]\"." +
                "Qui potrai trovare i Villaggi Barbari, le Città Barbare e anche i villaggi di altri giocatori." +
                "Al momento potrebbe sembrarti poco rilevante, ma la battaglia è l'unico modo per ottenere esperienza e salire di livello." +
                "Non sarà il tuo obiettivo principale, perché le cose da fare sono molte, ma non trascurarlo: farlo ti permetterà di progredire nel gioco senza restare indietro." +
                "Fin dall'inizio saranno disponibili i Villaggi Barbari. Non sottovalutarli: possono riservare spiacevoli sorprese, e ricorda che in ogni scontro tu sarai l'attaccante." +
                "Le Città Barbare sono più difficili: contengono più uomini e sono meglio organizzate. Fai attenzione quando ti troverai ad affrontarle." +
                "Quando sei pronto, completa l'obiettivo e andiamo avanti.",

            32 =>
                "Sei giunto alla fine di questo splendido viaggio." +
                "A questo punto credo tu sia pronto: ti ho raccontato tutto ciò che sapevo sulle meccaniche di questo nuovo mondo, o almeno spero di non aver dimenticato nulla." +
                "In caso contrario, lascio a te il piacere di scoprirlo con il tempo." +
                "Dopotutto, sei arrivato fin qui con successo, dimostrando di saper affrontare ogni sfida che ti è stata posta davanti." +
                "Non mi resta che salutarti. Io mi allontanerò da queste terre per rifugiarmi in un luogo sicuro, lasciandoti il regno nelle tue mani." +
                "Da ora in poi, il tuo destino dipenderà solo dalle tue scelte.",

            _ => ""
        };

        // Etichette UI
        public string Label_CostoCostruzione() => "Costo Costruzione";
        public string Label_CostoAddestramento() => "Costo Addestramento";
        public string Label_Statistiche() => "Statistiche";
        public string Label_Cibo() => "Cibo";
        public string Label_Legno() => "Legno";
        public string Label_Pietra() => "Pietra";
        public string Label_Ferro() => "Ferro";
        public string Label_Oro() => "Oro";
        public string Label_Popolazione() => "Popolazione";
        public string Label_Spade() => "Spade";
        public string Label_Lancie() => "Lancie";
        public string Label_Archi() => "Archi";
        public string Label_Scudi() => "Scudi";
        public string Label_Armature() => "Armature";
        public string Label_Frecce() => "Frecce";
        public string Label_Costruzione() => "Costruzione";
        public string Label_Addestramento() => "Addestramento";
        public string Label_Ricerca() => "Ricerca";
        public string Label_Produzione() => "Produzione risorse";
        public string Label_Mantenimento_Cibo() => "Mantenimento cibo";
        public string Label_Mantenimento_Legno() => "Mantenimento legno";
        public string Label_Mantenimento_Pietra() => "Mantenimento pietra";
        public string Label_Mantenimento_Ferro() => "Mantenimento ferro";
        public string Label_Mantenimento_Oro() => "Mantenimento oro";
        public string Label_Livello() => "Livello";
        public string Label_Salute() => "Salute";
        public string Label_Difesa() => "Difesa";
        public string Label_Attacco() => "Attacco";
        public string Label_Limite_Magazzino() => "Limite magazzino";
        public string Label_Limite_Unità() => "Limite";
        public string Label_Limite_Strutture() => "Limite";

        //Terreni Virtuali - Feudi
        public string Terreni_DiamantiInsufficienti() =>
            $"Log_Server|[title]Non hai abbastanza[/title] [viola]Diamanti Viola[/viola][icon:diamanteViola] [title]per un terreno virtuale.[/title]";
        public string Terreni_DiamantiUtilizzati() =>
            $"Log_Server|[warning][icon:diamanteViola]{Strutture.Edifici.Terreni_Virtuali.Diamanti_Viola}[viola] Diamanti Viola[/viola] [title]utilizzati per un terreno virtuale...[/title]";
        public string Terreni_Ottenuto(string terreno) =>
            $"Log_Server|[warning]Feudo ottenuto:[/warning] [{terreno.Replace(" ", "")}]{terreno}[/{terreno.Replace(" ", "")}][icon:{terreno.Replace(" ", "")}]";

        public string Costruzione_RisorseUtilizzate(int count, string buildingType, Strutture.Edifici cost) =>
            $"Log_Server|[info]Risorse utilizzate[/info] per la costruzione di [warning]{count} {buildingType}[/warning]:\r\n " +
            $"[cibo][icon:cibo]-{cost.Cibo * count:N0}[/cibo]  " +
            $"[legno][icon:legno]-{cost.Legno * count:N0}[/legno]  " +
            $"[pietra][icon:pietra]-{cost.Pietra * count:N0}[/pietra]  " +
            $"[ferro][icon:ferro]-{cost.Ferro * count:N0}[/ferro] " +
            $"[oro][icon:oro]-{cost.Oro * count:N0}[/oro]  " +
            $"[popolazione][icon:popolazione]-{cost.Popolazione * count:N0}[/popolazione]";

        //Descrizioni Costruzione
        public string Costruzione_RisorseInsufficienti(int count, string buildingType) =>
            $"Log_Server|[error]Risorse insufficienti per costruire [title]{count} {buildingType}.";
        public string Costruzione_Pausa(string struttura) =>
            $"Log_Server|[warning]Costruzione di [title]{struttura} [warning]messa in pausa per riduzione slot.";
        public string Costruzione_Avvio(string struttura, string tempo) =>
            $"Log_Server|[title]Costruzione di [info]{struttura} [title]iniziata, durata: [icon:tempo]{tempo}";
        public string Costruzione_Completata(string struttura) =>
            $"Log_Server|[success]Costruzione completata: [title]{struttura}[icon:{struttura}]";
        public string Costruzione_EdificioNonValido(string struttura) =>
            $"Log_Server|[error]Tipo edificio [title]{struttura}[icon:{struttura}] [error]non valido!";
        public string NumeroDiamantiNonValido() =>
            $"Log_Server|[error]Numero [blu][icon:diamanteBlu]diamanti [error]non valido.";
        public string DiamantiInsufficienti() =>
            $"Log_Server|[error]Non hai abbastanza [blu]Diamanti Blu![icon:diamanteBlu]";
        public string Costruzione_NessunaCostruzione() =>
            $"Log_Server|[warning]Non ci sono costruzioni da velocizzare.";
        public string Costruzione_Velocizzazione(int diamanti, string tempo) =>
            $"Log_Server|[title]Hai usato [blu][icon:diamanteBlu][warning]{diamanti} [blu]Diamanti Blu [title]per velocizzare le costruzioni! [icon:tempo]{tempo}";

        //Descrizioni Addestramento
        public string Addestramento_LimiteRaggiunto(string unità, string numero, string limite) =>
            $"Log_Server|[title]Limite truppe raggiunto per i {unità}.[icon:{unità}] [warning]{numero}/{limite}";
        public string Addestramento_RisorseUtilizzate(int count, string unità, Esercito.CostoReclutamento unitCost) =>
           $"Log_Server|[info]Risorse utilizzate[/info] per l'ddestramento di [warning]{count} {unità}[/warning]:\r\n " +
                $"[cibo][icon:cibo]-{(unitCost.Cibo * count):N0}[/cibo] " +
                $"[legno][icon:legno]]-{(unitCost.Legno * count):N0}[/legno] " +
                $"[pietra][icon:pietra]-{(unitCost.Pietra * count):N0}[/pietra] " +
                $"[ferro][icon:ferro]-{(unitCost.Ferro * count):N0}[/ferro] " +
                $"[oro][icon:oro]-{(unitCost.Oro * count):N0}[/oro] " +
                $"[popolazione][icon:popolazione]-{(unitCost.Popolazione * count):N0}[/oro]";

        public string Addestramento_RisorseInsufficienti(int count, string unità) =>
            $"Log_Server|[error]Risorse insufficienti per costruire [title]{count} {unità}.";
        public string Addestramento_Pausa(string unità) =>
            $"Log_Server|[tile]Reclutamento di [warning]{unità} [tile]messa in pausa per riduzione slot.";
        public string Addestramento_Avvio(string unità, string tempo) =>
            $"Log_Server|[title]Reclutamento di[/title] {unità} [title]iniziata. Durata[/title] [icon:tempo]{tempo}";
        public string Addestramento_Completata(string unità) =>
            $"Log_Server|[warning]{unità}[/warning] addestrato!";
        public string Addestramento_UnitàNonValido(string unità) =>
            $"Log_Server|[error]Tipo edificio [title]{unità}[icon:{unità}] [error]non valido!";
        public string Addestramento_NessunaUnità() =>
            $"Log_Server|[warning]Non ci sono unità da velocizzare.";
        public string Addestramento_Velocizzazione(int diamanti, string tempo) =>
            $"Log_Server|[title]Hai usato [blu][icon:diamanteBlu][warning]{diamanti} [blu]Diamanti Blu [title]per velocizzare l'addestramento! [icon:tempo]{tempo}";

        //Descrizioni Ricerca
        public string Ricerca_LivelloRichiesto(string ricerca, int livelloRicerca, string msg, int richiesto) =>
            $"Log_Server|[error]La ricerca [title]{ricerca} {livelloRicerca} [error]richiede che il livello del [title]{msg}[error] sia almeno lv [title]{richiesto}";
        public string Ricerca_Start(string ricerca, string tempo) =>
            $"Log_Server|[info]Ricerca di [title]{ricerca} [title]iniziata. Durata [icon:tempo]{tempo}";
        public string Ricerca_Completata(string ricerca) =>
            $"Log_Server|[success]Ricerca completata: [title]{ricerca}";
        public string Ricerca_NessunaRicerca() =>
            $"Log_Server|[warning]Nessuna ricerca da velocizzare.";
        public string Ricerca_Velocizzazione(int diamanti, string tempo) =>
            $"Log_Server|[title]Hai usato [blu][icon:diamanteBlu][warning]{diamanti} [blu]Diamanti Blu [title]per velocizzare la ricerca! [icon:tempo]{tempo}";

        // ── Descrizioni narrative: strutture civili ────────────────────────────
        public string Desc_Fattoria() =>
            "La fattoria è la struttura principale per la produzione di [icon:cibo]Cibo, fondamentale per la costruzione di edifici militari e civili, " +
            "l'addestramento delle unità militari ed il loro mantenimento. Indispensabile per la ricerca tecnologica e di componenti militari";

        public string Desc_Segheria() =>
            "La Segheria è la struttura principale per la produzione di [icon:legno]Legna, fondamentale per la costruzione di strutture militari, civili e " +
            "l'addestramento delle unità militari. Indispensabile per la ricerca tecnologica e di componenti militari";

        public string Desc_CavaPietra() =>
            "La cava di pietra è la struttura principale per la produzione di [icon:pietra]Pietra, fondamentale per la costruzione di strutture militari, civili e " +
            "l'addestramento delle unità militari. Indispensabile per la ricerca tecnologica e di componenti militari";

        public string Desc_MinieraFerro() =>
            "La Miniera di ferro è la struttura principale per la produzione di [icon:ferro]Ferro, fondamentale per la costruzione di strutture militari, civili e " +
            "l'addestramento delle unità militari. Indispensabile per la ricerca tecnologica e di componenti militari";

        public string Desc_MinieraOro() =>
            "La miniera d'oro è la struttura principale per la produzione di [icon:oro]oro, fondamentale per la costruzione di strutture militari, civili e " +
            "l'addestramento delle unità militari. Indispensabile per la ricerca tecnologica e di componenti militari";

        public string Desc_Case() =>
            "Le Case sono necessarie per attirare sempre più [icon:popolazione]cittadini, verso il vostro villaggio, " +
            "sono fondamentali per la costruzione di strutture militari e civili, oltre che per addestrare le unità militari";

        // ── Descrizioni narrative: workshop ────────────────────────────────────
        public string Desc_ProduzioneSpade() =>
            "Workshop Spade questa struttura produce equipaggiamento militare specifico, " +
            "essenziali per l'addestramento di unità militari, questa struttura produce [icon:spade]Spade";

        public string Desc_ProduzioneLance() =>
            "Workshop Lancie questa struttura produce equipaggiamento militare specifico, " +
            "essenziali per l'addestramento di unità militari, questa struttura produce [icon:lance]Lancie";

        public string Desc_ProduzioneArchi() =>
            "Workshop Archi questa struttura produce equipaggiamento militare specifico, " +
            "essenziali per l'addestramento di unità militari, questa struttura produce [icon:archi]Archi";

        public string Desc_ProduzioneScudi() =>
            "Workshop Scudi questa struttura produce equipaggiamento militare specifico, " +
            "essenziali per l'addestramento di unità militari, questa struttura produce [icon:scudi]Scudi";

        public string Desc_ProduzioneArmature() =>
            "Workshop Armature questa struttura produce equipaggiamento militare specifico, " +
            "essenziali per l'addestramento di unità militari, questa struttura produce [icon:armature]Armature";

        public string Desc_ProduzioneFrecce() =>
            "Workshop Frecce questa struttura produce equipaggiamento militare specifico, " +
            "essenziali per l'addestramento di unità militari, questa struttura produce [icon:frecce]Frecce";

        // ── Descrizioni narrative: caserme ─────────────────────────────────────
        public string Desc_CasermaGuerrieri() =>
            "Caserma guerrieri questa struttura militare di fondamentale presenza per ogni villaggio, permette l'addestramento ed il mantenimento di unità militari specifiche.\n\n" +
            "Ogni caserma è attrezzata per ospitare un certo numero di uomini, raccomandati di averne un numero sufficiente";

        public string Desc_CasermaLanceri() =>
            "Caserma lancieri questa struttura militare di fondamentale presenza per ogni villaggio, permette l'addestramento ed il mantenimento di unità militari specifiche.\n\n" +
            "Ogni caserma è attrezzata per ospitare un certo numero di uomini, raccomandati di averne un numero sufficiente";

        public string Desc_CasermaArceri() =>
            "Caserma arcieri questa struttura militare di fondamentale presenza per ogni villaggio permette l'addestramento ed il mantenimento di unità militari specifiche.\n\n" +
            "Ogni caserma è attrezzata per ospitare un certo numero di uomini, raccomandati di averne un numero sufficiente";

        public string Desc_CasermaCatapulte() =>
            "Caserma catapulte questa struttura militare di fondamentale presenza per ogni villaggio, permette l'addestramento ed il mantenimento di unità militari specifiche.\n\n" +
            "Ogni caserma è attrezzata per ospitare un certo numero di uomini, raccomandati di averne un numero sufficiente";

        // ── Descrizioni narrative: guerrieri ───────────────────────────────────
        public string Desc_Guerriero(int numero) =>
            $"I guerrieri {ToRoman(numero)} sono la spina dorsale dell'esercito, anche se sprovvisti di scudo sono, " +
            "sono facili da reclutare e non sono molto dispendiosi in cibo ed oro";

        // ── Descrizioni narrative: lancieri ────────────────────────────────────
        public string Desc_Lancere(int numero) =>
            $"I lancieri {ToRoman(numero)} sono la spina dorsale di ogni esercito ben organizzato. Armati di lance, " +
            "questi soldati costituiscono un baluardo formidabile contro gli assalti nemici";

        // ── Descrizioni narrative: arcieri ─────────────────────────────────────
        public string Desc_Arcere(int numero) =>
            $"Gli arcieri {ToRoman(numero)} armati di arco e faretra, sono soldati specializzati, dominano il campo di battaglia dalla distanza, " +
            "scagliando frecce mortali sulle linee nemiche, prima che possano avvicinarsi";

        // ── Descrizioni narrative: catapulte ───────────────────────────────────
        public string Desc_Catapulta(int numero) =>
            $"Le catapulte {ToRoman(numero)} sono potenti macchine d'assedio che cambiano le sorti delle battaglie, " +
            "scagliano enormi proiettili distruggendo mura e seminando il terrore tra le fila nemiche";

        // ── Descrizioni narrative: esperienza / livello ────────────────────────
        public string Desc_Esperienza(string esperienza) =>
            $"L’esperienza[icon:xp] rappresenta la crescita del giocatore nel tempo.\nAccumulare esperienza permette di salire di livello.\n\n Esperienza prossimo Livello: [icon:xp][acciaioBlu]{esperienza}[black]xp";

        public string Desc_Livello() =>
            "Il livello[icon:lv] indica il grado di avanzamento del giocatore, fondamentale per raggiungere le vette nella ricerca, poter migliorare unità e strutture.\n\n" +
            "Necessaria per avanzare nel 'PVP/PVE', migliorare le strutture difensive del proprio villaggio, oltre che per lo sblocco di unità militari avanzate.\n " +
            "Attualmente non è presente un limite al livello";

        public string Desc_Statistiche() =>
                $"Scheda statistiche, qui è possibile visualizzare le proprie statistiche di gioco cliccando l'icona, insieme a ulteriori informazioni molto utili durante l'avanzamento.\n";
        
        public string Desc_DiamantiBlu() =>
            $"I [blu]Diamanti Blu[/blu][black][icon:diamanteBlu] possono essere utilizzati all'interno dello shop per l'acquisto di pacchetti, per una migliore gestione della città.\n\n" +
            $"Inoltre possono essere richiesti in alcune quest, velocizzare i tempi d'attesa per strutture e unità militari.\n";

        public string Desc_DiamantiViola() =>
                $"I [viola]Diamanti Viola[/viola][black][icon:diamanteViola] fondamentali per l'acquisto di [warning]feudi[warning][black], sono alla base dell'economia.\n\nPossono essere scambiati per [blu]Diamanti Blu[/blu][black][icon:diamanteBlu] ed utilizzati " +
                $"all'interno dello shop per l'acquisto di pacchetti o per una migliore gestione della città.\n\n" +
                $"Oltre ad essere richiesti in alcune quest, dovrebbero essere sempre presenti nelle casse della città.";

        public string Desc_DollariVirtuali() =>
                $"I [icon:dollariVirtuali]Tributi dei feudi vengono generati tramite i feudi posseduti dal giocatore.\n\nPossono essere prelevati raggiunta la soglia di [verde]{Variabili_Server.prelievo_Minimo}$[/verde][black][icon:dollariVirtuali] " +
                $"oppure utilizzati all'interno dello shop per l'acquisto di pacchetti.\nAttualmente [icon:dollariVirtuali]1 tributo equivale ad [icon:usdt]1 USDT";

        public string Desc_Cibo() =>
                $"Il [icon:cibo]Cibo, è fondamentale per il mantenimento delle unità militari, per il loro addestramento e la costruzione di strutture.\n\n" +
                $"Necessario anche per la ricerca.\n\n";

        public string Desc_Legno() =>
                $"Il [icon:legno]Legno, è necessario per l'addestramento di unità militari e la costruzione di strutture. Necessario per la ricerca.\n\n";

        public string Desc_Pietra() =>
                $"La [icon:pietra]Pietra, fondamentale per la riparazione delle strutture difensive. Necessario per la ricerca.\n\n";

        public string Desc_Ferro() =>
                $"Il [icon:ferro]Ferro, fondamentale per la costruzione di edifici e la riparazione delle strutture difensive.\n\n" +
                $"E' necessiamo per la produzione di armamento militare e per la ricerca.\n\n";

        public string Desc_Oro() =>
                $"L [icon:oro]Oro è una risorsa primaria per le casse della città, necessario per la costruzione di edifici civili e militari, oltre che per il reclutamento delle unità.\n\n" +
                $"E' fondamentale per il mantenimento di unità e strutture belliche oltre alla ricerca.\n\n";

        public string Desc_Popolazione() =>
                $"La [icon:popolazione]Popolazione è fondamentale per la costruzione di strutture civili e militari, oltre al reclutamento di unità.\n\n";

        public string Desc_Spade() => 
            $"Descrizione|Spade|[black]Le Spade[icon:spade] sono necessarie per l'addestramento dei [cuoioScuro]guerrieri[icon:guerriero][black].\n\n";
        
        public string Desc_Lance() => 
            $"Descrizione|Lance|[black]Le Lancie[icon:lance] sono necessarie per l'addestramento dei [cuoioScuro]lancieri[icon:lancere][black].\n\n";
        
        public string Desc_Archi() => 
            $"Descrizione|Archi|[black]Gli Archi[icon:archi] sono necessari per l'addestramento degli [cuoioScuro]arcieri[icon:arcere][black].\n\n";
        
        public string Desc_Scudi() => 
            $"Descrizione|Scudi|[black]Gli Scudi[icon:scudi] sono necessari per l'addestramento delle unità militari.\n\n";
        
        public string Desc_Armature() => 
            $"Descrizione|Armature|[black]Le Armature[icon:armature] sono necessarie per l'addestramento delle unità militari.\n\n";
        
        public string Desc_Frecce() => 
            $"Descrizione|Frecce|[black]Le Frecce[icon:frecce] sono fondamentali per le [cuoioScuro]unità a distanza[black], senza di esse sono praticamente inutili.\n\n";

        // ── Descrizioni narrative: shop ────────────────────────────────────────
        public string Desc_Shop_GamePassBase() =>
            "Tramite l'acquito di questo pacchetto 'GamePass Base'...\nAttualmente non disponibili vantaggi. Ha una durata di [title]30 [black]giorni";

        public string Desc_Shop_GamePassAvanzato() =>
            "Tramite l'acquito di questo pacchetto 'GamePass Avanzato' ...\nAttualmente non disponibili vantaggi. Ha una durata di [title]30 [black]giorni";

        public string Desc_Shop_Vip1() =>
            "Tramite l'acquito di questo pacchetto 'Vip 1', il tempo del vip verrà incrementato.\n Un massimo di [ferroScuro]2 [black]giorni può essere accumulato, acquistando più pacchetti";

        public string Desc_Shop_Vip2() =>
            "Tramite l'acquito di questo pacchetto 'Vip 2', il tempo del vip verrà incrementato. Una volta confermata la transazione dalla blockchain i diamanti verranno accreditati immediatamente.\n Un massimo di [ferroScuro]2 [black]giorni può essere accumulato, acquistando più pacchetti";

        public string Desc_Shop_Costruttore(string durata) =>
            $"Tramite l'acquito di questo pacchetto 'Costruttore {durata}' è possibile richiedere un costruttore aggiuntivo.\n" +
            "Il tempo di costruzione verrà incrementato.\n Un massimo di [ferroScuro]3 [black]giorni può essere accumulato, acquistando più pacchetti";

        public string Desc_Shop_Reclutatore(string durata) =>
            $"Tramite l'acquito di questo pacchetto 'Reclutatore {durata}' è possibile richiedere un reclutatore aggiuntivo.\n" +
            "Il tempo di reclutamento verrà incrementato.\n Un massimo di [ferroScuro]2 [black]giorni può essere accumulato, acquistando più pacchetti";

        public string Desc_Shop_ScudoPace(string durata) =>
            $"Tramite l'acquito di questo pacchetto 'scudo della pace {durata}', si otterrà una protezione dagli attacchi degli altri giocatori.\n" +
            "Il tempo dello scudo verrà incrementato al tempo disponibile dello scudo.\n Un massimo di [ferroScuro]7 [black]giorni può essere accumulato, acquistando più pacchetti";

        //Ricerca
        public string Desc_SaluteMura() =>
            $"Descrizione|Mura Salute|[black]Ripara la [verdeF]salute[black] delle [porporaReale]mura[black] al massimo.\nCosto riparazioni per [verdeF]1 HP[/verdeF][black] per ogni ciclo.\n\n";
        public string Desc_DifesaMura() =>
            $"Descrizione|Mura Difesa|[black]Ripara la [blu]difesa[black] delle [porporaReale]mura[black] al massimo.\nCosto riparazioni per [blu]1 DEF[/blu][black] per ogni ciclo.\n\n";
        public string Desc_SaluteCancello() =>
            $"Descrizione|Cancello Salute|[black]Ripara la [verdeF]salute[black] del [porporaReale]cancello[black] al massimo.\nCosto riparazioni per [verdeF]1 HP[/verdeF][black] per ogni ciclo.\n\n";
        public string Desc_DifesaCancello() =>
            $"Descrizione|Cancello Difesa|[black]Ripara la [blu]difesa[black] del [porporaReale]cancello[black] al massimo.\nCosto riparazioni per [blu]1 DEF[/blu][black] per ogni ciclo.\n\n";
        public string Desc_SaluteTorri() =>
            $"Descrizione|Torri Salute|[black]Ripara la [verdeF]salute[black] delle [porporaReale]torri[black] al massimo.\nCosto riparazioni per [verdeF]1 HP[/verdeF][black] per ogni ciclo.\n\n";
        public string Desc_DifesaTorri() =>
            $"Descrizione|Torri Difesa|[black]Ripara la [blu]difesa[black] delle [porporaReale]torri[black]al massimo.\nCosto riparazioni per [blu]1 DEF[/blu][black] per ogni ciclo.\n\n";
        public string Desc_CastelloSalute() =>
            $"Descrizione|Castello Salute|[black]Ripara la [verdeF]salute[black] del [porporaReale]castello[black]al massimo.\nCosto riparazioni per [verdeF]1 HP[/verdeF][black] per ogni ciclo.\n\n";
        public string Desc_CastelloDifesa() =>
            $"Descrizione|Castello Difesa|[black]Ripara la [blu]difesa[black] del [porporaReale]castello[black]al massimo.\nCosto riparazioni per [blu]1 DEF[/blu][black] per ogni ciclo.\n\n";
        public string Desc_RicercaAddestramento(int livello) =>
            $"Descrizione|Ricerca Addestramento|[black]Permette di diminuire il tempo necessario per l'addestramento di ogni unità.\nCosto ricerca addestramento lv {livello}\n\n";
        public string Desc_RicercaCostruzione(int livello) =>
            $"Descrizione|Ricerca Costruzione|[black]Permette di diminuire il tempo necessario per la costruzione di ogni edificio.\nCosto ricerca costruzione lv {livello}\n\n";
        public string Desc_RicercaProduzione(int livello) =>
            $"Descrizione|Ricerca Produzione|[black]Permette di incrementare la quantità di risorse prodotte da ogni struttura produttiva.\nCosto ricerca produzione lv {livello}\n\n";
        public string Desc_RicercaPopolazione(int livello) =>
            $"Descrizione|Ricerca Popolazione|[black]Permette di implementaremigliori strategie per aumentare il numero di cittadini che giungono nel tuo villaggio.\nCosto ricerca popolazione lv {livello}\n\n";
        public string Desc_RicercaTrasporto(int livello) =>
            $"Descrizione|Ricerca Trasporto|[black]Permette di aumentare la capacità di trasporto delle singole unità militari.\nCosto ricerca trasporto lv {livello}\n\n";
        public string Desc_RicercaRiparazione(int livello) =>
            $"Descrizione|Ricerca Riparazione|[black]Permette di migliorare la riparazione delle singole strutture.\nCosto ricerca riparazione lv {livello}\n\n";
        public string Desc_RicercaGuerrieroLivello(int livello) =>
            $"Descrizione|Ricerca Guerriero Livello|[black]Aumenta il livello dei guerrieri.\nCosto ricerca livello guerriero lv {livello}\n\n";
        public string Desc_RicercaGuerrieroSalute(int livello) =>
            $"Descrizione|Ricerca Guerriero Salute|[black]Aumenta la salute dei guerrieri.\nCosto ricerca salute guerriero lv {livello}\n\n";
        public string Desc_RicercaGuerrieroAttacco(int livello) =>
            $"Descrizione|Ricerca Guerriero Attacco|[black]Aumenta l'attacco dei guerrieri.\nCosto ricerca attacco guerriero lv {livello}\n\n";
        public string Desc_RicercaGuerrieroDifesa(int livello) =>
            $"Descrizione|Ricerca Guerriero Difesa|[black]Aumenta la difesa dei guerrieri.\nCosto ricerca difesa guerriero lv {livello}\n\n";
        public string Desc_RicercaLancereLivello(int livello) =>
            $"Descrizione|Ricerca Lancere Livello|[black]Aumenta il livello dei Lancieri.\nCosto ricerca livello Lanciere lv {livello}\n\n";
        public string Desc_RicercaLancereSalute(int livello) =>
            $"Descrizione|Ricerca Lancere Salute|[black]Aumenta la salute dei Lancieri.\nCosto ricerca salute Lanciere lv {livello}\n\n";
        public string Desc_RicercaLancereAttacco(int livello) =>
            $"Descrizione|Ricerca Lancere Attacco|[black]Aumenta l'attacco dei Lancieri.\nCosto ricerca attacco Lanciere lv {livello}\n\n";
        public string Desc_RicercaLancereDifesa(int livello) =>
            $"Descrizione|Ricerca Lancere Difesa|[black]Aumenta la difesa dei Lancieri.\nCosto ricerca difesa Lanciere lv {livello}\n\n";
        public string Desc_RicercaArcereLivelloe(int livello) =>
            $"Descrizione|Ricerca Arcere Livello|[black]Aumenta il livello degli Arcieri.\nCosto ricerca livello Arciere lv {livello}\n\n";
        public string Desc_RicercaArcereSalute(int livello) =>
            $"Descrizione|Ricerca Arcere Salute|[black]Aumenta la salute degli Arcieri.\nCosto ricerca salute Arciere lv {livello}\n\n";
        public string Desc_RicercaArcereAttacco(int livello) =>
            $"Descrizione|Ricerca Arcere Attacco|[black]Aumenta l'attacco degli Arcieri.\nCosto ricerca attacco Arciere lv {livello}\n\n";
        public string Desc_RicercaArcereDifesa(int livello) =>
            $"Descrizione|Ricerca Arcere Difesa|[black]Aumenta la difesa degli Arcieri.\nCosto ricerca difesa Arciere lv {livello}\n\n";
        public string Desc_RicercaCatapultaLivello(int livello) =>
            $"Descrizione|Ricerca Catapulta Livello|[black]Aumenta il livello delle Catapulte.\nCosto ricerca livello catapulte lv {livello}\n\n";
        public string Desc_RicercaCatapultaSalute(int livello) =>
            $"Descrizione|Ricerca Catapulta Salute|[black]Aumenta la salute delle Catapulte.\nCosto ricerca salute catapulta lv {livello}\n\n";
        public string Desc_RicercaCatapultaAttacco(int livello) =>
            $"Descrizione|Ricerca Catapulta Attacco|[black]Aumenta l'attacco delle Catapulte.\nCosto ricerca attacco catapulta lv {livello}\n\n";
        public string Desc_RicercaCatapultaDifesa(int livello) =>
            $"Descrizione|Ricerca Catapulta Difesa|[black]Aumenta la difesa delle Catapulte.\nCosto ricerca difesa catapulta lv {livello}\n\n";
        public string Desc_RicercaIngressoGuarnigione(int livello) =>
            $"Descrizione|Ricerca Ingresso Guarnigione|[black]Aumenta il numero massimo di unità che può ospitare la struttura.\nCosto ricerca ingresso guarnigione lv {livello}\n\n";
        public string Desc_RicercaCittaGuarnigione(int livello) =>
            $"Descrizione|Ricerca Citta Guarnigione|[black]Aumenta il numero massimo di unità che può ospitare la struttura.\nCosto ricerca citta guarnigione lv {livello}\n\n";
        public string Desc_RicercaMuraLivello(int livello) =>
            $"Descrizione|Ricerca Mura Livello|[black]Aumenta il livello della struttura.\nCosto ricerca livello mura: {livello}\n\n";
        public string Desc_RicercaMuraGuarnigione(int livello) =>
            $"Descrizione|Ricerca Mura Guarnigione|[black]Aumenta il numero massimo di unità che può ospitare la struttura.\nCosto ricerca guarnigione mura lv: {livello}\n\n";
        public string Desc_RicercaMuraSalute(int livello) =>
            $"Descrizione|Ricerca Mura Salute|[black]Aumenta il numero massimo di punti salute della struttura.\nCosto ricerca salute mura lv: {livello}\n\n";
        public string Desc_RicercaMuraDifesa(int livello) =>
            $"Descrizione|Ricerca Mura Difesa|[black]Aumenta il numero massimo di punti difesa della struttura.\nCosto ricerca difesa mura lv: {livello}\n\n";
        public string Desc_RicercaCancelloLivello(int livello) =>
            $"Descrizione|Ricerca Cancello Livello|[black]Aumenta il numero massimo di unità che può ospitare la struttura.\nCosto ricerca livello cancello: {livello}\n\n";
        public string Desc_RicercaCancelloGuarnigione(int livello) =>
            $"Descrizione|Ricerca Cancello Guarnigione|[black]Aumenta il numero massimo di unità che può ospitare la struttura.\nCosto ricerca guarnigione cancello lv: {livello}\n\n";
        public string Desc_RicercaCancelloSalute(int livello) =>
            $"Descrizione|Ricerca Cancello Salute|[black]Aumenta il numero massimo di punti salute della struttura.\nCosto ricerca salute cancello lv: {livello}\n\n";
        public string Desc_RicercaCancelloDifesa(int livello) =>
            $"Descrizione|Ricerca Cancello Difesa|[black]Aumenta il numero massimo di punti difesa della struttura.\nCosto ricerca difesa cancello lv: {livello}\n\n";
        public string Desc_RicercaTorriLivello(int livello) =>
            $"Descrizione|Ricerca Torri Livello|[black]Aumenta il numero massimo di unità che può ospitare la struttura.\nCosto ricerca livello torri: {livello}\n\n";
        public string Desc_RicercaTorriGuarnigione(int livello) =>
            $"Descrizione|Ricerca Torri Guarnigione|[black]Aumenta il numero massimo di unità che può ospitare la struttura.\nCosto ricerca guarnigione torri lv: {livello}\n\n";
        public string Desc_RicercaTorriSalute(int livello) =>
            $"Descrizione|Ricerca Torri Salute|[black]Aumenta il numero massimo di punti salute della struttura.\nCosto ricerca salute torri lv: {livello}\n\n";
        public string Desc_RicercaTorriDifesa(int livello) =>
            $"Descrizione|Ricerca Torri Difesa|[black]Aumenta il numero massimo di punti difesa della struttura.\nCosto ricerca difesa torri lv: {livello}\n\n";
        public string Desc_RicercaCastelloLivello(int livello) =>
            $"Descrizione|Ricerca Castello Livello|[black]Aumenta il numero massimo di unità che può ospitare la struttura.\nCosto ricerca livello castello: {livello}\n\n";
        public string Desc_RicercaCastelloGuarnigione(int livello) =>
            $"Descrizione|Ricerca Castello Guarnigione|[black]Aumenta il numero massimo di unità che può ospitare la struttura.\nCosto ricerca guarnigione castello lv: {livello}\n\n";
        public string Desc_RicercaCastelloSalute(int livello) =>
            $"Descrizione|Ricerca Castello Salute|[black]Aumenta il numero massimo di punti salute della struttura.\nCosto ricerca salute castello lv: {livello}\n\n";
        public string Desc_RicercaCastelloDifesa(int livello) =>
            $"Descrizione|Ricerca Castello Difesa|[black]Aumenta il numero massimo di punti difesa della struttura.\nCosto ricerca difesa castello lv: {livello}\n\n";

        public string Desc_Città_Testo() =>
            $"Descrizione|Feudi Testo|[black]Questa è panoramica del tuo villaggio, fortifica ogni settore, dall'Ingresso alle imponenti Mura del Castello.\n" +
            $"Ogni struttura, possiede caratteristiche specifiche come salute, difesa e guarnigione, che dovrai monitorare costantemente.\n" +
            $"Assicurati che ogni edificio sia in ottima efficienza per proteggere al meglio i tuoi domini dagli attacchi nemici.";
        public string Desc_Ricerca_Testo() =>
            $"Descrizione|Feudi Testo|[black]La Ricerca rappresenta il progresso delle conoscenze del tuo regno. Investendo tempo e risorse potrai sbloccare nuove possibilità, migliorare strutture, eserciti e strategie. " +
            $"Un regno che non ricerca è destinato a restare indietro.";
        public string Desc_Feudi_Testo() =>
            $"Descrizione|Feudi Testo|[black]Acquista il tuo feudo e diventa proprietario di una porzione di terra. Ogni terreno genera una rendita giornaliera automatica. L’ammontare della rendita dipende dalla rarità.";
        public string Label_Comune() =>
            $"Descrizione|Feudi Testo|Comune";
        public string Label_NonComune() =>
            $"Descrizione|Feudi Testo|Non Comune";
        public string Label_Raro() =>
            $"Descrizione|Feudi Testo|Raro";
        public string Label_Epico() =>
            $"Descrizione|Feudi Testo|Epico";
        public string Label_Leggendario() =>
            $"Descrizione|Feudi Testo|Leggendario";

        public static string ToRoman(int n) => n switch
        {
            1 => "I",
            2 => "II",
            3 => "III",
            4 => "IV",
            5 => "V",
            _ => n.ToString()
        };
    }
}
