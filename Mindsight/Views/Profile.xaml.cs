using Plugin.Maui.Audio;

namespace MindSight;

public partial class Profile : ContentPage
{
    public Profile()
    {
        InitializeComponent();
    }

    //Method to handle the button click event for the "About Us" button.
    private async void OnButtonClick_About(object sender, EventArgs e)
    {
        // Navigates to the About Page
        await Shell.Current.GoToAsync("aboutPage");
    }

    //Method to handle the button click event for the "Support Hotline" button. 
    private async void OnButtonClick_Support(object sender, EventArgs e)
    {
        // Navigates to the Support Page
        await Shell.Current.GoToAsync("supportPage");
    }

    //Method to handle the button click event for the "FAQ" button.
    private async void OnButtonClick_FAQ(object sender, EventArgs e)
    {
        // Navigates to the FAQ Page
        await Shell.Current.GoToAsync("faqPage");
    }

    //Method to handle the button click event for the close icon button.
    private async void OnButtonClick_Home(object sender, EventArgs e)
    {
        // Navigates back to the Home Page
        await Shell.Current.GoToAsync("mainPage");
    }

    private async void OnButtonBack(object sender, EventArgs e)
    {
        // Navigates to the Main Page
        await Navigation.PopAsync();
    }
}
