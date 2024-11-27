using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MarketplaceApp.Data.Entities
{
    public class Transaction
    {
        Guid Id { get; }
        Guid ProductId { get; set; }

        Guid BuyerId { get; set; }
        Guid SellerId { get; set; }
        DateTime Date {  get; set; }

        // Kreira vec postojecu transakciju unutar aplikacije
        public Transaction(Guid productId, Guid buyerId, Guid sellerId, DateTime date)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            BuyerId = buyerId;
            SellerId = sellerId;
            Date = date;
        }

        // Dodavanje transakcije koristenjem aplikacije
        public Transaction(Guid productId, Guid buyerId, Guid sellerId) 
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            BuyerId = buyerId;
            SellerId = sellerId;
            Date = DateTime.Now;
        }
    }
}
