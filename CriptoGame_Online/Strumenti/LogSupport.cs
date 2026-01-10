using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CriptoGame_Online.Strumenti.GameTextBox;

namespace CriptoGame_Online.Strumenti
{
    internal class LogSupport
    {
        private static readonly Dictionary<string, Color> ColorScheme = new()
        {
            ["default"] = Color.White,
            ["black"] = Color.Black,
            ["verde"] = Color.Green,
            ["rosso"] = Color.FromArgb(139, 0, 0),          //Rosso scuro
            ["grigioGrafite"] = Color.FromArgb(80, 80, 80),
            ["ruggine"] = Color.FromArgb(160, 72, 40),

            ["legnoScuro"] = Color.FromArgb(70, 50, 30),   // legno bruciato
            ["legnoAntico"] = Color.FromArgb(139, 94, 60),
            ["ombraNotturna"] = Color.FromArgb(30, 30, 35),   // quasi nero
            ["ruggineScura"] = Color.FromArgb(90, 40, 25),   // ruggine medievale

            ["carbone"] = Color.FromArgb(30, 30, 30),     // Quasi nero, molto medievale
            ["grafite"] = Color.FromArgb(60, 60, 60),     // Grigio scuro
            ["marroneQuercia"] = Color.FromArgb(90, 60, 35),     // Legno antico
            ["verdeF"] = Color.FromArgb(34, 80, 44),     // Verde scuro, naturale
            ["bluGotico"] = Color.FromArgb(36, 52, 94),     // Blu profondo
            ["rossoAraldico"] = Color.FromArgb(130, 30, 30),    // Rosso medievale scuro
            ["porporaReale"] = Color.FromArgb(85, 30, 75),     // Porpora regale scura
            ["acciaioBlu"] = Color.FromArgb(70, 95, 120),    // Freddo, metallico
            ["ruggine"] = Color.FromArgb(120, 55, 25),    // Marrone-ruggine scuro
            ["ferroScuro"] = Color.FromArgb(50, 50, 60),     // Grigio metallo scuro

            ["arancione"] = Color.FromArgb(220, 78, 0),      // Arancione scuro

            ["success"] = Color.FromArgb(46, 204, 113),  // Verde
            ["warning"] = Color.FromArgb(241, 196, 15),  // Giallo
            ["error"] = Color.FromArgb(231, 76, 60),     // Rosso

            ["cibo"] = Color.FromArgb(255, 140, 0),      // Arancione
            ["legno"] = Color.FromArgb(139, 90, 43),      // Marrone
            ["pietra"] = Color.FromArgb(169, 169, 169),   // Grigio
            ["ferro"] = Color.FromArgb(192, 192, 192),    // Argento
            ["oro"] = Color.FromArgb(255, 215, 0),      // Oro
            ["popolazione"] = Color.FromArgb(70, 95, 120),

            ["viola"] = Color.FromArgb(155, 89, 182), // Viola brillante
            ["blu"] = Color.FromArgb(52, 152, 219), // Blu/azzurro intenso

            ["attacco"] = Color.FromArgb(192, 57, 43),     // Rosso scuro
            ["info"] = Color.FromArgb(52, 152, 219),     // Azzurro
            ["title"] = Color.FromArgb(255, 223, 186),   // Beige chiaro
            ["highlight"] = Color.FromArgb(255, 255, 150), // Giallo chiaro

            ["TerrenoComune"] = Color.FromArgb(128, 128, 128),   // Comune
            ["TerrenoNoncomune"] = Color.FromArgb(0, 200, 0),       // Non Comune
            ["TerrenoRaro"] = Color.FromArgb(0, 112, 255),     // Raro
            ["TerrenoEpico"] = Color.FromArgb(160, 32, 240),    // Epico
            ["TerrenoLeggendario"] = Color.FromArgb(205, 175, 0)     // Leggendario
        };

