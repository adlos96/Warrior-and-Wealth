using Server_Strategico.Gioco;
using static Server_Strategico.Gioco.Esercito;

namespace Server_Strategico.ServerData.Localization
{
    internal class ENG : ILocalization
    {
        public string GetQuestDescription(int questId) => questId switch
        {
            0 => "Buy a fief",
            1 => "Train warriors",
            2 => "Train spearmen",
            3 => "Train archers",
            4 => "Train catapults",
            5 => "Eliminate warriors",
            6 => "Eliminate spearmen",
            7 => "Eliminate archers",
            8 => "Eliminate catapults",
            9 => "Eliminate any unit",
            10 => "Build any civilian structure",
            11 => "Build farms",
            12 => "Build sawmills",
            13 => "Build stone quarries",
            14 => "Build iron mines",
            15 => "Build gold mines",
            16 => "Build houses",
            17 => "Build sword workshops",
            18 => "Build spear workshops",
            19 => "Build bow workshops",
            20 => "Build shield workshops",
            21 => "Build armor workshops",
            22 => "Build arrow workshops",
            23 => "Build warrior barracks",
            24 => "Build spearmen barracks",
            25 => "Build archer barracks",
            26 => "Build catapult barracks",
            27 => "Use blue diamonds",
            28 => "Use purple diamonds",
            29 => "Speed up constructions",
            30 => "Speed up training",
            31 => "Speed up anything",
            32 => "Attack players",
            33 => "Attack barbarian villages",
            34 => "Attack barbarian cities",
            35 => "Build any military structure",
            36 => "Train any military unit",
            37 => "Use food",
            38 => "Use wood",
            39 => "Use stone",
            40 => "Use iron",
            41 => "Use gold",
            42 => "Use population",
            43 => "Use swords",
            44 => "Use spears",
            45 => "Use bows",
            46 => "Use shields",
            47 => "Use armor",
            48 => "Use arrows",
            49 => "Complete all initial quests",
            50 => "Explore barbarian village",
            51 => "Explore barbarian cities",
            52 => "Explore players",
            53 => "Perform any exploration",
            54 => "Reach level 5",
            55 => "Reach level 10",
            56 => "Reach level 25",
            57 => "Reach level 50",
            58 => "Reach level 75",
            59 => "Reach level 100",
            _ => $"Unknown quest"
        };
        public string GetTutorialObiettivo(int statoTutorial) => statoTutorial switch
        {
            1 => "Left-click on the text",
            2 => "Left-click on the text",
            3 => "Left-click on the text",
            4 => "Left-click on the text",
            5 => "Left-click on the text.",
            6 => "Left-click on the text.",
            7 => "Left-click on the text.",
            8 => "Purchase a fief.",
            9 => "Left-click on the text.",
            10 => "Switch building view from civil to military [icon:scambio].",
            11 => "Press the Build button.",
            12 => "Build a farm",
            13 => "Exchange [icon:diamanteViola][viola]purple diamonds[/viola] for [icon:diamanteBlu][blu]blue diamonds[/blu].",
            14 => "Speed up construction. [warning]Complete the farm[/warning]",
            15 => "Build a sawmill.",
            16 => "Build a stone quarry.",
            17 => "Build an iron mine.",
            18 => "Build a gold mine.",
            19 => "Build a house.",
            20 => "Left-click on the text.",
            21 => "Click on the Warrior icon.",
            22 => "Click on the Catapult Barracks icon.",
            23 => "Close the 'Build' screen.",
            24 => "Open the 'City' screen.",
            25 => "Repair the damaged structure.",
            26 => "Press the 'Garrison' button in the 'Castle'.",
            27 => "Open the '[warning]Statistics[/warning]' screen.",
            28 => "Open the '[warning]Shop[/warning]' screen.",
            29 => "Open the 'Research' screen.",
            30 => "Open the 'Monthly Quests' screen.",
            31 => "Open the 'PVP/PVE' battles screen.",
            32 => "Left-click on the text.",
            _ => ""
        };
        public string GetTutorialDescrizione(int statoTutorial) => statoTutorial switch
        {
            1 =>
                "Don't worry: in this tutorial, you will learn how to navigate this new world and all the main screens." +
                "As you can see, there isn't much to look at right now... but soon everything will become clearer, perhaps..." +
                "What you see now are two screens. The first one, the larger one, is the main screen: it will allow you to manage practically everything." +
                "The second one, where you are reading this text, is the tutorial screen." +
                "It will remain active until completion. Let's focus on this one for a moment." +
                "Here you can see the tutorial progression, the objectives you need to reach to move forward... and also a brief description." +
                "In this case, to complete the objective, simply left-click on this text. When you are ready, complete the objective and let's move on.",

            2 =>
                "Welcome to \"[warning]Warrior & Wealth[/warning]\"! " +
                "I see you are a new player... excellent. This tutorial will be perfect for you! Before we begin, let me say one thing... " +
                "drink responsibly! Don't drive while drinking, or you might spill your beer! Perhaps it's better to move on to serious matters... " +
                "I can finally guide you, step by step, through this new world. Great adventures await you. " +
                "\"[warning]Warrior & Wealth[/warning]\" is divided into two modes. The first part will allow you to purchase Fiefs, which will earn you [icon:dollariVirtuali]Fief Tributes." +
                "Thanks to the immensity of the kingdom and the benevolence of the Emperor, every second new riches will flow into your village's coffers... " +
                "But that is an aspect we will see at the right time. The second part... as you might have guessed... concerns you. And if not, let me explain better." +
                "You have been chosen. Yes, you heard right. You have been chosen to manage this small village. I urge you... " +
                "harsh weather looms over these lands, and dangers are everywhere. Be very careful. When you are ready, we can move forward with the tutorial and show you what you can do." +
                "Oh, right... I haven't actually explained what you need to do yet. You will have to manage the village assigned to you by the Emperor." +
                "Resources will be your main focus: without them, all is lost. You will need them to build structures, military buildings, for research... to repair defenses, and much more." +
                "There are many aspects to learn... but let's not fill this text with too many words. Let's begin! ...When you are ready, complete the objective and let's move on.",

            3 =>
                "At the top you can see something, this is the resource bar, what we mentioned before; these are indispensable and in order we find, '[icon:cibo]Food, [icon:legno]Wood, [icon:pietra]Stone, [icon:ferro]Iron, " +
                "[icon:oro]Gold, [icon:popolazione]Population, [icon:xp]Experience and [icon:lv]Level', these resources are used for everything, I can only let you imagine what they might be for." +
                "By hovering your mouse pointer over the individual icons and leaving it there for a brief moment, you will notice a small tooltip appearing; it will show you what you are looking at, a brief description and all the information " +
                "you might need during the management of your village. You can do this with almost all the icons you encounter on this screen and others, remember that." +
                "Let's move on to introducing military resources; by pressing the [icon:scambio] icon, you can see in the following order '[icon:spade]Swords, [icon:lance]Spears, [icon:archi]Bows, [icon:scudi]Shields, [icon:armature]Armor and [icon:frecce]Arrows', the depiction of the individual icons should already be self-explanatory, but I'll let you imagine " +
                "what they could be used for. The only one I want to focus on more is [icon:frecce]arrows; since they are a projectile component, they will be used by your archers and catapults, so in moments " +
                "of high tension, I recommend paying attention to their quantity in your warehouses to avoid running out when needed." +
                "All resources can be produced directly in your village or obtained through other means..." +
                "When you are ready, complete the objective and let's move on.",

            4 =>
                "Still at the top, below the resource bar, you can see that a second bar further down has just appeared; it will allow you to view, for now, " +
                "a single icon: '[icon:diamanteViola][viola]Purple Diamonds[/viola]'. Let's pause for a moment. " +
                "The [icon:diamanteViola][viola]Purple Diamonds[/viola] are fundamental for the creation of 'fiefs'—we will see these later, don't worry—they can be spent in the 'Shop', " +
                "to obtain advantages that will accelerate your progression in this kingdom; the shop will naturally be visible soon. They can also be exchanged for Blue Diamonds, but we'll see that later. " +
                "This resource is first in terms of scarcity, so weigh your decision carefully before spending them; they are not easy to obtain, but just as easy to spend... " +
                "How can you obtain [icon:diamanteViola][viola]Purple Diamonds[/viola], you ask? " +
                "Well, you can get them in numerous ways: through monthly quests, in the shop, or won in battles against barbarian 'Villages' and 'Cities', " +
                "or even by attacking and plundering other players' resources, but you'll have to wait for that, it's too early... " +
                "First, let's focus on the rest. " +
                "When you are ready, complete the objective and let's move on..",

            5 =>
                "Look... next to the [icon:diamanteViola][viola]Purple Diamonds[/viola], a new icon has appeared. " +
                "It depicts [icon:diamanteBlu][blu]blue diamonds[/blu]. Right... I almost forgot about them. Let's talk about them. " +
                "The [icon:diamanteBlu][blu]blue diamonds[/blu] are very similar to their [icon:diamanteViola][viola]purple diamond[/viola] counterparts, but with one small difference: they are the second rarest resource in the kingdom. " +
                "These diamonds, however, tend to bring value and advantages to your city. They are mainly used for acceleration: Construction, troop training, and research. " +
                "All fundamental things to make your village grow... and make it stronger. " +
                "Of course, they are not indispensable for progression... but they can be a great help, especially during the most important moments. Furthermore, " +
                "you can spend them in the Shop to purchase powerful spells. One of the most useful is the Peace Shield. " +
                "Thanks to the illustrious wizards of the kingdom, this spell will create a massive blue shield around the borders of your domains... " +
                "and will protect you from attacks coming from other cities, scattered far and wide across the kingdom." +
                "When you are ready, complete the objective... and let's move on.",

            6 =>
                "It seems another resource has just appeared. Now you can view the Fief Tributes icon. " +
                "The amount of tributes you receive will depend on the number of Fiefs you manage... and their rarity level. " +
                "Each fief generates tributes every second, and the amount will be automatically credited. " +
                "You can use these tributes in the Shop. Unfortunately, you can't see it yet... but we'll discover it later. " +
                "Alternatively, you can also withdraw them once you reach the minimum required threshold. " +
                "The current exchange rate is 1 Tribute = 1 USDT." +
                "When you have reached the threshold, you will need to enter the USDT address where you wish to receive the withdrawal." +
                "When you are ready, complete the objective... and let's move on.",

            7 =>
                "Here we are at last! As you can see, a new screen has appeared on the left. " +
                "Here you can view the fiefs you own... and also purchase new ones. " +
                "Each fief has unique characteristics, such as tributes generated and their rarity. " +
                "To find out more information about a fief, left-click on the blue info icon at the top right, next to the fiefs. " +
                "When you are ready, complete the objective... and let's move on..",

            8 =>
                "Perfect... let's start by trying our luck! As you can see, at the bottom, below the fiefs box, there is a beautiful button: ”Buy”. " +
                "The Emperor will help you during this tutorial, so you have already been provided with [icon:diamanteViola][warning]150 [viola]diamonds[/viola]. " +
                "Press the ”Buy” button to start your adventure on \"[warning]Warrior & Wealth[/warning]\"… and thus obtain your first fief. " +
                "And finally, after all this immense effort and dedication... you can enjoy the earnings coming directly from the fief you just annexed. " +
                "You will notice that the number of your fiefs has increased. And from this moment on, you can see your Tributes grow over time. " +
                "As I mentioned before, tributes can also be spent in the Shop to obtain important advantages... and compete against other villages in the kingdom. " +
                "When you are ready, complete the objective... and let's move on.",

            9 =>
                "Great! You just got your first fief!... now let's start building something! Besides fiefs, there are many other structures we need to examine together. " +
                "Shortly, I will show you how to build them, using your population and the resources at your disposal... Well... not exactly right now. " +
                "For the moment, you are still missing some resources... but don't worry: soon everything will be possible. " +
                "And, just this once, we will provide you with what you need. That way you can start immediately and get to know the available structures. " +
                "When you are ready, complete the objective... and let's move on.",

            10 =>
                "Perfect, here we go... let's talk about structures. As you can see, a new fragment of the interface has appeared in the center of the game screen: ”Civil Structures”. " +
                "Along with this, a new interface element has also appeared. These are the main structures used for the production of civil and military resources. " +
                "For now, however, we will focus on civil resources. They are essential for building, researching, and maintaining your village. " +
                "And, with a good strategy, they will allow you to grow until you reach power worthy of the Emperor. " +
                "So think carefully about your choices... and plan your next actions to best settle into this new world. " +
                "As you can notice, in the structures panel, there is an [icon:scambio] icon that allows you to switch to military buildings, dedicated to the production of military resources. " +
                "But we'll talk about that later. " +
                "When you are ready, complete the objective... and let's move on.",

            11 =>
                "Finally, we are here. As you can see, a new button has appeared at the bottom of the game screen: ”Build”. " +
                "For this feat, you will be provided with all the necessary resources... but be careful: it will be the only time. If you make mistakes, your actions will be irreversible. I mean it. " +
                "Now look at the main screen. Next to the ”Farm” structure, besides its icon, you can notice two indicators. " +
                "The first shows the number of Available Buildings: these are the structures already built and active. The second indicates the number of Buildings in Queue: which are the structures waiting to be built. " +
                "You can see the remaining time for construction at the bottom, in the ”Civil Structures” screen. This indicator will appear as soon as you start your first construction. " +
                "By pressing the ”Build” button, a new screen will open, from which you can manage all these aspects. " +
                "When you are ready, complete the objective... and let's move on.",

            12 =>
                "Keep the \"[warning]Build[/warning]\" screen open: the next steps will guide you in creating all the necessary structures. " +
                "For convenience, move the newly opened window to a position on the screen that allows you to keep everything under control and see both screens. " +
                "Next to each structure, you can see the quantity you are about to build. At the moment, only the Farm is available, and the initial value is set to zero. " +
                "Near this number, you find the controls that allow you to modify it. With a simple left-click, you can increase the quantity by [warning]1[/warning] unit; " +
                "using the Ctrl + click combination, the increase will be [warning]5[/warning], while with Shift + click you can increase the value by [warning]10[/warning]. " +
                "Adjust the quantity as you wish... Build a [warning]Farm[/warning]. " +
                "When you are ready, complete the objective... and let's move on.",

            13 =>
                "You have just started the construction of your first Farm." +
                "Remember that you can assign the construction of multiple structures simultaneously, but a new construction can only begin when a builder is free again. " +
                "You can see the number of available builders at the top of the ”Civil Structures” screen, above the first structure, the Farm. " +
                "Here, both the maximum number of available builders... and how many of them are currently free are indicated. " +
                "Since you have already started a construction, you will now notice the remaining time, which counts down as the work proceeds. Before the Farm is completed, a little patience will be needed. " +
                "At any time, however, you can speed up construction using [icon:diamanteBlu][blu]blue diamonds[/blu]. And in fact... let's use some. But first, we need to get them. Let's see how. " +
                "Go back to the main screen and look in the Fiefs section. Below the ”Buy Fief” button, a new button has just appeared: ”Exchange”. " +
                "Press it, and you can convert some [icon:diamanteViola][viola]purple diamonds[/viola] into [icon:diamanteBlu][blu]blue diamonds[/blu]. " +
                "For this construction, [icon:diamanteBlu][warning]50 [blu]blue diamonds[/blu] should be more than enough." +
                "When you are ready, complete the objective... and let's move on.",

            14 =>
                "Great, you managed to get your first [icon:diamanteBlu][blu]blue diamonds[/blu] through the exchange." +
                "This way, if you run out and need them, you can get them at any time... provided you have [icon:diamanteViola][viola]purple diamonds[/viola] available. " +
                "In this tutorial, I will only show you how to speed up the construction of the Farm, but later you can do the same with other structures, with military units in training... " +
                "and even with research. Now left-click on the icon next to the remaining construction time. " +
                "It should look like a speed booster. Press it and increase the number of [icon:diamanteBlu][blu]blue diamonds[/blu] to be used up to [warning]50[/warning]. " +
                "The same rules as before apply here: you can click normally, or use key combinations to reach the desired value more quickly. " +
                "And don't worry: no need to ask yourself if you're using too many diamonds. " +
                "Only those actually necessary will be consumed... all others will be returned to you. " +
                "When you are ready, complete the objective... and let's move on.",

            15 =>
                "Great job, the farm is built and your settlement is starting to take shape.\n" +
                "Now we need to make sure we have the right materials to grow; [warning]wood[/warning] is among them.\n" +
                "[warning]Build a Sawmill[/warning] and make your economy stable.\n\n" +
                "When you are ready, complete the objective and let's move on.",

            16 =>
                "Excellent work! I see you built your first sawmill, and this will allow you to obtain wood steadily.\n" +
                "Now it's time to secure another fundamental resource for the growth of your settlement: [warning]Build a Stone Quarry[/warning] to start extracting this resource, " +
                "indispensable for new structures, repairs, and research.\n\n" +
                "When you are ready, complete the objective and let's move on.",

            17 =>
                "Perfect! I see you built your first stone quarry, great job.\n" +
                "Now your settlement is starting to have solid foundations, but to truly grow you need an even more important resource: [warning]iron[/warning].\n" +
                "Build an [warning]Iron Mine[/warning] to start extraction and obtain essential materials for the army and future development.\n\n" +
                "When you are ready, complete the objective and let's move on.",

            18 =>
                "Perfect! I see you built your first iron mine, great job.\n" +
                "Now you've started extracting essential resources for development, but something fundamental is still missing: a stable source of wealth.\n" +
                "[warning]Gold[/warning] is also fundamental for maintaining structures and military units; before a major expansion, always check your production, " +
                "your consumption should never exceed your production capacity.\n" +
                "Build a [warning]Gold Mine[/warning] to start extracting a precious resource, useful for sustaining your settlement's economy.\n\n" +
                "When you are ready, complete the objective and let's move on.",

            19 =>
                "Your gold mine is ready and the village is coming to life.\n" +
                "To make it truly grow, you need inhabitants who can live and work there.\n" +
                "Build a [warning]House[/warning] to welcome new citizens and give your settlement its first inhabitants.\n\n" +
                "When you are ready, complete the objective and let's move on.",

            20 =>
                "Now it's time to introduce the Military Structures." +
                "The same principle as civil construction applies to them as well, but these will produce resources essential for your units, such as [warning]Warriors[/warning], [warning]Spearmen[/warning], [warning]Archers[/warning], and [warning]Catapults[/warning]." +
                "The resources produced are mainly used to train the army. Depending on the unit type and the selected tier, the required quantities will increase along with the quality of the troops." +
                "We will delve into these aspects later when we talk directly about the units... and their training." +
                "In this tutorial we will not build military structures, but it's important to know them right away." +
                "Their cost is significant, and they all require constant resource consumption to produce equipment." +
                "You will decide when to start production at the end of this tutorial... when you feel truly ready. " +
                "When you are ready, complete the objective... and let's move on.",

            21 =>
                "Let's move on to Military Units. " +
                "Up until now they weren't visible, but now you can find them both in the ”Build” screen and directly on the game screen, next to the civil structures." +
                "In both cases, the interface is very similar. Above the military units, you can notice the tiers, ranging from one to six." +
                "For now, you will only be able to train level one units, but in the future, as you level up, you will unlock more advanced ones as well." +
                "Units are divided into Warriors, Spearmen, Archers, and Catapults." +
                "All require constant food and gold maintenance, and need resources and military equipment to be trained." +
                "At the moment, however, even if you had the necessary resources, you couldn't train them yet. You don't have access to Barracks, which will be unlocked later in the game." +
                "When you are ready, complete the objective... and let's move on.",

            22 =>
                "Let's move on to the Barracks." +
                "As anticipated, these structures are fundamental for founding your army: without them, you cannot form any military force." +
                "Their construction allows housing a certain number of men, which varies based on the structure type." +
                "There are barracks dedicated to Warriors, Spearmen, Archers, and Catapults, and each is designed to support a specific unit type." +
                "However, using them entails a cost: they require constant maintenance in food and gold to function correctly." +
                "Before building them, make sure your production is able to sustain these expenses over time." +
                "Careful resource management will be fundamental to keep your army efficient." +
                "When you are ready, complete the objective... and let's move on.",

            23 =>
                "Perfect, here we are. We have finally finished with constructions, so let me briefly introduce training, just to stay on topic with this screen." +
                "As you can see, the section dedicated to training has appeared on the right. Here you can observe the different units available." +
                "As mentioned previously, the selected tier is visible at the top: currently, only tier 1 is available, while the subsequent ones will be unlocked later." +
                "We will talk about this in detail when we address the units directly. That's all for now. Close the Build screen." +
                "When you are ready, complete the objective and let's move on.",

            24 =>
                "Great, all that's left is to introduce the village map. As you can notice, a new button has just appeared at the bottom: ”City”." +
                "Press it and let me show you how the villages of this kingdom are composed. Let's start with a fundamental structure: the Walls." +
                "As you can see, almost all structures share the same setup." +
                "At the top is the structure name; immediately to its right, you find a number in square brackets, indicating the order of the structures." +
                "Next to it, the structure level is visible. Immediately below are three bars: [warning]Health[/warning], [warning]Defense[/warning], and [warning]Garrison[/warning]. These are your defensive structures. There are six in total." +
                "Three of them, in addition to the Walls — Gate, Towers, and Castle — share the same statistics." +
                "The garrison represents the maximum number of units the structure can host and how many are actually assigned inside it. The remaining two structures, Entrance and Center, are similar but only have space for the garrison, without health and defense bars." +
                "It is important to keep the structures in good condition. In case of an attack on the village, a well-organized defense can make the difference and reduce losses." +
                "But be careful: if a structure loses all its health and reaches zero, it will collapse. In that case, all units inside will lose their lives during the collapse." +
                "Assigning units is therefore crucial. " +
                "Let's take an example: if there are units in the garrison at the Entrance, despite it being a structure without health and defense, their positioning, along with units assigned to the Walls and Gate, " +
                "can provide a defensive bonus to the units at the entrance, based on the number deployed. In general, it is always advisable to keep the garrisons on this screen well-supplied. " +
                "Often it's better to have more men in defense rather than risking a ruinous defeat. If the attacking units manage to bypass the Castle, the seventh and final attack will hit you directly. " +
                "In case of defeat, the attacker will be able to plunder your resources, including purple diamonds and blue diamonds." +
                "When you are ready, complete the objective and let's move on.",

            25 =>
                "Oh no, look... a saboteur has just damaged one of your structures." +
                "See? The health and defense of the Walls have decreased. This is not good, but it's the perfect opportunity to introduce repairs." +
                "When a structure is damaged following a siege, it needs to be repaired. To do so, you can notice that a ”Repair” icon has appeared next to its stats." +
                "This icon is only visible when the structure has taken damage. Press it and you'll see how your men hurry to fix it." +
                "Pay attention: repairs take time and resources. If resources run out, the repair process will stop automatically, so make sure you have enough." +
                "As already seen for the other icons, you can hover your mouse pointer over the repair icon to view the tooltip." +
                "Here you can check the resources needed to restore each health point and the time required to complete the work." +
                "When you are ready, complete the objective and let's move on.",

            26 =>
                "Great, we've reached the end of this screen." +
                "To assign men to a structure, simply press the ”Garrison” button under each one." +
                "From here you can select the units and their level, assigning them to the chosen structure." +
                "This step can be repeated for all available structures as soon as you have an army at your service. Also remember that Health, Defense, and garrison capacity can be upgraded through research." +
                "Your character's level will play a fundamental role in the development of the city and its ability to resist attacks." +
                "When you are ready, complete the objective and let's move on.",

            27 =>
                "You can close the ”City” screen, we won't need it anymore." +
                "Note, however, that on the game screen, next to the purple diamonds, a new icon has appeared: your character, with their name." +
                "By left-clicking on your icon, a new screen dedicated to ”Statistics” will open. This screen isn't fundamental, but it's worth pausing for a moment." +
                "You will find two columns: the right one shows your personal stats, where you can observe interesting information that might come in handy in the future." +
                "On the left, however, the current status of your village is indicated, with remaining times before the reset of various activities, overall power, and applied bonuses, both temporary and permanent." +
                "When you are ready, complete the objective and let's move on.",

            28 =>
                "You can close the ”Statistics” screen, we won't need it anymore." +
                "Note, however, that a new button is now available: the \"[warning]Shop[/warning]\". It's the place where you can get purple diamonds and much more." +
                "Inside the shop, you can increase the maximum number of builders and trainers, purchase the Peace Shield for extra defense of your lands, activate VIP, " +
                "which grants exclusive advantages and access to extra quest rewards, and choose between two GamePass variants, Basic and Advanced." +
                "As with other interfaces, you can hover your mouse pointer over the icons to view more detailed information about each available item." +
                "Take a moment to freely explore the shop." +
                "When you are ready, complete the objective and let's move on.",

            29 =>
                "You can close the ”Shop” screen, we won't need it anymore. Note, however, that a new button has appeared on the main screen: ”[warning]Research[/warning]”." +
                "This place is fundamental if you want to try and reach even a tiny fraction of the Emperor's power." +
                "The available researches are numerous, and others may be added in the future." +
                "As you'll be used to by now, you can hover your mouse pointer over each research to get detailed information on the costs and time required for completion." +
                "Remember, however, that only one research can be performed at a time, so choose carefully what to develop first." +
                "You will be able to improve almost everything: from resource production, to implementing strategies to attract new inhabitants, to upgrading training for both the city and individual military units, and much more." +
                "I'll give you a few moments to familiarize yourself with the research screen, but you can explore it more deeply later as well." +
                "When you are ready, complete the objective and let's move on.",

            30 =>
                "You can close the ”Research” screen, we won't need it anymore." +
                "Now, on the main screen, a new button has appeared: \"[warning]Monthly Quests[/warning]\". There's actually not much to explain here." +
                "It's a screen where you can see the available quests: the requirement to complete them and the experience they will provide individually." +
                "Further down you'll find a button that allows you to scroll through the available quests. Some quests can be completed multiple times, progressively increasing their requirement." +
                "Finally, you can observe the progress bar, which shows how much experience you've accumulated. The associated rewards can be claimed once the indicated thresholds are reached: " +
                "those above the bar are available to everyone, while those below can only be collected if you own the [warning]Silver Gamepass[/warning]." +
                "When you are ready, complete the objective and let's move on.",

            31 =>
                "You can close the ”Monthly Quests” screen, we won't need it anymore. Now, on the main screen, a new button has appeared: \"[warning]Battles[/warning]\". " +
                "Here you can find Barbarian Villages, Barbarian Cities, and also other players' villages." +
                "At the moment it might seem less relevant, but battle is the only way to gain experience and level up." +
                "It won't be your main focus, because there are many things to do, but don't neglect it: doing so will allow you to progress in the game without falling behind." +
                "Barbarian Villages will be available from the start. Don't underestimate them: they can hold unpleasant surprises, and remember that in every clash, you will be the attacker." +
                "Barbarian Cities are more difficult: they contain more men and are better organized. Be careful when you face them." +
                "When you are ready, complete the objective and let's move on.",

            32 =>
                "You have reached the end of this wonderful journey." +
                "At this point...",
            _ => ""
        };

