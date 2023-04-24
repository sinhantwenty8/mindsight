namespace MindSight;

[QueryProperty(nameof(GoalId), "goalId")]
public partial class EditGoalPage : ContentPage
{
    private string iconColor;
    private string goalId;
    private string iconImage;
    private List<String> daysOfTheWeekList = new List<string>();
    private List<Goal> goalList = new List<Goal>();
    private Goal curGoal;

    // This is a public property for the goal ID
    public string GoalId
    {
        get => goalId;  // Get the current value of the goal ID

        // The setter method updates the value of the goal ID and performs additional logic
        set
        {
            goalId = value;   // Update the value of the goal ID

            // Call the getAllGoals method to update the goal list
            getAllGoals();

            // If the goal ID is "new", set the text of the btnSave button to "Add"
            if (goalId == "new")
            {
                btnSave.Text = "Add";
            }
        }
    }

    // This is the constructor for the EditGoalPage class
    public EditGoalPage()
    {
        InitializeComponent();  // Initialize the page's components
    }

    // This method asynchronously retrieves all the goals from the database
    async void getAllGoals()
    {
        goalList = await App.GoalRepository.GetAllGoals();  // Retrieve all the goals from the database

        // If the goal ID is not "new", find the goal with the matching ID and update the UI
        if (goalId != "new")
        {
            findGoal(goalId);
            updateGoalUI();
        }
    }


    // This method finds the goal with the given ID in the goal list
    async void findGoal(string id)
    {
        foreach (Goal g in goalList)
        {
            if (g.Id.ToString() == id)
            {
                curGoal = g;   // Set the current goal to the goal with the matching ID
                getGoalDays();  // Call the getGoalDays method to get the days of the week for the goal
            }
        }
    }

    // This method populates the daysOfTheWeekList with the days of the week for the current goal
    void getGoalDays()
    {
        string[] days = curGoal.DaysOfTheWeek.Split(",");
        foreach (string day in days)
        {
            if (day != "" && day != " ")
            {
                daysOfTheWeekList.Add(day);   // Add the day to the daysOfTheWeekList if it's not an empty or whitespace string
            }
        }
    }

    // This method updates the UI with the details of the current goal
    void updateGoalUI()
    {
        entryTargetTitle.Text = curGoal.TargetTitle;   // Set the text of the entryTargetTitle to the current goal's target title
        editorTargetContent.Text = curGoal.TargetContent;   // Set the text of the editorTargetContent to the current goal's target content
        lblDaysAccumulated.Text = "Accumulated Day(s) Achieved = " + curGoal.AccumulateDays.ToString();   // Set the text of the lblDaysAccumulated to the number of accumulated days for the current goal

        // Update the color button
        foreach (var child in colorStackLayout.Children)
        {
            if (child is Button button)
            {
                if (button.BackgroundColor.ToHex().ToString() == curGoal.Color)   // If the button's background color matches the current goal's color, set its border width to 3 and set the iconColor variable to the current goal's color
                {
                    button.BorderWidth = 3;
                    iconColor = curGoal.Color;
                }
                else  // Otherwise, set the button's border width to 0
                {
                    button.BorderWidth = 0;
                }
            }
        }

        // Update the icon image
        foreach (var child in iconGrid.Children)
        {
            if (child is Frame frame)
            {
                if (frame.Children[0] is ImageButton button)
                {
                    if (button.Source.ToString().Substring(6) == curGoal.IconImage)
                    {
                        frame.BackgroundColor = Color.FromHex("#000000");
                        iconImage = curGoal.IconImage;
                    }
                    else
                    {
                        frame.BackgroundColor = Color.FromHex("#00000000");
                    }
                }
            }
        }

        //update Checkbox
        foreach (var child in checkboxStackLayout.Children)
        {
            if (child is VerticalStackLayout verticalStackLayout)
            {
                var label = (Label)verticalStackLayout.Children[0];
                var checkbox = (CheckBox)verticalStackLayout.Children[1];

                foreach(string day in daysOfTheWeekList)
                {
                    if (label.Text == day)
                    {
                        checkbox.IsChecked = true;
                       
                    }
                }
            }
        }
    }


    async void btnCancel_Clicked(object sender, EventArgs args)
    {
        await Navigation.PopAsync();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Button clickedButton = (Button)sender;

        // Deselect all buttons except the clicked one
        foreach (var child in colorStackLayout.Children)
        {
            if (child is Button button)
            {
                if (button == clickedButton)
                {
                    button.BorderWidth = 3;// set selected border width
                    iconColor = button.BackgroundColor.ToHex().ToString();
                }
                else
                {
                    button.BorderWidth = 0; // set selected border width
                }
            }
        }
    }

