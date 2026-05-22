using FlashQuizz_v2.Pages;

namespace FlashQuizz_v2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Register navigation routes
            Routing.RegisterRoute("EditDeck", typeof(EditDeckPage));
            Routing.RegisterRoute("EditCard", typeof(EditCardPage));
            Routing.RegisterRoute("CardsPage", typeof(CardsPage));
            Routing.RegisterRoute("TrainingPage", typeof(TrainingPage));

            MainPage = new AppShell();
        }
    }
}