using MarketplaceApp.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Data.Entities
{
    public class Buyer : User
    {
        public UserRoleEnum Role { get; protected set; }
        public decimal Balance { get; set; }
        public List<Guid> PurchasedProducts { get; set; } = new List<Guid>();
        public List<Guid> FavouriteProducts { get; set; } = new List<Guid>();

        // Kreiranje vec postojeceg kupca unutar aplikacije
        public Buyer(string name, string email, decimal balance, List<Guid> purchasedProducts, List<Guid> favouriteProducts): base(name, email) 
        {
            Role = UserRoleEnum.Buyer;
            Balance = balance;
            PurchasedProducts = purchasedProducts;
            FavouriteProducts = favouriteProducts;
        }

        // Registracija novog kupca koristenjem aplikacije
        public Buyer(string name, string email, decimal balance) : base(name, email)
        {
            Role = UserRoleEnum.Buyer;
            Balance = balance;
            PurchasedProducts = new List<Guid>();
            FavouriteProducts = new List<Guid>();
        }
    }
}
