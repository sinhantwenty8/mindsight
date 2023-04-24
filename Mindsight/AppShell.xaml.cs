namespace MindSight;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("articleDetailedPage", typeof(ArticleDetailedPage));
        Routing.RegisterRoute("insightsPage", typeof(InsightsPage));
        Routing.RegisterRoute("calendarPage", typeof(CalendarPage));
        Routing.RegisterRoute("journalPage", typeof(JournalPage));
        Routing.RegisterRoute("filterPage", typeof(FilterPage));
        Routing.RegisterRoute("editGoalPage", typeof(EditGoalPage));
        Routing.RegisterRoute("viewGoalsPage", typeof(ViewGoalsPage));
        Routing.RegisterRoute("mainPage", typeof(MainPage));
        Routing.RegisterRoute("profilePage", typeof(Profile));
        Routing.RegisterRoute("faqPage", typeof(FAQ));
        Routing.RegisterRoute("supportPage", typeof(SupportPage));
        Routing.RegisterRoute("aboutPage", typeof(AboutPage));
    }
}
