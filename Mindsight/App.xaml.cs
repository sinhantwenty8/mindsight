namespace MindSight;

public partial class App : Application
{
	public static DiaryRepository DiaryRepo { get; private set; }
    public static BookmarkedRepository BookmarkRepo { get; private set; }
	public static GoalRepository GoalRepository { get; private set; }

    public App(DiaryRepository repo,BookmarkedRepository bookmarkedRepository,GoalRepository goalRepository)
	{
		InitializeComponent();

		MainPage = new AppShell();

		DiaryRepo = repo;

		BookmarkRepo = bookmarkedRepository;

		GoalRepository = goalRepository;
	}
}
