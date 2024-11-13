using DictionaireNamespace;
using TreeNamespace;
using ProgramNamespace;
using PlateauNamespace;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Collections;
using System.Resources;

namespace JeuNamespace
{
    public class Jeu{
        Plateau board;
        string playerName1="Paul";
        string playerName2="IA_noob";
        char[] lettersAlphabet;
        int [] lettersProbas;
        int gameTime=4;
        int actualRound=0;
        int scorePlayer1;
        int scorePlayer2;
        List<string> currentWords;
        Dictionary<char,int> lettersScores;
        Tree tree;
        int size=4;
        bool testMode;

        public static int AskSize(){
            int max=20;
            int size=max+1;
            while(size>max || size<1){
                Console.Write("Taille pour du plateau (max 20): ");
                int.TryParse(Console.ReadLine(), out size);
            }
            return size;
        }

        public static int AskTime(){
            int max=60;
            int size=max+1;
            while(size>max || size<1 || size%2!=0){
                Console.Write("Durée de la partie (en minutes, doit être pair): ");
                int.TryParse(Console.ReadLine(), out size);
            }
            return size;
        }

        public int coordsToIndex(int x, int y){
            return y*this.size+x;
        }

        public int[] indexToCoords(int index){
            int[] coords=new int[2];
            coords[0]=index%this.size;
            coords[1]=index/this.size;
            return coords;
        }

        public string[] Dig(string actualSpelling,int actualPos,List<int> usedDicesIndex, List<string> foundWords){
            if(this.tree.Contains(actualSpelling) && !foundWords.Contains(actualSpelling)){
                foundWords.Add(actualSpelling);
            }

            if(this.tree.anyStartingWith(actualSpelling)==false){
                return foundWords.ToArray();
            }
            List<string> wordList=new List<string>();
            int[] coords=indexToCoords(actualPos);
            int x=coords[0];
            int y=coords[1];
            for(int dy=-1;dy<=1;dy++){
                for(int dx=-1;dx<=1;dx++){
                    if((dx!=0 || dy!=0) && x+dx>=0 && x+dx<this.size && y+dy>=0 && y+dy<this.size){
                        int voisin=coordsToIndex(x+dx,y+dy);
                        //pour chaque voisin de la case actuelle
                        if(usedDicesIndex.Contains(voisin)==false){
                            //le dé voisin n'a pas été utilisé
                            usedDicesIndex.Add(voisin);
                            string[] newWords=Dig(actualSpelling+this.board.Dices[voisin].Letter.ToString(),voisin,usedDicesIndex,foundWords);
                            for(int w=0;w<newWords.Length;w++){
                                if(wordList.Contains(newWords[w])==false){
                                    wordList.Add(newWords[w]);
                                }
                            }
                            usedDicesIndex.Remove(voisin);
                        }
                    }
                }
            }
            return wordList.ToArray();
        }

        public string[] findAllWords(){
            List<string> wordsList=new List<string>();
            for(int i=0;i<this.size*this.size;i++){
                // i is the starting dice index in the board
                List<int> usedDicesIndex=new List<int>();
                usedDicesIndex.Add(i);
                List<string> foundWords=new List<string>();
                string[] allPosWords=Dig(this.board.Dices[i].Letter.ToString(),i,usedDicesIndex,foundWords);
                for(int w=0;w<allPosWords.Length;w++){
                    if(wordsList.Contains(allPosWords[w])==false){
                        wordsList.Add(allPosWords[w]);
                    }
                }
            }
            return wordsList.ToArray();
            
        }

        public string AskWord(){
            Console.Write("Entrez un mot trouvé: ");
            string word=Console.ReadLine();
            return word.ToUpper();
        }

