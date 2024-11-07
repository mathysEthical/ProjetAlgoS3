using System;
using System.IO;
using System.Collections.Generic;

public class Alphabet{
    
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

public class Program
{
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

    public static int scoreFromWorld(string word,Dictionary <char,int> letterScores){
        int scoreToReturn=0;
        for(int i=0;i<word.Length;i++){
            char c = word[i];
            scoreToReturn+=letterScores[c];
        }
        return scoreToReturn;
    }

    public static int AskSize(){
        int max=20;
        int size=max+1;
        while(size>max || size<1){
            Console.Write("Taille pour du plateau (max 20): ");
            int.TryParse(Console.ReadLine(), out size);
        }
        return size;
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
        string language=AskLanguage();
        Dictionaire dico=new Dictionaire(LoadFile(language+".txt").Split(' '));

        string[] lettersContentArray = LoadFile("lettres.txt").Split('\n');
        char[] lettersAlphabet= LoadLettersAlphabet(lettersContentArray);
        Dictionary <char,int> letterScores=LoadLettersScore(lettersContentArray);
        int[] lettersProbas=LoadLettersProbas(lettersContentArray);
        int size=AskSize();
        Plateau board=new Plateau(size,lettersAlphabet,letterScores,lettersProbas);
        Console.WriteLine(board);
    }

}