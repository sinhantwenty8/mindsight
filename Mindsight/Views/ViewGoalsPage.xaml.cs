using System.Collections.ObjectModel;

namespace MindSight;

public partial class ViewGoalsPage : ContentPage
{
    // Create an observable collection of goals
    public ObservableCollection<Goal> goalList;

    // Constructor method
    public ViewGoalsPage()
    {
        InitializeComponent();
        getAllGoals();
        BindingContext = goalList;
    }

    // Method to retrieve all goals from the repository and populate the goalList
    async void getAllGoals()
    {
        goalList = new ObservableCollection<Goal>(await App.GoalRepository.GetAllGoals());
        goalCollectionView.ItemsSource = goalList;
    }

    // Event handler for when a goal is tapped
    async void OnSectionTapped(System.Object sender, System.EventArgs e)
    {
        // Get the selected goal from the sender's BindingContext
        Goal selectedGoal = (Goal)((sender as VerticalStackLayout).BindingContext);

        // Navigate to the edit goal page with the selected goal's ID as a parameter
        await Shell.Current.GoToAsync($"editGoalPage?goalId={selectedGoal.Id}");
    }

    // Event handler for the back button
    async void btnBack_Clicked(System.Object sender, System.EventArgs e)
    {
        // Navigate back to the previous page
        await Navigation.PopAsync();
    }
}