    private string checkCheckbox()
    {
        string daysOfTheWeek = "";
        foreach (var child in checkboxStackLayout.Children)
        {
            if (child is VerticalStackLayout verticalStackLayout)
            {
                var label = (Label)verticalStackLayout.Children[0];
                var checkbox = (CheckBox)verticalStackLayout.Children[1];

                if (checkbox.IsChecked)
                {
                    daysOfTheWeek += label.Text+",";
                }
            }
        }
        return daysOfTheWeek;
    }

    // This method is called when an image button is clicked
    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        // Get the clicked image button
        var clickedButton = (ImageButton)sender;

        // Deselect all image buttons except the clicked one
        foreach (var child in iconGrid.Children)
        {
            // Check if the child is a frame
            if (child is Frame frame)
            {
                // Get the image button inside the frame
                if (frame.Children[0] is ImageButton button)
                {
                    if (button == clickedButton)
                    {
                        // If the clicked button is the same as the current button, change the frame color and set the icon image
                        frame.BackgroundColor = Color.FromHex("#000000");
                        iconImage = button.Source.ToString().Substring(6);
                    }
                    else
                    {
                        // If the clicked button is not the same as the current button, change the frame color to transparent
                        frame.BackgroundColor = Color.FromHex("#00000000");
                    }
                }
            }
        }

    }

    // This method is called when the edit button is clicked
    private void btnEdit_Clicked(object sender, EventArgs e)
    {
        ImageButton editIcon = sender as ImageButton;
        // Change the edit icon based on its current source
        editIcon.Source = editIcon.Source.ToString().Substring(6) == "edit_icon.png" ? "edit_icon_filled.png" : "edit_icon.png";

        // Enable/disable the target title entry based on its current state
        entryTargetTitle.IsEnabled = entryTargetTitle.IsEnabled ? false : true;

    }

    // This method handles the event when the Save button is clicked
    async void btnSave_Clicked(System.Object sender, System.EventArgs e)
    {
        // Get the text from the entry and editor controls, and the days of the week from the checkbox
        string targetTitle = entryTargetTitle.Text;
        string targetContent = editorTargetContent.Text;
        string daysOfTheWeek = checkCheckbox();
        int accumulateDays = 0;
        // If the goal is new, add it to the database
        if (goalId == "new")
        {
            await App.GoalRepository.AddNewGoal(targetTitle, targetContent, iconColor, daysOfTheWeek, iconImage, accumulateDays, "False");

            // If the goal was added successfully, display a message and navigate back to the previous page
            if (App.GoalRepository.StatusMessage == "Added Successfully")
            {
                await DisplayAlert("Added Successfully", "Your goal is added.", "OK");
                await Navigation.PopAsync();
            }
            // If the goal was not added successfully, display an error message
            else
            {
                await DisplayAlert(App.GoalRepository.StatusMessage, "Please check if everything is filled.", "OK");
            }
        }
        // If the goal already exists, update it in the database
        else
        {
            await App.GoalRepository.UpdateGoal(Convert.ToInt32(goalId), targetTitle, targetContent, iconColor, daysOfTheWeek, iconImage, curGoal.AccumulateDays, curGoal.TodayIsChecked, curGoal.LastUpdatedDate);

            // If the goal was updated successfully, display a message and navigate back to the previous page
            if (App.GoalRepository.StatusMessage == "Updated Successfully")
            {
                await DisplayAlert("Updated Successfully", "Your goal is updated.", "OK");
                await Navigation.PopAsync();
            }
            // If the goal was not updated successfully, display an error message
            else
            {
                await DisplayAlert(App.GoalRepository.StatusMessage, "Please check if everything is filled.", "OK");
            }
        }
    }

    async void deleteGoal_Clicked(System.Object sender, System.EventArgs e)
    {
        // check if the goalId is not new
        if (goalId != "new")
        {
            // ask for confirmation before deleting the goal
            Boolean result = await DisplayAlert("Confirm to Delete", "Are you sure you want to delete the goal?", "Yes", "No");

            if (result == true) // if the user confirms to delete the goal
            {
                // delete the goal from the database
                await App.GoalRepository.DeleteGoal(Convert.ToInt32(goalId));
                await Navigation.PopAsync(); // navigate back to the previous page
            }
            else // if the user cancels the deletion
            {
                return; // do nothing
            }
        }
    }
}