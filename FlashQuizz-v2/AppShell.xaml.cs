using FlashQuizz_v2.Pages;

namespace FlashQuizz_v2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            var flyout = new FlyoutItem { FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems };
            flyout.Items.Add(new ShellContent { Title = "Decks", Content = new DecksPage() });
            flyout.Items.Add(new ShellContent { Title = "Profil", Content = new ProfilePage() });
            flyout.Items.Add(new ShellContent { Title = "Infos", Content = new InfosPage() });

            Items.Add(flyout);
        }
    }
}
