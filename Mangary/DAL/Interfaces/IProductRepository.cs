using System;
using System.Collections.Generic;
using Mangary.Models;

namespace Mangary.DAL
{
	public interface IProductRepository : IDisposable
	{
		IEnumerable<Product> GetAll();
		Product GetProductById(Guid ProductId);
		IEnumerable<Product> GetLatest(int NumOfProducts);
		IEnumerable<Product> GetProductById(IEnumerable<Guid> GuidList);
		IEnumerable<Product> GetLatestProductsAdded(int page, int NumOfProducts);
		IEnumerable<Guid> GetProductIdByCategoryId(int id);
		int Count();
		void InsertProduct(Product product);
		void UpdateProduct(Product product);
		void DeleteProduct(Guid ProductId);
		void DeleteProduct(Product product);
		IEnumerable<Product> Search(string pattern);
		void Save();
	}
}