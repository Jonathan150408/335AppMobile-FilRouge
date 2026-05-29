# Protocoles de test : AppFilRouge - FlashQuizz

## Test 1 - Créer un deck

|             En-tête              |     Valeur     |
| :------------------------------: | :------------: |
| Version des l'application testée |     2.0.0      |
|           Date du test           |   29.05.2026   |
|          Nom du testeur          | Jonathan Junod |

| Étape   | Description                                          | Remarque                                                                                              |
| :------ | :--------------------------------------------------- | :---------------------------------------------------------------------------------------------------- |
| Arrange | Lancer l'application et cliquer sur le bouton '+'    | On arrive sur la page d'accueil où se trouvent les decks, le bouton nous mène sur la page de création |
| Act     | Entrer un titre et une description, puis sauvegarder | Sauvegarder doit nous ramener sur la page des decks                                                   |
| Assert  | Vérifier que le deck est crée                        | Le deck doit figurer sur la page avec les autres                                                      |

Résultat :  
[ ] OK  
[ ] KO

Remarque :

>

## Test 2 - Éditer un deck

|             En-tête              |     Valeur     |
| :------------------------------: | :------------: |
| Version des l'application testée |     2.0.0      |
|           Date du test           |   29.05.2026   |
|          Nom du testeur          | Jonathan Junod |

| Étape   | Description                                                                           | Remarque                                                          |
| :------ | :------------------------------------------------------------------------------------ | :---------------------------------------------------------------- |
| Arrange | Créer 2 decks de noms différents, cliquer sur l'un d'eux, puis cliquer sur 'modifier' | Cliquer sur un deck montre les cartes, 'modifier' modifie le deck |
| Act     | Sur la page du formulaire, entrer des informations différentes que précédement        | Sauvegarder doit nous ramener sur la page des decks               |
| Assert  | Vérifier que le bonn deck est modifié                                                 |                                                                   |

Résultat :  
[ ] OK  
[ ] KO

Remarque :

>

## Test 3 - Tester le lien avec les cartes

|             En-tête             |     Valeur     |
| :-----------------------------: | :------------: |
| Version des l'applicatin testée |     2.0.0      |
|          Date du test           |   29.05.2026   |
|         Nom du testeur          | Jonathan Junod |

| Étape   | Description                                                                               | Remarque                                                      |
| :------ | :---------------------------------------------------------------------------------------- | :------------------------------------------------------------ |
| Arrange | Créer 2 decks.                                                                            | Les decks doivent avoir des noms différents.                  |
| Act     | Cliquer sur l'un des decks, puis cliquer sur '+'. Enfin créer une nouvelle carte.         | Sauvegarder doit nous ramener sur la page des cartes du deck. |
| Assert  | Vérifier que la carte est créé. Revenir en arrière et vérifier que l'autre deck est vide. | Le deck doit figurer sur la page avec les autres.             |

Résultat :  
[ ] OK  
[ ] KO

Remarque :

>
