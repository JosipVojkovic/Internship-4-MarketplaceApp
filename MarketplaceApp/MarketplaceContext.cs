using MarketplaceApp.Data.Entities;
using MarketplaceApp.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Data
{
    public class MarketplaceContext
    {
        public List<User> Users { get; set; } = Seed.Users;
        public List<Product> Products { get; set; } = Seed.Products;
        public List<Category> Categories { get; set; } = Seed.Categories;
        public List<PromoCode> PromoCodes { get; set; } = Seed.PromoCodes;

        public List<Transaction> Transactions { get; set; } = Seed.Transactions;
    }
}
