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

        /// <summary>
        /// Constructeur de la classe Plateau.
        /// </summary>
        /// <param name="letters">Alphabet</param>
        /// <param name="lettersProbas">Probabilités d'apparition des lettres</param>
        /// <param name="lettersScores">Score associé à chaque lettre</param>
        /// <param name="size">Taille du plateau</param>
        public Plateau(int size, char[] letters, Dictionary<char, int> lettersScores, int[] lettersProbas)
        {
            this.size = size;/// initialise la taille du plateau
            this.rnd = new Random();
            this.lettersProbas = lettersProbas;
            this.alphabet = letters;
            this.dices = new Dice[size * size];
            for (int i = 0; i < size * size; i++)/// créé les dés du plateau
            {
                this.dices[i] = new Dice(rnd, this.alphabet, this.lettersProbas);
            }
            this.RollAllDices();///initialise chaque dé du plateau
        }

        /// <summary>
        /// Lance tous les dés du plateau.
        /// </summary>
        public void RollAllDices()
        {
            for (int i = 0; i < size * size; i++)
            {
                this.dices[i].Roll();
            }
        }

        /// <summary>
        /// représente le plateau sous forme de string
        /// </summary>
        public override string ToString()
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