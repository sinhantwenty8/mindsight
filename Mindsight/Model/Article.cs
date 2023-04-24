using SQLite;
using System;

namespace MindSight;

    public class Article
	    {
            public int ArticleID { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public Category Category { get; set; }
            public string Author { get; set; }
            public string Image { get; set; }
            public Boolean Bookmarked { get; set; }

    public Article(int id, string title, string description, Category category, string author, string image)
    {
        ArticleID = id;
        Title = title;
        Description = description;
        Author = "By " + author;
        Category = category;
        Image = image;
        Bookmarked = false;
    }
}


