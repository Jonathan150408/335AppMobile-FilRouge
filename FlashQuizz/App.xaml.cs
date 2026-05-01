using FlashQuizz;

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

            MainPage = new AppShell();
        }
    }
}