using System;


public class Dice{
    char letter;
    char[] alphabet={'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'};
    
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



public class Program
{
    string[] LoadWordsFromFile(string fileName){
        string[] toReturn={};

        return toReturn;
    }
    int scoreFromWorld(string word){
        int toReturn=0;
        return toReturn;
    }
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Dice testDice=new Dice();
        Console.WriteLine(testDice.Letter);
    }

}