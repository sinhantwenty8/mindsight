using Syncfusion.Maui.Core.Hosting;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;

namespace MindSight;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder.ConfigureSyncfusionCore();
        builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
        SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
        string diaryDbPath = FileAccessHelper.GetLocalFilePath("diary1.db3");
		string bookmarkDbPath = FileAccessHelper.GetLocalFilePath("bookmarktest.db3");
        string goalDbPath = FileAccessHelper.GetLocalFilePath("goaltesting1.db3");
        builder.Services.AddSingleton<DiaryRepository>(s => ActivatorUtilities.CreateInstance<DiaryRepository>(s, diaryDbPath));
        builder.Services.AddSingleton<BookmarkedRepository>(s => ActivatorUtilities.CreateInstance<BookmarkedRepository>(s, bookmarkDbPath));
        builder.Services.AddSingleton<GoalRepository>(s => ActivatorUtilities.CreateInstance<GoalRepository>(s, goalDbPath));

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddSingleton(AudioManager.Current);
        builder.Services.AddTransient<MainPage>();

        return builder.Build();
	}
}
