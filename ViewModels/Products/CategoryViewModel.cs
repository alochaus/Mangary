using System.Collections.Generic;

namespace Mangary.ViewModels.Products
{
	public class CategoryViewModel
	{
		public string				Name			{ get; set; }

		public bool	 				IsSelected		{ get; set; }

		public List<Models.Product>	MangaList		{ get; set; }
	}
}