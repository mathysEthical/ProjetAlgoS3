using DictionnaireNamespace;
using TreeNamespace;
using System;
using System.IO;
using System.Collections.Generic;
using JeuNamespace;

namespace ProgramNamespace
{
    public class Program
    {
        /// <summary>
        /// parcours le contenu d'un fichier et le retourne
        /// <param name="filename">Nom du fichier à lire</param>
        /// </summary>
        public static string LoadFile(string filename)
        {
            string contents = "";
            try
            {

                string BaseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                string filePath = BaseDirectory + "..\\..\\..\\files\\" + filename;
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    // Read line by line
                    bool isFirst = true;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (isFirst==false)
                        {
                            contents += "\n";
                        }
                        contents += line;
                        isFirst = false;
                    }

                }
                return contents;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "error";
            }
        }

        /// <summary>
        /// Extrait les probas du fichier lettres
        /// <param name="lettersContentArray">Tableau contenu dans le fichier lettres</param>
        /// </summary>
        public static int[] LoadLettersProbas(string[] lettersContentArray)
        {
            int[] probas = new int[lettersContentArray.Length];
            for (int i = 0; i < lettersContentArray.Length; i++)
            {
                string[] lineArray = lettersContentArray[i].Split(';');
                int proba = int.Parse(lineArray[2]);
                probas[i] = proba;
            }
            return probas;
        }

        /// <summary>
        /// constructeur de la classe Dictionnaire
        /// <param name="lettersContentArray">Tableau contenu dans le fichier lettres</param>
        /// </summary>
        public static Dictionary<char, int> LoadLettersScore(string[] lettersContentArray)
        {
            Dictionary<char, int> letterScores = new Dictionary<char, int>();

            for (int i = 0; i < lettersContentArray.Length; i++)
            {
                string[] lineArray = lettersContentArray[i].Split(';');
                string letter = lineArray[0];
                int score = int.Parse(lineArray[1]);
                letterScores.Add(letter[0], score);
            }
            return letterScores;
        }

        /// <summary>
        /// demande la langue de jeu
        /// </summary>
        public static string AskLanguage()
        {
            string language = "LANGUAGE";
            while (language != "FR" && language != "EN")
            {
                Console.Write("Language (FR/EN): ");
                language = Console.ReadLine();
            }
            return language;
        }

        /// <summary>
        /// lance et configure le jeu
        /// </summary>
        public static void Main(string[] args)
        {
            /// config of the game
            string language = "FR";
            language = AskLanguage();
            Dictionnaire dico = new Dictionnaire(LoadFile(language + ".txt").Split(' '));
            Tree mainTree = new Tree();
            for (int i = 0; i < dico.Words.Length; i++)
            {
                mainTree.AddWord(dico.Words[i]);
            }

            string[] lettersContentArray = LoadFile("lettres.txt").Split('\n');
            char[] lettersAlphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            Dictionary<char, int> letterScores = LoadLettersScore(lettersContentArray);
            int[] lettersProbas = LoadLettersProbas(lettersContentArray);

            Jeu game = new Jeu(lettersAlphabet, letterScores, lettersProbas, mainTree);
        }

    }

}