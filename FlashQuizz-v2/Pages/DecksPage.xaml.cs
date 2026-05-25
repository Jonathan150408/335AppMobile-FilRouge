using System.Collections.ObjectModel;
using System.Xml.Linq;
using FlashQuizz_v2.Models;
using FlashQuizz_v2.Services;

namespace FlashQuizz_v2.Pages
{
    public partial class DecksPage : ContentPage
    {
        private DeckService _dataService;
        private ObservableCollection<Deck> _decks;
        private int _nextId = 1;

        public DecksPage()
        {
            InitializeComponent();
            _dataService = new DeckService();
            _decks = new ObservableCollection<Deck>();
            LoadDecks();
        }

        private async void LoadDecks()
        {
            List<Deck> loadedDecks = await _dataService.LoadDecksAsync();

            // Clear and repopulate ObservableCollection
            _decks.Clear();
            foreach (Deck deck in loadedDecks)
            {
                _decks.Add(deck);
            }

            if (_decks.Any())
            {
                _nextId = _decks.Max(d => d.Id) + 1;
            }

            // Assign ItemsSource ONCE (no need to reassign every time)
            if (DecksCollectionView.ItemsSource == null)
            {
                DecksCollectionView.ItemsSource = _decks;
            }
        }

        /// <summary>
        /// Permet de créer/ajouter de nouveaux decks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnAddDeckClicked(object sender, EventArgs e)
        {
            _nextId++;
            Deck deck = new Deck
            {
                Id = _nextId,
                Name = "",
                CardCount = 0
            };

            // Pass deck, dataService and decks list so EditDeckPage can save
            Dictionary<string, object> navigationParameter = new Dictionary<string, object>
            {
                { "deck", deck },
                { "dataService", _dataService },
                { "decks", _decks }
            };
            await Shell.Current.GoToAsync("EditDeck", navigationParameter);
        }
        /// <summary>
        /// Permet de mettre un deck à jour (uniquement le nom puisque seul le nom est éditable)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnEditDeckClicked(object sender, EventArgs e)
        {
            Button? button = sender as Button;
            Deck? deck = button?.CommandParameter as Deck;

            if (deck == null) return;

            // Navigate to edit page using Shell
            // Pass deck, dataService and decks list so EditDeckPage can save
            Dictionary<string, object> navigationParameter = new Dictionary<string, object>
            {
                { "deck", deck },
                { "dataService", _dataService },
                { "decks", _decks }
            };
            await Shell.Current.GoToAsync("EditDeck", navigationParameter);
        }

        // Refresh view when returning from edit page
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //manuallay refresh (usefull for the edit action)
            DecksCollectionView.ItemsSource = null;
            DecksCollectionView.ItemsSource = _decks;
        }
        /// <summary>
        /// Permet de Delete un deck (on sait lequel en se basant sur quel bouton à été cliqué)
        /// </summary>
        /// <param name="sender">Le bouton déterminant quel deck sera supprimé</param>
        /// <param name="e"></param>
        private async void OnDeleteDeckClicked(object sender, EventArgs e)
        {
            Button? button = sender as Button;
            Deck? deck = button?.CommandParameter as Deck;

            if (deck == null) return;

            bool confirm = await DisplayAlert(
                "Confirmation",
                $"Voulez-vous vraiment supprimer '{deck.Name}' ?",
                "Supprimer",
                "Annuler"
            );

            if (!confirm) return;

            _decks.Remove(deck);  // La vue se met à jour automatiquement !
            await _dataService.SaveDecksAsync(_decks.ToList());
        }

        // Search filter
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = e.NewTextValue?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                DecksCollectionView.ItemsSource = _decks;
            }
            else
            {
                List<Deck> filtered = _decks.Where(d =>
                    d.Name.ToLower().Contains(searchText)
                ).ToList();
                DecksCollectionView.ItemsSource = filtered;
            }
        }

        /// <summary>
        /// Permet de montrer toutes les cartes d'un deck (afin de edit et delete = gérer le deck)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnDeckClicked(object sender, EventArgs e)
        {
            //naviguer pour show toutes les cartes sur une nouvelle page
            Grid? grid = sender as Grid;
            Deck? deck = grid?.BindingContext as Deck;

            //Deck? deck = sender.BindingContext as Deck;

            if (deck == null)
            {
                return;
            }
            Dictionary<string, object> navigationParameter = new Dictionary<string, object>
            {
                { "deck", deck },
                { "dataService", _dataService }
            };
            await Shell.Current.GoToAsync("CardsPage", navigationParameter);
        }

        /// <summary>
        /// Starts the training
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnStartTrainingClicked(object sender, EventArgs e)
        {
            Button? button = sender as Button;
            Deck? deck = button?.CommandParameter as Deck;

            if (deck == null)
            {
                return;
            }

            //naviguer pour commencer l'entrainement
            Dictionary<string, object> navigationParameter = new Dictionary<string, object>
            {
                { "deck", deck }
            };
            await Shell.Current.GoToAsync("TrainingPage", navigationParameter);
        }
    }
}