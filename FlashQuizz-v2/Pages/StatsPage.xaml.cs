using FlashQuizz_v2.Models;
using FlashQuizz_v2.Services;
using System.Collections.ObjectModel;

namespace FlashQuizz_v2.Pages;

public partial class StatsPage : ContentPage, IQueryAttributable
{
    private Deck _deck;
    //stats
    private int timeElapsed;
    private int firstTryCounter;
    private int average;


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
            finishMessage.Text = "Vous avez terminé de réviser " + _deck.Name;
        }
        if (query.TryGetValue("timeElapsed", out object? timeObj) && timeObj is int time)
        {
            timeElapsed = time;
        }
        if (query.TryGetValue("firstTryCounter", out object? firstTryObj) && firstTryObj is int firstTry)
        {
            firstTryCounter = firstTry;
        }
        if (query.TryGetValue("average", out object? averageObj) && averageObj is int avg)
        {
            average = avg;
        }

    }
}