        private static readonly Dictionary<string, Image> Icons = new()
        {
            ["xp"] = Properties.Resources.Exp_1,
            ["lv"] = Properties.Resources.level_up_1_,

            ["cibo"] = Properties.Resources.wheat_sack,
            ["legno"] = Properties.Resources.wood_log_6,
            ["pietra"] = Properties.Resources.Stone_2,
            ["ferro"] = Properties.Resources.Ingot__Iron_icon_icon,
            ["oro"] = Properties.Resources.Gold_Ingot_icon_icon,
            ["popolazione"] = Properties.Resources.manpower_2,

            ["diamanteBlu"] = Properties.Resources.diamond_1,
            ["diamanteViola"] = Properties.Resources.diamond_2,
            ["dollariVirtuali"] = Properties.Resources.dollars_Edit_removebg_preview,
            ["usdt"] = Properties.Resources.USDT_Logo_removebg_preview,

            ["TerrenoComune"] = Properties.Resources.Comune,
            ["TerrenoNoncomune"] = Properties.Resources.Non_Comune,
            ["TerrenoRaro"] = Properties.Resources.Raro,
            ["TerrenoEpico"] = Properties.Resources.Epico,
            ["TerrenoLeggendario"] = Properties.Resources.Leggendario,

            ["tempo"] = Properties.Resources.Clessidra_removebg_preview,

            ["spade"] = Properties.Resources.Sword_1,
            ["lance"] = Properties.Resources.spears,
            ["archi"] = Properties.Resources.icons8_tiro_con_l_arco_48_1_,
            ["scudi"] = Properties.Resources.icons8_scudo_48_2_,
            ["armature"] = Properties.Resources.icons8_armor_48_1_,
            ["frecce"] = Properties.Resources.icons8_freccia_di_arcieri_48,

            ["Fattoria"] = Properties.Resources.icons8_fattoria_48_1_,
            ["Segheria"] = Properties.Resources.wood_cutting,
            ["CavaPietra"] = Properties.Resources.icons8_carrello_da_miniera_48_3_,
            ["MinieraFerro"] = Properties.Resources.icons8_carrello_da_miniera_48_2_,
            ["MinieraOro"] = Properties.Resources.icons8_carrello_da_miniera_48_1_,
            ["Case"] = Properties.Resources.medieval_house_1_,

            ["guerriero"] = Properties.Resources.Guerriero_V2_removebg_preview,
            ["lancere"] = Properties.Resources.Lancere_V2_removebg_preview,
            ["arcere"] = Properties.Resources.Arciere_V2_removebg_preview,
            ["catapulta"] = Properties.Resources.icons8_medieval_48,

            ["workshopSpade"] = Properties.Resources.Workshop_Spade,
            ["workshopLance"] = Properties.Resources.Workshop_Lance,
            ["workshopArchi"] = Properties.Resources.Workshop_Archi,
            ["workshopScudi"] = Properties.Resources.Workshop_Scudi,
            ["workshopArmture"] = Properties.Resources.Workshop_Armature,
            ["workshopFrecce"] = Properties.Resources.Workshop_Frecce,

            ["CasermaGuerrieri"] = Properties.Resources.icons8_freccia_di_arcieri_48,  //No immagine D:
            ["CasermaLanceri"] = Properties.Resources.icons8_freccia_di_arcieri_48,  //No immagine D:
            ["CasermaArceri"] = Properties.Resources.icons8_freccia_di_arcieri_48,  //No immagine D:
            ["CasermaCatapulte"] = Properties.Resources.icons8_freccia_di_arcieri_48,  //No immagine D:
        };

        public static List<GameTextBox.Segment> Parse(string message)
        {
            var segments = new List<GameTextBox.Segment>();

            int i = 0;
            Color currentColor = ColorScheme["default"];
            string currentText = "";

            while (i < message.Length)
            {
                if (message[i] == '[')
                {
                    // Salva il testo accumulato prima del tag
                    if (currentText.Length > 0)
                    {
                        segments.Add(new GameTextBox.Segment
                        {
                            Text = currentText,
                            Color = currentColor,
                            IsIcon = false
                        });
                        currentText = "";
                    }

                    int closeIdx = message.IndexOf(']', i);
                    if (closeIdx == -1)
                    {
                        // Tag non chiuso, tratta come testo normale
                        currentText += message[i];
                        i++;
                        continue;
                    }

                    string tag = message.Substring(i + 1, closeIdx - i - 1);

                    // Tag di chiusura
                    if (tag.StartsWith("/"))
                    {
                        currentColor = ColorScheme["default"];
                    }
                    // Tag icona
                    else if (tag.StartsWith("icon:"))
                    {
                        string iconName = tag.Substring(5);
                        if (Icons.ContainsKey(iconName))
                        {
                            segments.Add(new GameTextBox.Segment
                            {
                                Icon = Icons[iconName],
                                IsIcon = true
                            });
                        }
                    }
                    else if (ColorScheme.ContainsKey(tag)) // Tag colore
                    {
                        currentColor = ColorScheme[tag];
                    }
                    i = closeIdx + 1; // Tag sconosciuto, ignora
                }
                else
                {
                    currentText += message[i];
                    i++;
                }
            }

            // Aggiungi l'ultimo testo rimasto
            if (currentText.Length > 0)
            {
                segments.Add(new GameTextBox.Segment
                {
                    Text = currentText,
                    Color = currentColor,
                    IsIcon = false
                });
            }

            return segments;
        }

    }
}