        // UI Labels
        public string Label_CostoCostruzione() => "Construction Cost";
        public string Label_CostoAddestramento() => "Training Cost";
        public string Label_Statistiche() => "Statistics";
        public string Label_Cibo() => "Food";
        public string Label_Legno() => "Wood";
        public string Label_Pietra() => "Stone";
        public string Label_Ferro() => "Iron";
        public string Label_Oro() => "Gold";
        public string Label_Popolazione() => "Population";
        public string Label_Spade() => "Swords";
        public string Label_Lancie() => "Spears";
        public string Label_Archi() => "Bows";
        public string Label_Scudi() => "Shields";
        public string Label_Armature() => "Armor";
        public string Label_Frecce() => "Arrows";
        public string Label_Costruzione() => "Construction";
        public string Label_Addestramento() => "Training";
        public string Label_Ricerca() => "Research";
        public string Label_Produzione() => "Resource Production";
        public string Label_Mantenimento_Cibo() => "Food Upkeep";
        public string Label_Mantenimento_Legno() => "Wood Upkeep";
        public string Label_Mantenimento_Pietra() => "Stone Upkeep";
        public string Label_Mantenimento_Ferro() => "Iron Upkeep";
        public string Label_Mantenimento_Oro() => "Gold Upkeep";
        public string Label_Livello() => "Level";
        public string Label_Salute() => "Health";
        public string Label_Difesa() => "Defense";
        public string Label_Attacco() => "Attack";
        public string Label_Limite_Magazzino() => "Storage Limit";
        public string Label_Limite_Unità() => "Limit";
        public string Label_Limite_Strutture() => "Limit";

