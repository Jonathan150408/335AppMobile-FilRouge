using FlashQuizz_v2.Models;
using FlashQuizz_v2.Services;
using System.Collections.ObjectModel;

namespace FlashQuizz_v2.Pages
{
    public partial class EditDeckPage : ContentPage, IQueryAttributable
    {
        private Deck _deck;
        private int _cardCount;
        private DeckService _dataService;
        private ObservableCollection<Deck> _decks;
        //handle the deck creation
        private bool isNew = false;
        public EditDeckPage()
        {
            InitializeComponent();
        }

        // Receive navigation parameters
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("deck", out object? deckObj) && deckObj is Deck deck)
            {
                _deck = deck;
                _cardCount = deck.CardCount;

                // Initialize fields
                NameEntry.Text = deck.Name;
                CardCountLabel.Text = _cardCount.ToString();
            }

            if (query.TryGetValue("dataService", out object? serviceObj) && serviceObj is DeckService service)
            {
                _dataService = service;
            }

            if (query.TryGetValue("decks", out object? decksObj) && decksObj is ObservableCollection<Deck> decks)
            {
                _decks = decks;
            }

            //if the deck is not in the list we add it
            if (!_decks.ToList().Any(d => d.Id == _deck.Id))
            {
                isNew = true;
                _decks.Add(_deck);
            }
        }

        /// <summary>
        /// /Temp/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIncrementClicked(object sender, EventArgs e)
        {
            _cardCount++;
            CardCountLabel.Text = _cardCount.ToString();
        }

        /// <summary>
        /// /Temp/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDecrementClicked(object sender, EventArgs e)
        {
            if (_cardCount > 0)
            {
                _cardCount--;
                CardCountLabel.Text = _cardCount.ToString();
            }
        }

        /// <summary>
        /// Save the deck
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string? newName = NameEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(newName))
            {
                await DisplayAlert("Erreur", "Le nom ne peut pas être vide", "OK");
                return;
            }

            // Update deck (ObservableCollection détecte le changement si on remplace l'objet)
            _deck.Name = newName;
            _deck.CardCount = _cardCount;

            // Save to JSON
            await _dataService.SaveDecksAsync(_decks.ToList());

            await Shell.Current.GoToAsync("..");
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            //remove the new deck
            if (isNew)
            {
                _decks.RemoveAt(_decks.ToList().FindIndex(d => d.Id == _deck.Id));
            }

            await Shell.Current.GoToAsync("..");
        }
    }
}