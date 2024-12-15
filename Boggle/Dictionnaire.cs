namespace DictionnaireNamespace
{
    public class Dictionnaire
    {
        string[] words;
        public string[] Words
        {
            get
            {
                return this.words;
            }
        }
        public int Length
        {
            get
            {
                return this.words.Length;
            }
        }

        public bool Contains(string word)
        {
            for (int i = 0; i < this.words.Length; i++)
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
        public bool anyStartingWith(string prefix)
        {
            for (int i = 0; i < this.words.Length; i++)
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