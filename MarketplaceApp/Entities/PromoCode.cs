using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Data.Entities
{
    public class PromoCode
    {
        public Guid Id { get; }
        public string Code { get; set; }
        public int DiscountPercentage { get; set; }
        public DateTime ExpirationDate {  get; set; }

        public PromoCode(string code, int discountPercentage, DateTime expirationDate) 
        {
            Id = Guid.NewGuid();
            Code = code;
            DiscountPercentage = discountPercentage;
        }
        
    }
}
