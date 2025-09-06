# Conference Manager

Une application de gestion de conférences développée avec .NET WPF.

## Prérequis

- Windows 10 ou version ultérieure
- .NET 6.0 SDK ou version ultérieure
- Visual Studio 2022 (recommandé) ou Visual Studio Code
- SQLite (inclus dans le projet)
- Au moins 500 Mo d'espace disque disponible
- 4 Go de RAM minimum recommandé

## Guide d'installation détaillé

### 1. Téléchargement

1. Clonez le dépôt ou téléchargez le code source :
   ```bash
   git clone [URL_DU_REPO]
   ```
   Ou téléchargez le fichier ZIP depuis la page des releases.

2. Extrayez le fichier ZIP dans un dossier de votre choix si vous avez téléchargé l'archive.

### 2. Installation des dépendances

1. Assurez-vous d'avoir installé le .NET 6.0 SDK sur votre machine :
   - Téléchargez-le depuis : https://dotnet.microsoft.com/download/dotnet/6.0
   - Choisissez la version "SDK" pour Windows x64
   - Exécutez l'installateur et suivez les instructions

2. Vérifiez l'installation en ouvrant un terminal (PowerShell ou CMD) et en tapant :
   ```bash
   dotnet --version
   ```
   Vous devriez voir la version 6.0.x s'afficher.

### 3. Configuration de l'environnement

1. Ouvrez le projet dans Visual Studio 2022 :
   - Lancez Visual Studio 2022
   - Sélectionnez "Ouvrir un projet ou une solution"
   - Naviguez jusqu'au dossier du projet
   - Sélectionnez le fichier `ConferenceManager.sln`

   OU

   Ouvrez le projet dans Visual Studio Code :
   - Lancez VS Code
   - Sélectionnez "File > Open Folder"
   - Sélectionnez le dossier du projet

2. Restaurez les packages NuGet :
   ```bash
   dotnet restore
   ```
   Attendez que tous les packages soient téléchargés.

### 4. Compilation et exécution

1. Compilez le projet :
   ```bash
   dotnet build
   ```
   Vérifiez qu'il n'y a pas d'erreurs dans la sortie.

2. Lancez l'application :
   ```bash
   dotnet run --project ConferenceManager.WPF
   ```

### 5. Base de données

- La base de données SQLite est incluse dans le projet (`conference.db`)
- Elle sera automatiquement initialisée au premier lancement de l'application
- Les données de test seront créées automatiquement

### 6. Dépannage courant

Si vous rencontrez des problèmes :

1. Erreur "dotnet command not found" :
   - Vérifiez que .NET SDK est bien installé
   - Redémarrez votre terminal
   - Vérifiez les variables d'environnement

2. Erreurs de compilation :
   - Vérifiez que tous les packages NuGet sont restaurés
   - Exécutez `dotnet clean` puis `dotnet restore`
   - Vérifiez que vous utilisez la bonne version de .NET

3. Problèmes de base de données :
   - Vérifiez que le fichier `conference.db` existe
   - Assurez-vous d'avoir les droits d'accès au dossier

## Fonctionnalités principales

- Gestion des conférences
  - Création et modification de conférences
  - Gestion des dates et lieux
  - Suivi des participants
- Gestion des intervenants
  - Profils détaillés
  - Historique des présentations
- Gestion des documents
  - Upload et téléchargement
  - Organisation par conférence
- Profils utilisateurs
  - Personnalisation
  - Préférences
- Recherche avancée
  - Filtres multiples
  - Recherche en temps réel
- Interface moderne avec Material Design
  - Thème clair/sombre
  - Interface responsive

## Support

Pour toute question ou problème, veuillez :
1. Consulter la documentation
2. Créer une issue sur le dépôt
3. Contacter l'équipe de support à  khawla_mayssoune_fatimazahra@gmail.com 

## Licence

MIT License

Copyright (c) 2024 Conference Manager

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

## Structure du Projet

- `Models/` : Classes de modèle pour les entités
- `Views/` : Interfaces utilisateur XAML
- `Data/` : Contexte de base de données et migrations
- `ViewModels/` : Logique de présentation (MVVM)
- `Services/` : Services métier et interfaces
- `Extensions/` : Extensions et utilitaires

## Technologies Utilisées

- .NET 6.0
- WPF (Windows Presentation Foundation)
- Entity Framework Core
- SQLite
- Material Design In XAML
- LiveCharts2 (pour les graphiques)
- CommunityToolkit.Mvvm

## Contribution

Les contributions sont les bienvenues ! N'hésitez pas à :
1. Ouvrir une issue pour signaler un bug
2. Proposer une amélioration
3. Soumettre une pull request

Pour contribuer :
1. Fork le projet
2. Créez une branche pour votre fonctionnalité
3. Committez vos changements
4. Poussez vers votre fork
5. Créez une pull request 