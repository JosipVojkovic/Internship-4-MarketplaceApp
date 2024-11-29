using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Data.Entities
{
    public class Transaction
    {
        public Guid Id { get; }
        public Guid ProductId { get; set; }

        public Guid BuyerId { get; set; }
        public Guid SellerId { get; set; }
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        // Kreira vec postojecu transakciju unutar aplikacije
        public Transaction(Guid productId, Guid buyerId, Guid sellerId, DateTime date, decimal amount)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            BuyerId = buyerId;
            SellerId = sellerId;
            Date = date;
            Amount = amount;
        }

        // Dodavanje transakcije koristenjem aplikacije
        public Transaction(Guid productId, Guid buyerId, Guid sellerId, decimal amount)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            BuyerId = buyerId;
            SellerId = sellerId;
            Date = DateTime.Now;
            Amount = amount;
        }
    }
}

