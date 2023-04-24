using System.Collections.ObjectModel;

namespace MindSight;

public partial class MainGoalPage : ContentPage
{
    // Declaring private variables to hold goal and task lists and a list to hold checked tasks
    private ObservableCollection<Goal> goalList;
    private ObservableCollection<Goal> taskList = new ObservableCollection<Goal>();
    private List<Goal> checkedTaskList = new List<Goal>();
    // get the current day of the week as a string
    private string currentDayOfWeek = DateTime.Now.DayOfWeek.ToString().Substring(0, 3);


    public MainGoalPage()
	{
		InitializeComponent();
        getAllGoals();
        BindingContext = goalList;
	}

    // Event handler for add goal button click
    async void btnAdd_Clicked(System.Object sender, System.EventArgs e)
    {
        // Get the image button that was clicked and navigate to the edit goal page with goalId set to "new"
        ImageButton imageButton = sender as ImageButton;
        await Shell.Current.GoToAsync("editGoalPage?goalId=new");
    }

    async void OnSectionTapped(System.Object sender, System.EventArgs e)
    {
        // Get the selected goal and navigate to the edit goal page with goalId set to the selected goal's ID
        Goal selectedGoal = (Goal)((sender as VerticalStackLayout).BindingContext);
        await Shell.Current.GoToAsync($"editGoalPage?goalId={selectedGoal.Id}");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        goalList?.Clear(); // Clear the goalList if it's not null
        getAllGoals();
    }

    async void btnViewAllGoals_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("viewGoalsPage");
    }

    private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        // cast sender as CheckBox and get the binding context
        CheckBox checkbox = sender as CheckBox;
        Goal goal = checkbox?.BindingContext as Goal;

        // get the checked status of the checkbox as a string
        String isChecked = checkbox.IsChecked.ToString();

        // if the checkbox is checked
        if (isChecked == "True")
        {
            // if the checked task list does not contain the goal
            if (!checkedTaskList.Contains(goal))
            {
                // if the goal was not updated today, update the goal's accumulated days and last updated date
                if (goal.LastUpdatedDate != DateTime.Today.ToShortDateString())
                {
                    goal.AccumulateDays++;
                    goal.LastUpdatedDate = DateTime.Today.ToShortDateString();
                    goal.TodayIsChecked = "True";
                }

                // check if the goal is scheduled for the current day of the week
                String[] daysOfWeek = goal.DaysOfTheWeek.Split(",");
                foreach (String day in daysOfWeek)
                {
                    if (day == currentDayOfWeek)
                    {
                        checkedTaskList.Add(goal);
                    }
                }
            }
        }

        // if the checkbox is unchecked
        if (isChecked == "False")
        {
            // if the checked task list contains the goal
            if (checkedTaskList.Contains(goal))
            {
                // clear the last updated date, reduce the goal's accumulated days, and remove the goal from the checked task list
                goal.LastUpdatedDate = "";
                goal.AccumulateDays--;
                goal.TodayIsChecked = "False";
                checkedTaskList.Remove(goal);
            }
        }

        // update the goal achieved count and save the goal in the database
        getGoalAchievedCount();
        await App.GoalRepository.UpdateGoal(goal.Id, goal.TargetTitle, goal.TargetContent, goal.Color, goal.DaysOfTheWeek, goal.IconImage, goal.AccumulateDays, isChecked, goal.LastUpdatedDate);
    }

    private async void getAllGoals()
    {
        // Retrieve all goals from the database and store them in an ObservableCollection
        goalList = new ObservableCollection<Goal>(await App.GoalRepository.GetAllGoals());

        // Uncheck checkboxes of tasks that were not updated today
        foreach (Goal goal in goalList)
        {
            if (goal.LastUpdatedDate != DateTime.Today.ToShortDateString())
            {
                goal.TodayIsChecked = "False";
            }
        }

        // Bind the goal collection to the goalCollectionView
        goalCollectionView.ItemsSource = goalList;

        // Filter the goals for today's tasks and store them in an ObservableCollection
        getTodayTasks();

        // Bind the task collection to the taskCollectionView
        taskCollectionView.ItemsSource = taskList;

        // Update the goal achieved count
        getGoalAchievedCount();
    }

    private void getTodayTasks()
    {
        // Clear the taskList
        taskList.Clear();

        // Iterate through all goals in goalList
        foreach (Goal goal in goalList)
        {
            // Split the DaysOfTheWeek string by comma and iterate through each day
            String[] daysOfWeek = goal.DaysOfTheWeek.Split(",");
            foreach (String day in daysOfWeek)
            {
                // If the current day matches the day in DaysOfTheWeek, add the goal to taskList
                if (day == currentDayOfWeek)
                {
                    taskList.Add(goal);
                }
            }
        }
    }

    // Update the label that shows the number of goals achieved and the total number of tasks for today
    private void getGoalAchievedCount()
    {
        String goalAchievedCount = checkedTaskList.Count().ToString();
        String totalTaskCount = taskList.Count().ToString();
        lblGoalAchieved.Text = goalAchievedCount + " / " + totalTaskCount + " Goal(s) Achieved";
    }

}
