using FlashQuizz.Models;
using FlashQuizz.Services;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace FlashQuizz
{
    public partial class ShowDeckPage : ContentPage, IQueryAttributable
    {
        private CardService _dataService;
        private Deck _deck;

        public ShowDeckPage()
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
            if (query.TryGetValue("deck", out object? deckObj) && deckObj is Deck deck)
            {
                _deck = deck;
                CardsCollectionView.ItemsSource = deck.Cards;
            }
            title.Title = $"Cartes du deck {_deck.Name}";
            //if (query.TryGetValue("dataService", out object? serviceObj) && serviceObj is CardService service)
            //{
            //    _dataService = service;
            //}
        }
    }
}