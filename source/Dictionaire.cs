using ProgramNamespace;
using PlateauNamespace;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace DictionaireNamespace
{    
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

}