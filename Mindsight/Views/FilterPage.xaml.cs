using Android.App.AppSearch;

namespace MindSight;

public partial class FilterPage : ContentPage
{
    private string _searchQuery { set; get; }
    private List<Article> articleList;
    private List<Article> displayedArticles;

    public FilterPage()
    {
        InitializeComponent();

        // Load articles
        articleList = ArticlesData.articles;

        // Set initial displayed articles to all articles
        displayedArticles = articleList;

        // Set the BindingContext and ItemsSource to the displayed articles
        BindingContext = displayedArticles;
        searchResults.ItemsSource = displayedArticles;

    }

    // Handle back button click
    async void btnBack_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // Handle search bar text changed event
    void OnTextChanged(object sender, EventArgs e)
    {
        // Get the search query from the search bar
        SearchBar searchBar = (SearchBar)sender;
        SearchQuery = searchBar.Text ?? string.Empty;
    }

    // This method is called when an item is selected from the search results list
    async void searchResults_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        Article article = e.SelectedItem as Article;

        if (article != null)
        {
            // Navigate to the detailed page for the selected article
            await Shell.Current.GoToAsync("articleDetailedPage?articleID=" + article.ArticleID);
        }

    }

    // This property holds the current search query entered by the user
    public string SearchQuery
    {
        get { return _searchQuery; }
        set
        {
            _searchQuery = value;
            // Notify that the value of SearchQuery has changed
            OnPropertyChanged(nameof(SearchQuery));
            // Perform the search with the updated query
            PerformSearch();
        }
    }

    // This method is called whenever the search query changes
    private void PerformSearch()
    {
        // Set the displayed articles to the full list by default
        displayedArticles = articleList;
        // If the search query is null, empty, or equal to "n", show the full list
        if (string.IsNullOrWhiteSpace(SearchQuery) || SearchQuery == "n")
        {
            displayedArticles = articleList;
        }
        else
        {
            // Otherwise, filter the articles based on whether their title or description contains the search query
            displayedArticles = displayedArticles.Where(a => a.Title.ToLowerInvariant().Contains(SearchQuery.ToLowerInvariant()) || a.Description.ToLowerInvariant().Contains(SearchQuery.ToLowerInvariant())).ToList<Article>();
        }

        // Set the search results list to show the filtered articles
        searchResults.ItemsSource = displayedArticles;

    }

}
