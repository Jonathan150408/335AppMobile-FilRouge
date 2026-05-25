using FlashQuizz_v2.Models;
using FlashQuizz_v2.Services;
using System.Collections.ObjectModel;
using System.Collections;
using System.Diagnostics;

namespace FlashQuizz_v2.Pages;

public partial class TrainingPage : ContentPage, IQueryAttributable
{
    /// <summary>
    /// This is the deck to show and to use
    /// </summary>
    private Deck _deck;
    /// <summary>
    /// These are the cards the the user must do to finish
    /// </summary>
    private List<Card> cardsLeft;
    /// <summary>
    /// These are the cards validated
    /// </summary>
    private List<Card> cardsDone = new List<Card>();

    /// <summary>
    /// Dictionnary of cards + tuples | Card + (numberOfTry + isCorrect)
    /// </summary>
    //private Dictionary<Card, (int, bool)> _cards;
    private List<(Card, int, bool)> _cards;

    private bool showQuestion = true;
    private int currentPosition = 0;
    private int cardsCount;

    //counters for stats
    private int firstTryCounter = 0;
    private int correctCounter = 0;
    private int wrongCounter = 0;
    private Stopwatch startTime;

    public TrainingPage()
    {
        InitializeComponent();

        //start the chrono
        startTime = new Stopwatch();
        startTime.Start();
    }

    /// <summary>
    /// Get the navigation parameters
    /// </summary>
    /// <param name="query"></param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("deck", out object? deckObj)  && deckObj is Deck deck)
        {
            //deck is used to navigate
            _deck = deck;
            //others variables used in this page
            cardsLeft = Shuffle(deck.Cards.ToList());
            cardsCount = deck.Cards.Count;
            currentPosition = new Random().Next(0, cardsCount);
            rotateButton.Text = cardsLeft[currentPosition].Question;

            //the list of card
            _cards = new Dictionary<Card, (int, bool)>();
            foreach (Card c in _deck.Cards)
            {
                _cards.Add(c, (0, false));
            }
        }
    }

    private int PickRandomAllowedCard()
    {
        int index = new Random().Next(0, _cards.Count);

        //loop inside the list until a non-validated card is found
        while (_cards[index].Item2)
        {
            index = (index + 1) % _cards.Count;
        }

        return index;
    }


    /// <summary>
    /// a random value used to shuffle
    /// </summary>
    private static readonly Random rand = new Random();
    /// <summary>
    /// Shuffle the list of cards
    /// Source - https://stackoverflow.com/a/1262619
    /// Posted by grenade, modified by community. See post 'Timeline' for change history
    /// Retrieved 2026-05-22, License - CC BY-SA 4.0
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static List<Card> Shuffle(List<Card> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            Card value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }


    public async void OnRotateClicked(object sender, EventArgs e)
    {
        //trouver le "sens" de la carte
        double angle = rotateButton.RotationY;

        //tourner la carte
        //await rotateButton.RotateYTo(angle + 360, 100, Easing.Linear);
        if (angle == 0)
        {
            await rotateButton.RotateYTo(360, 180, Easing.Linear);
        }
        else if (angle == 360)
        {
            await rotateButton.RotateYTo(0, 180, Easing.Linear);
        }

        //update text and buttons
        showQuestion = !showQuestion;
        if (showQuestion)
        {
            rotateButton.Text = _cards[currentPosition].Item1.Question;
            EvalButtons.IsVisible = false;
        }
        else
        {
            rotateButton.Text = _cards[currentPosition].Item1.Answer;
            EvalButtons.IsVisible = true;
        }

    }

    /// <summary>
    /// Increments the counter and update the lists + UI
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddCorrectAnswer(object sender, EventArgs e)
    {
        correctCounter++;
        //_cards[currentPosition].Item2;
        _cards[currentPosition].Item3 = true;
        UpdateUI();
    }

    /// <summary>
    /// Increments the counter and update UI
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddWrongAnswer(object sender, EventArgs e)
    {
        wrongCounter++;
        UpdateUI();
    }

    /// <summary>
    /// Updates the shown infos and if no cards are left, navigates to the stats page
    /// </summary>
    private async void UpdateUI()
    {
        //update the average
        double average = correctCounter / (currentPosition + 1) * 100;
        average = Math.Round(average, 0);
        correctResponsesAverage.Text = $"{average} %";

        //update the displayed card or go to stats if no cards left
        if (cardsLeft.FindAll(c => c != null).Count == 0)
        {
            //training finished
            startTime.Stop();

            //naviguer pour commencer l'entrainement
            Dictionary<string, object> navigationParameter = new Dictionary<string, object>
            {
                { "timeElapsed", startTime.ElapsedMilliseconds },
                { "deck", _deck},
                { "knownCards", firstTryCounter },
                { "goodAnswersAverage", average }
            };
            await Shell.Current.GoToAsync("Stats", navigationParameter);
        }
        else
        {
            //next card
            do
            {
                currentPosition = new Random().Next(0, cardsCount);
            } while (cardsLeft[currentPosition] == null);
            rotateButton.Text = cardsLeft[currentPosition].Question;
            showQuestion = true;
            EvalButtons.IsVisible = false;
        }
    }
}