        //Virtual Lands - Fiefs
        public string Terreni_DiamantiInsufficienti() =>
            $"Log_Server|[title]You don't have enough[/title] [viola]Purple Diamonds[/viola][icon:diamanteViola] [title]for a virtual land.[/title]";
        public string Terreni_DiamantiUtilizzati() =>
            $"Log_Server|[warning][icon:diamanteViola]{Strutture.Edifici.Terreni_Virtuali.Diamanti_Viola}[viola] Purple Diamonds[/viola] [title]used for a virtual land...[/title]";
        public string Terreni_Ottenuto(string terreno) =>
            $"Log_Server|[warning]Fief obtained:[/warning] [{terreno.Replace(" ", "")}]{terreno}[/{terreno.Replace(" ", "")}][icon:{terreno.Replace(" ", "")}]";

        public string Costruzione_RisorseUtilizzate(int count, string buildingType, Strutture.Edifici cost) =>
            $"Log_Server|[info]Resources used[/info] to build [warning]{count} {buildingType}[/warning]:\r\n " +
            $"[cibo][icon:cibo]-{cost.Cibo * count:N0}[/cibo]  " +
            $"[legno][icon:legno]-{cost.Legno * count:N0}[/legno]  " +
            $"[pietra][icon:pietra]-{cost.Pietra * count:N0}[/pietra]  " +
            $"[ferro][icon:ferro]-{cost.Ferro * count:N0}[/ferro] " +
            $"[oro][icon:oro]-{cost.Oro * count:N0}[/oro]  " +
            $"[popolazione][icon:popolazione]-{cost.Popolazione * count:N0}[/popolazione]";

