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
            ["ferroScuro"] = Color.FromArgb(50, 50, 60),     // Grigio metallo scuro
            ["verdeF"] = Color.FromArgb(34, 80, 44),     // Verde scuro, naturale
            ["bluGotico"] = Color.FromArgb(36, 52, 94),     // Blu profondo
            ["porporaReale"] = Color.FromArgb(85, 30, 75),     // Porpora regale scura
            ["acciaioBlu"] = Color.FromArgb(70, 95, 120),    // Freddo, metallico
            ["arancione"] = Color.FromArgb(220, 78, 0),      // Arancione scuro

            //Test colori
            ["ocraDorata"] = Color.FromArgb(184, 134, 11),          //Rosso scuro
            ["bluNotte"] = Color.FromArgb(26, 42, 51),          //Rosso scuro

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

            ["spade"] = Properties.Resources.Spade_V2,
            ["lance"] = Properties.Resources.Lance_V2,
            ["archi"] = Properties.Resources.Archi_V2,
            ["scudi"] = Properties.Resources.Scudi_V2,
            ["armature"] = Properties.Resources.Armature_V2,
            ["frecce"] = Properties.Resources.Frecce_V2,

            ["fattoria"] = Properties.Resources.Fattoria_V2,
            ["segheria"] = Properties.Resources.Segheria_V2,
            ["cavaPietra"] = Properties.Resources.CavaDiPietra_V2,
            ["minieraFerro"] = Properties.Resources.MinieraFerro_V2,
            ["minieraOro"] = Properties.Resources.MinieraOro_V2,
            ["case"] = Properties.Resources.Abitazioni_V2,

            ["guerriero"] = Properties.Resources.Guerriero_V2,
            ["lancere"] = Properties.Resources.Lanciere_V2,
            ["arcere"] = Properties.Resources.Arciere_V2,
            ["catapulta"] = Properties.Resources.Catapulta_V2,

            ["workshopSpade"] = Properties.Resources.Workshop_Spade_V2,
            ["workshopLance"] = Properties.Resources.Workshop_Lance_V2,
            ["workshopArchi"] = Properties.Resources.Workshop_Archi_V2,
            ["workshopScudi"] = Properties.Resources.Workshop_Scudi_V2,
            ["workshopArmture"] = Properties.Resources.Workshop_Armature_V2,
            ["workshopFrecce"] = Properties.Resources.Workshop_Frecce_V2,

            ["casermaGuerrieri"] = Properties.Resources.Caserma_Guerieri_V2,
            ["casermaLanceri"] = Properties.Resources.Caserma_Lanceri_V2,
            ["casermaArceri"] = Properties.Resources.Caserma_Arcieri_V2,
            ["casermaCatapulte"] = Properties.Resources.Caserma_Catapulte_V2,

            ["info"] = Properties.Resources.info,
            ["scambio"] = Properties.Resources.exchange_Edit_removebg_preview,
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
