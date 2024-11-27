using MarketplaceApp.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Data.Entities
{
    public class Seller : User
    {
        public UserRoleEnum Role { get; protected set; }
        public List<Guid> OwnedProducts { get; set; }
        public decimal TotalEarnings { get; set; }

        // Kreira vec postojeceg prodavaca unutar aplikacije
        public Seller(string name, string email, List<Guid> ownedProducts, decimal totalEarnings) : base(name, email)
        {
            Role = UserRoleEnum.Seller;
            OwnedProducts = ownedProducts;
            TotalEarnings = totalEarnings;
        }

        // Registracija novog prodavaca koristenjem aplikacije
        public Seller(string name, string email): base(name, email) 
        {
            Role = UserRoleEnum.Seller;
            OwnedProducts = new List<Guid>();
            TotalEarnings = 0;
        }
    }
}
