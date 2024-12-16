﻿using TreeNamespace;
using PlateauNamespace;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel.Design;

namespace JeuNamespace
{
    public class Jeu
    {
        Plateau board;
        string playerName1 = "Paul";
        string playerName2 = "IA_troll";
        char[] lettersAlphabet;
        int[] lettersProbas;
        int gameTime = 4;
        int actualRound = 0;
        int scorePlayer1;
        int scorePlayer2;
        Dictionary<string, int> motsTrouves;
        List<string> currentWords;
        Dictionary<char, int> lettersScores;
        Tree tree;
        int size = 4;
        bool testMode;

        public static int AskSize()
        {
            int max = 20;
            int min = 4;
            int size = max + 1;
            while (size > max || size < min)
            {
                Console.Write("Taille pour du plateau (max 20): ");
                int.TryParse(Console.ReadLine(), out size);
            }
            return size;
        }

        public static string AskPlayer2Name()
        {
            string name = null;
            Console.Clear();
            while (name == null)
            {
                Console.Write("Voulez vous jouez contre une IA ou un autre joueur ? \n 1) IA \n 2) Autre joueur \n");
                int choix = 0;
                int.TryParse(Console.ReadLine(), out choix);
                switch (choix)
                {
                    case 1:
                        Console.Write("Niveau de l'IA : \n 1) Facile \n 2) Moyen \n 3) Impossible \n");
                        int choix2 = 0;
                        int.TryParse(Console.ReadLine(), out choix2);
                        switch (choix2)
                        {
                            case 1:
                                name = "IA_noob";
                                break;
                            case 2:
                                name = "IA_moy";
                                break;
                            case 3:
                                name = "IA_troll";
                                break;
                            default:
                                Console.Clear();
                                Console.WriteLine("Choix invalide");
                                break;
                        }
                        break;
                    case 2:
                        Console.Write("Nom du joueur 2: ");
                        name = Console.ReadLine();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Choix invalide");
                        break;
                }
            }
            return name;
        }

        public static int AskTime()
        {
            int max = 60;
            int size = max + 1;
            while (size > max || size < 1 || size % 2 != 0)
            {
                Console.Write("Durée de la partie (en minutes, doit être pair): ");
                int.TryParse(Console.ReadLine(), out size);
            }
            return size;
        }

        public int coordsToIndex(int x, int y)
        {
            return y * this.size + x;
        }

        public int[] indexToCoords(int index)
        {
            int[] coords = new int[2];
            coords[0] = index % this.size;
            coords[1] = index / this.size;
            return coords;
        }

