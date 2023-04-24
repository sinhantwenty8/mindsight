using System.Collections.Generic;
using Java.Util;
using static System.Net.Mime.MediaTypeNames;

namespace MindSight;


// The QueryProperty attribute allows the page to receive a value for Date from the navigation URI
[QueryProperty(nameof(Date), "date")]
public partial class JournalPage : ContentPage
{
    // Store the current date and a Diary object
    string date;
    Diary diaryCur;
    private Diary curDiary;
    private ImageButton selectedImageButton;

    // Store all the diary entries for a particular date
    List<Diary> diaries = new List<Diary>();

    // Define the public Date property, which has a getter and a setter method
    public String Date
    {
        get => date;
        // The setter method updates the value of date and calls the getAllDiary method
        set
        {
            date = value;
            getAllDiary();
        }
    }

    public JournalPage()
    {
        InitializeComponent();
        // If the date variable is null or empty, set it to the current date
        if (string.IsNullOrEmpty(date))
        {
            date = DateTime.Now.Date.ToString("dd/MM/yyyy");
        }
        // Call the getAllDiary method to retrieve all the diary 
        getAllDiary();
    }

    // Method to update the diary based on the selected date
    void UpdateDiary(string date)
    {
        // Find the diary for the selected date
        FindDiary(date);

        // If the diary is found, update the UI elements
        if (diaryCur != null)
        {
            diaryEntry.Text = diaryCur.Content;
            btnDiary.Text = "🗓 " + diaryCur.Date;
            diaryEntry.IsReadOnly = false;
        }
        // If the diary is not found, update the UI elements to show the selected date
        else
        {
            btnDiary.Text = "🗓 " + date;
        }

    }

    // Method to get all diaries from the database
    async void getAllDiary()
    {
        // Get all the diaries from the database
        diaries = await App.DiaryRepo.GetAllDiary();
        // Update the diary for the current date
        UpdateDiary(date);
    }

    // Method to find the diary for a specific date
    void FindDiary(string date)
    {
        foreach (Diary diary in diaries)
        {
            // Convert the diary date and the selected date to DateTime objects
            DateTime dDate = DateTime.ParseExact(diary.Date,"dd/MM/yyyy",null);
            DateTime curDate = DateTime.ParseExact(date, "dd/MM/yyyy", null);
            if (dDate == curDate)
            {
                diaryCur = diary;
            }
        }
    }

    // Method to handle the click event of the calendar button
    async void btnCalendar_Clicked(System.Object sender, System.EventArgs e)
    {
        // Navigate to the calendar page
        await Shell.Current.GoToAsync("calendarPage");
    }

    // Method to handle the click event of the save button
    async void btnSave_Clicked(System.Object sender, System.EventArgs e)
    {
        // Set the status message to empty
        statusMessage.Text = "";

        // If a diary entry already exists for the selected date, update it
        if (diaryCur != null)
        {
            await App.DiaryRepo.UpdateDiary(diaryEntry.Text, date, "");
        }
        // Otherwise, add a new diary entry for the selected date
        else
        {
            await App.DiaryRepo.AddNewDiary(diaryEntry.Text, date, "");
        }

        // Set the status message to the repository's status message
        statusMessage.Text = App.DiaryRepo.StatusMessage;

        // Get all diary entries and update the current diary
        getAllDiary();

    }

    // Event handler for the Clicked event of all ImageButtons
    private void moodBtn_Clicked(object sender, EventArgs e)
    {
        // Get the clicked ImageButton
        var clickedButton = (ImageButton)sender;

        if (clickedButton != selectedImageButton)
        {
            // If the clicked button is different from the currently selected one,
            // set the Source property of the currently selected ImageButton to the
            // unselected image (assuming the unselected image is named f?_unselected.png)
            if (selectedImageButton != null)
            {
                selectedImageButton.Source = $"f{selectedImageButton.ClassId}.png";
            }

            // Set the Source property of the clicked ImageButton to the selected image
            clickedButton.Source = $"f{clickedButton.ClassId}_new.png";

            // Update the currently selected ImageButton
            selectedImageButton = clickedButton;

            saveMood(clickedButton.Source.ToString().Substring(6));
        }
    }

    async void getTodayDiary()
    {
        // get all diaries from the repository
        diaries = await App.DiaryRepo.GetAllDiary();

        // iterate through all diaries to find today's diary
        foreach (Diary diary in diaries)
        {
            if (diary.Date == date)
            {
                curDiary = diary;
            }
        }

        // if today's diary is found, set the mood image
        if (curDiary != null)
        {
            SetSelectedMoodImage();
        }
    }

    async void saveMood(string moodImg)
    {
        // if there is no diary for today, add a new diary entry
        if (curDiary == null)
        {
            await App.DiaryRepo.AddNewDiary("", date, moodImg);
        }
        // otherwise, update the mood image of today's diary entry
        else
        {
            await App.DiaryRepo.UpdateDiary(curDiary.Content, curDiary.Date, moodImg);
            curDiary.MoodImage = moodImg;
        }
    }

    // Set the selected mood image
    private void SetSelectedMoodImage()
    {
        if (curDiary.MoodImage == "") return;
        String selectedMoodId = curDiary.MoodImage.Substring(1, 1);

        // Find the mood image button with the same class ID as the selected mood
        foreach (var child in moodStackLayout.Children)
        {
            if (child is ImageButton imageButton)
            {
                if (imageButton.ClassId == selectedMoodId.ToString())
                {
                    imageButton.Source = curDiary.MoodImage;
                    selectedImageButton = imageButton;
                }
                else
                {
                    String articleId= imageButton.Source.ToString().Substring(6).Substring(1,1);
                    imageButton.Source = $"f{articleId}.png";
                }
            }
        }

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        getTodayDiary();
    }
}
