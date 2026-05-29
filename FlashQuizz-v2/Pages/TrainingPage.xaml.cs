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
    /// The list of cards to train and it's stats
    /// </summary>
    private List<CardStats> _cardsStats;

    /// <summary>
    /// Determines wether we show the question (if the, only the question if shown)
    /// </summary>
    private bool showQuestion = true;

    /// <summary>
    /// Th index od _cardsStats we are testing
    /// </summary>
    private int _currentPosition = 0;

    /// <summary>
    /// The number of correct answers given
    /// </summary>
    private int _correctAnswers = 0;

    /// <summary>
    /// Total number of attempts
    /// </summary>
    private int _totalAnswers = 0;

    /// <summary>
    /// A simple chrono to mesure the time taken
    /// </summary>
    private Stopwatch startTime;

    /// <summary>
    /// Constructor
    /// </summary>
    public TrainingPage()
    {
        InitializeComponent();

        //start the chrono
        startTime = new Stopwatch();
        startTime.Start();

        //create a new instance
        _cardsStats = new List<CardStats>();

        //set up the UI
        EvalButtons.IsVisible = false;
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

            //each card get added to the stats list
            foreach (Card c in _deck.Cards)
            {
                _cardsStats.Add(new CardStats(c));
            }

            rotateButton.Text = _cardsStats[_currentPosition].Card.Question;

        }
    }

    /// <summary>
    /// Pick a random card in the list _cardStats that is not validated
    /// </summary>
    /// <returns>The index of the card</returns>
    private int PickRandomAllowedCard()
    {
        int index = new Random().Next(0, _cardsStats.Count);

        //loop inside the list until a non-validated card is found
        while (_cardsStats[index].IsDone)
        {
            index = (index + 1) % _cardsStats.Count;
        }

        return index;
    }

    /// <summary>
    /// Rotates the card when the card is hit, let the user see the answer
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public async void OnRotateClicked(object sender, EventArgs e)
    {
        //rotate the card 1 time on 2 i each direction
        if (showQuestion)
        {
            await rotateButton.RotateYTo(360, 180, Easing.Linear);
        }
        else
        {
            await rotateButton.RotateYTo(0, 180, Easing.Linear);
        }

        //update text and buttons
        showQuestion = !showQuestion;
        if (showQuestion)
        {
            rotateButton.Text = _cardsStats[_currentPosition].Card.Question;
            EvalButtons.IsVisible = false;
        }
        else
        {
            rotateButton.Text = _cardsStats[_currentPosition].Card.Answer;
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
        //update the card's stats
        _cardsStats[_currentPosition].IsDone = true;
        _cardsStats[_currentPosition].NumberOTrials++;

        //update the global stats
        _correctAnswers++;
        _totalAnswers++;

        //update the ui
        UpdateUI();
    }

    /// <summary>
    /// Increments the counter and update UI
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddWrongAnswer(object sender, EventArgs e)
    {
        //update the card's stats
        _cardsStats[_currentPosition].NumberOTrials++;

        //update the global stats
        _totalAnswers++;

        //update the ui
        UpdateUI();
    }

    /// <summary>
    /// Updates the shown infos and if no cards are left, navigates to the stats page
    /// </summary>
    private async void UpdateUI()
    {
        //update the average
        double average = (double)_correctAnswers / (double)_totalAnswers * 100;
        average = Math.Round(average);
        correctResponsesAverage.Text = $"{average} % de bonnes réponses";

        //update the displayed card or go to stats if no cards left
        if (_cardsStats.FindAll(c => c.IsDone == false).Count == 0)
        {
            //training finished
            startTime.Stop();

            //naviguer pour commencer l'entrainement
            Dictionary<string, object> navigationParameter = new Dictionary<string, object>
            {
                { "timeElapsed", startTime.ElapsedMilliseconds },
                { "deck", _deck},
                { "average", (int)average },
                { "cardsStats", _cardsStats }
            };
            await Shell.Current.GoToAsync("Stats", navigationParameter);
        }
        else
        {
            //next card
            _currentPosition = PickRandomAllowedCard();

            rotateButton.Text = _cardsStats[_currentPosition].Card.Question;
            showQuestion = true;
            EvalButtons.IsVisible = false;
        }
    }
}