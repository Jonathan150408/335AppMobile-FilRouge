using FlashQuizz.Models;
using FlashQuizz.Services;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace FlashQuizz
{
    public partial class DecksPage : ContentPage
    {
        private DeckService _dataService;
        private ObservableCollection<Deck> _decks;  // List devient ObservableCollection
        private int _nextId = 1;

        public DecksPage()
        {
            InitializeComponent();
            _dataService = new DeckService();
            _decks = new ObservableCollection<Deck>();  // new ObservableCollection
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

            UpdateInfo($"Chargé: {_decks.Count} deck(s)");
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
        /// Permet de créer/ajouter de nouveaux decks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnAddDeckClicked(object sender, EventArgs e)
        {
            string? name = NewDeckEntry.Text?.Trim();

            if (string.IsNullOrEmpty(name))
            {
                await DisplayAlert("Erreur", "Veuillez entrer un nom", "OK");
                return;
            }

            Deck newDeck = new Deck
            {
                Id = _nextId++,
                Name = name,
                CardCount = 0
            };

            _decks.Add(newDeck);  // ← La vue se met à jour automatiquement !
            await _dataService.SaveDecksAsync(_decks.ToList());

            NewDeckEntry.Text = string.Empty;
            UpdateInfo($"Ajouté: {name} et trié tous les decks");
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

            UpdateInfo($"Modifié: {deck}");
        }

        // Refresh view when returning from edit page
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //sort the decks
            //sortDecks();

            // Nécessaire pour refléter les modifications de propriétés (ex: deck.Name changé dans EditDeckPage)
            // ObservableCollection détecte les ajouts/suppressions, mais PAS les changements de propriétés
            //RefreshView();
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

            _decks.Remove(deck);  // ← La vue se met à jour automatiquement !
            await _dataService.SaveDecksAsync(_decks.ToList());

            UpdateInfo($"Supprimé: {deck.Name}");
        }

        ///améliorations

        //sort the decks
        private void sortDecks()
        {
            _decks.OrderBy(d => d.Name);
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
            UpdateInfo("En train de chercher les cartes");

            //naviguer pour show toutes les cartes sur une nouvelle page
            Button? button = sender as Button;
            Deck? deck = button?.CommandParameter as Deck;

            UpdateInfo($"En train de naviguer vers : {deck.Name}");

            if (deck == null)
            {
                UpdateInfo($"Un problème est survenu : {deck.Name} n'a pas été trouvé ou est inaccessible.");
                return;
            }
            Dictionary<string, object> navigationParameter = new Dictionary<string, object>
            {
                { "deck", deck },
                { "dataService", _dataService }
            };
            await Shell.Current.GoToAsync("ShowDeck", navigationParameter);
            UpdateInfo($"Navigué jusqu'à : {deck.Name}");

        }
    }
}