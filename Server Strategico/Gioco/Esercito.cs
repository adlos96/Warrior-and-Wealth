namespace Server_Strategico.Gioco
{
    public class Esercito
    {
        public class CostoReclutamento
        {
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
            public double TempoReclutamento { get; set; }
            public double Popolazione { get; set; }

            // Costruttore per inizializzare i costi
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
                TempoReclutamento = 5, //55
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
                TempoReclutamento = 109,
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
                TempoReclutamento = 127,
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
                TempoReclutamento = 159,
                Popolazione = 6
            };
            public static CostoReclutamento Guerriero_2 = new CostoReclutamento
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
                TempoReclutamento = 5, //55
                Popolazione = 1
            };
            public static CostoReclutamento Lancere_2 = new CostoReclutamento
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
                TempoReclutamento = 109,
                Popolazione = 1
            };
            public static CostoReclutamento Arcere_2 = new CostoReclutamento
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
                TempoReclutamento = 127,
                Popolazione = 1
            };
            public static CostoReclutamento Catapulta_2 = new CostoReclutamento
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
                TempoReclutamento = 159,
                Popolazione = 6
            };
            public static CostoReclutamento Guerriero_3 = new CostoReclutamento
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
                TempoReclutamento = 5, //55
                Popolazione = 1
            };
            public static CostoReclutamento Lancere_3 = new CostoReclutamento
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
                TempoReclutamento = 109,
                Popolazione = 1
            };
            public static CostoReclutamento Arcere_3 = new CostoReclutamento
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
                TempoReclutamento = 127,
                Popolazione = 1
            };
            public static CostoReclutamento Catapulta_3 = new CostoReclutamento
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
                TempoReclutamento = 159,
                Popolazione = 6
            };
            public static CostoReclutamento Guerriero_4 = new CostoReclutamento
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
                TempoReclutamento = 5, //55
                Popolazione = 1
            };
            public static CostoReclutamento Lancere_4 = new CostoReclutamento
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
                TempoReclutamento = 109,
                Popolazione = 1
            };
            public static CostoReclutamento Arcere_4 = new CostoReclutamento
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
                TempoReclutamento = 127,
                Popolazione = 1
            };
            public static CostoReclutamento Catapulta_4 = new CostoReclutamento
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
                TempoReclutamento = 159,
                Popolazione = 6
            };
            public static CostoReclutamento Guerriero_5 = new CostoReclutamento
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
                TempoReclutamento = 5, //55
                Popolazione = 1
            };
            public static CostoReclutamento Lancere_5 = new CostoReclutamento
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
                TempoReclutamento = 109,
                Popolazione = 1
            };
            public static CostoReclutamento Arcere_5 = new CostoReclutamento
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
                TempoReclutamento = 127,
                Popolazione = 1
            };
            public static CostoReclutamento Catapulta_5 = new CostoReclutamento
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
                TempoReclutamento = 159,
                Popolazione = 6
            };

        }
        public class EsercitoNemico
        {
            public static Unità Guerrieri_1 = new Unità
            {
                Salute = 6,
                Attacco = 3,
                Difesa = 3,
                Distanza = 1,
                Quantità = 0,
                TempoReclutamento = 95,
                Esperienza = 1
            };
            public static Unità Lanceri_1 = new Unità
            {
                Salute = 7,
                Attacco = 4,
                Difesa = 4,
                Distanza = 2,
                Quantità = 0,
                TempoReclutamento = 109,
                Esperienza = 1
            };
            public static Unità Arceri_1 = new Unità
            {
                Salute = 5,
                Attacco = 6,
                Difesa = 2,
                Distanza = 6,
                Quantità = 0,
                TempoReclutamento = 127,
                Esperienza = 2
            };
            public static Unità Catapulte_1 = new Unità
            {
                Salute = 11,
                Attacco = 12,
                Difesa = 13,
                Distanza = 14,
                Quantità = 0,
                TempoReclutamento = 159,
                Esperienza = 3
            };

            public static Unità Guerrieri_2 = new Unità
            {
                Salute = 6,
                Attacco = 3,
                Difesa = 3,
                Distanza = 1,
                Quantità = 0,
                TempoReclutamento = 95,
                Esperienza = 1
            };
            public static Unità Lanceri_2 = new Unità
            {
                Salute = 7,
                Attacco = 4,
                Difesa = 4,
                Distanza = 2,
                Quantità = 0,
                TempoReclutamento = 109,
                Esperienza = 1
            };
            public static Unità Arceri_2 = new Unità
            {
                Salute = 5,
                Attacco = 6,
                Difesa = 2,
                Distanza = 6,
                Quantità = 0,
                TempoReclutamento = 127,
                Esperienza = 2
            };
            public static Unità Catapulte_2 = new Unità
            {
                Salute = 11,
                Attacco = 12,
                Difesa = 13,
                Distanza = 14,
                Quantità = 0,
                TempoReclutamento = 159,
                Esperienza = 3
            };

            public static Unità Guerrieri_3 = new Unità
            {
                Salute = 6,
                Attacco = 3,
                Difesa = 3,
                Distanza = 1,
                Quantità = 0,
                TempoReclutamento = 95,
                Esperienza = 1
            };
            public static Unità Lanceri_3 = new Unità
            {
                Salute = 7,
                Attacco = 4,
                Difesa = 4,
                Distanza = 2,
                Quantità = 0,
                TempoReclutamento = 109,
                Esperienza = 1
            };
            public static Unità Arceri_3 = new Unità
            {
                Salute = 5,
                Attacco = 6,
                Difesa = 2,
                Distanza = 6,
                Quantità = 0,
                TempoReclutamento = 127,
                Esperienza = 2
            };
            public static Unità Catapulte_3 = new Unità
            {
                Salute = 11,
                Attacco = 12,
                Difesa = 13,
                Distanza = 14,
                Quantità = 0,
                TempoReclutamento = 159,
                Esperienza = 3
            };

            public static Unità Guerrieri_4 = new Unità
            {
                Salute = 6,
                Attacco = 3,
                Difesa = 3,
                Distanza = 1,
                Quantità = 0,
                TempoReclutamento = 95,
                Esperienza = 1
            };
            public static Unità Lanceri_4 = new Unità
            {
                Salute = 7,
                Attacco = 4,
                Difesa = 4,
                Distanza = 2,
                Quantità = 0,
                TempoReclutamento = 109,
                Esperienza = 1
            };
            public static Unità Arceri_4 = new Unità
            {
                Salute = 5,
                Attacco = 6,
                Difesa = 2,
                Distanza = 6,
                Quantità = 0,
                TempoReclutamento = 127,
                Esperienza = 2
            };
            public static Unità Catapulte_4 = new Unità
            {
                Salute = 11,
                Attacco = 12,
                Difesa = 13,
                Distanza = 14,
                Quantità = 0,
                TempoReclutamento = 159,
                Esperienza = 3
            };

            public static Unità Guerrieri_5 = new Unità
            {
                Salute = 6,
                Attacco = 3,
                Difesa = 3,
                Distanza = 1,
                Quantità = 0,
                TempoReclutamento = 95,
                Esperienza = 1
            };
            public static Unità Lanceri_5 = new Unità
            {
                Salute = 7,
                Attacco = 4,
                Difesa = 4,
                Distanza = 2,
                Quantità = 0,
                TempoReclutamento = 109,
                Esperienza = 1
            };
            public static Unità Arceri_5 = new Unità
            {
                Salute = 5,
                Attacco = 6,
                Difesa = 2,
                Distanza = 6,
                Quantità = 0,
                TempoReclutamento = 127,
                Esperienza = 2
            };
            public static Unità Catapulte_5 = new Unità
            {
                Salute = 11,
                Attacco = 12,
                Difesa = 13,
                Distanza = 14,
                Quantità = 0,
                TempoReclutamento = 159,
                Esperienza = 3
            };
        }
        public class Unità
        {
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

            public static Unità Guerriero_1 = new Unità
            {
                Salute = 5,
                Attacco = 3,
                Difesa = 3,
                Distanza = 1,
                Salario = 0.15,
                Cibo = 0.31,
                Quantità = 0,
                Esperienza = 1,
                Trasporto = 100
            };
            public static Unità Lancere_1 = new Unità
            {
                Salute = 6,
                Attacco = 4,
                Difesa = 4,
                Distanza = 2,
                Salario = 0.19,
                Cibo = 0.36,
                Quantità = 0,
                Esperienza = 1,
                Trasporto = 150
            };
            public static Unità Arcere_1 = new Unità
            {
                Salute = 4,
                Attacco = 7,
                Difesa = 3,
                Distanza = 6,
                Salario = 0.24,
                Cibo = 0.43,
                Quantità = 0,
                Esperienza = 2,
                Componente_Lancio = 4,
                Trasporto = 200
            };
            public static Unità Catapulta_1 = new Unità
            {
                Salute = (int)(Guerriero_1.Salute * 0.60 * CostoReclutamento.Catapulta_1.Popolazione),
                Attacco = 12,
                Difesa = (int)(Guerriero_1.Difesa * 0.60 * CostoReclutamento.Catapulta_1.Popolazione),
                Distanza = 14,
                Salario = CostoReclutamento.Catapulta_1.Popolazione * Guerriero_1.Salario * 0.629,
                Cibo = CostoReclutamento.Catapulta_1.Popolazione * Guerriero_1.Cibo * 0.779,
                Quantità = 0,
                Esperienza = 3,
                Componente_Lancio = 8,
                Trasporto = 300
            };

            public static Unità Guerriero_2 = new Unità
            {
                Salute = 5,
                Attacco = 3,
                Difesa = 3,
                Distanza = 1,
                Salario = 0.15,
                Cibo = 0.31,
                Quantità = 0,
                Esperienza = 1,
                Trasporto = 100
            };
            public static Unità Lancere_2 = new Unità
            {
                Salute = 6,
                Attacco = 4,
                Difesa = 4,
                Distanza = 2,
                Salario = 0.19,
                Cibo = 0.36,
                Quantità = 0,
                Esperienza = 1,
                Trasporto = 150
            };
            public static Unità Arcere_2 = new Unità
            {
                Salute = 4,
                Attacco = 7,
                Difesa = 3,
                Distanza = 6,
                Salario = 0.24,
                Cibo = 0.43,
                Quantità = 0,
                Esperienza = 2,
                Componente_Lancio = 4,
                Trasporto = 200
            };
            public static Unità Catapulta_2 = new Unità
            {
                Salute = (int)(Guerriero_2.Salute * 0.60 * CostoReclutamento.Catapulta_2.Popolazione),
                Attacco = 12,
                Difesa = (int)(Guerriero_2.Difesa * 0.60 * CostoReclutamento.Catapulta_2.Popolazione),
                Distanza = 14,
                Salario = CostoReclutamento.Catapulta_2.Popolazione * Guerriero_2.Salario * 0.629,
                Cibo = CostoReclutamento.Catapulta_2.Popolazione * Guerriero_2.Cibo * 0.779,
                Quantità = 0,
                Esperienza = 3,
                Componente_Lancio = 8,
                Trasporto = 300
            };

            public static Unità Guerriero_3 = new Unità
            {
                Salute = 5,
                Attacco = 3,
                Difesa = 3,
                Distanza = 1,
                Salario = 0.15,
                Cibo = 0.31,
                Quantità = 0,
                Esperienza = 1,
                Trasporto = 100
            };
            public static Unità Lancere_3 = new Unità
            {
                Salute = 6,
                Attacco = 4,
                Difesa = 4,
                Distanza = 2,
                Salario = 0.19,
                Cibo = 0.36,
                Quantità = 0,
                Esperienza = 1,
                Trasporto = 150
            };
            public static Unità Arcere_3 = new Unità
            {
                Salute = 4,
                Attacco = 7,
                Difesa = 3,
                Distanza = 6,
                Salario = 0.24,
                Cibo = 0.43,
                Quantità = 0,
                Esperienza = 2,
                Componente_Lancio = 4,
                Trasporto = 200
            };
            public static Unità Catapulta_3 = new Unità
            {
                Salute = (int)(Guerriero_3.Salute * 0.60 * CostoReclutamento.Catapulta_3.Popolazione),
                Attacco = 12,
                Difesa = (int)(Guerriero_3.Difesa * 0.60 * CostoReclutamento.Catapulta_3.Popolazione),
                Distanza = 14,
                Salario = CostoReclutamento.Catapulta_3.Popolazione * Guerriero_3.Salario * 0.629,
                Cibo = CostoReclutamento.Catapulta_3.Popolazione * Guerriero_3.Cibo * 0.779,
                Quantità = 0,
                Esperienza = 3,
                Componente_Lancio = 8,
                Trasporto = 300
            };

            public static Unità Guerriero_4 = new Unità
            {
                Salute = 5,
                Attacco = 3,
                Difesa = 3,
                Distanza = 1,
                Salario = 0.15,
                Cibo = 0.31,
                Quantità = 0,
                Esperienza = 1,
                Trasporto = 100
            };
            public static Unità Lancere_4 = new Unità
            {
                Salute = 6,
                Attacco = 4,
                Difesa = 4,
                Distanza = 2,
                Salario = 0.19,
                Cibo = 0.36,
                Quantità = 0,
                Esperienza = 1,
                Trasporto = 150
            };
            public static Unità Arcere_4 = new Unità
            {
                Salute = 4,
                Attacco = 7,
                Difesa = 3,
                Distanza = 6,
                Salario = 0.24,
                Cibo = 0.43,
                Quantità = 0,
                Esperienza = 2,
                Componente_Lancio = 4,
                Trasporto = 200
            };
            public static Unità Catapulta_4 = new Unità
            {
                Salute = (int)(Guerriero_4.Salute * 0.60 * CostoReclutamento.Catapulta_4.Popolazione),
                Attacco = 12,
                Difesa = (int)(Guerriero_4.Difesa * 0.60 * CostoReclutamento.Catapulta_4.Popolazione),
                Distanza = 14,
                Salario = CostoReclutamento.Catapulta_4.Popolazione * Guerriero_4.Salario * 0.629,
                Cibo = CostoReclutamento.Catapulta_4.Popolazione * Guerriero_4.Cibo * 0.779,
                Quantità = 0,
                Esperienza = 3,
                Componente_Lancio = 8,
                Trasporto = 300
            };

            public static Unità Guerriero_5 = new Unità
            {
                Salute = 5,
                Attacco = 3,
                Difesa = 3,
                Distanza = 1,
                Salario = 0.15,
                Cibo = 0.31,
                Quantità = 0,
                Esperienza = 1,
                Trasporto = 100
            };
            public static Unità Lancere_5 = new Unità
            {
                Salute = 6,
                Attacco = 4,
                Difesa = 4,
                Distanza = 2,
                Salario = 0.19,
                Cibo = 0.36,
                Quantità = 0,
                Esperienza = 1,
                Trasporto = 150
            };
            public static Unità Arcere_5 = new Unità
            {
                Salute = 4,
                Attacco = 7,
                Difesa = 3,
                Distanza = 6,
                Salario = 0.24,
                Cibo = 0.43,
                Quantità = 0,
                Esperienza = 2,
                Componente_Lancio = 4,
                Trasporto = 200
            };
            public static Unità Catapulta_5 = new Unità
            {
                Salute = (int)(Guerriero_5.Salute * 0.60 * CostoReclutamento.Catapulta_5.Popolazione),
                Attacco = 12,
                Difesa = (int)(Guerriero_5.Difesa * 0.60 * CostoReclutamento.Catapulta_5.Popolazione),
                Distanza = 14,
                Salario = CostoReclutamento.Catapulta_5.Popolazione * Guerriero_5.Salario * 0.629,
                Cibo = CostoReclutamento.Catapulta_5.Popolazione * Guerriero_5.Cibo * 0.779,
                Quantità = 0,
                Esperienza = 3,
                Componente_Lancio = 8,
                Trasporto = 300
            };
            public static Unità Saccheggiatore = new Unità
            {
                Salute = 2,
                Attacco = 0,
                Difesa = 0,
                Distanza = 0,
                Salario = 0.18,
                Cibo = 0.23,
                Quantità = 0,
                Esperienza = 1,
                Trasporto = 500
            };
            public static Unità Carretto = new Unità
            {
                Salute = 5,
                Attacco = 0,
                Difesa = 2,
                Distanza = 0,
                Salario = 0.26,
                Cibo = 0.29,
                Quantità = 0,
                Esperienza = 2,
                Trasporto = 1500
            };
        }
    }
}
