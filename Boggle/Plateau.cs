using System;
using System.Collections.Generic;
using DiceNamespace;

namespace PlateauNamespace
{

    public class Plateau
    {
        Dice[] dices;
        int size;
        int[] lettersProbas;
        char[] alphabet;
        public Random rnd;
        public Dice[] Dices
        {
            get
            {
                return this.dices;
            }
        }
        public Plateau(int size, char[] letters, Dictionary<char, int> lettersScores, int[] lettersProbas, bool testMode)
        {
            this.size = size;/// initialise la taille du plateau
            if (testMode == false)
            {
                this.rnd = new Random();
            }
            else
            {
                this.rnd = new Random(1234);
            }

            this.lettersProbas = lettersProbas;
            this.alphabet = letters;
            this.dices = new Dice[size * size];
            for (int i = 0; i < size * size; i++)/// créé les dés du plateau
            {
                this.dices[i] = new Dice(rnd, this.alphabet, this.lettersProbas);
            }
            this.RollAllDices();///initialise chaque dé du plateau
        }

        public void RollAllDices()
        {
            for (int i = 0; i < size * size; i++)
            {
                this.dices[i].Roll();
            }
        }

        public override string ToString()/// représente le plateau sous forme de string
        {
            string toReturn = "";/// créé la string à retourner
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    toReturn += this.dices[i * this.size + j] + " ";/// rajoute une par une les valeurs de chaque dé du plateau à la chaine de caractères
                }
                toReturn += "\n";/// retourne à la ligne pour donner à la chaine de caractères la forme du plateau
            }
            return toReturn;
        }
    }

}