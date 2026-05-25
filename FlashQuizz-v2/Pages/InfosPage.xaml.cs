namespace FlashQuizz_v2.Pages;

public partial class InfosPage : ContentPage
{
	private string _authorMessage = "I created Flashcards because I believe learning should be accessible and enjoyable for everyone.\n\n" +
		"This app is built with the goal of helping students achieve their academic goals through proven study techniques.\n\n" +
		"Happy learning!";
	private string _appMessage = "Flashcards is designed to help you master any subject through active recall and spaced repetition.\n\n" +
		"Create your own flashcard decks, track your progress, and make learning more effective.\n\n" +
		"Whether you're learning a new language, preparing for exams, or just expanding your knowledge, StudyCards makes it simple and fun to retain information.";
    public InfosPage()
	{
		InitializeComponent();

		AppMessage.Text = _appMessage;
		AuthorMessage.Text = _authorMessage;
    }
}