        public string VerifWord(string word,DateTime start, string[] allWords,string actualPlayer) 
        {
            string affichage="";
            if (this.tree.Contains(word))
            {
                if (allWords.Contains(word))
                {
                    if (this.currentWords.Contains(word) == false)
                    {
                        this.currentWords.Add(word);
                        if ((DateTime.Now - start).Minutes == 0)
                        {
                            affichage = $"Mot {word} valide ! {scoreFromWord(word)} points";
                            if (actualPlayer == this.playerName1)
                            {
                                this.scorePlayer1 += scoreFromWord(word);
                            }
                            else
                            {
                                this.scorePlayer2 += scoreFromWord(word);
                            }
                        }
                        else
                        {
                            affichage = "Temps écoulé avant soumission du mot.";
                        }
                    }
                    else
                    {
                        affichage = $"Mot {word} déjà accepté.";
                    }
                }
                else
                {
                    affichage =$"Mot {word} non présent sur le plateau.";
                }
            }
            else 
            {
                affichage = $"Mot {word} non présent dans le dictionnaire.";
            }
            if ((DateTime.Now - start).Minutes == 0)
            {
                affichage += $"\nIl vous reste {(60 - (DateTime.Now - start).Seconds).ToString()} secondes";
            }
            Console.Clear();

            return affichage;
        }
        public int scoreFromWord(string word){
            int scoreToReturn=0;
            for(int i=0;i<word.Length;i++){
                char c = word[i];
                scoreToReturn+=this.lettersScores[c];
            }
            return scoreToReturn;
        }


