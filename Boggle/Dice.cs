using System;

namespace DiceNamespace
{
    public class Dice
    {
        char letter;
        char[] alphabet;/// créé un tableau de caractères contenant toutes les lettres de l'alphabet.
        char[] facesLetters = new char[6];/// créé un tableau de caractères dont les éléments correspondent aux valeurs des 6 faces du dé.
        Random rnd;
        int[] lettersProbas;/// créé un tableau d'entiers correspondants aux fréquences d'apparition respectives de chaque lettre de l'alphabet.
        public char Letter
        {
            get
            {
                return this.letter;
            }
        }

        public int ladderValueToIndex(int value)/// associe à une valeur "value" en entrée, un index correspondant à une lettre, suivant le respect des probabilité associées à chaque lettre
        {
            int index = 0;
            int actualLadderValue = 0;
            while (actualLadderValue + this.lettersProbas[index] < value)
            {
                actualLadderValue += this.lettersProbas[index];/// ajoute la probabilité de la lettre suivante tant que la valeur "value" n'est pas atteinte
                index++;
            }
            return index;
        }

        int sumOfIntArray(int[] array)/// utilisé pour effectuer la somme des probas de chaque lettre de l'alphabet.
        {
            int total = 0;
            for (int i = 0; i < array.Length; i++)
            {
                total += array[i];/// implémente total avec chaque entier du tableau array.
            }
            return total;
        }
        public Dice(Random rnd, char[] alphabet, int[] lettersProbas)
        {
            this.rnd = rnd;
            this.alphabet = alphabet;
            this.lettersProbas = lettersProbas;
            for (int i = 0; i < 6; i++)
            {
                this.facesLetters[i] = this.alphabet[ladderValueToIndex(this.rnd.Next(0, sumOfIntArray(this.lettersProbas)))];
            }
        }

        public override string ToString()
        {
            return this.Letter.ToString();
        }

        public char Roll()
        {
            this.letter = this.facesLetters[this.rnd.Next(0, 6)];
            return this.Letter;
        }
    }

}