using MarketplaceApp.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Data.Entities
{
    public abstract class User
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public string Email { get; set; }

        public User(string name, string email) 
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
        }
    }
}
