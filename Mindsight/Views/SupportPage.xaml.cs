using System.Reflection.Metadata;

namespace MindSight;

public partial class SupportPage : ContentPage
{
	public SupportPage()
    {
		InitializeComponent();
    }

    //Method to handle the button click event for the close icon button.
    private async void OnButtonClick_Profile(object sender, EventArgs e)
    {
        // Navigates back to the Profile Page
        await Shell.Current.GoToAsync("profilePage");
    }

    //Links image button to open an external website that provides counselling services.
    private async void OnButtonClick_Counselling(object sender, EventArgs e)
    {
        // Open the link in the default browser
        await Launcher.OpenAsync("https://www.carecorner.org.sg/services/counselling-centre/"); 
    }

    //Links image button to open an external website that provides community support.
    private async void OnButtonClick_Community(object sender, EventArgs e)
    {
        // Open the link in the default browser
        await Launcher.OpenAsync("https://www.carecorner.org.sg/services/community-and-workplace-mental-health/"); 
    }

    //Links image button to open an external website that provides professional services.
    private async void OnButtonClick_Professional(object sender, EventArgs e)
    {
        // Open the link in the default browser
        await Launcher.OpenAsync("https://www.healthhub.sg/"); 
    }

    private async void OnButtonBack(object sender, EventArgs e)
    {
        // Navigates to the Main Page
        await Navigation.PopAsync();
    }
}
