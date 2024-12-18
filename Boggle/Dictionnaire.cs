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

        public bool Contains(string word)///vérifie la présence du mot "word" dans le dictionnaire "words".
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
        public bool anyStartingWith(string prefix)///cherche des mots commençant par le préfix "préfix".
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