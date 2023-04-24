
using System.Collections.ObjectModel;

namespace MindSight;

public partial class InsightsPage : ContentPage
{
    // Collection of bookmarked articles
    ObservableCollection<BookmarkArticle> bookmarkedArticle = new ObservableCollection<BookmarkArticle>();

    // Collection of all articles
    ObservableCollection<Article> articleList = new ObservableCollection<Article>(ArticlesData.articles);

    // Collections of articles categorized by topic
    ObservableCollection<Article> selfCareArticleList = new ObservableCollection<Article>();
    ObservableCollection<Article> physchologyArticleList = new ObservableCollection<Article>();
    ObservableCollection<Article> letsTalkArticleList = new ObservableCollection<Article>();
    ObservableCollection<Article> bookRecommendArticleList = new ObservableCollection<Article>();

    // Keeps track of the last image icon source used
    private string lastImgIconSource;

    // Constructor for InsightsPage
    public InsightsPage()
    {
        InitializeComponent();

        // Load bookmarks on first load
        bookmarkOnFirstLoad();

        // Categorize all articles by topic
        categorizeArticle(articleList);

        // Set the binding context of the page to articleList
        BindingContext = articleList;
    }

    void categorizeArticle(ObservableCollection<Article> articles)
    {
        // Clear previous lists
        selfCareArticleList.Clear();
        physchologyArticleList.Clear();
        letsTalkArticleList.Clear();
        bookRecommendArticleList.Clear();

        // Categorize articles and add to respective lists
        foreach (Article article in articles)
        {
            Category articleCategory = article.Category;
            if (articleCategory.Name == CategoriesData.categorySelfCare.Name)
            {
                selfCareArticleList.Add(article);
            }else if (articleCategory.Name == CategoriesData.categoryPsychology.Name)
            {
                physchologyArticleList.Add(article);
            }
            else if (articleCategory.Name == CategoriesData.categoryMentalHealth.Name)
            {
                letsTalkArticleList.Add(article);
            }
            else if (articleCategory.Name == CategoriesData.categoryBookRecommend.Name)
            {
                bookRecommendArticleList.Add(article);
            }
        }
        // Set article list item sources
        articleListSelfCare.ItemsSource = selfCareArticleList;
        articleListPsychology.ItemsSource = physchologyArticleList;
        articleListMental.ItemsSource = letsTalkArticleList;
        articleListBookRecommend.ItemsSource = bookRecommendArticleList;

        // Reset binding context
        BindingContext = null;
        BindingContext = this;
    }

    async void article_Clicked(System.Object sender, System.EventArgs e)
    {
        // Get clicked article and navigate to detailed page
        ImageButton imageButton = sender as ImageButton;

        Article article = imageButton?.BindingContext as Article;

        if (article != null)
        {
            await Shell.Current.GoToAsync("articleDetailedPage?articleID=" + article.ArticleID);
        }
    }

    async void SearchButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("filterPage");
    }

    async void btnBookmark_Clicked(System.Object sender, System.EventArgs e)
    {
        // Cast the sender to an ImageButton
        ImageButton bookmark = sender as ImageButton;

        // Get the source of the ImageButton, ignoring the "File:" prefix
        String bookmarkSource = bookmark.Source.ToString().Substring(6);

        // Toggle the bookmark icon based on its current state
        bookmark.Source = bookmarkSource == "bookmark.png" ? "bookmark_outline.png" : "bookmark.png";

        // Get the list of bookmarked articles
        await getBookmarkedArticles(bookmarkSource);

        // Remember the last icon source for future use
        lastImgIconSource = bookmark.Source.ToString().Substring(6);
    }

    async Task getBookmarkedArticles(string bookmarkSource = "bookmark.png")
    {
        // Get the list of bookmarked articles
        bookmarkedArticle = new ObservableCollection<BookmarkArticle>(await App.BookmarkRepo.GetAllBookmarkArticle());

        if (bookmarkSource != "bookmark_outline.png")
        {
            // If the bookmark icon is currently "filled in", display all articles
            articleList = new ObservableCollection<Article>(ArticlesData.articles);
            categorizeArticle(articleList);
        }
        else
        {
            // If the bookmark icon is currently "empty", display only bookmarked articles
            if (bookmarkedArticle != null)
            {
                ObservableCollection<Article> bookmarkedArticles = new ObservableCollection<Article>();

                // Loop through all bookmarked articles and find them in the master list of articles
                foreach (BookmarkArticle bookmarkArticle in bookmarkedArticle)
                {
                    foreach (Article article in articleList)
                    {
                        if (article.ArticleID == bookmarkArticle.ArticleId)
                        {
                            bookmarkedArticles.Add(article);
                            article.Bookmarked = true;
                        }
                    }
                }

                // Display only bookmarked articles
                articleList = bookmarkedArticles;
                categorizeArticle(articleList);
            }
        }
    }


    // This method loads the bookmarked articles from the database and marks them as bookmarked in the articleList.
    async void bookmarkOnFirstLoad()
    {
        bookmarkedArticle = new ObservableCollection<BookmarkArticle>(await App.BookmarkRepo.GetAllBookmarkArticle());

        foreach (BookmarkArticle bookmarkArticle in bookmarkedArticle)
        {
            foreach (Article article in articleList)
            {
                if (article.ArticleID == bookmarkArticle.ArticleId)
                {
                    article.Bookmarked = true;
                }
            }
        }
    }

    // This method is called when the InsightsPage is displayed on the screen.
    // It sets the image source of the bookmark button and reloads the articleList depending on whether the bookmark button is pressed or not.
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Determine the image source of the bookmark button based on the lastImgIconSource variable.
        String imgSource = lastImgIconSource == "bookmark.png" ? "bookmark_outline.png" : "bookmark.png";

        // Reload the articleList based on whether the bookmark button is pressed or not.
        getBookmarkedArticles(imgSource);
    }
}


