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
    public class SellerActions
    {
        private readonly MarketplaceService _marketplaceService;

        public SellerActions(MarketplaceService marketplaceService)
        {
            _marketplaceService = marketplaceService;
        }

        public void SellerMenu(Seller seller)
        {
            Console.WriteLine($"Prodavac: {seller.Name}\n");
            Console.WriteLine("1 - Dodavanje proizvoda\n2 - Pregled vlastitih proizvoda");
            Console.WriteLine("3 - Pregled ukupne zarade\n4 - Pregled prodanih proizvoda po kategoriji");
            Console.WriteLine("5 - Pregled zarade po vremenskom razdoblju\n0 - Odjava\n");
            Console.Write("Odaberite radnju: ");
            var decision = Console.ReadLine();

            switch (decision)
            {
                case "1":
                    Console.Clear();
                    AddProduct(seller);
                    return;
                case "2":
                    Console.Clear();
                    ReviewOwnedProducts(seller);
                    return;
                case "3":
                    Console.Clear();
                    ReviewTotalEarnings(seller);
                    return;
                case "4":
                    Console.Clear();
                    ReviewSoldProducts(seller);
                    return;
                case "5":
                    Console.Clear();
                    ReviewTotalEarningsByDate(seller);
                    return;
                case "0":
                    Console.Clear();
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
                    SellerMenu(seller);
                    return;
            }
        }

        public void AddProduct(Seller seller)
        {
            var categories = _marketplaceService.GetCategories();
            var name = InputValidator.ValidateString($"Prodavac: {seller.Name} => Dodavanje proizvoda\n\nUnesite ime novog proizvoda: ");
            Console.Clear();
            var price = InputValidator.ValidateDecimal($"Prodavac: {seller.Name} => Dodavanje proizvoda\n\nUnesite cijenu novog proizvoda: ");
            Console.Clear();
            var description = InputValidator.ValidateString($"Prodavac: {seller.Name} => Dodavanje proizvoda\n\nUnesite opis novog proizvoda: ");

            Console.Clear();
            var choosenCategory = "";

            while (choosenCategory != "0" && !categories.Any(c => c.Name.ToLower() == choosenCategory))
            {
                var counter = 1;
                Console.WriteLine($"Prodavac: {seller.Name} => Dodavanje proizvoda\n\nKategorije:\n");

                foreach (var category in categories)
                {
                    Console.WriteLine($"{counter}. {category.Name}");
                    counter++;
                }

                Console.Write("\nUnesite ime kategorije novog produkta prema listi ili 0 za natrag: ");
                choosenCategory = Console.ReadLine().ToLower();

                if (choosenCategory != "0" && !categories.Any(c => c.Name.ToLower() == choosenCategory))
                {
                    Console.Clear();
                    Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
                }
            }
            
            if (choosenCategory == "0")
            {
                Console.Clear();
                SellerMenu(seller);
                return;
            }

            Console.Clear();
            var newCategory = categories.FirstOrDefault(c => c.Name.ToLower() == choosenCategory);
            _marketplaceService.AddProductForSale(seller.Id, name, description, price, newCategory.Id);
            SellerMenu(seller);
            return;
        }

        public void ReviewOwnedProducts(Seller seller)
        {
            Console.WriteLine($"Prodavac: {seller.Name} => Vlastiti proizvodi\n");

            var ownedProducts = _marketplaceService.GetSellerProducts(seller.Id);

            if (ownedProducts.Count < 1)
            {
                Console.WriteLine("Nemate vlastitih proizvoda.");
            }
            else
            {
                var counter = 1;
                Console.WriteLine("   IME - CIJENA - OPIS\n-------------------------");
                foreach (var product in ownedProducts)
                {
                    Console.WriteLine($"{counter}. {product.Name} - {product.Price} - {product.Description}");
                    counter++;
                }
            }

            Console.Write("\nUnesite 0 za natrag: ");
            var decision = Console.ReadLine();
            Console.Clear();

            if (decision != "0")
            {

                Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
                ReviewOwnedProducts(seller);
                return;
            }

            SellerMenu(seller);
            return;
        }

        public void ReviewTotalEarnings(Seller seller)
        {
            Console.WriteLine($"Prodavac: {seller.Name} => Ukupna zarada\n");
            var totalEarnings = _marketplaceService.GetTotalSaleEarnings(seller.Id);

            Console.WriteLine($"Vasa ukupna zarada je {totalEarnings}e.");

            Console.Write("\nUnesite 0 za natrag: ");
            var decision = Console.ReadLine();
            Console.Clear();

            if (decision != "0")
            {

                Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
                ReviewTotalEarnings(seller);
                return;
            }

            SellerMenu(seller);
            return;
        }

        public void ReviewSoldProducts(Seller seller)
        {
            Console.WriteLine($"Prodavac: {seller.Name} => Prodani proizvodi\n");
            var categories = _marketplaceService.GetCategories();
            var counter = 1;

            foreach (var category in categories)
            {
                Console.WriteLine($"{counter}. {category.Name}");
                counter++;
            }

            Console.Write("\nUnesite ime kategorije ili 0 za natrag: ");
            var enteredCategory = Console.ReadLine().ToLower();
            Console.Clear();

            if (enteredCategory == "0")
            {
                SellerMenu(seller);
                return;
            }
            else if(enteredCategory != "0" && !categories.Any(c => c.Name.ToLower() == enteredCategory))
            {
                Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
                ReviewSoldProducts(seller);
                return;
            }

            var choosenCategory = categories.Find(c => c.Name.ToLower() == enteredCategory);
            var products = _marketplaceService.GetSoldProductsByCategory(seller.Id, choosenCategory.Id);
            var decision = "";
            Console.Clear();

            while (decision != "0")
            {
                Console.WriteLine($"Prodavac: {seller.Name} => Prodani proizvodi\n");
                if (products.Count < 1)
                {
                    Console.WriteLine($"Nemate prodanih proizvoda za kategoriju {choosenCategory.Name}.");
                }
                else
                {
                    var counter1 = 1;
                    Console.WriteLine($"Prodani proizvodi za kategoriju {choosenCategory.Name}\n\n   IME - CIJENA - OPIS\n-------------------------");
                    foreach (var product in products)
                    {
                        Console.WriteLine($"{counter1}. {product.Name} - {product.Price} - {product.Description}");
                        counter1++;
                    }
                }

                Console.Write("\nUnesite 0 za natrag: ");
                decision = Console.ReadLine();

                if (decision != "0")
                {
                    Console.Clear();
                    Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
                }
            }

            Console.Clear();
            SellerMenu(seller);
            return;
        }

        public void ReviewTotalEarningsByDate(Seller seller)
        {
            var startDate = InputValidator.ValidateDate($"Prodavac: {seller.Name} => Ukupna zarada po datumu\n\nUnesite datum (format: dd.MM.yyyy ili yyyy-MM-dd) od kad zelite vidjeti ukupnu zaradu: ");
            Console.Clear();
            var endDate = InputValidator.ValidateDate($"Prodavac: {seller.Name} => Ukupna zarada po datumu\n\nPocetni datum: {startDate}\nUnesite datum (format: dd.MM.yyyy ili yyyy-MM-dd) do kad zelite vidjeti ukupnu zaradu: ");
            Console.Clear();

            if (startDate > endDate)
            {
                Console.WriteLine("Zavrsni datum ne moze biti manji od pocetnog datuma. Pokusajte ponovno.");
                ReviewTotalEarningsByDate(seller);
                return;
            }

            var decision = "";
            while(decision != "0")
            {
                Console.WriteLine($"Prodavac: {seller.Name} => Ukupna zarada po datumu\n\nPocetni datum: {startDate}\nZavrsni datum: {endDate}\n");
                var totalEarnings = _marketplaceService.GetTotalEarningsByDate(seller.Id, startDate, endDate);
                Console.WriteLine($"Ukupna zarada u razdoblju od {startDate} do {endDate} iznosi {totalEarnings}e.");

                Console.Write("\nUnesite 0 za natrag: ");
                decision = Console.ReadLine();

                if(decision != "0")
                {
                    Console.Clear();
                    Console.WriteLine("Pogresan unos. Pokusajte ponovno.\n");
                }
            }

            Console.Clear();
            SellerMenu(seller);
            return;
        }
    }
}
