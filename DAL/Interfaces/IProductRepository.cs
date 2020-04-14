using System;
using System.Collections.Generic;
using Mangary.Models;

namespace Mangary.DAL
{
	public interface IProductRepository : IDisposable
	{
		IEnumerable<Product> GetAll();
		Product GetProductById(Guid ProductId);
		int Count();
		void InsertProduct(Product product);
		void UpdateProduct(Product product);
		void DeleteProduct(Guid ProductId);
		void Save();
	}
}