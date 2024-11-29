using MarketplaceApp.Data;
using MarketplaceApp.Data.Entities;
using MarketplaceApp.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Domain.Repositories
{
    public class ProductRepository
    {
        private readonly MarketplaceContext _marketplaceContext;

        public ProductRepository(MarketplaceContext marketplaceContext)
        {
            _marketplaceContext = marketplaceContext;
        }

        public void AddProduct(Product product)
        {
            _marketplaceContext.Products.Add(product);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _marketplaceContext.Products;
        }

        public Product GetProductById(Guid productId)
        {
            return _marketplaceContext.Products.FirstOrDefault(p => p.Id == productId);
        }

        public IEnumerable<Product> GetProductsByCategory(Guid categoryId)
        {
            return _marketplaceContext.Products.Where(p => p.CategoryId == categoryId);
        }
        public IEnumerable<Product> GetProductsByStatus(ProductStatusEnum status)
        {
            return _marketplaceContext.Products.Where(p => p.Status == status);
        }

        public bool UpdateProductStatus(Guid productId, ProductStatusEnum newStatus)
        {
            var product = GetProductById(productId);
            if (product != null)
            {
                product.Status = newStatus;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteProduct(Guid productId)
        {
            var product = GetProductById(productId);
            if (product != null)
            {
                _marketplaceContext.Products.Remove(product);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
