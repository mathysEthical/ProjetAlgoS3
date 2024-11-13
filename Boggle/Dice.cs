using ProgramNamespace;
using PlateauNamespace;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace DiceNamespace
{    
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

}