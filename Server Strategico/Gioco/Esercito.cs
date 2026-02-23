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
                TempoReclutamento = 2000,   //tempo reclutamento in secondi
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
                TempoReclutamento = 2421,
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
                TempoReclutamento = 2843,
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
                TempoReclutamento = 3266,
                Popolazione = 6
            };
            public static CostoReclutamento Guerriero_2 = new CostoReclutamento
            {
                Spade = 2,
                Lance = 0,
                Archi = 0,
                Scudi = 1,
                Armature = 2,

                Cibo = 690,
                Legno = 577,
                Pietra = 549,
                Ferro = 593,
                Oro = 565,
                TempoReclutamento = 2500, //55
                Popolazione = 2
            };
            public static CostoReclutamento Lancere_2 = new CostoReclutamento
            {
                Spade = 0,
                Lance = 2,
                Archi = 0,
                Scudi = 2,
                Armature = 2,

                Cibo = 841,
                Legno = 690,
                Pietra = 662,
                Ferro = 719,
                Oro = 689,
                TempoReclutamento = 2921,
                Popolazione = 2
            };
            public static CostoReclutamento Arcere_2 = new CostoReclutamento
            {
                Spade = 1,
                Lance = 0,
                Archi = 2,
                Scudi = 0,
                Armature = 2,

                Cibo = 914,
                Legno = 789,
                Pietra = 748,
                Ferro = 768,
                Oro = 757,
                TempoReclutamento = 3643,
                Popolazione = 2
            };
            public static CostoReclutamento Catapulta_2 = new CostoReclutamento
            {
                Spade = 5,
                Lance = 5,
                Archi = 0,
                Scudi = 10,
                Armature = 10,

                Cibo = 1099,
                Legno = 1012,
                Pietra = 1044,
                Ferro = 931,
                Oro = 998,
                TempoReclutamento = 3766,
                Popolazione = 12
            };
            public static CostoReclutamento Guerriero_3 = new CostoReclutamento
            {
                Spade = 4,
                Lance = 0,
                Archi = 0,
                Scudi = 2,
                Armature = 4,

                Cibo = 945,
                Legno = 832,
                Pietra = 804,
                Ferro = 848,
                Oro = 820,
                TempoReclutamento = 3400, //55
                Popolazione = 3
            };
            public static CostoReclutamento Lancere_3 = new CostoReclutamento
            {
                Spade = 0,
                Lance = 4,
                Archi = 0,
                Scudi = 4,
                Armature = 4,

                Cibo = 1096,
                Legno = 945,
                Pietra = 917,
                Ferro = 974,
                Oro = 944,
                TempoReclutamento = 3821,
                Popolazione = 3
            };
            public static CostoReclutamento Arcere_3 = new CostoReclutamento
            {
                Spade = 2,
                Lance = 0,
                Archi = 4,
                Scudi = 1,
                Armature = 4,

                Cibo = 1169,
                Legno = 1044,
                Pietra = 1003,
                Ferro = 1023,
                Oro = 1012,
                TempoReclutamento = 4243,
                Popolazione = 3
            };
            public static CostoReclutamento Catapulta_3 = new CostoReclutamento
            {
                Spade = 8,
                Lance = 8,
                Archi = 0,
                Scudi = 16,
                Armature = 16,

                Cibo = 1354,
                Legno = 1267,
                Pietra = 1299,
                Ferro = 1186,
                Oro = 1253,
                TempoReclutamento = 4666,
                Popolazione = 18
            };
            public static CostoReclutamento Guerriero_4 = new CostoReclutamento
            {
                Spade = 8,
                Lance = 0,
                Archi = 0,
                Scudi = 4,
                Armature = 8,

                Cibo = 1300,
                Legno = 1187,
                Pietra = 1159,
                Ferro = 1203,
                Oro = 1175,
                TempoReclutamento = 4800, //55
                Popolazione = 4
            };
            public static CostoReclutamento Lancere_4 = new CostoReclutamento
            {
                Spade = 0,
                Lance = 8,
                Archi = 0,
                Scudi = 8,
                Armature = 8,

                Cibo = 1451,
                Legno = 1300,
                Pietra = 1272,
                Ferro = 1329,
                Oro = 1299,
                TempoReclutamento = 5221,
                Popolazione = 4
            };
            public static CostoReclutamento Arcere_4 = new CostoReclutamento
            {
                Spade = 4,
                Lance = 0,
                Archi = 8,
                Scudi = 2,
                Armature = 8,

                Cibo = 1524,
                Legno = 1399,
                Pietra = 1358,
                Ferro = 1378,
                Oro = 1367,
                TempoReclutamento = 5643,
                Popolazione = 4
            };
            public static CostoReclutamento Catapulta_4 = new CostoReclutamento
            {
                Spade = 12,
                Lance = 12,
                Archi = 0,
                Scudi = 24,
                Armature = 24,

                Cibo = 1709,
                Legno = 1622,
                Pietra = 1654,
                Ferro = 1541,
                Oro = 1608,
                TempoReclutamento = 6066,
                Popolazione = 24
            };
            public static CostoReclutamento Guerriero_5 = new CostoReclutamento
            {
                Spade = 16,
                Lance = 0,
                Archi = 0,
                Scudi = 8,
                Armature = 16,

                Cibo = 1755,
                Legno = 1642,
                Pietra = 1614,
                Ferro = 1658,
                Oro = 1630,
                TempoReclutamento = 6900, //55
                Popolazione = 5
            };
            public static CostoReclutamento Lancere_5 = new CostoReclutamento
            {
                Spade = 0,
                Lance = 16,
                Archi = 0,
                Scudi = 16,
                Armature = 16,

                Cibo = 1906,
                Legno = 1755,
                Pietra = 1727,
                Ferro = 1784,
                Oro = 1754,
                TempoReclutamento = 7321,
                Popolazione = 5
            };
            public static CostoReclutamento Arcere_5 = new CostoReclutamento
            {
                Spade = 8,
                Lance = 0,
                Archi = 16,
                Scudi = 4,
                Armature = 16,

                Cibo = 1979,
                Legno = 1854,
                Pietra = 1813,
                Ferro = 1833,
                Oro = 1822,
                TempoReclutamento = 7743,
                Popolazione = 5
            };
            public static CostoReclutamento Catapulta_5 = new CostoReclutamento
            {
                Spade = 16,
                Lance = 16,
                Archi = 0,
                Scudi = 32,
                Armature = 32,

                Cibo = 2164,
                Legno = 2077,
                Pietra = 2109,
                Ferro = 1996,
                Oro = 2063,
                TempoReclutamento = 8166,
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
                Salario = 0.05, //Consumo al secondo
                Cibo = 0.08,    //Consumo al secondo
                Esperienza = 1, //Esperienza data in battaglia
                Trasporto = 100 //Capacità di trasporto
            };
            public static Unità Lancere_1 = new Unità
            {
                Salute = 7,
                Attacco = 4,
                Difesa = 6,
                Distanza = 2,
                Salario = 0.07,
                Cibo = 0.12,
                Esperienza = 1,
                Trasporto = 150
            };
            public static Unità Arcere_1 = new Unità
            {
                Salute = 6,
                Attacco = 6,
                Difesa = 3,
                Distanza = 6,
                Salario = 0.10,
                Cibo = 0.16,
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
                Salario = 0.15,
                Cibo = 0.22,
                Esperienza = 3,
                Componente_Lancio = 8,
                Trasporto = 250
            };

            public static Unità Guerriero_2 = new Unità
            {
                Salute = 8,
                Attacco = 6,
                Difesa = 6,
                Distanza = 1,
                Salario = 0.07,
                Cibo = 0.23,
                Esperienza = 1,
                Trasporto = 150
            };
            public static Unità Lancere_2 = new Unità
            {
                Salute = 9,
                Attacco = 6,
                Difesa = 7,
                Distanza = 2,
                Salario = 0.09,
                Cibo = 0.27,
                Esperienza = 1,
                Trasporto = 200
            };
            public static Unità Arcere_2 = new Unità
            {
                Salute = 8,
                Attacco = 8,
                Difesa = 4,
                Distanza = 6,
                Salario = 0.12,
                Cibo = 0.31,
                Esperienza = 2,
                Componente_Lancio = 8,
                Trasporto = 250
            };
            public static Unità Catapulta_2 = new Unità
            {
                Salute = 10,
                Attacco = 10,
                Difesa = 8,
                Distanza = 14,
                Salario = 0.17,
                Cibo = 0.37,
                Esperienza = 3,
                Componente_Lancio = 16,
                Trasporto = 300
            };

            public static Unità Guerriero_3 = new Unità
            {
                Salute = 10,
                Attacco = 8,
                Difesa = 7,
                Distanza = 1,
                Salario = 0.09,
                Cibo = 0.38,
                Esperienza = 2,
                Trasporto = 200
            };
            public static Unità Lancere_3 = new Unità
            {
                Salute = 11,
                Attacco = 8,
                Difesa = 8,
                Distanza = 2,
                Salario = 0.11,
                Cibo = 0.42,
                Esperienza = 2,
                Trasporto = 250
            };
            public static Unità Arcere_3 = new Unità
            {
                Salute = 10,
                Attacco = 10,
                Difesa = 5,
                Distanza = 6,
                Salario = 0.14,
                Cibo = 0.46,
                Esperienza = 3,
                Componente_Lancio = 12,
                Trasporto = 300
            };
            public static Unità Catapulta_3 = new Unità
            {
                Salute = 12,
                Attacco = 12,
                Difesa = 9,
                Distanza = 14,
                Salario = 0.19,
                Cibo = 0.52,
                Esperienza = 4,
                Componente_Lancio = 24,
                Trasporto = 350
            };

            public static Unità Guerriero_4 = new Unità
            {
                Salute = 12,
                Attacco = 10,
                Difesa = 8,
                Distanza = 1,
                Salario = 0.12,
                Cibo = 0.53,
                Esperienza = 2,
                Trasporto = 250
            };
            public static Unità Lancere_4 = new Unità
            {
                Salute = 13,
                Attacco = 10,
                Difesa = 9,
                Distanza = 2,
                Salario = 0.14,
                Cibo = 0.57,
                Esperienza = 2,
                Trasporto = 300
            };
            public static Unità Arcere_4 = new Unità
            {
                Salute = 12,
                Attacco = 12,
                Difesa = 6,
                Distanza = 6,
                Salario = 0.16,
                Cibo = 0.61,
                Esperienza = 3,
                Componente_Lancio = 16,
                Trasporto = 350
            };
            public static Unità Catapulta_4 = new Unità
            {
                Salute = 14,
                Attacco = 14,
                Difesa = 10,
                Distanza = 14,
                Salario = 0.21,
                Cibo = 0.67,
                Esperienza = 4,
                Componente_Lancio = 32,
                Trasporto = 400
            };

            public static Unità Guerriero_5 = new Unità
            {
                Salute = 14,
                Attacco = 12,
                Difesa = 9,
                Distanza = 1,
                Salario = 0.15,
                Cibo = 0.68,
                Esperienza = 3,
                Trasporto = 300
            };
            public static Unità Lancere_5 = new Unità
            {
                Salute = 15,
                Attacco = 12,
                Difesa = 10,
                Distanza = 2,
                Salario = 0.16,
                Cibo = 0.72,
                Esperienza = 3,
                Trasporto = 350
            };
            public static Unità Arcere_5 = new Unità
            {
                Salute = 14,
                Attacco = 14,
                Difesa = 7,
                Distanza = 6,
                Salario = 0.19,
                Cibo = 0.76,
                Esperienza = 4,
                Componente_Lancio = 20,
                Trasporto = 400
            };
            public static Unità Catapulta_5 = new Unità
            {
                Salute = 16,
                Attacco = 16,
                Difesa = 11,
                Distanza = 14,
                Salario = 0.24,
                Cibo = 0.82,
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
