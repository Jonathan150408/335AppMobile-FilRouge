using FlashQuizz;
using FlashQuizz.Pages;

namespace FlashQuizz
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Register navigation routes
            Routing.RegisterRoute("EditDeck", typeof(EditDeckPage));
            Routing.RegisterRoute("EditCard", typeof(EditCardPage));
            Routing.RegisterRoute("ShowCard", typeof(ShowCardPage));
            Routing.RegisterRoute("ShowDeck", typeof(ShowDeckPage));

            MainPage = new AppShell();
        }
    }
}