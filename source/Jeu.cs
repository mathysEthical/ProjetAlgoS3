using DictionaireNamespace;
using TreeNamespace;
using ProgramNamespace;
using PlateauNamespace;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace JeuNamespace
{
    public class Jeu{
        Plateau board;
        string playerName1="Mathys";
        string playerName2="Paul";
        char[] lettersAlphabet;
        int [] lettersProbas;
        int gameTime=4;
        int actualRound=0;
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

        public int scoreFromWord(string word){
            int scoreToReturn=0;
            for(int i=0;i<word.Length;i++){
                char c = word[i];
                scoreToReturn+=this.lettersScores[c];
            }
            return scoreToReturn;
        }

        public void NextRound(){
            actualRound++;
            string actualPlayer=this.playerName1;
            if(actualRound%2==0){
                actualPlayer=this.playerName2;
            }
            Console.WriteLine("C'est au tour de "+actualPlayer);
            Console.WriteLine("Génération du plateau...");
            this.board=new Plateau(this.size, this.lettersAlphabet,this.lettersScores,this.lettersProbas,this.testMode);
            string[] allWords;
            if(testMode){
                Stopwatch sw = new Stopwatch();
                sw.Start();
                allWords=findAllWords();
                sw.Stop();
                Console.WriteLine(sw.Elapsed);
            }else{
                allWords=findAllWords();
            }
            this.currentWords=new List<string>();
            Console.WriteLine("C'est parti ! Voici le plateau");
            Console.WriteLine(board);
            DateTime start = DateTime.Now;
            // for(int w=0;w<allWords.Length;w++){
            //     Console.WriteLine("Mot possible: "+allWords[w]);
            // }
            while((DateTime.Now-start).Minutes==0){
                string word=AskWord();
                if(this.tree.Contains(word)){
                    if(allWords.Contains(word)){
                        if(this.currentWords.Contains(word)==false){
                            this.currentWords.Add(word);
                            if((DateTime.Now-start).Minutes==0){
                                Console.WriteLine("Mot valide ! +"+scoreFromWord(word)+" points");
                            }else{
                                Console.WriteLine("Temps écoulé avant soumission du mot.");
                            }
                        }else{
                            Console.WriteLine("Mot déjà accepté.");
                        }
                    }else{
                        Console.WriteLine("Mot non présent sur le plateau.");
                    }
                }else{
                    Console.WriteLine("Mot non présent dans le dictionaire.");
                }
                if((DateTime.Now-start).Minutes==0){
                    Console.WriteLine("Il vous reste "+(60-(DateTime.Now-start).Seconds).ToString()+" secondes");
                }
            }
            if(actualRound<gameTime){
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
                Console.Write("Nom du joueur 1: ");
                this.playerName1=Console.ReadLine();
                Console.Write("Nom du joueur 2: ");
                this.playerName2=Console.ReadLine();
                this.gameTime=AskTime();
            }
            NextRound();

        }
    }

}