        //Construction Descriptions
        public string Costruzione_RisorseInsufficienti(int count, string buildingType) =>
            $"Log_Server|[error]Insufficient resources to build [title]{count} {buildingType}.";
        public string Costruzione_Pausa(string struttura) =>
            $"Log_Server|[warning]Construction of [title]{struttura} [warning]paused due to slot reduction.";
        public string Costruzione_Avvio(string struttura, string tempo) =>
            $"Log_Server|[title]Construction of [info]{struttura} [title]started, duration: [icon:tempo]{tempo}";
        public string Costruzione_Completata(string struttura) =>
            $"Log_Server|[success]Construction completed: [title]{struttura}[icon:{struttura}]";
        public string Costruzione_EdificioNonValido(string struttura) =>
            $"Log_Server|[error]Building type [title]{struttura}[icon:{struttura}] [error]is invalid!";
        public string NumeroDiamantiNonValido() =>
            $"Log_Server|[error]Invalid number of [blu][icon:diamanteBlu]diamonds [error].";
        public string DiamantiInsufficienti() =>
            $"Log_Server|[error]You don't have enough [blu]Blue Diamonds![icon:diamanteBlu]";
        public string Costruzione_NessunaCostruzione() =>
            $"Log_Server|[warning]There are no constructions to speed up.";
        public string Costruzione_Velocizzazione(int diamanti, string tempo) =>
            $"Log_Server|[title]You used [blu][icon:diamanteBlu][warning]{diamanti} [blu]Blue Diamonds [title]to speed up construction! [icon:tempo]{tempo}";

