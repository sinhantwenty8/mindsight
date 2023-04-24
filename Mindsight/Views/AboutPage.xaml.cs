using Plugin.Maui.Audio;

namespace MindSight;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
	}

    //Method to handle the button click event for the close icon button.
    public async void OnButtonClick_Profile(object sender, EventArgs e)
    {
        // Navigates back to the Profile Page
        await Shell.Current.GoToAsync("supportPage");
    }

    //Method to handle the button click event for the "Get Started" button.
    private async void OnButtonClick_Home(object sender, EventArgs e)
    {
        // Navigates to the Main Page
        await Shell.Current.GoToAsync("mainPage");
    }

    private async void OnButtonBack(object sender, EventArgs e)
    {
        // Navigates to the Main Page
        await Navigation.PopAsync();
    }
    
}
