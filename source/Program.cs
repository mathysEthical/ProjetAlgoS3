using System;
using System.IO;
using System.Collections.Generic;

public class Alphabet{
    
}

public class Dice{
    char letter;
    char[] alphabet={'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
    
    public char Letter{
        get{
            return this.letter;
        }
    }

    public Dice(){
        this.Roll();
    }
    public char Roll(){
        Random rnd = new Random();
        this.letter=alphabet[rnd.Next(0, alphabet.Length)];
        return this.Letter;
    }
}

public class Plateau{
    Dice[] dices;
    public Dice[] Dices{
        get{
            return this.dices;
        }
    }
    public Plateau(int size){
        this.dices=new Dice[size*size];
        for(int i=0;i<size*size;i++){
            this.dices[i]=new Dice();
        }
    }

    //todo: ToString
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

    public static Dictionary<char,int> LoadLetterScore(){
        Dictionary <char,int> toReturn=new Dictionary<char, int>();
        string[] scoreArray=LoadFile("scores.txt").Split('\n');
        for(int i=0;i<scoreArray.Length;i++){
            string[] lineArray=scoreArray[i].Split(' ');
            string letter=lineArray[0];
            int ratio=int.Parse(lineArray[1]);
            toReturn.Add(letter[0],ratio);
        }
        return toReturn;
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
        Console.WriteLine("Hello, World!");
        Dice testDice=new Dice();
        Console.WriteLine(testDice.Letter);
        Dictionary <char,int> letterScores=LoadLetterScore();
        Plateau board=new Plateau(3);
    }

}