        //Training Descriptions
        public string Addestramento_LimiteRaggiunto(string unità, string numero, string limite) =>
            $"Log_Server|[title]Troop limit reached for {unità}.[icon:{unità}] [warning]{numero}/{limite}";
        public string Addestramento_RisorseUtilizzate(int count, string unità, Esercito.CostoReclutamento unitCost) =>
           $"Log_Server|[info]Resources used[/info] to train [warning]{count} {unità}[/warning]:\r\n " +
                $"[cibo][icon:cibo]-{(unitCost.Cibo * count):N0}[/cibo] " +
                $"[legno][icon:legno]-{(unitCost.Legno * count):N0}[/legno] " +
                $"[pietra][icon:pietra]-{(unitCost.Pietra * count):N0}[/pietra] " +
                $"[ferro][icon:ferro]-{(unitCost.Ferro * count):N0}[/ferro] " +
                $"[oro][icon:oro]-{(unitCost.Oro * count):N0}[/oro] " +
                $"[popolazione][icon:popolazione]-{(unitCost.Popolazione * count):N0}[/popolazione]";

        public string Addestramento_RisorseInsufficienti(int count, string unità) =>
            $"Log_Server|[error]Insufficient resources to train [title]{count} {unità}.";
        public string Addestramento_Pausa(string unità) =>
            $"Log_Server|[title]Recruitment of [warning]{unità} [title]paused due to slot reduction.";
        public string Addestramento_Avvio(string unità, string tempo) =>
            $"Log_Server|[title]Recruitment of[/title] {unità} [title]started. Duration[/title] [icon:tempo]{tempo}";
        public string Addestramento_Completata(string unità) =>
            $"Log_Server|[warning]{unità}[/warning] trained!";
        public string Addestramento_UnitàNonValido(string unità) =>
            $"Log_Server|[error]Unit type [title]{unità}[icon:{unità}] [error]is invalid!";
        public string Addestramento_NessunaUnità() =>
            $"Log_Server|[warning]There are no units to speed up.";
        public string Addestramento_Velocizzazione(int diamanti, string tempo) =>
            $"Log_Server|[title]You used [blu][icon:diamanteBlu][warning]{diamanti} [blu]Blue Diamonds [title]to speed up training! [icon:tempo]{tempo}";

