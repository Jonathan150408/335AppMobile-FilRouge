using FlashQuizz_v2.Models;
using FlashQuizz_v2.Services;
using System.Collections.ObjectModel;

namespace FlashQuizz_v2.Pages
{
    public partial class EditCardPage : ContentPage, IQueryAttributable
    {
        private Card _card;
        private Deck _deck;
        private DeckService _dataService;
        //handle card creation
        private bool isNew = false;
        public EditCardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Receive navigation parameters
        /// </summary>
        /// <param name="query"></param>
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("card", out object? cardObj) && cardObj is Card card)
            {
                _card = card;

                // Initialize fields
                QuestionEntry.Text = card.Question;
                AnswerEntry.Text = card.Answer;
            }

            if (query.TryGetValue("dataService", out object? serviceObj) && serviceObj is DeckService service)
            {
                _dataService = service;
            }

            if (query.TryGetValue("deck", out object? deckObj) && deckObj is Deck deck)
            {
                _deck = deck;
            }

            //if the card isn't in the deck (on Add for example), we add it now
            if (!_deck.Cards.Any(c => c.Id == _card.Id))
            {
                isNew = true;
                _deck.Cards.Add(_card);
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string? newQuestion = QuestionEntry.Text?.Trim();
            string? newAnswer = AnswerEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(newQuestion))
            {
                await DisplayAlert("Erreur", "Le champ question ne peut pas être vide", "OK");
                return;
            }
            else if (string.IsNullOrWhiteSpace(newAnswer))
            {
                await DisplayAlert("Erreur", "Le champ réponse ne peut pas être vide", "OK");
                return;
            }

            // Update card
            _card.Question = newQuestion;
            _card.Answer = newAnswer;

            // Save to JSON
            await _dataService.SaveDeckAsync(_deck);

            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// Go to the previous page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnCancelClicked(object sender, EventArgs e)
        {
            //remove the new card
            if (isNew)
            {
                _deck.Cards.RemoveAt(_deck.Cards.ToList().FindIndex(c => c.Id == _card.Id));
            }

            await Shell.Current.GoToAsync("..");
        }
    }
}