using System;
namespace MindSight
{
	public class Affirmation
	{
        public int AffirmationId { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }

        public Affirmation(string author, string content)
		{
			Author = author;
			Content = content;
		}
	}
}

