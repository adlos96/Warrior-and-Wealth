namespace Server_Strategico.Gioco
{
    public class Esercito
    {
        public class CostoReclutamento //costi di reclutamento
        {
            #region Dati
            public double Spade { get; set; }
            public double Lance { get; set; }
            public double Archi { get; set; }
            public double Scudi { get; set; }
            public double Armature { get; set; }
            public double Cibo { get; set; }
            public double Legno { get; set; }
            public double Pietra { get; set; }
            public double Ferro { get; set; }
            public double Oro { get; set; }
            public double Salute_Repair { get; set; }
            public double Difesa_Repair { get; set; }
            public double Trasporto { get; set; }
            public double TempoReclutamento { get; set; }
            public double Popolazione { get; set; }
            #endregion
            public static CostoReclutamento Guerriero_1 = new CostoReclutamento
            {
                Spade = 1,
                Lance = 0,
                Archi = 0,
                Scudi = 0,
                Armature = 1,

                Cibo = 535,
                Legno = 422,
                Pietra = 394,
                Ferro = 438,
                Oro = 410,                  //Consumo risorse per reclutamento
                TempoReclutamento = 1600,   //tempo reclutamento in secondi
                Popolazione = 1
            };
            public static CostoReclutamento Lancere_1 = new CostoReclutamento
            {
                Spade = 0,
                Lance = 1,
                Archi = 0,
                Scudi = 1,
                Armature = 1,

                Cibo = 686,
                Legno = 535,
                Pietra = 507,
                Ferro = 564,
                Oro = 543,
                TempoReclutamento = 2021,
                Popolazione = 1
            };
            public static CostoReclutamento Arcere_1 = new CostoReclutamento
            {
                Spade = 0,
                Lance = 0,
                Archi = 1,
                Scudi = 0,
                Armature = 1,

                Cibo = 759,
                Legno = 634,
                Pietra = 593,
                Ferro = 613,
                Oro = 602,
                TempoReclutamento = 2443,
                Popolazione = 1
            };
            public static CostoReclutamento Catapulta_1 = new CostoReclutamento
            {
                Spade = 3,
                Lance = 3,
                Archi = 0,
                Scudi = 6,
                Armature = 6,

                Cibo = 944,
                Legno = 857,
                Pietra = 889,
                Ferro = 776,
                Oro = 843,
                TempoReclutamento = 2866,
                Popolazione = 6
            };
            public static CostoReclutamento Guerriero_2 = new CostoReclutamento
            {
                Spade = 2,
                Lance = 0,
                Archi = 0,
                Scudi = 1,
                Armature = 2,

                Cibo = 785,
                Legno = 672,
                Pietra = 644,
                Ferro = 688,
                Oro = 660,
                TempoReclutamento = 1900, //55
                Popolazione = 2
            };
            public static CostoReclutamento Lancere_2 = new CostoReclutamento
            {
                Spade = 0,
                Lance = 2,
                Archi = 0,
                Scudi = 2,
                Armature = 2,

                Cibo = 936,
                Legno = 785,
                Pietra = 757,
                Ferro = 814,
                Oro = 793,
                TempoReclutamento = 2321,
                Popolazione = 2
            };
            public static CostoReclutamento Arcere_2 = new CostoReclutamento
            {
                Spade = 1,
                Lance = 0,
                Archi = 2,
                Scudi = 0,
                Armature = 2,

                Cibo = 1009,
                Legno = 884,
                Pietra = 843,
                Ferro = 863,
                Oro = 852,
                TempoReclutamento = 2743,
                Popolazione = 2
            };
            public static CostoReclutamento Catapulta_2 = new CostoReclutamento
            {
                Spade = 5,
                Lance = 5,
                Archi = 0,
                Scudi = 10,
                Armature = 10,

                Cibo = 1194,
                Legno = 1107,
                Pietra = 1139,
                Ferro = 1026,
                Oro = 1093,
                TempoReclutamento = 3166,
                Popolazione = 12
            };
            public static CostoReclutamento Guerriero_3 = new CostoReclutamento
            {
                Spade = 4,
                Lance = 0,
                Archi = 0,
                Scudi = 2,
                Armature = 4,

                Cibo = 985,
                Legno = 872,
                Pietra = 844,
                Ferro = 888,
                Oro = 860,
                TempoReclutamento = 2500, //55
                Popolazione = 3
            };
            public static CostoReclutamento Lancere_3 = new CostoReclutamento
            {
                Spade = 0,
                Lance = 4,
                Archi = 0,
                Scudi = 4,
                Armature = 4,

                Cibo = 1136,
                Legno = 985,
                Pietra = 957,
                Ferro = 1014,
                Oro = 993,
                TempoReclutamento = 2921,
                Popolazione = 3
            };
            public static CostoReclutamento Arcere_3 = new CostoReclutamento
            {
                Spade = 2,
                Lance = 0,
                Archi = 4,
                Scudi = 1,
                Armature = 4,

                Cibo = 1209,
                Legno = 1084,
                Pietra = 1043,
                Ferro = 1063,
                Oro = 1052,
                TempoReclutamento = 3343,
                Popolazione = 3
            };
            public static CostoReclutamento Catapulta_3 = new CostoReclutamento
            {
                Spade = 8,
                Lance = 8,
                Archi = 0,
                Scudi = 16,
                Armature = 16,

                Cibo = 1394,
                Legno = 1307,
                Pietra = 1339,
                Ferro = 1226,
                Oro = 1293,
                TempoReclutamento = 3766,
                Popolazione = 18
            };
            public static CostoReclutamento Guerriero_4 = new CostoReclutamento
            {
                Spade = 8,
                Lance = 0,
                Archi = 0,
                Scudi = 4,
                Armature = 8,

                Cibo = 1285,
                Legno = 1172,
                Pietra = 1144,
                Ferro = 1188,
                Oro = 1160,
                TempoReclutamento = 3400, //55
                Popolazione = 4
            };
            public static CostoReclutamento Lancere_4 = new CostoReclutamento
            {
                Spade = 0,
                Lance = 8,
                Archi = 0,
                Scudi = 8,
                Armature = 8,

                Cibo = 1436,
                Legno = 1285,
                Pietra = 1257,
                Ferro = 1314,
                Oro = 1293,
                TempoReclutamento = 3821,
                Popolazione = 4
            };
            public static CostoReclutamento Arcere_4 = new CostoReclutamento
            {
                Spade = 4,
                Lance = 0,
                Archi = 8,
                Scudi = 2,
                Armature = 8,

                Cibo = 1509,
                Legno = 1384,
                Pietra = 1343,
                Ferro = 1363,
                Oro = 1352,
                TempoReclutamento = 4243,
                Popolazione = 4
            };
            public static CostoReclutamento Catapulta_4 = new CostoReclutamento
            {
                Spade = 12,
                Lance = 12,
                Archi = 0,
                Scudi = 24,
                Armature = 24,

                Cibo = 1694,
                Legno = 1607,
                Pietra = 1639,
                Ferro = 1526,
                Oro = 1593,
                TempoReclutamento = 4666,
                Popolazione = 24
            };
            public static CostoReclutamento Guerriero_5 = new CostoReclutamento
            {
                Spade = 16,
                Lance = 0,
                Archi = 0,
                Scudi = 8,
                Armature = 16,

                Cibo = 1685,
                Legno = 1572,
                Pietra = 1544,
                Ferro = 1588,
                Oro = 1560,
                TempoReclutamento = 4600, //55
                Popolazione = 5
            };
            public static CostoReclutamento Lancere_5 = new CostoReclutamento
            {
                Spade = 0,
                Lance = 16,
                Archi = 0,
                Scudi = 16,
                Armature = 16,

                Cibo = 1836,
                Legno = 1685,
                Pietra = 1657,
                Ferro = 1714,
                Oro = 1693,
                TempoReclutamento = 5021,
                Popolazione = 5
            };
            public static CostoReclutamento Arcere_5 = new CostoReclutamento
            {
                Spade = 8,
                Lance = 0,
                Archi = 16,
                Scudi = 4,
                Armature = 16,

                Cibo = 1909,
                Legno = 1784,
                Pietra = 1743,
                Ferro = 1763,
                Oro = 1752,
                TempoReclutamento = 5443,
                Popolazione = 5
            };
            public static CostoReclutamento Catapulta_5 = new CostoReclutamento
            {
                Spade = 16,
                Lance = 16,
                Archi = 0,
                Scudi = 32,
                Armature = 32,

                Cibo = 2094,
                Legno = 2007,
                Pietra = 2039,
                Ferro = 1926,
                Oro = 1993,
                TempoReclutamento = 5866,
                Popolazione = 30
            };

        }
        public class Unità //statistiche unità e manutenzione in risorse
        {
            #region Dati
            public int Salute { get; set; }
            public int Attacco { get; set; }
            public int Difesa { get; set; }
            public int Distanza { get; set; }
            public double Salario { get; set; }
            public double Cibo { get; set; }
            public int Quantità { get; set; }
            public int TempoReclutamento { get; set; }
            public int Esperienza { get; set; }
            public int Componente_Lancio { get; set; }
            public int Trasporto { get; set; }
            #endregion
            public static Unità Guerriero_1 = new Unità
            {
                Salute = 6,
                Attacco = 4,
                Difesa = 5,
                Distanza = 1,   //Non serve a nulla al momento
                Salario = 0.14, //Consumo al secondo
                Cibo = 0.24,    //Consumo al secondo
                Esperienza = 1, //Esperienza data in battaglia
                Trasporto = 100 //Capacità di trasporto
            };
            public static Unità Lancere_1 = new Unità
            {
                Salute = 7,
                Attacco = 4,
                Difesa = 6,
                Distanza = 2,
                Salario = 0.20,
                Cibo = 0.32,
                Esperienza = 1,
                Trasporto = 150
            };
            public static Unità Arcere_1 = new Unità
            {
                Salute = 6,
                Attacco = 6,
                Difesa = 3,
                Distanza = 6,
                Salario = 0.28,
                Cibo = 0.46,
                Esperienza = 2,
                Componente_Lancio = 4,
                Trasporto = 200
            };
            public static Unità Catapulta_1 = new Unità
            {
                Salute = 8,
                Attacco = 8,
                Difesa = 7,
                Distanza = 14,
                Salario = 0.44,
                Cibo = 0.69,
                Esperienza = 3,
                Componente_Lancio = 8,
                Trasporto = 250
            };

            public static Unità Guerriero_2 = new Unità
            {
                Salute = 8,
                Attacco = 5,
                Difesa = 6,
                Distanza = 1,
                Salario = 0.17,
                Cibo = 0.29,
                Esperienza = 1,
                Trasporto = 150
            };
            public static Unità Lancere_2 = new Unità
            {
                Salute = 9,
                Attacco = 5,
                Difesa = 7,
                Distanza = 2,
                Salario = 0.21,
                Cibo = 0.34,
                Esperienza = 1,
                Trasporto = 200
            };
            public static Unità Arcere_2 = new Unità
            {
                Salute = 8,
                Attacco = 7,
                Difesa = 4,
                Distanza = 6,
                Salario = 0.26,
                Cibo = 0.42,
                Esperienza = 2,
                Componente_Lancio = 8,
                Trasporto = 250
            };
            public static Unità Catapulta_2 = new Unità
            {
                Salute = 10,
                Attacco = 9,
                Difesa = 8,
                Distanza = 14,
                Salario = 0.44,
                Cibo = 0.68,
                Esperienza = 3,
                Componente_Lancio = 16,
                Trasporto = 300
            };

            public static Unità Guerriero_3 = new Unità
            {
                Salute = 10,
                Attacco = 6,
                Difesa = 7,
                Distanza = 1,
                Salario = 0.21,
                Cibo = 0.34,
                Esperienza = 2,
                Trasporto = 200
            };
            public static Unità Lancere_3 = new Unità
            {
                Salute = 11,
                Attacco = 6,
                Difesa = 8,
                Distanza = 2,
                Salario = 0.24,
                Cibo = 0.38,
                Esperienza = 2,
                Trasporto = 250
            };
            public static Unità Arcere_3 = new Unità
            {
                Salute = 10,
                Attacco = 8,
                Difesa = 5,
                Distanza = 6,
                Salario = 0.30,
                Cibo = 0.46,
                Esperienza = 3,
                Componente_Lancio = 12,
                Trasporto = 300
            };
            public static Unità Catapulta_3 = new Unità
            {
                Salute = 12,
                Attacco = 10,
                Difesa = 9,
                Distanza = 14,
                Salario = 0.48,
                Cibo = 0.72,
                Esperienza = 4,
                Componente_Lancio = 24,
                Trasporto = 350
            };

            public static Unità Guerriero_4 = new Unità
            {
                Salute = 12,
                Attacco = 7,
                Difesa = 8,
                Distanza = 1,
                Salario = 0.25,
                Cibo = 0.38,
                Esperienza = 2,
                Trasporto = 250
            };
            public static Unità Lancere_4 = new Unità
            {
                Salute = 13,
                Attacco = 7,
                Difesa = 9,
                Distanza = 2,
                Salario = 0.30,
                Cibo = 0.42,
                Esperienza = 2,
                Trasporto = 300
            };
            public static Unità Arcere_4 = new Unità
            {
                Salute = 12,
                Attacco = 9,
                Difesa = 6,
                Distanza = 6,
                Salario = 0.34,
                Cibo = 0.50,
                Esperienza = 3,
                Componente_Lancio = 16,
                Trasporto = 350
            };
            public static Unità Catapulta_4 = new Unità
            {
                Salute = 14,
                Attacco = 11,
                Difesa = 10,
                Distanza = 14,
                Salario = 0.52,
                Cibo = 0.76,
                Esperienza = 4,
                Componente_Lancio = 32,
                Trasporto = 400
            };

            public static Unità Guerriero_5 = new Unità
            {
                Salute = 14,
                Attacco = 8,
                Difesa = 9,
                Distanza = 1,
                Salario = 0.29,
                Cibo = 0.42,
                Esperienza = 3,
                Trasporto = 300
            };
            public static Unità Lancere_5 = new Unità
            {
                Salute = 15,
                Attacco = 8,
                Difesa = 10,
                Distanza = 2,
                Salario = 0.34,
                Cibo = 0.46,
                Esperienza = 3,
                Trasporto = 350
            };
            public static Unità Arcere_5 = new Unità
            {
                Salute = 14,
                Attacco = 10,
                Difesa = 7,
                Distanza = 6,
                Salario = 0.38,
                Cibo = 0.54,
                Esperienza = 4,
                Componente_Lancio = 20,
                Trasporto = 400
            };
            public static Unità Catapulta_5 = new Unità
            {
                Salute = 16,
                Attacco = 12,
                Difesa = 11,
                Distanza = 14,
                Salario = 0.52,
                Cibo = 0.76,
                Esperienza = 5,
                Componente_Lancio = 40,
                Trasporto = 450
            };
        }
        public class EsercitoNemico
        {
            public static Unità Guerrieri_1 = new Unità
            {
                Salute = 6,
                Attacco = 4,
                Difesa = 5,
                Distanza = 1,
                Quantità = 0,
                Esperienza = 1
            };
            public static Unità Lanceri_1 = new Unità
            {
                Salute = 7,
                Attacco = 4,
                Difesa = 6,
                Distanza = 2,
                Quantità = 0,
                Esperienza = 1
            };
            public static Unità Arceri_1 = new Unità
            {
                Salute = 6,
                Attacco = 6,
                Difesa = 3,
                Distanza = 6,
                Quantità = 0,
                Esperienza = 2
            };
            public static Unità Catapulte_1 = new Unità
            {
                Salute = 9,
                Attacco = 9,
                Difesa = 8,
                Distanza = 14,
                Quantità = 0,
                Esperienza = 3
            };

            public static Unità Guerrieri_2 = new Unità
            {
                Salute = 8,
                Attacco = 5,
                Difesa = 6,
                Distanza = 1,
                Quantità = 0,
                Esperienza = 1
            };
            public static Unità Lanceri_2 = new Unità
            {
                Salute = 9,
                Attacco = 5,
                Difesa = 7,
                Distanza = 2,
                Quantità = 0,
                Esperienza = 1
            };
            public static Unità Arceri_2 = new Unità
            {
                Salute = 8,
                Attacco = 7,
                Difesa = 4,
                Distanza = 6,
                Quantità = 0,
                Esperienza = 2
            };
            public static Unità Catapulte_2 = new Unità
            {
                Salute = 11,
                Attacco = 10,
                Difesa = 9,
                Distanza = 14,
                Quantità = 0,
                Esperienza = 3
            };

            public static Unità Guerrieri_3 = new Unità
            {
                Salute = 10,
                Attacco = 6,
                Difesa = 7,
                Distanza = 1,
                Quantità = 0,
                Esperienza = 1
            };
            public static Unità Lanceri_3 = new Unità
            {
                Salute = 11,
                Attacco = 6,
                Difesa = 8,
                Distanza = 2,
                Quantità = 0,
                Esperienza = 1
            };
            public static Unità Arceri_3 = new Unità
            {
                Salute = 10,
                Attacco = 8,
                Difesa = 5,
                Distanza = 6,
                Quantità = 0,
                Esperienza = 2
            };
            public static Unità Catapulte_3 = new Unità
            {
                Salute = 13,
                Attacco = 11,
                Difesa = 10,
                Distanza = 14,
                Quantità = 0,
                Esperienza = 3
            };

            public static Unità Guerrieri_4 = new Unità
            {
                Salute = 12,
                Attacco = 7,
                Difesa = 8,
                Distanza = 1,
                Quantità = 0,
                Esperienza = 1
            };
            public static Unità Lanceri_4 = new Unità
            {
                Salute = 13,
                Attacco = 7,
                Difesa = 9,
                Distanza = 2,
                Quantità = 0,
                Esperienza = 1
            };
            public static Unità Arceri_4 = new Unità
            {
                Salute = 12,
                Attacco = 9,
                Difesa = 6,
                Distanza = 6,
                Quantità = 0,
                Esperienza = 2
            };
            public static Unità Catapulte_4 = new Unità
            {
                Salute = 15,
                Attacco = 12,
                Difesa = 11,
                Distanza = 14,
                Quantità = 0,
                Esperienza = 3
            };

            public static Unità Guerrieri_5 = new Unità
            {
                Salute = 14,
                Attacco = 8,
                Difesa = 9,
                Distanza = 1,
                Quantità = 0,
                Esperienza = 1
            };
            public static Unità Lanceri_5 = new Unità
            {
                Salute = 15,
                Attacco = 8,
                Difesa = 10,
                Distanza = 2,
                Quantità = 0,
                Esperienza = 1
            };
            public static Unità Arceri_5 = new Unità
            {
                Salute = 14,
                Attacco = 10,
                Difesa = 7,
                Distanza = 6,
                Quantità = 0,
                Esperienza = 2
            };
            public static Unità Catapulte_5 = new Unità
            {
                Salute = 17,
                Attacco = 13,
                Difesa = 12,
                Distanza = 14,
                Quantità = 0,
                Esperienza = 3
            };
        }
    }
}
