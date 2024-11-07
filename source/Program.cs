using System;
using System.IO;
using System.Collections.Generic;

public class Alphabet{
    
}

public class Dice{
    char letter;
    char[] alphabet;
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
        this.Roll();
    }

    public override string ToString(){
        return this.Letter.ToString();
    }

    public char Roll(){
        this.letter=alphabet[ladderValueToIndex(this.rnd.Next(0, sumOfIntArray(this.lettersProbas)))];
        return this.Letter;
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
    public static void Main(string[] args)
    {
        string[] lettersContentArray = LoadFile("lettres.txt").Split('\n');
        char[] lettersAlphabet= LoadLettersAlphabet(lettersContentArray);
        Dictionary <char,int> letterScores=LoadLettersScore(lettersContentArray);
        int[] lettersProbas=LoadLettersProbas(lettersContentArray);
        Plateau board=new Plateau(4,lettersAlphabet,letterScores,lettersProbas);
        Console.WriteLine(board);
    }

}