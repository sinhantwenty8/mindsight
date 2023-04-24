using System;
using SQLite;

namespace MindSight
{
	public class GoalRepository
	{
        string _dbPath;

        public string StatusMessage { get; set; }

        private SQLiteAsyncConnection conn;

        private static int goalID = 0;

        private async Task Init()
        {
            //Check if connection already established
            if (conn != null)
                return;
            conn = new SQLiteAsyncConnection(_dbPath);

            //Create table for storing goal
            await conn.CreateTableAsync<Goal>();
        }

        public GoalRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        //Add new goal to the databse
        public async Task AddNewGoal(string targetTitle, string targetContent, string color, string daysOfTheWeek,string iconImage, int accumulateDays,string todayIsChecked,string lastUpdatedDate = "")
        {
            int result = 0;
            try
            {
                await Init();

                //If either parameter is null or empty, an exception is thrown

                if (string.IsNullOrEmpty(targetTitle))
                    throw new Exception("Title is empty");

                if (string.IsNullOrEmpty(targetContent))
                    throw new Exception("Content is empty");

                if (string.IsNullOrEmpty(color))
                    throw new Exception("Color selection is empty");

                if (string.IsNullOrEmpty(targetContent))
                    throw new Exception("Days of the week is empty");

                if (string.IsNullOrEmpty(iconImage))
                    throw new Exception("Icon image is empty");

                if(int.IsNegative(accumulateDays))
                    throw new Exception("Accumulate days is not empty");

                //The result of the insertion is stored in the result variable
                result = await conn.InsertAsync(new Goal {Id = goalID,TargetTitle = targetTitle, TargetContent = targetContent, Color = color, DaysOfTheWeek = daysOfTheWeek, IconImage = iconImage, AccumulateDays = accumulateDays, TodayIsChecked = todayIsChecked,LastUpdatedDate = lastUpdatedDate});


                StatusMessage = "Added Successfully";

                goalID++;

            }
            catch (Exception ex)
            {
                //Error message is stored in the StatusMessage variable
                StatusMessage = "Error In Adding";
            }
        }

        public async Task<List<Goal>> GetAllGoals()
        {
            try
            {
                await Init();
                return await conn.Table<Goal>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            };

            return new List<Goal>();
        }


        public async Task UpdateGoal(int id,string targetTitle, string targetContent, string color, string daysOfTheWeek, string iconImage, int accumulateDays,string todayIsChecked, string lastUpdatedDate)
        {
            int result = 0;
            try
            {
                await Init();

                var goal = await conn.FindAsync<Goal>(g => g.Id == id);

                goal.TargetTitle = targetTitle;
                goal.TargetContent = targetContent;
                goal.Color = color;
                goal.DaysOfTheWeek = daysOfTheWeek;
                goal.IconImage = iconImage;
                goal.AccumulateDays = accumulateDays;
                goal.TodayIsChecked = todayIsChecked;
                goal.LastUpdatedDate = lastUpdatedDate;

                result = await conn.UpdateAsync(goal);

                StatusMessage = "Updated Successfully";

            }
            catch (Exception ex)
            {
                StatusMessage = "Error in updating";
            }
        }

        public async Task DeleteGoal(int id)
        {
            int result = 0;

            try
            {
                var goal = await conn.FindAsync<Goal>(g => g.Id == id);
                result = await conn.DeleteAsync(goal);
                StatusMessage = "Deleted Successfully";
            }
            catch (Exception ex)
            {
                StatusMessage = "Error in deleting";
            };

            
        }

    }
}

