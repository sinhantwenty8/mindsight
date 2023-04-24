using System;
namespace MindSight
{
	public class Category
	{
		public int CategoryId { get; set; }
		public string Name { get; set; }

		public Category(int categoryID, string name)
		{
			CategoryId = categoryID;
			Name = name;
		}
	}
}

