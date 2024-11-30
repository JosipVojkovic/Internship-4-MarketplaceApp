using MarketplaceApp.Data;
using MarketplaceApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MarketplaceApp.Domain.Repositories
{
    public class UserRepository
    {
        private readonly MarketplaceContext _marketplaceContext;

        public UserRepository(MarketplaceContext marketplaceContext)
        {
            _marketplaceContext = marketplaceContext;
        }

        public void Add(User user)
        {
            _marketplaceContext.Users.Add(user);
        }

        public User GetById(Guid id)
        {
            return _marketplaceContext.Users.FirstOrDefault(user => user.Id == id);
        }

        public User GetUser(string name, string email)
        {
            return _marketplaceContext.Users.FirstOrDefault(user => user.Name.ToLower() == name.ToLower() && user.Email == email);
        }

        public User GetUser(string email)
        {
            return _marketplaceContext.Users.FirstOrDefault(user => user.Email == email);
        }

        public IEnumerable<User> GetAll()
        {
            return _marketplaceContext.Users;
        }

        public IEnumerable<Seller> GetAllSellers()
        {
            return _marketplaceContext.Users.OfType<Seller>();
        }

        public IEnumerable<Buyer> GetAllBuyers()
        {
            return _marketplaceContext.Users.OfType<Buyer>();
        }
    }
}
