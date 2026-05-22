using FlashQuizz_v2.Models;
using FlashQuizz_v2.Services;
using System.Collections.ObjectModel;

namespace FlashQuizz_v2.Pages
{
    public partial class EditCardPage : ContentPage, IQueryAttributable
    {
        private Card _card;
        private CardService _dataService;
        private ObservableCollection<Card> _cards;
        public EditCardPage()
        {
            InitializeComponent();
        }

        // Receive navigation parameters
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("card", out object? cardObj) && cardObj is Card card)
            {
                _card = card;

                // Initialize fields
                QuestionEntry.Text = card.Question;
                AnswerEntry.Text = card.Answer;
            }

            if (query.TryGetValue("dataService", out object? serviceObj) && serviceObj is CardService service)
            {
                _dataService = service;
            }

            if (query.TryGetValue("cards", out object? cardsObj) && cardsObj is ObservableCollection<Card> cards)
            {
                _cards = cards;
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

            // Update card (ObservableCollection détecte le changement si on remplace l'objet)
            _card.Question = newQuestion;
            _card.Answer = newAnswer;

            // Save to JSON
            await _dataService.SaveCardsAsync(_cards.ToList());

            await Shell.Current.GoToAsync("..");
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}