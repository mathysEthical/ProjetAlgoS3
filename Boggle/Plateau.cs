using ProgramNamespace;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using DiceNamespace;

namespace PlateauNamespace
{

    public class Plateau{
        Dice[] dices;
        int size;
        int [] lettersProbas;
        char[] alphabet;
        public Random rnd;
        public Dice[] Dices{
            get{
                return this.dices;
            }
        }
        public Plateau(int size,char[] letters,Dictionary<char,int> lettersScores,int[] lettersProbas,bool testMode){
            this.size=size;
            if(testMode==false){
                this.rnd=new Random();
            }else{
                this.rnd=new Random(1234);
            }
            
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

}