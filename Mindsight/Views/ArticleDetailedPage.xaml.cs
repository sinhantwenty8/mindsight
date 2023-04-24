namespace MindSight;

[QueryProperty(nameof(ArticleID), "articleID")]
public partial class ArticleDetailedPage : ContentPage
{
    // This is the constructor for the ArticleDetailedPage class
    public ArticleDetailedPage()
    {
        InitializeComponent();  // Initialize the page's components
    }

    // This is a public property for the article ID
    int articleID;

    public int ArticleID
    {
        get => articleID;   // Get the current value of the article ID

        set   // Set the article ID, and update the article details based on the new ID
        {
            articleID = value;
            UpdateArticleId(articleID);
        }
    }

    // This is the event handler for the btnCancel button click event
    async void btnCancel_Clicked(object sender, EventArgs args)
    {
        await Navigation.PopAsync();    // Navigate back to the previous page (pop the current page off the navigation stack)
    }

    // Define the btnBookmark_Clicked event handler method
    async void btnBookmark_Clicked(object sender, EventArgs args)
    {
        // Get the ImageButton control that triggered the event and extract the source file name
        ImageButton bookmark = sender as ImageButton;
        String bookmarkSource = bookmark.Source.ToString().Substring(6);

        // Toggle the bookmark icon by setting the source file to either bookmark.png or bookmark_outline.png
        bookmark.Source = bookmarkSource == "bookmark.png" ? "bookmark_outline.png" : "bookmark.png";

        // Find the Article object corresponding to the articleID
        Article article = FindArticle(articleID);

        // If the article is not currently bookmarked, add a new bookmark to the database
        if (article.Bookmarked == false)
        {
            await App.BookmarkRepo.AddNewBookmarkArticle(articleID);
        }
        // If the article is already bookmarked, delete the bookmark from the database
        else
        {
            await App.BookmarkRepo.DeleteBookmarkArticle(articleID);
        }

        // Toggle the bookmarked status of the article
        article.Bookmarked = article.Bookmarked == true ? false : true;
    }


    // This method updates the article based on the article ID passed in as a parameter
    void UpdateArticleId(int articleID)
    {
        // Find the article with the matching ID
        Article article = FindArticle(articleID);

        // Update the UI with the article details
        Title = article.Title;                          // Set the page title
        lblDescription.Text = article.Description;      // Set the article description label
        lblTitle.Text = article.Title;                  // Set the article title label
        lblAuthor.Text = article.Author;                // Set the article author label
        imgArticle.Source = article.Image;              // Set the article image
        btnBookmark.Source = article.Bookmarked == false ? "bookmark_outline.png" : "bookmark.png"; // Set the bookmark button image
    }

    // This method finds the article with the matching ID
    Article FindArticle(int articleID)
    {
        // Initialize the current article to null
        Article articleCur = null;

        // Loop through all the articles in the ArticlesData.articles collection
        foreach (Article article in ArticlesData.articles)
        {
            // If the current article has the matching ID, set the current article to the matching article
            if (article.ArticleID == articleID)
                articleCur = article;
        }

        // Return the current article (which may be null if no article with the matching ID was found)
        return articleCur;
    }

}
