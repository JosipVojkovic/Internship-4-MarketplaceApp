using MarketplaceApp.Presentation.Actions;
using MarketplaceApp.Domain;
using System;
using MarketplaceApp.Domain.Repositories;
using MarketplaceApp.Data;
using System.Security.Cryptography.X509Certificates;

namespace MarketplaceApp.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var context = new MarketplaceContext();

            var userRepository = new UserRepository(context);
            var productRepository = new ProductRepository(context);
            var transactionRepository = new TransactionRepository(context);
            var categoryRepository = new CategoryRepository(context);
            var promoCodeRepository = new PromoCodeRepository(context);

            var marketplaceService = new MarketplaceService(
                userRepository,
                productRepository,
                transactionRepository,
                categoryRepository,
                promoCodeRepository
            );
            var buyerActions = new BuyerActions(marketplaceService);
            var sellerActions = new SellerActions(marketplaceService);

            var actions = new LoginAndRegistrationActions(marketplaceService, buyerActions, sellerActions);

            Console.WriteLine("=== Dobrodosli u Marketplace aplikaciju ===\n");
            actions.MainMenu();
        }
    }
}