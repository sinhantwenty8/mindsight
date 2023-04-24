using System;
using SQLite;

namespace MindSight
{
	[Table("diary")]
	public class Diary
	{
		[PrimaryKey,AutoIncrement]
		public int Id { get; set; }

        [MaxLength(250)]
        public string Content { get; set; }

		[MaxLength(250)]
		public string Date { get; set; }

        [MaxLength(250)]
        public string MoodImage { get; set; }
    }
}

