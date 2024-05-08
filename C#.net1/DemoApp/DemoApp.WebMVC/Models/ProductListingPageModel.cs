﻿using DemoApp.Domain.Models.Categories;

namespace DemoApp.WebMVC.Models
{
	public class ProductListingPageModel
	{
		public List<CategoryViewModel> Categories { get; set; }
		public Dictionary<int, string> OrderBys { get; set; }
		public List<int> SelectPageSize { get; set; }
		public string CategoryId { get; set; }
		public string KeyWord { get; set; }
	}

}
