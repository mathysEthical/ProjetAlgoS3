namespace DictionnaireNamespace
{
    public class Dictionnaire
    {
        string[] words;///introduit l'attribut "words" (tableau de mots constituant le dictionnaire).
        public string[] Words///définit la propriété de lecture
        {
            get
            {
                return this.words;
            }
        }
        public int Length///definit la propriété de lecture.
        {
            get
            {
                return this.words.Length;
            }
        }

        /// <summary>
        /// Vérifie la présence du mot "word" dans le dictionnaire "words".
        /// </summary>
        public bool Contains(string word)
        {
            for (int i = 0; i < this.words.Length; i++)///passe en revue tous les mots du tableau "words".
            {
                if (this.words[i] == word)
                {
                    return true;
                }
            }
            return false;
        }

        public Dictionnaire(string[] words)
        {
            this.words = words;
        }

        /// <summary>
        /// cherche des mots commençant par le préfix .
        /// </summary>
        public bool anyStartingWith(string prefix)
        {
            for (int i = 0; i < this.words.Length; i++)///passe en revue tous les mots du tableau "words".
            {
                if (this.words[i].StartsWith(prefix))
                {
                    return true;
                }
            }
            return false;
        }
    }

}