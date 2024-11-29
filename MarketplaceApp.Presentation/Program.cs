using MarketplaceApp.Presentation.Actions;
using MarketplaceApp.Domain;
using System;
using MarketplaceApp.Domain.Repositories;
using MarketplaceApp.Data;

namespace MarketplaceApp.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var context = new MarketplaceContext();

            // Inicijalizacija repozitorija s kontekstom
            var userRepository = new UserRepository(context);
            var productRepository = new ProductRepository(context);
            var transactionRepository = new TransactionRepository(context);
            var categoryRepository = new CategoryRepository(context);
            var promoCodeRepository = new PromoCodeRepository(context);

            // Inicijalizacija MarketplaceService-a
            var marketplaceService = new MarketplaceService(
                userRepository,
                productRepository,
                transactionRepository,
                categoryRepository,
                promoCodeRepository
            );

            // Inicijalizacija akcija (prijava, registracija itd.)
            var actions = new LoginAndRegistrationActions(marketplaceService);

            Console.WriteLine("=== Dobrodosli u Marketplace aplikaciju ===\n");
            MainMenu();
        }

        public static void MainMenu()
        {
            Console.WriteLine("1 - Registracija korisnika\n2 - Prijava korisnika\n3 - Izlaz\n");
            Console.Write("Odaberite radnju: ");

            string decision = Console.ReadLine();

            switch (decision)
            {
                case "1":
                    Console.WriteLine("Registracija u tijeku...");
                    break;
                case "2":
                    
                    break;
                case "3":
                    Console.Clear();
                    Console.WriteLine("Hvala sto koristite Marketplace aplikaciju!");
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Pogresan unos, pokusajte ponovo.\n");
                    MainMenu();
                    return;
            }
        }
    }
}