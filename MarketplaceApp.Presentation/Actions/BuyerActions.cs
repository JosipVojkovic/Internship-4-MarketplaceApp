using MarketplaceApp.Data.Entities;
using MarketplaceApp.Domain;
using MarketplaceApp.Presentation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Presentation.Actions
{
    public class BuyerActions
    {
        private readonly MarketplaceService _marketplaceService;

        public BuyerActions(MarketplaceService marketplaceService)
        {
            _marketplaceService = marketplaceService;
        }

        public void BuyerMenu(Buyer buyer)
        {
            Console.WriteLine($"Kupac: {buyer.Name}\n");
            Console.WriteLine("1 - Pregled svih proizvoda\n2 - Kupnja proizvoda\n3 - Povratak proizvoda");
            Console.WriteLine("4 - Dodavanje proizvoda u listu omiljenih\n5 - Pregled povijesti kupljenih proizvoda");
            Console.WriteLine("6 - Pregled liste omiljenih proizvoda\n0 - Odjava\n");
            Console.Write("Odaberite radnju: ");
            var decision = Console.ReadLine();

            switch (decision)
            {
                case "1":
                    Console.Clear();
                    ProductsForSale(buyer);
                    return;
                case "2":
                    Console.Clear();
                    BuyProduct(buyer);
                    return;
                case "3":
                    Console.Clear();
                    ReturnProduct(buyer);
                    return;
                case "4":
                    Console.Clear();
                    AddToFavourites(buyer);
                    return;
                case "5":
                    Console.Clear();
                    ReviewBoughtProducts(buyer);
                    return;
                case "6":
                    Console.Clear();
                    ReviewFavourites(buyer);
                    return;
                case "0":
                    Console.Clear();
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
                    BuyerMenu(buyer);
                    return;
            }
        }

        public void ProductsForSale(Buyer buyer)
        {
            Console.WriteLine($"Kupac: {buyer.Name} => Produkti na prodaju\n");
            var products = _marketplaceService.GetAllProductsForSale();
            var counter = 1;
            Console.WriteLine("   IME - CIJENA - OPIS\n-------------------------");

            foreach (var product in products)
            {
                
                Console.WriteLine($"{counter}. {product.Name} - {product.Price}e - {product.Description}");
                counter++;
            }

            Console.WriteLine();
            var decision = InputValidator.ValidateString("Unesite 0 za natrag: ");
            Console.Clear();

            if(decision != "0")
            {  
                
                Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
                ProductsForSale(buyer);
                return;
            }

            BuyerMenu(buyer);
            return;
        }

        public void BuyProduct(Buyer buyer)
        {
            Console.WriteLine($"Kupac: {buyer.Name} => Kupnja proizvoda\n");
            var products = _marketplaceService.GetAllProductsForSale();
            Console.WriteLine("ID - IME - CIJENA\n-----------------");
            foreach (var product in products)
            {
                Console.WriteLine($"{product.Id} - {product.Name} - {product.Price}e");
            }

            Console.Write("\nUnesite id proizvoda kojeg zelite kupiti ili 0 za natrag: ");
            var productId = Console.ReadLine();

            if (productId == "0")
            {
                Console.Clear();
                BuyerMenu(buyer);
                return;
            }

            var choosenProduct = products.FirstOrDefault(p => p.Id.ToString() == productId);

            if (choosenProduct == null)
            {
                Console.Clear();
                Console.WriteLine("Ne postoji proizvod sa tim id-om. Pokusajte ponovno.\n");
                BuyProduct(buyer);
                return;
            }

            var promoCodes = _marketplaceService.GetPromoCodes(choosenProduct.Id);

            Console.Clear();

            var decision = "";

            while (decision != "1" && decision != "0" && !promoCodes.Any(pc => pc.Code == decision))
            {
                Console.WriteLine($"Kupac: {buyer.Name} => Kupnja proizvoda\n");

                if (promoCodes.Count < 1)
                {
                    Console.WriteLine("Nemate promotivnih kodova.\n");
                }
                else
                {
                    Console.WriteLine("Dostupni promotivni kodovi:\n");
                    Console.WriteLine("IME - POPUST - DATUM ISTEKA\n------------------------------");
                    foreach (var promoCode in promoCodes)
                    {
                        Console.WriteLine($"{promoCode.Code} - {promoCode.DiscountPercentage}% - {promoCode.ExpirationDate}");
                    }
                }
                Console.Write("\nUnesite ime promo koda kojeg zelite iskoristiti za kupnju, 1 za nastavak kupnje ili 0 za odustati: ");
                decision = Console.ReadLine();

                if (decision != "1" && !promoCodes.Any(pc => pc.Code == decision) && decision != "0")
                {
                    Console.Clear();
                    Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
                }
                else if (decision == "0")
                {
                    Console.Clear();
                    BuyerMenu(buyer);
                    return;
                }
                else if (decision == "1")
                {
                    Console.Clear();
                    _marketplaceService.BuyProduct(buyer.Id, choosenProduct.Id);
                    BuyerMenu(buyer);
                    return;
                }
            }

            var choosenPromoCode = promoCodes.Find(pc => pc.Code == decision);
            Console.Clear();
            _marketplaceService.BuyProduct(buyer.Id, choosenProduct.Id, choosenPromoCode.Id);
            BuyerMenu(buyer);
            return;
        }

        public void ReturnProduct(Buyer buyer)
        {
            Console.WriteLine($"Kupac: {buyer.Name} => Povrat proizvoda\n");
            var products = _marketplaceService.GetPurchasedProducts(buyer.Id);

            if (products.Count < 1)
            {
                Console.WriteLine("Nemate proizvoda za vratiti.");
            }
            else
            {
                Console.WriteLine("Kupljeni proizvodi:\n\nID - IME - CIJENA\n-----------------");
                foreach (var product in products)
                {
                    Console.WriteLine($"{product.Id} - {product.Name} - {product.Price}e");
                }
            }
            
            Console.Write("\nUnesite id proizvoda kojeg zelite obrisati ili 0 za odustati: ");
            var decision = Console.ReadLine();

            if (decision == "0")
            {
                Console.Clear();
                BuyerMenu(buyer);
                return;
            }
            else if(products.Any(p => p.Id.ToString() == decision))
            {
                Console.Clear();
                var product = products.Find(p => p.Id.ToString() == decision);
                _marketplaceService.ReturnProduct(buyer.Id, product.Id);
                BuyerMenu(buyer);
                return;
            }

            Console.Clear();
            Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
            ReturnProduct(buyer);
            return;
        }

        public void AddToFavourites(Buyer buyer)
        {
            Console.WriteLine($"Kupac: {buyer.Name} => Dodavanje u listu omiljenih\n");
            var notFavouriteProducts = _marketplaceService.GetAllProductsForSale().Where(p => !buyer.FavouriteProducts.Contains(p.Id)).ToList();

            if (notFavouriteProducts.Count < 1)
            {
                Console.WriteLine("Nema dostupnih proizvoda za dodati u listu omiljenih.");
            }
            else
            {
                Console.WriteLine("ID - IME - CIJENA\n-----------------");
                foreach (var product in notFavouriteProducts)
                {
                    Console.WriteLine($"{product.Id} - {product.Name} - {product.Price}e");
                }
            }

            Console.Write("\nUnesite id proizvoda kojeg zelite dodati u omiljene ili 0 za odustati: ");
            var decision = Console.ReadLine();

            if (decision == "0")
            {
                Console.Clear();
                BuyerMenu(buyer);
                return;
            }
            else if (notFavouriteProducts.Any(p => p.Id.ToString() == decision))
            {
                Console.Clear();
                var product = notFavouriteProducts.Find(p => p.Id.ToString() == decision);
                _marketplaceService.AddProductToFavourites(buyer.Id, product.Id);
                BuyerMenu(buyer);
                return;
            }

            Console.Clear();
            Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
            AddToFavourites(buyer);
            return;
        }

        public void ReviewBoughtProducts(Buyer buyer)
        {
            Console.WriteLine($"Kupac: {buyer.Name} => Povijest kupljenih proizvoda\n");
            var transactions = _marketplaceService.GetBuyerTransactions(buyer.Id).OrderBy(p => p.Date);
            var products = _marketplaceService.GetPurchasedProducts(buyer.Id);
            Console.WriteLine("   IME - IZNOS - DATUM\n-------------------------");

            var counter = 1;
            foreach (var transaction in transactions)
            {
                var product = products.FirstOrDefault(p => p.Id == transaction.ProductId);
                Console.WriteLine($"{counter}. {product.Name} - {-transaction.Amount}e - {transaction.Date}");
                counter++;
            }

            Console.Write("\nUnesite 0 za natrag: ");
            var decision = Console.ReadLine();
            Console.Clear();

            if (decision != "0")
            {
                
                Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
                ReviewBoughtProducts(buyer);
                return;
            }


            BuyerMenu(buyer);
            return;
        }

        public void ReviewFavourites(Buyer buyer)
        {
            Console.WriteLine($"Kupac: {buyer.Name} => Omiljeni proizvodi\n");

            var favouriteProducts = _marketplaceService.GetFavouriteProducts(buyer.Id);
            var counter = 1;

            if (favouriteProducts.Count < 1)
            {
                Console.WriteLine("Nemate omiljenih proizvoda.");
            }
            else
            {
                Console.WriteLine("   IME - CIJENA - OPIS\n-------------------------");
                foreach (var product in favouriteProducts)
                {
                    Console.WriteLine($"{counter}. {product.Name} - {product.Price}e - {product.Description}");
                    counter++;
                }
            }

            Console.Write("\nUnesite 0 za natrag: ");
            var decision = Console.ReadLine();
            Console.Clear();

            if (decision != "0")
            {

                Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
                ReviewFavourites(buyer);
                return;
            }


            BuyerMenu(buyer);
            return;

        }
    }
}
