using System.Collections.Generic;
using Mangary.Models;

namespace Mangary.Services
{
	public interface ICategoryServices
	{
		public List<Product> Search(ref string id);
	}
}