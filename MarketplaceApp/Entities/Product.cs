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

        public Guid SellerId { get; set; }

        public ProductStatusEnum Status { get; set; }

        public Guid CategoryId { get; set; }

        // Kreiranje vec postojeceg produkt unutar aplikacije
        public Product(string name, string description, decimal price, ProductStatusEnum status, Guid categoryId) 
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            Status = status;
            CategoryId = categoryId;
        }

        // Dodavanje produkta koristenjem aplikacije
        public Product(string name, string description, decimal price, Guid categoryId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            Status = ProductStatusEnum.OnSale;
            CategoryId = categoryId;
        }
    }
}
