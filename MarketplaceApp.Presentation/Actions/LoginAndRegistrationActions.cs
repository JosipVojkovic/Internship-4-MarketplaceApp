using MarketplaceApp.Data.Entities;
using MarketplaceApp.Domain;
using MarketplaceApp.Domain.Repositories;
using MarketplaceApp.Presentation;
using MarketplaceApp.Presentation.Helpers;
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
        public readonly BuyerActions _buyerActions;
        public readonly SellerActions _sellerActions;

        public LoginAndRegistrationActions(MarketplaceService marketplaceService, BuyerActions buyerActions, SellerActions sellerActions)
        {
            _marketplaceService = marketplaceService;
            _buyerActions = buyerActions;
            _sellerActions = sellerActions;
        }

        public void MainMenu()
        {
            var decision = "";

            while (decision != "1" && decision != "2" && decision != "3")
            {
                Console.WriteLine("1 - Prijava\n2 - Registracija\n3 - Izlaz\n");
                Console.Write("Odaberite radnju: ");

                decision = Console.ReadLine();

                switch (decision)
                {
                    case "1":
                        Console.Clear();
                        Login();
                        return;
                    case "2":
                        Console.Clear();
                        Register();
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine("Hvala sto koristite Marketplace aplikaciju!");
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("Pogresan unos, pokusajte ponovo.\n");
                        break;
                }
            }
        }

        public void Login()
        {        
            var name = InputValidator.ValidateString("=> Prijava <=\n\nIme: ");
            Console.Clear();
            var email = InputValidator.ValidateEmail($"=> Prijava <=\n\nIme: {name}\nEmail: ");

            var currentUser = _marketplaceService.LoginUser(name, email);

            if (currentUser == null)
            {
                Console.Clear();
                Console.WriteLine("Pogresan unos, taj korisnik ne postoji. Pokusajte ponovno.\n");
                Login();
                return;
            }
            
            Console.Clear();
            Console.WriteLine($"== Dobrodosli {currentUser.Name}! ==\n");

            if (currentUser is Buyer)
            {
                _buyerActions.BuyerMenu(currentUser as Buyer);
                MainMenu();
                return;
            }
            else if(currentUser is Seller)
            {
                _sellerActions.SellerMenu(currentUser as Seller);
                MainMenu();
                return;
            }
        }

        public void Register()
        {
            var decision = "";

            while(decision != "1" && decision != "2")
            {
                Console.WriteLine("=> Registracija <=\n\n1 - Kupac\n2 - Prodavac\n");
                Console.Write("Odaberite radnju: ");
                decision = Console.ReadLine();

                if (decision != "1" && decision != "2")
                {
                    Console.Clear();
                    Console.WriteLine("Pogresan unos. Pokusajte ponovno\n");
                }
            }

            Console.Clear();
            var userString = decision == "1" ? "kupca" : "prodavaca";
            var name = InputValidator.ValidateString($"=> Registracija {userString} <=\n\nUnesite svoje ime: ");
            Console.Clear();
            var email = InputValidator.ValidateEmail($"=> Registracija {userString} <=\n\nUnesite svoj email: ");
            Console.Clear();
            var isRegistered = false;

            if(decision == "1")
            {
                var balance = InputValidator.ValidateDecimal($"=> Registracija {userString} <=\n\nUnesite svoj pocetni balans: ");

                isRegistered = _marketplaceService.RegisterBuyer(name, email, balance);
                if (!isRegistered)
                {
                    Console.Clear();
                    Console.WriteLine("Email koji ste unijeli je zauzet. Pokusajte ponovno.\n");
                    Register();
                    return;
                }
                Console.Clear();
                Console.WriteLine($"Kupac {name} uspjesno kreiran!\n");
                MainMenu();
                return;
            }

            isRegistered = _marketplaceService.RegisterSeller(name, email);
            if (!isRegistered)
            {
                Console.Clear();
                Console.WriteLine("Email koji ste unijeli je zauzet. Pokusajte ponovno.\n");
                Register();
                return;
            }

            Console.Clear();
            Console.WriteLine($"Prodavac {name} uspjesno kreiran!\n");
            MainMenu();
            return;
        }
    }
}
