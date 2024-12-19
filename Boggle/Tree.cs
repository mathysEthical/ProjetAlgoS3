using System.Collections.Generic;

namespace TreeNamespace
{
    public class Tree
    {
        bool isEnd = false;/// variable permettant de se situer dans l'arbre par rapport à la fin du mot
        public Dictionary<char, Tree> subTrees = new Dictionary<char, Tree>();/// définie un sous arbre associé à un enfant
        public Tree()
        {

        }

        /// <summary>
        /// Vérifie si un mot commence par une certaine séquence de lettres.
        /// </summary>
        public bool anyStartingWith(string word)
        {
            if (word.Length > 0)
            {
                if (this.subTrees.ContainsKey(word[0]))
                {
                    return this.subTrees[word[0]].anyStartingWith(word.Substring(1));/// utilise la récursivité pour orienter la recherche de chaque lettre du mot dans l'arbre, change de branche si nécessaire.
                }
                else
                {
                    return false;/// change de branche 
                }
            }
            else
            {
                return true;/// retourne true car le mot en entrée a été parcouru en entier
            }
        }

        /// <summary>
        /// Vérifie si un mot est contenu dans l'arbre.
        /// </summary>
        public bool Contains(string word)
        {
            if (word.Length > 0)
            {
                if (this.subTrees.ContainsKey(word[0]))
                {
                    return this.subTrees[word[0]].Contains(word.Substring(1));
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return this.isEnd;
            }
        }


        /// <summary>
        /// Ajoute un mot à l'arbre.
        /// </summary>
        public void AddWord(string word)
        {
            Tree subTree;
            if (this.subTrees.ContainsKey(word[0]))
            {
                ///if the first letter of the word is already in the tree
                subTree = this.subTrees[word[0]];
            }
            else
            {
                ///if the first letter of the word is not in the tree
                subTree = new Tree();
                this.subTrees.Add(word[0], subTree);
            }

            if (word.Length == 1)
            {
                ///if the word is only one letter long
                subTree.isEnd = true;
            }
            else
            {
                ///if the word is more than one letter long
                subTree.AddWord(word.Substring(1));
            }
        }
    }
}