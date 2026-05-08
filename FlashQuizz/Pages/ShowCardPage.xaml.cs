using FlashQuizz.Models;
using FlashQuizz.Services;
using System.Collections.ObjectModel;

namespace FlashQuizz.Pages;

public partial class ShowCardPage : ContentPage, IQueryAttributable
{
    private Card _card;
    private CardService _dataService;
    private bool _showQuestion = true;
    private int correctCount = 0;
    private int wrongCount = 0;
    public ShowCardPage()
	{
		InitializeComponent();
	}

    /// <summary>
    /// Get the navigation parameters
    /// </summary>
    /// <param name="query"></param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        //find the card
        if (query.TryGetValue("card", out object? cardObj) && cardObj is Card card)
        {
            _card = card;
            rotateButton.Text = card.Question;
        }
        if (query.TryGetValue("dataService", out object? serviceObj) && serviceObj is CardService service)
        {
            _dataService = service;
        }
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

        //update le texte
        _showQuestion = !_showQuestion;
        if (_showQuestion)
        {
            rotateButton.Text = _card.Question;
        }
        else
        {
            rotateButton.Text = _card.Answer;
        }

    }

    private void addCorrectAnswer(object sender, EventArgs e)
    {
        correctCount++;
    }
    private void addWrongAnswer(object sender, EventArgs e)
    {
        wrongCount++;
    }
    private void updateCorrectAnswerAverage(object sender, EventArgs e)
    {
        double average = correctCount / wrongCount * 100;
        average = Math.Round(average, 0);
        correctResponsesAverage.Text = $"{average} %";
    }
}