        //Research Descriptions
        public string Ricerca_LivelloRichiesto(string ricerca, int livelloRicerca, string msg, int richiesto) =>
            $"Log_Server|[error]Research [title]{ricerca} {livelloRicerca} [error]requires [title]{msg}[error] level to be at least lv [title]{richiesto}";
        public string Ricerca_Start(string ricerca, string tempo) =>
            $"Log_Server|[info]Researching [title]{ricerca} [title]started. Duration [icon:tempo]{tempo}";
        public string Ricerca_Completata(string ricerca) =>
            $"Log_Server|[success]Research completed: [title]{ricerca}";
        public string Ricerca_NessunaRicerca() =>
            $"Log_Server|[warning]No research to speed up.";
        public string Ricerca_Velocizzazione(int diamanti, string tempo) =>
            $"Log_Server|[title]You used [blu][icon:diamanteBlu][warning]{diamanti} [blu]Blue Diamonds [title]to speed up research! [icon:tempo]{tempo}";

        // ── Narrative Descriptions: Civil Structures ────────────────────────────
        public string Desc_Fattoria() =>
            "The Farm is the primary structure for [icon:cibo]Food production, essential for military building and civil structures, " +
            "training military units and their upkeep. Indispensable for technological research and military components.";

        public string Desc_Segheria() =>
            "The Sawmill is the primary structure for [icon:legno]Wood production, fundamental for building military and civil structures, " +
            "and training military units. Indispensable for technological research and military components.";

        public string Desc_CavaPietra() =>
            "The Stone Quarry is the primary structure for [icon:pietra]Stone production, fundamental for building military and civil structures, " +
            "and training military units. Indispensable for technological research and military components.";

        public string Desc_MinieraFerro() =>
            "The Iron Mine is the primary structure for [icon:ferro]Iron production, fundamental for building military and civil structures, " +
            "and training military units. Indispensable for technological research and military components.";

        public string Desc_MinieraOro() =>
            "The Gold Mine is the primary structure for [icon:oro]Gold production, fundamental for building military and civil structures, " +
            "and training military units. Indispensable for technological research and military components.";

        public string Desc_Case() =>
            "Houses are necessary to attract more and more [icon:popolazione]citizens to your village; " +
            "they are fundamental for constructing military and civil structures, as well as for training military units.";

        // ── Narrative Descriptions: Workshop ────────────────────────────────────
        public string Desc_ProduzioneSpade() =>
            "Sword Workshop: this structure produces specific military equipment " +
            "essential for training military units. This structure produces [icon:spade]Swords.";

        public string Desc_ProduzioneLance() =>
            "Spear Workshop: this structure produces specific military equipment " +
            "essential for training military units. This structure produces [icon:lance]Spears.";

        public string Desc_ProduzioneArchi() =>
            "Bow Workshop: this structure produces specific military equipment " +
            "essential for training military units. This structure produces [icon:archi]Bows.";

        public string Desc_ProduzioneScudi() =>
            "Shield Workshop: this structure produces specific military equipment " +
            "essential for training military units. This structure produces [icon:scudi]Shields.";

        public string Desc_ProduzioneArmature() =>
            "Armor Workshop: this structure produces specific military equipment " +
            "essential for training military units. This structure produces [icon:armature]Armor.";

        public string Desc_ProduzioneFrecce() =>
            "Arrow Workshop: this structure produces specific military equipment " +
            "essential for training military units. This structure produces [icon:frecce]Arrows.";

        // ── Narrative Descriptions: Barracks ─────────────────────────────────────
        public string Desc_CasermaGuerrieri() =>
            "Warrior Barracks: this vital military structure for any village allows the training and upkeep of specific military units.\n\n" +
            "Each barracks is equipped to house a certain number of men; it is recommended to have a sufficient number of them.";

        public string Desc_CasermaLanceri() =>
            "Spearman Barracks: this vital military structure for any village allows the training and upkeep of specific military units.\n\n" +
            "Each barracks is equipped to house a certain number of men; it is recommended to have a sufficient number of them.";

        public string Desc_CasermaArceri() =>
            "Archer Barracks: this vital military structure for any village allows the training and upkeep of specific military units.\n\n" +
            "Each barracks is equipped to house a certain number of men; it is recommended to have a sufficient number of them.";

        public string Desc_CasermaCatapulte() =>
            "Catapult Barracks: this vital military structure for any village allows the training and upkeep of specific military units.\n\n" +
            "Each barracks is equipped to house a certain number of men; it is recommended to have a sufficient number of them.";

        // ── Narrative Descriptions: Warriors ───────────────────────────────────
        public string Desc_Guerriero(int numero) =>
            $"{ITA.ToRoman(numero)} Warriors are the backbone of the army; although they lack shields, " +
            "they are easy to recruit and are not very demanding in terms of food and gold.";

        // ── Narrative Descriptions: Spearmen ────────────────────────────────────
        public string Desc_Lancere(int numero) =>
            $"{ITA.ToRoman(numero)} Spearmen are the backbone of any well-organized army. Armed with spears, " +
            "these soldiers constitute a formidable bulwark against enemy assaults.";

        // ── Narrative Descriptions: Archers ─────────────────────────────────────
        public string Desc_Arcere(int numero) =>
            $"{ITA.ToRoman(numero)} Archers, armed with bows and quivers, are specialized soldiers who dominate the battlefield from a distance, " +
            "showering enemy lines with deadly arrows before they can even get close.";

        // ── Narrative Descriptions: Catapults ───────────────────────────────────
        public string Desc_Catapulta(int numero) =>
            $"{ITA.ToRoman(numero)} Catapults are powerful siege engines that turn the tide of battle, " +
            "launching massive projectiles to destroy walls and sow terror among enemy ranks.";

        // ── Narrative Descriptions: Experience / Level ────────────────────────
        public string Desc_Esperienza(string esperienza) =>
            $"Experience[icon:xp] represents the player's growth over time.\nAccumulating experience allows you to level up.\n\n Experience for next Level: [icon:xp][acciaioBlu]{esperienza}[black]xp\n";

        public string Desc_Livello() =>
            "The Level[icon:lv] indicates the player's advancement progress, essential for reaching the peaks of research and improving units and structures.\n\n" +
            "Required to advance in 'PVP/PVE', upgrade your village's defensive structures, and unlock advanced military units.\n " +
            "Currently, there is no level cap.\n";

        public string Desc_Statistiche() =>
            "Statistics Tab: here you can view your game statistics by clicking the icon, along with additional useful information for your progress.\n";

