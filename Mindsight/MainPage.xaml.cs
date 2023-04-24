using System;
using System.Collections.Generic;
using Plugin.Maui.Audio;
using static Android.Provider.ContactsContract;

namespace MindSight;

public partial class MainPage : ContentPage
{
    // Define a private list of Affirmation objects and initialize it with data from AffirmationData class
    private List<Affirmation> affimationList = AffirmationData.affirmations;
    private String date;
    private List<Diary> diaries = new List<Diary>();
    private Diary curDiary;
    private ImageButton selectedImageButton;
    private readonly IAudioManager audioManager;

    public MainPage()
    {
        InitializeComponent();

        // Set the affirmation for the day
        setAffirmation();

        // Set the date variable to the current date in the format dd/MM/yyyy
        date = DateTime.Now.Date.ToString("dd/MM/yyyy");

        // Get the diary entries for the current date
        getTodayDiary();

    }

    public MainPage(IAudioManager audioManager)
    {
        InitializeComponent();

        // Set the audioManager field to the specified value
        this.audioManager = audioManager;

        // Set the affirmation for the day
        setAffirmation();

        // Set the date variable to the current date in the format dd/MM/yyyy
        date = DateTime.Now.Date.ToString("dd/MM/yyyy");

        // Get the diary entries for the current date
        getTodayDiary();

    }

    private async void OnButtonClick_Profile(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("profilePage");
    }


    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        // Navigate to the calendar page when the image button is clicked
        await Shell.Current.GoToAsync("calendarPage");
    }

    async void article_Clicked(System.Object sender, System.EventArgs e)
    {
        // Get the image button that triggered the click event
        ImageButton imageButton = sender as ImageButton;

        String articleId = imageButton.Source.ToString().Substring(6).Substring(7,1);
        // If an article object was retrieved, navigate to the detailed article page with its ID as a parameter
        if (lbl1.Text != null)
        {
            await Shell.Current.GoToAsync("articleDetailedPage?articleID=" + articleId);
        }
    }

    private void setAffirmation()
    {
        Random rand = new Random();
        int index = rand.Next(affimationList.Count); // Generate a random index within the range of the list
        Affirmation affirmation = affimationList[index]; // Get the affirmation at the randomly generated index
        lblAffirmation.Text = affirmation.Content + " - " + affirmation.Author; // Display the affirmation's text in a label on the page
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
                    String articleId = imageButton.Source.ToString().Substring(6).Substring(1, 1);
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

    public async void OnButtonClick_Audio(object sender, EventArgs e)
    {
        // Get the audio file from the application package and Create an audio player 
        var player = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("MindsightAudio.wav"));

        // start playing the file
        player.Play();
    }

    public async void supportButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("supportPage");
    }
}
