using MarketplaceApp.Domain;
using MarketplaceApp.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Presentation.Actions
{
    public class LoginAndRegistrationActions
    {
        public readonly MarketplaceService _marketplaceService;

        public LoginAndRegistrationActions(MarketplaceService marketplaceService)
        {
            _marketplaceService = marketplaceService;
        }

        public void Login()
        {        
            Console.Clear();
            Console.Write("=> Prijava <=\n\nIme: ");
            var name = Console.ReadLine();
            Console.Write("\nEmail: ");
            var email = Console.ReadLine();

            var currentUser = _marketplaceService.LoginUser(name, email);
            Console.WriteLine($"{currentUser.Name} - {currentUser.Email}");
        }
    }
}