        public string Desc_DiamantiBlu() =>
            $"[blu]Blue Diamonds[/blu][black][icon:diamanteBlu] can be used within the shop to purchase packs for better city management.\n\n" +
            "They may also be required for certain quests and to speed up waiting times for structures and military units.\n";

        public string Desc_DiamantiViola() =>
            $"[viola]Purple Diamonds[/viola][black][icon:diamanteViola] are fundamental for purchasing [warning]fiefs[/warning][black] and are the basis of the economy.\n\n" +
            $"They can be exchanged for [blu]Blue Diamonds[/blu][black][icon:diamanteBlu] and used within the shop to purchase packs or for better city management.\n\n" +
            $"Besides being required in some quests, they should always be present in the city treasury.";

        public string Desc_DollariVirtuali() =>
            $"[icon:dollariVirtuali]Fief Tributes are generated by the fiefs owned by the player.\n\nThey can be withdrawn once the threshold of [verde]{Variabili_Server.prelievo_Minimo}$[/verde][black][icon:dollariVirtuali] is reached " +
            "or used within the shop to purchase packs.\nCurrently, [icon:dollariVirtuali]1 tribute equals [icon:usdt]1 USDT";

        public string Desc_Cibo() =>
            "[icon:cibo]Food is fundamental for the maintenance of military units, their training, and building construction. Also required for research.\n\n";

        public string Desc_Legno() =>
            "[icon:legno]Wood is necessary for training military units and constructing structures. Required for research.";

        public string Desc_Pietra() =>
            "[icon:pietra]Stone is fundamental for repairing defensive structures. Required for research.\n";

        public string Desc_Ferro() =>
            "[icon:ferro]Iron is fundamental for building construction and repairing defensive structures.\n" +
            "It is required for the production of military equipment and for research.\n\n";

        public string Desc_Oro() =>
            "[icon:oro]Gold is a primary resource for the city treasury, necessary for civil and military buildings, as well as for recruiting units.\n\n" +
            "It is fundamental for the upkeep of units and war structures, in addition to research.\n\n";

        public string Desc_Popolazione() =>
            "[icon:popolazione]Population is fundamental for the construction of civil and military structures, as well as for recruiting units.\n\n";

        public string Desc_Spade() =>
            $"Descrizione|Spade|[black]Swords[icon:spade] are necessary for training [cuoioScuro]warriors[icon:guerriero][black].\n\n";

        public string Desc_Lance() =>
            $"Descrizione|Lance|[black]Spears[icon:lance] are necessary for training [cuoioScuro]spearmen[icon:lancere][black].\n\n";

        public string Desc_Archi() =>
            $"Descrizione|Archi|[black]Bows[icon:archi] are necessary for training [cuoioScuro]archers[icon:arcere][black].\n\n";

        public string Desc_Scudi() =>
            $"Descrizione|Scudi|[black]Shields[icon:scudi] are necessary for training military units.\n\n";

        public string Desc_Armature() =>
            $"Descrizione|Armature|[black]Armor[icon:armature] is necessary for training military units.\n\n";

        public string Desc_Frecce() =>
            $"Descrizione|Frecce|[black]Arrows[icon:frecce] are fundamental for [cuoioScuro]ranged units[black]; without them, they are practically useless.\n\n";

        // ── Shop Descriptions ────────────────────────────────────────
        public string Desc_Shop_GamePassBase() =>
            "[black]By purchasing this 'Basic GamePass' pack...\nNo advantages currently available. It lasts for [title]30 [black]days.";

        public string Desc_Shop_GamePassAvanzato() =>
            "[black]By purchasing this 'Advanced GamePass' pack...\nNo advantages currently available. It lasts for [title]30 [black]days.";

        public string Desc_Shop_Vip1() =>
            "[black]By purchasing this 'Vip 1' pack, your VIP time will be increased.\n A maximum of [ferroScuro]2 [black]days can be accumulated by purchasing multiple packs.";

        public string Desc_Shop_Vip2() =>
            "[black]By purchasing this 'Vip 2' pack, your VIP time will be increased. Once the transaction is confirmed on the blockchain, the diamonds will be credited immediately.\n A maximum of [ferroScuro]2 [black]days can be accumulated by purchasing multiple packs.";

        public string Desc_Shop_Costruttore(string durata) =>
            $"[black]By purchasing this 'Builder {durata}' pack, you can request an additional builder.\n" +
            "Construction time will be increased.\n A maximum of [ferroScuro]3 [black]days can be accumulated by purchasing multiple packs.";

        public string Desc_Shop_Reclutatore(string durata) =>
            $"[black]By purchasing this 'Recruiter {durata}' pack, you can request an additional recruiter.\n" +
            "Recruitment time will be increased.\n A maximum of [ferroScuro]2 [black]days can be accumulated by purchasing multiple packs.";

        public string Desc_Shop_ScudoPace(string durata) =>
            $"[black]By purchasing this 'Peace Shield {durata}' pack, you will obtain protection from other players' attacks.\n" +
            "The shield duration will be added to your current shield time.\n A maximum of [ferroScuro]7 [black]days can be accumulated by purchasing multiple packs.";

        //Ricerca
        public string Desc_RicercaCostruzione(int livello) =>
            $"[black]Reduces the time required to build each structure.\nConstruction research cost lv {livello}\n\n";
        public string Desc_RicercaProduzione(int livello) =>
            $"[black]Increases the amount of resources produced by every production structure.\nProduction research cost lv {livello}\n\n";
        public string Desc_RicercaAddestramento(int livello) =>
           $"[black]Reduces the time required to train each unit.\nTraining research cost lv {livello}\n\n";
        public string Desc_RicercaPopolazione(int livello) =>
            $"[black]Implements better strategies to increase the number of citizens arriving at your village.\nPopulation research cost lv {livello}\n\n";
        public string Desc_RicercaTrasporto(int livello) =>
            $"[black]Increases the transport capacity of individual military units.\nTransport research cost lv {livello}\n\n";
        public string Desc_RicercaRiparazione(int livello) =>
            $"[black]Improves the repair efficiency of individual structures.\nRepair research cost lv {livello}\n\n";

        public string Desc_SaluteMura() =>
            $"[black]Restores the [verdeF]health[black] of the [porporaReale]walls[black] to maximum.\nRepair cost: [verdeF]1 HP[/verdeF][black] per cycle.\n\n";

        public string Desc_DifesaMura() =>
            $"[black]Restores the [blu]defense[black] of the [porporaReale]walls[black] to maximum.\nRepair cost: [blu]1 DEF[/blu][black] per cycle.\n\n";

        public string Desc_SaluteCancello() =>
            $"[black]Restores the [verdeF]health[black] of the [porporaReale]gate[black] to maximum.\nRepair cost: [verdeF]1 HP[/verdeF][black] per cycle.\n\n";

