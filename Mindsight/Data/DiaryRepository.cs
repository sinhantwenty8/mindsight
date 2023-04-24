using System;
using SQLite;
namespace MindSight;

public class DiaryRepository
{
	string _dbPath;

	public string StatusMessage { get; set; }

	private SQLiteAsyncConnection conn;

	//Set up the database and establish connection
	private async Task Init()
	{
		//Check if connection already established
		if (conn != null)
			return;
		conn = new SQLiteAsyncConnection(_dbPath);

		//Create table for storing diaries
		await conn.CreateTableAsync<Diary>();
	}

	//Constructor for the class
	public DiaryRepository(string dbPath)
	{
		_dbPath = dbPath;
	}

	//Add new diary to the databse
	public async Task AddNewDiary(string content,string date,string moodImg)
	{
		int result = 0;
		try
		{
			await Init();

			//If either parameter is null or empty, an exception is thrown

			if (string.IsNullOrEmpty(date))
				throw new Exception("Please choose a date");

			//The result of the insertion is stored in the result variable
			result = await conn.InsertAsync(new Diary { Content = content, Date = date, MoodImage = moodImg });


			StatusMessage = string.Format("{0} record(s) added [Content:{1},[Date:{2})", result, content, date);

		}catch(Exception ex)
		{
			//Error message is stored in the StatusMessage variable
			StatusMessage = string.Format("Failed to add {0}. Error: {1}", content, ex.Message);
		}
	}

	public async Task<List<Diary>> GetAllDiary()
	{
		try
		{
			await Init();
			return await conn.Table<Diary>().ToListAsync();
		}catch(Exception ex)
		{
			StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
		};

		return new List<Diary>();
	}


	public async Task UpdateDiary(string content, string date, string moodImg)
	{
		int result = 0;
		try
		{
			await Init();

			var diary = await conn.FindAsync<Diary>(d => d.Date == date);

			diary.Content = content;
			diary.MoodImage = moodImg;

			result = await conn.UpdateAsync(diary);

            StatusMessage = string.Format("{0} record(s) updated [Content:{1},[Date:{2})", result, content, date);

		}
		catch (Exception ex)
		{
			StatusMessage = string.Format("Failed to update {0}. Error: {1}", content, ex.Message);
		}
	}
}

