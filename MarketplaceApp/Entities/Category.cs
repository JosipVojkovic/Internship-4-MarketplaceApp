using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Data.Entities
{
    public class Category
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
        public List<PromoCode> PromoCodes { get; set; }

        public Category(string name, List<Product> products, List<PromoCode> promoCodes)
        {
            Id = Guid.NewGuid();
            Name = name;
            Products = products;
            PromoCodes = promoCodes;
        }
    }
}