        public string Desc_DifesaCancello() =>
            $"[black]Restores the [blu]defense[black] of the [porporaReale]gate[black] to maximum.\nRepair cost: [blu]1 DEF[/blu][black] per cycle.\n\n";

        public string Desc_SaluteTorri() =>
            $"[black]Restores the [verdeF]health[black] of the [porporaReale]towers[black] to maximum.\nRepair cost: [verdeF]1 HP[/verdeF][black] per cycle.\n\n";

        public string Desc_DifesaTorri() =>
            $"[black]Restores the [blu]defense[black] of the [porporaReale]towers[black] to maximum.\nRepair cost: [blu]1 DEF[/blu][black] per cycle.\n\n";

        public string Desc_CastelloSalute() =>
            $"[black]Restores the [verdeF]health[black] of the [porporaReale]castle[black] to maximum.\nRepair cost: [verdeF]1 HP[/verdeF][black] per cycle.\n\n";

        public string Desc_CastelloDifesa() =>
            $"[black]Restores the [blu]defense[black] of the [porporaReale]castle[black] to maximum.\nRepair cost: [blu]1 DEF[/blu][black] per cycle.\n\n";

        public string Desc_RicercaGuerrieroLivello(int livello) =>
            $"[black]Increases the level of Warriors.\nWarrior level research cost lv {livello}\n\n";

        public string Desc_RicercaGuerrieroSalute(int livello) =>
            $"[black]Increases the health of Warriors.\nWarrior health research cost lv {livello}\n\n";

        public string Desc_RicercaGuerrieroAttacco(int livello) =>
            $"[black]Increases the attack power of Warriors.\nWarrior attack research cost lv {livello}\n\n";

        public string Desc_RicercaGuerrieroDifesa(int livello) =>
            $"[black]Increases the defense of Warriors.\nWarrior defense research cost lv {livello}\n\n";

        public string Desc_RicercaLancereLivello(int livello) =>
            $"[black]Increases the level of Spearmen.\nSpearman level research cost lv {livello}\n\n";

        public string Desc_RicercaLancereSalute(int livello) =>
            $"[black]Increases the health of Spearmen.\nSpearman health research cost lv {livello}\n\n";

        public string Desc_RicercaLancereAttacco(int livello) =>
            $"[black]Increases the attack power of Spearmen.\nSpearman attack research cost lv {livello}\n\n";

        public string Desc_RicercaLancereDifesa(int livello) =>
            $"[black]Increases the defense of Spearmen.\nSpearman defense research cost lv {livello}\n\n";

        public string Desc_RicercaArcereLivelloe(int livello) =>
            $"[black]Increases the level of Archers.\nArcher level research cost lv {livello}\n\n";

        public string Desc_RicercaArcereSalute(int livello) =>
            $"[black]Increases the health of Archers.\nArcher health research cost lv {livello}\n\n";

        public string Desc_RicercaArcereAttacco(int livello) =>
            $"[black]Increases the attack power of Archers.\nArcher attack research cost lv {livello}\n\n";

        public string Desc_RicercaArcereDifesa(int livello) =>
            $"[black]Increases the defense of Archers.\nArcher defense research cost lv {livello}\n\n";

        public string Desc_RicercaCatapultaLivello(int livello) =>
            $"[black]Increases the level of Catapults.\nCatapult level research cost lv {livello}\n\n";

        public string Desc_RicercaCatapultaSalute(int livello) =>
            $"[black]Increases the health of Catapults.\nCatapult health research cost lv {livello}\n\n";

        public string Desc_RicercaCatapultaAttacco(int livello) =>
            $"[black]Increases the attack power of Catapults.\nCatapult attack research cost lv {livello}\n\n";

        public string Desc_RicercaCatapultaDifesa(int livello) =>
            $"[black]Increases the defense of Catapults.\nCatapult defense research cost lv {livello}\n\n";

        public string Desc_RicercaIngressoGuarnigione(int livello) =>
            $"[black]Increases the maximum number of units the structure can hold.\nEntrance garrison research cost lv {livello}\n\n";

        public string Desc_RicercaCittaGuarnigione(int livello) =>
            $"[black]Increases the maximum number of units the structure can hold.\nCity garrison research cost lv {livello}\n\n";

        public string Desc_RicercaMuraLivello(int livello) =>
            $"[black]Increases the level of the structure.\nWall level research cost: {livello}\n\n";

        public string Desc_RicercaMuraGuarnigione(int livello) =>
            $"[black]Increases the maximum number of units the structure can hold.\nWall garrison research cost lv: {livello}\n\n";

        public string Desc_RicercaMuraSalute(int livello) =>
            $"[black]Increases the maximum health points of the structure.\nWall health research cost lv: {livello}\n\n";

        public string Desc_RicercaMuraDifesa(int livello) =>
            $"[black]Increases the maximum defense points of the structure.\nWall defense research cost lv: {livello}\n\n";

        public string Desc_RicercaCancelloLivello(int livello) =>
            $"[black]Increases the maximum number of units the structure can hold.\nGate level research cost: {livello}\n\n";

        public string Desc_RicercaCancelloGuarnigione(int livello) =>
            $"[black]Increases the maximum number of units the structure can hold.\nGate garrison research cost lv: {livello}\n\n";

        public string Desc_RicercaCancelloSalute(int livello) =>
            $"[black]Increases the maximum health points of the structure.\nGate health research cost lv: {livello}\n\n";

        public string Desc_RicercaCancelloDifesa(int livello) =>
            $"[black]Increases the maximum defense points of the structure.\nGate defense research cost lv: {livello}\n\n";

        public string Desc_RicercaTorriLivello(int livello) =>
            $"[black]Increases the maximum number of units the structure can hold.\nTower level research cost: {livello}\n\n";

        public string Desc_RicercaTorriGuarnigione(int livello) =>
            $"[black]Increases the maximum number of units the structure can hold.\nTower garrison research cost lv: {livello}\n\n";

        public string Desc_RicercaTorriSalute(int livello) =>
            $"[black]Increases the maximum health points of the structure.\nTower health research cost lv: {livello}\n\n";

        public string Desc_RicercaTorriDifesa(int livello) =>
            $"[black]Increases the maximum defense points of the structure.\nTower defense research cost lv: {livello}\n\n";

        public string Desc_RicercaCastelloLivello(int livello) =>
            $"[black]Increases the maximum number of units the structure can hold.\nCastle level research cost: {livello}\n\n";

        public string Desc_RicercaCastelloGuarnigione(int livello) =>
            $"[black]Increases the maximum number of units the structure can hold.\nCastle garrison research cost lv: {livello}\n\n";

        public string Desc_RicercaCastelloSalute(int livello) =>
            $"[black]Increases the maximum health points of the structure.\nCastle health research cost lv: {livello}\n\n";

        public string Desc_RicercaCastelloDifesa(int livello) =>
            $"[black]Increases the maximum defense points of the structure.\nCastle defense research cost lv: {livello}\n\n";

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
    }
}
