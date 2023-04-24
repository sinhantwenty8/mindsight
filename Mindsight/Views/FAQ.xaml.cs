namespace MindSight;

public partial class FAQ : ContentPage
{
	public FAQ()
	{
		InitializeComponent();
	}

    public async void OnButtonClick_Profile(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("profilePage");
    }

    private async void OnButtonBack(object sender, EventArgs e)
    {
        // Navigates to the Main Page
        await Navigation.PopAsync();
    }


}
