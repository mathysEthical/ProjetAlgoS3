# Logs

<details>
    <summary>6/11/24:</summary>

- Création du projet Git & Github
- Création de la TODO
- Création de la classe Dé
- Pour les scores de chaque lettre: [Scrabble français](https://fr.wikipedia.org/wiki/Lettres_du_Scrabble#Fran%C3%A7ais)
</details>

<details>
    <summary>7/11/24:</summary>

- LoadFile function
- clase Plateau
- Ajout du chrono de 1 min

</details>

<details>
    <summary>8/11/24</summary>

- Séparation du fichier Program.cs en sous fichiers (namespace)
- Création du testMode pour tester notre programme rapidement et mesurer ses performances
- Actuellement le temps de calcul sur le plateau de test est de **19s**
- testMode deterministic avec une seed pour le random
</details>

<details>
    <summary>10/11/24</summary>

- Tree optimization: **3ms** sur le plateau de test ! (5000 fois plus rapide) (https://www.geeksforgeeks.org/trie-insert-and-search/)

</details>

<details>
    <summary>12/11/24</summary>

- VerifWord
- Score acuel
</details>

<details>
    <summary>13/11/24</summary>

- Conversion du projet en projet C# .NET
- Utilisation du dossier baseFolder dans loadFile
- Ajout de Console.Clear() pour améliorer l'interface
</details>

## Nuage de mots

### Premier prompt : 

- Tu es un expert en programmation C#. Tu dois ecrire une fonction qui prend en entrée un Dictionary<string,int> motsTrouves contenant un mot et son nombre d'apparition et qui retourne un nuage de mots. Je souhaite une version graphique.
- Résultat : le code est fonctionnel mais les mots sont éparpillés de façon aléatoire sur l'image générée

Il faut donc bien préciser ce qu'il faut faire car il ne comprenait pas bien le principe du nuage de mots
    
### Deuxième prompt

- Les mots doivent etre groupés au centre de l'image avec le plus gros mot au milieu et ainsi de suite
- Résultat : les mots sont bien centrés autour du plus gros mot au centre mais ils se superposaient

### Troisième prompt :

- les mots se superposent, tu dois faire en sorte de prendre en compte la taille des autres mots affichées pour savoir où il reste de la place pour mettre le prochain mot
- Résultat : les mots ont arrétés de se superposer. Cependant certains mots étaient trop petits donc illisibles
    
### Quatrième prompt : 

- les mots se superposent, tu dois faire en sorte de prendre en compte la taille des autres mots affichées pour savoir où il reste de la place pour mettre le prochain mot
- Résultat : le nuage de mot était bien joli comme voulu mais il fallait encore ouvrir l'image a la main

### Cinquième prompt :

- Fais en sorte d'ouvrir l'image une fois qu'elle est crée
- Résultat : Tout est bon !

ChatGPT a besoin que l'on soit très précis lorsqu'on lui décrit ce qu'il doit faire sinon il risque de ne pas bien le réaliser. En effet il a fallu plusieurs itérations pour arriver a un résultat cohérent cependant il est fonctionnel.
    


    


