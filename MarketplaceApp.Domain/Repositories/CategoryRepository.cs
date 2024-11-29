using MarketplaceApp.Data;
using MarketplaceApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Domain.Repositories
{
    public class CategoryRepository
    {
        private readonly MarketplaceContext _marketplaceContext;

        public CategoryRepository(MarketplaceContext marketplaceContext)
        {
            _marketplaceContext = marketplaceContext;
        }

        public IEnumerable<Category> GetAll()
        {
            return _marketplaceContext.Categories;
        }

        public Category GetById(Guid id)
        {
            return _marketplaceContext.Categories.FirstOrDefault(c => c.Id == id);
        }

        public void Add(Category category)
        {
            _marketplaceContext.Categories.Add(category);
        }

        public void Remove(Guid id)
        {
            var category = _marketplaceContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                _marketplaceContext.Categories.Remove(category);
            }
        }
    }
}
