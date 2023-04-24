using System;
using SQLite;
namespace MindSight
{
    [Table("bookmarked")]
    public class BookmarkArticle
    {
        [PrimaryKey, AutoIncrement]
        public int BookmarkId { get; set; }

        public int ArticleId { get; set; }
    }
}

