using FlashQuizz_v2.Models;
using FlashQuizz_v2.Services;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace FlashQuizz_v2.Pages
{
    public partial class CardsPage : ContentPage
    {
        private CardService _dataService;
        private ObservableCollection<Card> _cards;
        private int _nextId = 1;

        public CardsPage()
        {
            InitializeComponent();
            _dataService = new CardService();
            _cards = new ObservableCollection<Card>();
            LoadCards();
        }

        private async void LoadCards()
        {
            List<Card> loadedCards = await _dataService.LoadCardsAsync();

            // Clear and repopulate ObservableCollection
            _cards.Clear();
            foreach (Card Card in loadedCards)
            {
                _cards.Add(Card);
            }

            if (_cards.Any())
            {
                _nextId = _cards.Max(d => d.Id) + 1;
            }

            // Assign ItemsSource ONCE (no need to reassign every time)
            if (CardsCollectionView.ItemsSource == null)
            {
                CardsCollectionView.ItemsSource = _cards;
            }

            UpdateInfo($"Chargé: {_cards.Count} Card(s)");
        }
        /// <summary>
        /// met les infos à jour
        /// </summary>
        /// <param name="message"></param>
        private void UpdateInfo(string message)
        {
            InfoLabel.Text = $"{DateTime.Now:HH:mm:ss} - {message}";
        }

        /// <summary>
        /// Permet de créer/ajouter de nouveaux Cards
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnAddCardClicked(object sender, EventArgs e)
        {
            string? question = NewCardQuestionEntry.Text?.Trim();
            string? answer = NewCardAnswerEntry.Text?.Trim();

            if (string.IsNullOrEmpty(question))
            {
                await DisplayAlert("Erreur", "Veuillez entrer une question", "OK");
                return;
            }
            else if (string.IsNullOrEmpty(answer))
            {
                await DisplayAlert("Erreur", "Veuillez entrer une réponse", "OK");
                return;
            }

            Card newCard = new Card
            {
                Id = _nextId++,
                Question = question,
                Answer = answer
            };

            _cards.Add(newCard);  // La vue se met à jour automatiquement !
            await _dataService.SaveCardsAsync(_cards.ToList());

            NewCardQuestionEntry.Text = string.Empty;
            NewCardAnswerEntry.Text = string.Empty;
            UpdateInfo($"Ajouté: {question}");
        }
        /// <summary>
        /// Permet de mettre un Card à jour (uniquement le nom puisque seul le nom est éditable)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnEditCardClicked(object sender, EventArgs e)
        {
            Button? button = sender as Button;
            Card? Card = button?.CommandParameter as Card;

            if (Card == null) return;

            // Navigate to edit page using Shell
            // Pass Card, dataService and Cards list so EditCardPage can save
            Dictionary<string, object> navigationParameter = new Dictionary<string, object>
            {
                { "Card", Card },
                { "dataService", _dataService },
                { "Cards", _cards }
            };
            await Shell.Current.GoToAsync("EditCard", navigationParameter);

            UpdateInfo($"Modifié: {Card}");
        }

        // Refresh view when returning from edit page
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //sort the Cards
            //sortCards();

            // Nécessaire pour refléter les modifications de propriétés (ex: Card.Question changé dans EditCardPage)
            // ObservableCollection détecte les ajouts/suppressions, mais PAS les changements de propriétés
            //RefreshView();
        }
        /// <summary>
        /// Permet de Delete un Card (on sait lequel en se basant sur quel bouton à été cliqué)
        /// </summary>
        /// <param name="sender">Le bouton déterminant quel Card sera supprimé</param>
        /// <param name="e"></param>
        private async void OnDeleteCardClicked(object sender, EventArgs e)
        {
            Button? button = sender as Button;
            Card? Card = button?.CommandParameter as Card;

            if (Card == null) return;

            bool confirm = await DisplayAlert(
                "Confirmation",
                $"Voulez-vous vraiment supprimer '{Card.Question}' ?",
                "Supprimer",
                "Annuler"
            );

            if (!confirm) return;

            _cards.Remove(Card);  // La vue se met à jour automatiquement !
            await _dataService.SaveCardsAsync(_cards.ToList());

            UpdateInfo($"Supprimé: {Card.Question}");
        }

        ///améliorations

        //sort the Cards
        private void sortCards()
        {
            _cards.OrderBy(d => d.Question);
        }

        // Search filter
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

        //show a card
        private async void OnCardClicked(object sender, EventArgs e)
        {
            UpdateInfo("En train de naviguer");

            //naviguer pour show la carte sur une nouvelle page
            Button? button = sender as Button;
            Card? card = button?.CommandParameter as Card;

            UpdateInfo($"En train de naviguer vers : {card.Question}");

            if (card == null) return;
            Dictionary<string, object> navigationParameter = new Dictionary<string, object>
            {
                { "card", card },
                { "dataService", _dataService }
            };
            await Shell.Current.GoToAsync("ShowCard", navigationParameter);
            UpdateInfo($"Navigué jusqu'à : {card}");

        }
    }
}