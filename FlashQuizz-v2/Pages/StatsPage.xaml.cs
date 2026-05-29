using FlashQuizz_v2.Models;
using System.Collections.ObjectModel;

namespace FlashQuizz_v2.Pages;

public partial class StatsPage : ContentPage, IQueryAttributable
{
    private Deck _deck;

    /// <summary>
    /// The list with stats
    /// </summary>
    private List<CardStats> _cardStats;

    //stats
    private int _timeElapsed;
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
        if (query.TryGetValue("timeElapsed", out object? timeObj) && timeObj is long time)
        {
            _timeElapsed = (int)(time / 1000);
        }
        if (query.TryGetValue("cardsStats", out object? cardsStatsObj) && cardsStatsObj is List<CardStats> cardsStats)
        {
            _cardStats = cardsStats;
        }
        if (query.TryGetValue("average", out object? averageObj) && averageObj is int avg)
        {
            _average = avg;
        }

        //set up the display
        //message
        finishMessage.Text = "Vous avez terminé de réviser " + _deck.Name;
        //time
        timeElapsed.Text = ((int)Math.Floor((double) _timeElapsed / 60)).ToString() + " m " + (_timeElapsed % 60).ToString() + " s";
        //% of good answers
        knownPercentage.Text = "Vous avez donné " + _average.ToString() + " % de bonnes réponses";
        knownPercentageBar.Progress = ((double)_average / (double)100);
        // number of first try
        knownCards.Text = _cardStats.FindAll(c => c.NumberOTrials == 1).Count().ToString() + " / " + _deck.Cards.Count.ToString();

        //hardest card
        int hardestCardIndex = _cardStats.ToList().FindIndex(c => c.NumberOTrials == _cardStats.ToList().Max(c => c.NumberOTrials));
        numberOfReview.Text =  "Revue  " + _cardStats[hardestCardIndex].NumberOTrials.ToString() + " fois";
        cardQuestion.Text = _cardStats[hardestCardIndex].Card.Question;
        cardResponse.Text = _cardStats[hardestCardIndex].Card.Answer;
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

    /// <summary>
    /// Navigate to the main page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void LeaveTraining(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Home");
    }
}