        public void NextRound() {
            actualRound++;
            string actualPlayer = this.playerName1;
            if (actualRound % 2 == 0) {
                actualPlayer = this.playerName2;
            }
            Console.WriteLine("C'est au tour de " + actualPlayer);
            Console.WriteLine("Génération du plateau...");
            this.board = new Plateau(this.size, this.lettersAlphabet, this.lettersScores, this.lettersProbas, this.testMode);
            string[] allWords;
            if (testMode) {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                allWords = findAllWords();
                sw.Stop();
                Console.WriteLine(sw.Elapsed);
            } else {
                allWords = findAllWords();
            }
            this.currentWords = new List<string>();
            DateTime start = DateTime.Now;
            bool pasDeReecriture = false;
            // for(int w=0;w<allWords.Length;w++){
            //     Console.WriteLine("Mot possible: "+allWords[w]);
            // }
            while ((DateTime.Now - start).Minutes == 0)
            {
                Console.WriteLine("Voici le plateau:");
                Console.WriteLine(this.board);

                if (actualPlayer == "IA_noob") 
                {
                    if (!pasDeReecriture)
                    {
                        Console.Write("Entrez un mot trouvé: ");
                    }
                    pasDeReecriture = false ;
                    Random probas = new Random();
                    Random temps = new Random();
                    int nombre = probas.Next(1,7);
                    int idx = 0;
                    if (nombre <= 3)
                    {
                        Thread.Sleep(10000 + temps.Next(0,2000));//Attend entre 10 et 12 secondes
                        while (allWords[idx].Length > this.size + 1 || this.currentWords.Contains(allWords[idx]))
                        {
                            idx++;
                            
                        }
                        string word = allWords[idx];
                        Console.WriteLine(VerifWord(word, start, allWords, actualPlayer));

                    }
                    else
                    {
                        switch (nombre)
                        {
                            case 4:
                                Thread.Sleep(6000);//Attend 6 secondes
                                pasDeReecriture = true;
                                break;
                            case 5:
                                Thread.Sleep(4000);//Attend 4 secondes
                                if (currentWords.Count != 0) 
                                {
                                    Console.WriteLine(currentWords.Last());
                                    Console.WriteLine(VerifWord(currentWords.Last(), start, allWords, actualPlayer));

                                }
                                else 
                                {
                                    pasDeReecriture=true;
                                }
                                break;
                            case 6:
                                Thread.Sleep(8000);//Attend 8 secondes
                                while (allWords[idx].Length > 3 || this.currentWords.Contains(allWords[idx]))
                                {
                                    idx++;
                                    if (idx >= allWords.Length) 
                                    {
                                        break;
                                    }//Si aucun mot ne satisfie les condition on sort de la boucle

                                }
                                string word = allWords[idx];
                                Console.WriteLine(word);
                                Console.WriteLine(VerifWord(word, start, allWords, actualPlayer));
                                break;
                        }
                    }

                }
                else if(actualPlayer == "IA_moy") 
                {
                    if (!pasDeReecriture)
                    {
                        Console.Write("Entrez un mot trouvé: ");
                    }
                    pasDeReecriture = false;
                    Random probas = new Random();
                    Random temps = new Random();
                    int nombre = probas.Next(1, 7);
                    int idx = 0;
                    if (nombre <= 4)
                    {
                        Thread.Sleep(5000 + temps.Next(-2000, 2000));//Attend entre 3 et 7 secondes
                        while (allWords[idx].Length > this.size + 2 || this.currentWords.Contains(allWords[idx]))
                        {
                            idx++;

                        }
                        string word = allWords[idx];
                        Console.WriteLine(word);
                        Console.WriteLine(VerifWord(word, start, allWords, actualPlayer));
                    }
                    else
                    {
                        switch (nombre)
                        {
                            case 5:
                                Thread.Sleep(4000);//Attend 4 secondes
                                if (currentWords.Count != 0)
                                {
                                    Console.WriteLine(currentWords.Last());
                                    Console.WriteLine(VerifWord(currentWords.Last(), start, allWords, actualPlayer));

                                }
                                else
                                {
                                    pasDeReecriture = true;
                                }
                                break;
                            case 6:
                                Thread.Sleep(3000);//Attend 3 secondes
                                while (allWords[idx].Length > 2 || this.currentWords.Contains(allWords[idx]))
                                {
                                    idx++;
                                    if (idx >= allWords.Length)
                                    {
                                        break;
                                    }//Si aucun mot ne satisfie les condition on sort de la boucle

                                }
                                string word = allWords[idx];
                                Console.WriteLine(word);
                                Console.WriteLine(VerifWord(word, start, allWords, actualPlayer));
                                break;
                        }
                    }
                }
                else if(actualPlayer == "IA_troll") 
                {
                    int idx = 0;
                    if (this.scorePlayer2 > this.scorePlayer1 && pasDeReecriture == false) 
                    {
                        Console.Write("Entrez un mot trouvé: ");//Juste pour l'affichage joli
                        pasDeReecriture = true;
                    }
                    while (this.scorePlayer2 <= this.scorePlayer1) 
                    {
                        Console.Write("Entrez un mot trouvé: ");
                        Thread.Sleep(193);
                        while ( this.currentWords.Contains(allWords[idx]))
                        {
                            idx++;

                        }
                        
                        string word = allWords[idx];
                        Console.WriteLine(word);
                        Console.WriteLine(VerifWord(word, start, allWords, actualPlayer));


                    }
                    if ((DateTime.Now - start).Minutes != 0) 
                    {
                        Console.WriteLine("Temps écoulé");
                    }

                }
                else
                {
                    string word = AskWord();
                    Console.WriteLine(VerifWord(word, start, allWords, actualPlayer));
                }
                
            }
            Console.WriteLine("Temps écoulé !");
            Console.WriteLine("Scores actuels : ");
            Console.WriteLine($"{this.playerName1} : {this.scorePlayer1}");
            Console.WriteLine($"{this.playerName2} : {this.scorePlayer2}");
            if (actualRound<gameTime){
                NextRound();
            }else{
                Console.WriteLine("Fin du Jeu !");
            }
        }

        public Jeu(char[] lettersAlphabet,Dictionary<char,int> lettersScores,int[] lettersProbas, Tree mainTree, bool testMode){
            this.lettersProbas=lettersProbas;
            this.lettersScores=lettersScores;
            this.tree=mainTree;
            this.lettersAlphabet=lettersAlphabet;
            this.testMode=testMode;
            if(this.testMode==false){
                this.size=AskSize();
                while (this.playerName1==null)
                {                    
                    Console.Write("Nom du joueur 1: ");
                    this.playerName1=Console.ReadLine();
                }
                while (this.playerName2==null)
                {                    
                    Console.Write("Nom du joueur 1: ");
                    this.playerName2=Console.ReadLine();
                }
                this.gameTime=AskTime();
            }
            NextRound();

        }
    }

}