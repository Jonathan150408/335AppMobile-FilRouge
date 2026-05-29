using FlashQuizz_v2.Models;
using FlashQuizz_v2.Services;
using System.Collections.ObjectModel;

namespace FlashQuizz_v2.Pages
{
    public partial class EditDeckPage : ContentPage, IQueryAttributable
    {
        /// <summary>
        /// The deck to edit (or a new deck if we create)
        /// </summary>
        private Deck _deck;

        /// <summary>
        /// The dataservice use to save and delete
        /// </summary>
        private DeckService _dataService;

        /// <summary>
        /// All decks
        /// </summary>
        private ObservableCollection<Deck> _decks;

        /// <summary>
        /// A booleean used to handle the deck creation
        /// </summary>
        private bool isNew = false;

        /// <summary>
        /// Defaukt contructor
        /// </summary>
        public EditDeckPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Receive navigation parameters
        /// </summary>
        /// <param name="query"></param>
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("deck", out object? deckObj) && deckObj is Deck deck)
            {
                _deck = deck;

                // Initialize fields
                NameEntry.Text = deck.Name;
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
                DeleteButton.IsVisible = false;
                _decks.Add(_deck);
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

            // Update deck
            _deck.Name = newName;

            // Save to JSON
            await _dataService.SaveDecksAsync(_decks.ToList());

            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// Cancels the operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnCancelClicked(object sender, EventArgs e)
        {
            //remove the new deck
            if (isNew)
            {
                _decks.RemoveAt(_decks.ToList().FindIndex(d => d.Id == _deck.Id));
            }

            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// Delete the deck
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            await _dataService.DeleteDeckByIdAsync(_deck.Id);
            await Shell.Current.GoToAsync("Home");
        }
    }
}