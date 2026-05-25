using FlashQuizz_v2.Models;
using FlashQuizz_v2.Services;
using System.Collections.ObjectModel;

namespace FlashQuizz_v2.Pages;

public partial class StatsPage : ContentPage, IQueryAttributable
{
    private Deck _deck;
    //stats
    private int _timeElapsed;
    private int _firstTryCounter;
    private int _average;


    public StatsPage()
	{
		InitializeComponent();
	}
    /// <summary>
    /// Get the navigation parameters
    /// </summary>
    /// <param name="query"></param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("deck", out object? deckObj) && deckObj is Deck deck)
        {
            _deck = deck;
        }
        if (query.TryGetValue("timeElapsed", out object? timeObj) && timeObj is int time)
        {
            _timeElapsed = time;
        }
        if (query.TryGetValue("firstTryCounter", out object? firstTryObj) && firstTryObj is int firstTry)
        {
            _firstTryCounter = firstTry;
        }
        if (query.TryGetValue("average", out object? averageObj) && averageObj is int avg)
        {
            _average = avg;
        }

        //set up the display
        finishMessage.Text = "Vous avez terminé de réviser " + _deck.Name;
        timeElapsed.Text = (_timeElapsed / 60).ToString() + " m " + (_timeElapsed % 60).ToString() + " s";
        knownPercentage.Text = (_firstTryCounter / _deck.Cards.Count * 100).ToString() + " %";
        knownCards.Text = _firstTryCounter.ToString() + " / " + _deck.Cards.Count.ToString();
        //numberOfReview
        //cardQuestion
        //cardResponse
    }

    /// <summary>
    ///restart the training with the same deck
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Restart(object sender, EventArgs e)
    {
        Dictionary<string, object> navigationParameter = new Dictionary<string, object>
        {
            { "deck", _deck }
        };
        await Shell.Current.GoToAsync("TrainingPage", navigationParameter);
    }

    private async void LeaveTraining(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Home");
    }
}