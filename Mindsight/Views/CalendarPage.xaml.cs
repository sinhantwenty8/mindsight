//using the library of Syncfusion.Maui.Calendar
using Syncfusion.Maui.Calendar;

namespace MindSight;

public partial class CalendarPage : ContentPage
{
    String date = "";
	public CalendarPage()
	{
		InitializeComponent();
        //Calendar.Tapped event is subscribed the OnCalendarTapped method
        //whenever user taps on a date in the calendar
        Calendar.Tapped += OnCalendarTapped;
    }

    async void btnBack_Clicked(System.Object sender, System.EventArgs e)
    {
        //pops the current page from the navigation stack
        await Navigation.PopAsync();
    }

    //Extracts the selected date from the CalendarTappedEventArgs
    private void OnCalendarTapped(object sender, CalendarTappedEventArgs e)
    {
        var selectedDate = e.Date;
        //stores the date as a string in the "dd/MM/yyyy" format
        date = e.Date.Date.ToString("dd/MM/yyyy");
        //Update the label with the selected date
        lblCalendar.Text = "Selected Date : " + selectedDate.ToString("dd/MM/yyyy");
    }

    async void btnGoToJournal_Clicked(System.Object sender, System.EventArgs e)
    {
        if(date == "")
        {
            await DisplayAlert(App.GoalRepository.StatusMessage, "Please select the date.", "OK");
            return;
        }
        //constructs a link with the selected date as a parameter 
        String link = string.Format("journalPage?date={0}",date);
        //navigates to the "journalPage"
        await Shell.Current.GoToAsync(link);
    }

}
