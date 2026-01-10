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

                Cibo = 135,
                Legno = 87,
                Pietra = 64,
                Ferro = 118,
                Oro = 110,
                TempoReclutamento = 1600, //55
                Popolazione = 1
            };
            public static CostoReclutamento Lancere_1 = new CostoReclutamento
            {
                Spade = 0,
                Lance = 1,
                Archi = 0,
                Scudi = 1,
                Armature = 1,

                Cibo = 186,
                Legno = 135,
                Pietra = 107,
                Ferro = 164,
                Oro = 143,
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

                Cibo = 259,
                Legno = 234,
                Pietra = 193,
                Ferro = 213,
                Oro = 202,
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

                Cibo = 344,
                Legno = 357,
                Pietra = 389,
                Ferro = 276,
                Oro = 313,
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

                Cibo = 235,
                Legno = 187,
                Pietra = 164,
                Ferro = 218,
                Oro = 210,
                TempoReclutamento = 1900, //55
                Popolazione = 1
            };
            public static CostoReclutamento Lancere_2 = new CostoReclutamento
            {
                Spade = 0,
                Lance = 2,
                Archi = 0,
                Scudi = 2,
                Armature = 2,

                Cibo = 286,
                Legno = 235,
                Pietra = 207,
                Ferro = 264,
                Oro = 243,
                TempoReclutamento = 2321,
                Popolazione = 1
            };
            public static CostoReclutamento Arcere_2 = new CostoReclutamento
            {
                Spade = 1,
                Lance = 0,
                Archi = 2,
                Scudi = 0,
                Armature = 2,

                Cibo = 359,
                Legno = 334,
                Pietra = 293,
                Ferro = 313,
                Oro = 302,
                TempoReclutamento = 2743,
                Popolazione = 1
            };
            public static CostoReclutamento Catapulta_2 = new CostoReclutamento
            {
                Spade = 5,
                Lance = 5,
                Archi = 0,
                Scudi = 10,
                Armature = 10,

                Cibo = 444,
                Legno = 457,
                Pietra = 489,
                Ferro = 376,
                Oro = 413,
                TempoReclutamento = 3166,
                Popolazione = 6
            };
            public static CostoReclutamento Guerriero_3 = new CostoReclutamento
            {
                Spade = 4,
                Lance = 0,
                Archi = 0,
                Scudi = 2,
                Armature = 4,

                Cibo = 335,
                Legno = 287,
                Pietra = 264,
                Ferro = 318,
                Oro = 310,
                TempoReclutamento = 2500, //55
                Popolazione = 1
            };
            public static CostoReclutamento Lancere_3 = new CostoReclutamento
            {
                Spade = 0,
                Lance = 4,
                Archi = 0,
                Scudi = 4,
                Armature = 4,

                Cibo = 386,
                Legno = 335,
                Pietra = 307,
                Ferro = 364,
                Oro = 343,
                TempoReclutamento = 2921,
                Popolazione = 1
            };
            public static CostoReclutamento Arcere_3 = new CostoReclutamento
            {
                Spade = 2,
                Lance = 0,
                Archi = 4,
                Scudi = 1,
                Armature = 4,

                Cibo = 459,
                Legno = 434,
                Pietra = 393,
                Ferro = 413,
                Oro = 402,
                TempoReclutamento = 3343,
                Popolazione = 1
            };
            public static CostoReclutamento Catapulta_3 = new CostoReclutamento
            {
                Spade = 8,
                Lance = 8,
                Archi = 0,
                Scudi = 16,
                Armature = 16,

                Cibo = 544,
                Legno = 557,
                Pietra = 589,
                Ferro = 476,
                Oro = 513,
                TempoReclutamento = 3766,
                Popolazione = 6
            };
            public static CostoReclutamento Guerriero_4 = new CostoReclutamento
            {
                Spade = 8,
                Lance = 0,
                Archi = 0,
                Scudi = 4,
                Armature = 8,

                Cibo = 435,
                Legno = 387,
                Pietra = 364,
                Ferro = 418,
                Oro = 410,
                TempoReclutamento = 3400, //55
                Popolazione = 1
            };
            public static CostoReclutamento Lancere_4 = new CostoReclutamento
            {
                Spade = 0,
                Lance = 8,
                Archi = 0,
                Scudi = 8,
                Armature = 8,

                Cibo = 486,
                Legno = 435,
                Pietra = 407,
                Ferro = 464,
                Oro = 443,
                TempoReclutamento = 3821,
                Popolazione = 1
            };
            public static CostoReclutamento Arcere_4 = new CostoReclutamento
            {
                Spade = 4,
                Lance = 0,
                Archi = 8,
                Scudi = 2,
                Armature = 8,

                Cibo = 559,
                Legno = 534,
                Pietra = 493,
                Ferro = 513,
                Oro = 502,
                TempoReclutamento = 4243,
                Popolazione = 1
            };
            public static CostoReclutamento Catapulta_4 = new CostoReclutamento
            {
                Spade = 12,
                Lance = 12,
                Archi = 0,
                Scudi = 24,
                Armature = 24,

                Cibo = 644,
                Legno = 657,
                Pietra = 689,
                Ferro = 576,
                Oro = 613,
                TempoReclutamento = 4666,
                Popolazione = 6
            };
            public static CostoReclutamento Guerriero_5 = new CostoReclutamento
            {
                Spade = 16,
                Lance = 0,
                Archi = 0,
                Scudi = 8,
                Armature = 16,

                Cibo = 635,
                Legno = 587,
                Pietra = 564,
                Ferro = 618,
                Oro = 610,
                TempoReclutamento = 4600, //55
                Popolazione = 1
            };
            public static CostoReclutamento Lancere_5 = new CostoReclutamento
            {
                Spade = 0,
                Lance = 16,
                Archi = 0,
                Scudi = 16,
                Armature = 16,

                Cibo = 686,
                Legno = 635,
                Pietra = 607,
                Ferro = 664,
                Oro = 643,
                TempoReclutamento = 5021,
                Popolazione = 1
            };
            public static CostoReclutamento Arcere_5 = new CostoReclutamento
            {
                Spade = 8,
                Lance = 0,
                Archi = 16,
                Scudi = 4,
                Armature = 16,

                Cibo = 759,
                Legno = 734,
                Pietra = 693,
                Ferro = 713,
                Oro = 702,
                TempoReclutamento = 5443,
                Popolazione = 1
            };
            public static CostoReclutamento Catapulta_5 = new CostoReclutamento
            {
                Spade = 16,
                Lance = 16,
                Archi = 0,
                Scudi = 32,
                Armature = 32,

                Cibo = 844,
                Legno = 857,
                Pietra = 889,
                Ferro = 776,
                Oro = 813,
                TempoReclutamento = 5866,
                Popolazione = 6
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
                Distanza = 1,
                Salario = 0.13,
                Cibo = 0.24,
                Esperienza = 1,
                Trasporto = 100
            };
            public static Unità Lancere_1 = new Unità
            {
                Salute = 7,
                Attacco = 4,
                Difesa = 6,
                Distanza = 2,
                Salario = 0.17,
                Cibo = 0.31,
                Esperienza = 1,
                Trasporto = 150
            };
            public static Unità Arcere_1 = new Unità
            {
                Salute = 6,
                Attacco = 6,
                Difesa = 3,
                Distanza = 6,
                Salario = 0.22,
                Cibo = 0.38,
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
                Salario = 0.40,
                Cibo = 0.64,
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
    }
}