        public string[] Dig(string actualSpelling, int actualPos, List<int> usedDicesIndex, List<string> foundWords)
        {
            if (this.tree.Contains(actualSpelling) && !foundWords.Contains(actualSpelling))
            {
                foundWords.Add(actualSpelling);
            }

            if (this.tree.anyStartingWith(actualSpelling) == false)
            {
                return foundWords.ToArray();
            }
            List<string> wordList = new List<string>();
            int[] coords = indexToCoords(actualPos);
            int x = coords[0];
            int y = coords[1];
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if ((dx != 0 || dy != 0) && x + dx >= 0 && x + dx < this.size && y + dy >= 0 && y + dy < this.size)
                    {
                        int voisin = coordsToIndex(x + dx, y + dy);
                        //pour chaque voisin de la case actuelle
                        if (usedDicesIndex.Contains(voisin) == false)
                        {
                            //le dé voisin n'a pas été utilisé
                            usedDicesIndex.Add(voisin);
                            string[] newWords = Dig(actualSpelling + this.board.Dices[voisin].Letter.ToString(), voisin, usedDicesIndex, foundWords);
                            for (int w = 0; w < newWords.Length; w++)
                            {
                                if (wordList.Contains(newWords[w]) == false)
                                {
                                    wordList.Add(newWords[w]);
                                }
                            }
                            usedDicesIndex.Remove(voisin);
                        }
                    }
                }
            }
            return wordList.ToArray();
        }

        public string[] findAllWords()
        {
            List<string> wordsList = new List<string>();
            for (int i = 0; i < this.size * this.size; i++)
            {
                // i is the starting dice index in the board
                List<int> usedDicesIndex = new List<int>();
                usedDicesIndex.Add(i);
                List<string> foundWords = new List<string>();
                string[] allPosWords = Dig(this.board.Dices[i].Letter.ToString(), i, usedDicesIndex, foundWords);
                for (int w = 0; w < allPosWords.Length; w++)
                {
                    if (wordsList.Contains(allPosWords[w]) == false)
                    {
                        wordsList.Add(allPosWords[w]);
                    }
                }
            }
            return wordsList.ToArray();

        }

        public string AskWord()
        {
            Console.Write("Entrez un mot trouvé: ");
            string word = Console.ReadLine();
            return word.ToUpper();
        }

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

        public string VerifWord(string word, DateTime start, string[] allWords, string actualPlayer)
        {
            string affichage = "";
            if (this.tree.Contains(word))
            {
                if (Contains(allWords, word))
                {
                    if (this.currentWords.Contains(word) == false)
                    {
                        this.currentWords.Add(word);
                        if ((DateTime.Now - start).Minutes == 0)
                        {
                            affichage = "Mot "+word+" valide ! "+scoreFromWord(word)+" points";
                            if (actualPlayer == this.playerName1)
                            {
                                this.scorePlayer1 += scoreFromWord(word);
                            }
                            else
                            {
                                this.scorePlayer2 += scoreFromWord(word);
                            }

                            if (motsTrouves.ContainsKey(word))
                            {
                                motsTrouves[word]++;
                            }
                            else
                            {
                                motsTrouves.Add(word, 1);
                            }

                        }
                        else
                        {
                            affichage = "Temps écoulé avant soumission du mot.";
                        }
                    }
                    else
                    {
                        affichage = "Mot "+word+" déjà accepté.";
                    }
                }
                else
                {
                    affichage = "Mot "+word+" non présent sur le plateau.";
                }
            }
            else
            {
                affichage = "Mot "+word+" non présent dans le dictionnaire.";
            }
            if ((DateTime.Now - start).Minutes == 0)
            {
                affichage += "\nIl vous reste "+(60 - (DateTime.Now - start).Seconds).ToString()+" secondes";
            }
            Console.Clear();

            return affichage;
        }
        public int scoreFromWord(string word)
        {
            int scoreToReturn = 0;
            for (int i = 0; i < word.Length; i++)
            {
                char c = word[i];
                scoreToReturn += this.lettersScores[c];
            }
            return scoreToReturn;
        }



        public void NextRound()
        {
            actualRound++;
            string actualPlayer = this.playerName1;
            if (actualRound % 2 == 0)
            {
                actualPlayer = this.playerName2;
            }
            Console.WriteLine("C'est au tour de " + actualPlayer);
            Console.WriteLine("Génération du plateau...");
            this.board = new Plateau(this.size, this.lettersAlphabet, this.lettersScores, this.lettersProbas, this.testMode);
            string[] allWords;
            if (testMode)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                allWords = findAllWords();
                sw.Stop();
                Console.WriteLine(sw.Elapsed);
            }
            else
            {
                allWords = findAllWords();
                Console.WriteLine("Nombre de mots trouvés: "+allWords.Length);
            }
            this.currentWords = new List<string>();
            DateTime start = DateTime.Now;
            bool pasDeReecriture = false;
            // for(int w=0;w<allWords.Length;w++){
            //     Console.WriteLine("Mot possible: "+allWords[w]);
            // }
            while ((DateTime.Now - start).Minutes == 0)
            {


                if (actualPlayer == "IA_noob")
                {
                    if (!pasDeReecriture)
                    {
                        Console.WriteLine("Voici le plateau:");
                        Console.WriteLine(this.board);
                        Console.Write("Entrez un mot trouvé: ");
                    }
                    pasDeReecriture = false;
                    Random probas = new Random();
                    Random temps = new Random();
                    int nombre = probas.Next(1, 7);
                    Random idx = new Random();
                    int nb = 0;
                    int nextIdx = idx.Next(0, allWords.Length);
                    string lastWord = "";
                    if (nombre <= 3)
                    {
                        Thread.Sleep(10000 + temps.Next(0, 2000));//Attend entre 10 et 12 secondes
                        while (allWords[nextIdx].Length > 4 || this.currentWords.Contains(allWords[nextIdx]))
                        {
                            nb++;
                            if (nb >= allWords.Length)
                            {
                                break;
                            }//Si aucun mot ne satisfie les condition on sort de la boucle

                        }
                        lastWord = allWords[nextIdx];
                        Console.WriteLine(VerifWord(lastWord, start, allWords, actualPlayer));

                    }
                    else
                    {
                        switch (nombre)
                        {
                            case 4:
                                Thread.Sleep(6000);//Attend 6 secondes
                                pasDeReecriture = true;
                                break;
                            case 5:
                                Thread.Sleep(4000);//Attend 4 secondes
                                if (currentWords.Count != 0)
                                {
                                    Console.WriteLine(lastWord);
                                    Console.WriteLine(VerifWord(lastWord, start, allWords, actualPlayer));

                                }
                                else
                                {
                                    pasDeReecriture = true;
                                }
                                break;
                            case 6:
                                Thread.Sleep(8000);//Attend 8 secondes
                                while (allWords[nextIdx].Length > 3 || this.currentWords.Contains(allWords[nextIdx]))
                                {
                                    nb++;
                                    if (nb >= allWords.Length)
                                    {
                                        break;
                                    }//Si aucun mot ne satisfie les condition on sort de la boucle

                                }
                                string word = allWords[nextIdx];
                                Console.WriteLine(word);
                                Console.WriteLine(VerifWord(word, start, allWords, actualPlayer));
                                break;
                        }
                    }

                }
                else if (actualPlayer == "IA_moy")
                {
                    if (!pasDeReecriture)
                    {
                        Console.WriteLine("Voici le plateau:");
                        Console.WriteLine(this.board);
                        Console.Write("Entrez un mot trouvé: ");
                    }
                    pasDeReecriture = false;
                    Random probas = new Random();
                    Random temps = new Random();
                    int nombre = probas.Next(1, 7);
                    Random idx = new Random();
                    int nb = 0;
                    int nextIdx = idx.Next(0, allWords.Length);
                    string lastWord = "";
                    if (nombre <= 4)
                    {
                        Thread.Sleep(5000 + temps.Next(-2000, 2000));///Attend entre 3 et 7 secondes
                        while (allWords[nextIdx].Length > 5 || this.currentWords.Contains(allWords[nextIdx]))
                        {
                            nb++;
                            if (nb >= allWords.Length)
                            {
                                break;
                            }///Si aucun mot ne satisfie les condition on sort de la boucle

                        }
                        lastWord = allWords[nextIdx];
                        Console.WriteLine(lastWord);
                        Console.WriteLine(VerifWord(lastWord, start, allWords, actualPlayer));
                    }
                    else
                    {
                        switch (nombre)
                        {
                            case 5:
                                Thread.Sleep(4000);///Attend 4 secondes
                                if (currentWords.Count != 0)
                                {
                                    Console.WriteLine(lastWord);
                                    Console.WriteLine(VerifWord(lastWord, start, allWords, actualPlayer));

                                }
                                else
                                {
                                    pasDeReecriture = true;
                                }
                                break;
                            case 6:
                                Thread.Sleep(3000);///Attend 3 secondes
                                while (allWords[nextIdx].Length > 2 || this.currentWords.Contains(allWords[nextIdx]))
                                {
                                    nb++;
                                    if (nb >= allWords.Length)
                                    {
                                        break;
                                    }///Si aucun mot ne satisfie les condition on sort de la boucle

                                }
                                lastWord = allWords[nextIdx];
                                Console.WriteLine(lastWord);
                                Console.WriteLine(VerifWord(lastWord, start, allWords, actualPlayer));
                                break;
                        }
                    }
                }
                else if (actualPlayer == "IA_troll")
                {
                    Random idx = new Random();
                    int nb = 0;
                    int nextIdx = idx.Next(0,allWords.Length);
                    List<int> idxUtilises = new List<int>();
                    int scoreP2 = 0;
                    while (scoreP2 <= this.scorePlayer1)
                    {
                        while (idxUtilises.Contains(nextIdx))
                        {
                            nextIdx = idx.Next(0, allWords.Length);
                        }
                        idxUtilises.Add(nextIdx);
                        scoreP2 += scoreFromWord(allWords[nextIdx]);
                        nb++;
                    }
                    int temps = 60000 / (nb + 2);

                    foreach (int i in idxUtilises)
                        {
                            Console.WriteLine("Voici le plateau:");
                            Console.WriteLine(this.board);
                            Console.Write("Entrez un mot trouvé: ");
                            Thread.Sleep(temps);
                            string word = allWords[i];
                            Console.WriteLine(word);
                            Console.WriteLine(VerifWord(word, start, allWords, actualPlayer));
                    }
                    if (!pasDeReecriture)
                    {
                        Console.WriteLine("Voici le plateau:");
                        Console.WriteLine(this.board);
                        Console.Write("Entrez un mot trouvé: ");
                        pasDeReecriture = true;
                    }

                }
                else
                {
                    Console.WriteLine("Voici le plateau:");
                    Console.WriteLine(this.board);
                    string word = AskWord();
                    Console.WriteLine(VerifWord(word, start, allWords, actualPlayer));
                }

            }
            Console.WriteLine("Temps écoulé !");
            Console.WriteLine("Scores actuels : ");
            Console.WriteLine(this.playerName1+" : "+this.scorePlayer1);
            Console.WriteLine(this.playerName2+" : "+this.scorePlayer2);
            if (actualRound < gameTime)
            {
                NextRound();
            }
            else
            {
                Console.WriteLine("Fin du Jeu !");
            }
        }

        static void GenererNuageDeMotsGraphique(Dictionary<string, int> motsTrouves, string cheminFichier)
        {
            if (motsTrouves == null || motsTrouves.Count == 0)
            {
                Console.WriteLine("Aucun mot à afficher.");
                return;
            }

            // Dimensions de l'image
            int largeur = 1600;
            int hauteur = 1200;

            using (Bitmap bitmap = new Bitmap(largeur, hauteur))
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                // Fond blanc
                graphics.Clear(Color.White);

                // Trie les mots par fréquence décroissante sans LINQ
                List<KeyValuePair<string, int>> motsTries = new List<KeyValuePair<string, int>>(motsTrouves);
                motsTries.Sort((x, y) => y.Value.CompareTo(x.Value));

                // Détermine la fréquence maximale
                int maxFrequence = motsTries[0].Value;

                // Définir une liste de couleurs
                Color[] couleurs = { Color.Blue, Color.Green, Color.Red, Color.Orange, Color.Purple, Color.Brown };
                Random random = new Random();

                // Liste pour stocker les rectangles des mots déjà placés
                List<RectangleF> zonesOccupees = new List<RectangleF>();

                // Centre de l'image
                float centreX = largeur / 2;
                float centreY = hauteur / 2;

                // Variable pour ajuster la position des mots
                float step = 50; // Distance initiale entre les couches de mots

                foreach (var mot in motsTries)
                {
                    // Taille proportionnelle à la fréquence avec une taille minimale de 12
                    int taille = Math.Max(12, (int)Math.Ceiling((double)mot.Value / maxFrequence * 40));

                    // Police
                    Font font = new Font("Arial", taille, FontStyle.Bold);

                    // Couleur aléatoire
                    Color couleur = couleurs[random.Next(couleurs.Length)];
                    Brush brush = new SolidBrush(couleur);

                    // Mesure la taille du mot
                    SizeF tailleMot = graphics.MeasureString(mot.Key, font);
                    RectangleF rectangleMot = new RectangleF();

                    // Trouver une position disponible
                    bool positionTrouvee = false;
                    for (float angle = 0; !positionTrouvee; angle += (float)Math.PI / 18)
                    {
                        float x = centreX + (float)Math.Cos(angle) * step - tailleMot.Width / 2;
                        float y = centreY + (float)Math.Sin(angle) * step - tailleMot.Height / 2;
                        rectangleMot = new RectangleF(x, y, tailleMot.Width, tailleMot.Height);

                        // Vérifie si la zone chevauche une autre
                        positionTrouvee = true;
                        foreach (var zone in zonesOccupees)
                        {
                            if (rectangleMot.IntersectsWith(zone))
                            {
                                positionTrouvee = false;
                                break;
                            }
                        }

                        if (angle >= 2 * Math.PI) // Augmente la distance si aucune position trouvée dans un tour
                        {
                            angle = 0;
                            step += 10;
                        }
                    }

                    // Dessine le mot
                    graphics.DrawString(mot.Key, font, brush, rectangleMot.Location);

                    // Ajoute le rectangle à la liste des zones occupées
                    zonesOccupees.Add(rectangleMot);

                    // Recentre les mots pour équilibrer la répartition
                    if (zonesOccupees.Count % 2 == 0)
                    {
                        centreX -= 15; // Décale légèrement vers la gauche pour équilibrer
                    }
                    else
                    {
                        centreX += 15; // Décale légèrement vers la droite pour équilibrer
                    }
                }

                // Sauvegarde l'image
                bitmap.Save(cheminFichier, ImageFormat.Png);
                ProcessStartInfo startInfo = new ProcessStartInfo(cheminFichier)
                {
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
        }

        

        public Jeu(char[] lettersAlphabet, Dictionary<char, int> lettersScores, int[] lettersProbas, Tree mainTree, bool testMode)
        {
           
            
            this.lettersProbas = lettersProbas;
            this.lettersScores = lettersScores;
            this.tree = mainTree;
            this.lettersAlphabet = lettersAlphabet;
            this.testMode = testMode;
            this.motsTrouves = new Dictionary<string, int>();
            if (this.testMode == false)
            {
                this.size = AskSize();
                this.playerName1 = null;
                this.playerName2 = null;
                while (this.playerName1 == null)
                {
                    Console.Write("Nom du joueur 1: ");
                    this.playerName1 = Console.ReadLine();
                }
                this.playerName2 = AskPlayer2Name();
                this.gameTime = AskTime();
            }
            NextRound();
            GenererNuageDeMotsGraphique(motsTrouves, "nuage_de_mots.png");
            Console.WriteLine("Nuage de mots généré : nuage_de_mots.png");
            Console.ReadKey();
            

        }
    }

}