using MarketplaceApp.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Data.Entities
{
    public class Product
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public ProductStatusEnum Status { get; set; }

        // Kreiranje vec postojeceg produkt unutar aplikacije
        public Product(string name, string description, decimal price, ProductStatusEnum status) 
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            Status = status;
        }

        // Dodavanje produkta koristenjem aplikacije
        public Product(string name, string description, decimal price)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            Status = ProductStatusEnum.OnSale;
        }
    }
}
