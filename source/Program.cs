using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class Jeu{
    Plateau board;
    string playerName1;
    string playerName2;
    char[] lettersAlphabet;
    int [] lettersProbas;
    int gameTime;
    int actualRound=0;
    List<string> currentWords;
    Dictionary<char,int> lettersScores;
    Dictionaire dictionaire;
    int size;

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
        if(this.dictionaire.Contains(actualSpelling) && !foundWords.Contains(actualSpelling)){
            foundWords.Add(actualSpelling);
        }

        if(this.dictionaire.searchStartingWith(actualSpelling).Length==0){
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
        string word="";
        while(word==""){
            Console.Write("Entrez un mot trouvé: ");
            word=Console.ReadLine();
        }
        return word.ToUpper();
    }

    public int scoreFromWorld(string word){
        int scoreToReturn=0;
        for(int i=0;i<word.Length;i++){
            char c = word[i];
            scoreToReturn+=this.lettersScores[c];
        }
        return scoreToReturn;
    }

    public void NextRound(){
        actualRound++;
        this.board=new Plateau(this.size, this.lettersAlphabet,this.lettersScores,this.lettersProbas);
        string actualPlayer=this.playerName1;
        if(actualRound%2==0){
            actualPlayer=this.playerName2;
        }
        Console.WriteLine("C'est au tour de "+actualPlayer);
        Console.WriteLine(board);
        string[] allWords=findAllWords();
        this.currentWords=new List<string>();
        // for(int w=0;w<allWords.Length;w++){
        //     Console.WriteLine(allWords[w]);
        // }
        bool timeCondition=true;
        while(timeCondition){
            string word=AskWord();
            if(this.dictionaire.Contains(word)){
                if(allWords.Contains(word)){
                    if(this.currentWords.Contains(word)==false){
                        this.currentWords.Add(word);
                        Console.WriteLine("Mot valide ! +"+scoreFromWorld(word)+" points");
                    }else{
                        Console.WriteLine("Mot déjà accepté.");
                    }
                }else{
                    Console.WriteLine("Mot non présent sur le plateau.");
                }
            }else{
                Console.WriteLine("Mot non présent dans le dictionaire.");
            }
        }
    }

    public Jeu(char[] lettersAlphabet,Dictionary<char,int> lettersScores,int[] lettersProbas, Dictionaire dictionaire){
        // this.size=AskSize();
        this.size=4;
        this.lettersProbas=lettersProbas;
        this.lettersScores=lettersScores;
        this.dictionaire=dictionaire;
        this.lettersAlphabet=lettersAlphabet;
        // Console.Write("Nom du joueur 1: ");
        // this.playerName1=Console.ReadLine();
        this.playerName1="Mathys";
        // Console.Write("Nom du joueur 2: ");
        // this.playerName2=Console.ReadLine();
        this.playerName1="Paul";
        // this.gameTime=AskTime();
        this.gameTime=10;
        NextRound();

    }
}

public class Dice{
    char letter;
    char[] alphabet;
    char[] facesLetters=new char[6];
    Random rnd;
    int[] lettersProbas;
    public char Letter{
        get{
            return this.letter;
        }
    }

    public int ladderValueToIndex(int value){
        int index=0;
        int actualLadderValue=0;
        while(actualLadderValue+this.lettersProbas[index]<value){
            actualLadderValue+=this.lettersProbas[index];
            index++;
        }
        return index;
    }

    int sumOfIntArray(int[] array){
        int total=0;
        for(int i=0;i<array.Length;i++){
            total+=array[i];
        }
        return total;
    }
    public Dice(Random rnd,char[] alphabet,int[] lettersProbas){
        this.rnd=rnd;
        this.alphabet=alphabet;
        this.lettersProbas=lettersProbas;
        for(int i=0;i<6;i++){
            this.facesLetters[i]=this.alphabet[ladderValueToIndex(this.rnd.Next(0, sumOfIntArray(this.lettersProbas)))];
        }
    }

    public override string ToString(){
        return this.Letter.ToString();
    }

    public char Roll(){
        this.letter=this.facesLetters[this.rnd.Next(0, 6)];
        return this.Letter;
    }
}

public class Dictionaire{
    string[] words;
    public int Length{
        get{
            return this.words.Length;
        }
    }

    public bool Contains(string word){
        return this.words.Contains(word);
    }

    public Dictionaire(string[] words){
        this.words=words;
    }
    public Dictionaire searchStartingWith(string prefix){
        List<string> wordList=new List<string>();
        for(int i=0;i<this.words.Length;i++){
            if(this.words[i].StartsWith(prefix)){
                wordList.Add(this.words[i]);
            }
        }
        return new Dictionaire(wordList.ToArray());
    }
}

public class Plateau{
    Dice[] dices;
    int size;
    int [] lettersProbas;
    char[] alphabet;
    public Random rnd = new Random();
    public Dice[] Dices{
        get{
            return this.dices;
        }
    }
    public Plateau(int size,char[] letters,Dictionary<char,int> lettersScores,int[] lettersProbas){
        this.size=size;
        this.lettersProbas=lettersProbas;
        this.alphabet=letters;
        this.dices=new Dice[size*size];
        for(int i=0;i<size*size;i++){
            this.dices[i]=new Dice(rnd,this.alphabet, this.lettersProbas);
        }
        this.RollAllDices();
    }

    public void RollAllDices(){
        for(int i=0;i<size*size;i++){
            this.dices[i].Roll();
        }
    }

    public override string ToString(){
        string toReturn="";
        for(int i=0;i<this.size;i++){
            for(int j=0;j<this.size;j++){
                toReturn+=this.dices[i*this.size+j]+" ";
            }
            toReturn+="\n";
        }
        return toReturn;
    }
}

public class Program{
    public static string LoadFile(string filename){
        string contents = File.ReadAllText(".\\files\\"+filename);
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
        // config of the game
        // string language=AskLanguage();
        string language="FR";
        Dictionaire dico=new Dictionaire(LoadFile(language+".txt").Split(' '));

        string[] lettersContentArray = LoadFile("lettres.txt").Split('\n');
        char[] lettersAlphabet= LoadLettersAlphabet(lettersContentArray);
        Dictionary <char,int> letterScores=LoadLettersScore(lettersContentArray);
        int[] lettersProbas=LoadLettersProbas(lettersContentArray);
        
        Jeu game=new Jeu(lettersAlphabet,letterScores,lettersProbas,dico);
    }

}