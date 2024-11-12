using PlateauNamespace;
using DictionaireNamespace;
using DiceNamespace;
using TreeNamespace;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using JeuNamespace;

namespace ProgramNamespace
{
    public class Program{
        public static string LoadFile(string filename){
            string contents = File.ReadAllText(".\\ProjetAlgoS3\\files\\"+filename);
            return contents;
        }

        public static string[] LoadWordsFromFile(string fileName){
            string[] toReturn={};

            return toReturn;
        }

        public static int[] LoadLettersProbas(string[] lettersContentArray){
            int[] probas=new int[lettersContentArray.Length];
            for(int i=0;i<lettersContentArray.Length;i++){
                string[] lineArray=lettersContentArray[i].Split(';');
                int proba=int.Parse(lineArray[2]);
                probas[i]=proba;
            }
            return probas;
        }

        public static char[] LoadLettersAlphabet(string[] lettersContentArray){
            char[] lettersAlphabet=new char[lettersContentArray.Length];
            for(int i=0;i<lettersContentArray.Length;i++){
                string[] lineArray=lettersContentArray[i].Split(';');
                char character=char.Parse(lineArray[0]);
                lettersAlphabet[i]=character;
            }
            return lettersAlphabet;
        }

        public static Dictionary<char,int> LoadLettersScore(string[] lettersContentArray){
            Dictionary <char,int> letterScores=new Dictionary<char, int>();
            
            for(int i=0;i<lettersContentArray.Length;i++){
                string[] lineArray=lettersContentArray[i].Split(';');
                string letter=lineArray[0];
                int score=int.Parse(lineArray[1]);
                letterScores.Add(letter[0],score);
            }
            return letterScores;
        }


        public static string AskLanguage(){
            string language="LANGUAGE";
            while(language!="FR" && language!="EN"){
                Console.Write("Language (FR/EN): ");
                language=Console.ReadLine();
            }
            return language;
        }

        public static void Main(string[] args)
        {
            const bool testMode = true;
            // config of the game
            string language="FR";
            if(testMode==false){
                language=AskLanguage();
            }
            Dictionaire dico=new Dictionaire(LoadFile(language+".txt").Split(' '));
            Tree mainTree=new Tree();
            for(int i=0;i<dico.Words.Length;i++){
                mainTree.AddWord(dico.Words[i]);
            }

            string[] lettersContentArray = LoadFile("lettres.txt").Split('\n');
            char[] lettersAlphabet= LoadLettersAlphabet(lettersContentArray);
            Dictionary <char,int> letterScores=LoadLettersScore(lettersContentArray);
            int[] lettersProbas=LoadLettersProbas(lettersContentArray);
            
            Jeu game=new Jeu(lettersAlphabet,letterScores,lettersProbas,mainTree,testMode);
        }

    }

}