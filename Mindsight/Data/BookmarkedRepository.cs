using System;
using SQLite;

namespace MindSight
{
	public class BookmarkedRepository
	{
        string _dbPath;

        public string StatusMessage { get; set; }

        private SQLiteAsyncConnection conn;


        private async Task Init()
        {
            if (conn != null)
                return;
            conn = new SQLiteAsyncConnection(_dbPath);

            await conn.CreateTableAsync<BookmarkArticle>();
        }

        public BookmarkedRepository(string dbPath)
        {
            _dbPath = dbPath;

        }

        public async Task AddNewBookmarkArticle(int articleID)
        {
            int result = 0;
            try
            {
                await Init();

                if (int.IsNegative(articleID))
                    throw new Exception("ArticleID should be prositive");

                result = await conn.InsertAsync(new BookmarkArticle { ArticleId =articleID});

                StatusMessage = string.Format("{0} record(s) added [Bookmark ID:{1},[Article ID:{2})", result, articleID);

            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", articleID, ex.Message);
            }
        }

        public async Task<List<BookmarkArticle>> GetAllBookmarkArticle()
        {
            try
            {
                await Init();
                return await conn.Table<BookmarkArticle>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            };

            return new List<BookmarkArticle>();
        }

        public async Task DeleteBookmarkArticle(int articleId)
        {
            int result = 0;

            try
            {
                await Init();

                result = await conn.Table<BookmarkArticle>()
            .Where(ba => ba.ArticleId == articleId)
            .DeleteAsync();

                StatusMessage = string.Format("{0} record(s) deleted [Bookmark ID:{1},[Article ID:{2})", result, articleId);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to delete bookmarked article. Error: {0}", ex.Message);
            }
        }



    }


}

