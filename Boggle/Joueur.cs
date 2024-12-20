using TreeNamespace;

using System.Collections.Generic;


namespace JoueurNamespace
{
    public class Joueur
    {
        string playerName;
        int scorePlayer = 0;
        Dictionary<string, int> motsTrouves;

        public string PlayerName
        {
            get
            {
                return this.playerName;
            }
            set
            {
                this.playerName = value;
            }
        }
        public Dictionary<string, int> MotsTrouves
        {
            get
            {
                return this.motsTrouves;
            }
            set
            {
                this.motsTrouves = value;
            }
        }

        /// <summary>
        /// Vérifie si un mot est contenu dans un tableau.
        /// </summary>
        public bool Contains(string[] array, string word)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == word)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Ajoute un mot au dictionnaire des mots trouvés.
        /// <param name="word">mot à ajouter</param>
        /// </summary>
        public void AddWord(string word)
        {
            if (motsTrouves.ContainsKey(word))
            {
                motsTrouves[word]++;
            }
            else
            {
                motsTrouves.Add(word, 1);
            }
        }

        /// <summary>
        /// Retourne une chaîne représentant le joueur et ses mots trouvés.
        /// </summary>
        public string toString()
        {
            string result = playerName + " : " + scorePlayer + " points\n";
            foreach (KeyValuePair<string, int> entry in motsTrouves)
            {
                result += entry.Key + " : " + entry.Value + "\n";
            }
            return result;
        }

        /// <summary>
        /// Constructeur de la classe Joueur.
        /// <param name="playerName">nom du joueur</param>
        /// </summary>
        public Joueur(string playerName)
        {


            this.motsTrouves = new Dictionary<string, int>();
            this.playerName = playerName;


        }
    }

}