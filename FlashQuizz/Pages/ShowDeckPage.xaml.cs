using FlashQuizz.Models;
using FlashQuizz.Services;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace FlashQuizz
{
    public partial class ShowDeckPage : ContentPage, IQueryAttributable
    {
        private DeckService _dataService;
        private Deck _deck;
        private ObservableCollection<Card> _cards;

        public ShowDeckPage()
        {
            InitializeComponent();
            _cards = new();
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
                _cards = deck.Cards;
                CardsCollectionView.ItemsSource = _cards;
                title.Title = $"Cartes du deck {_deck.Name}";
            }
            if (query.TryGetValue("dataService", out object? serviceObj) && serviceObj is DeckService service)
            {
                _dataService = service;
            }
        }

        /// <summary>
        /// Navigate to the card's form page (to create a new card)
        /// </summary>
        public async void OnAddCardClicked(object sender, EventArgs e)
        {            
            Dictionary<string, object> navigationParameters = new Dictionary<string, object>
            {
                { "deck", _deck },
                { "dataService", _dataService },
                { "card", null }
            };
            await Shell.Current.GoToAsync("EditCard", navigationParameters);
        }

        /// <summary>
        /// Allow the user to make a research
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = e.NewTextValue?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                CardsCollectionView.ItemsSource = _cards;
            }
            else
            {
                List<Card> filtered = _cards.Where(d =>
                    d.Question.ToLower().Contains(searchText)
                ).ToList();
                CardsCollectionView.ItemsSource = filtered;
            }
        }
    }
}