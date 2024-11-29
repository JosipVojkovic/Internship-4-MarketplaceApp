using MarketplaceApp.Data;
using MarketplaceApp.Data.Entities;
using MarketplaceApp.Data.Entities.Enums;
using MarketplaceApp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Domain
{
    public class MarketplaceService
    {
        private readonly UserRepository _userRepository;
        private readonly ProductRepository _productRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly PromoCodeRepository _promoCodeRepository;
        private readonly TransactionRepository _transactionRepository;

        public MarketplaceService(UserRepository userRepository, ProductRepository productRepository, TransactionRepository transactionRepository, CategoryRepository categoryRepository, PromoCodeRepository promoCodeRepository)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
            _promoCodeRepository = promoCodeRepository;
        }

        public void RegisterBuyer(string name, string email, decimal balance)
        {
            var buyer = new Buyer(name, email, balance);

            _userRepository.Add(buyer);
        }

        public void RegisterSeller(string name, string email)
        {
            var seller = new Seller(name, email);

            _userRepository.Add(seller);
        }

        public User LoginUser(string name, string email)
        {
            return _userRepository.GetUser(name, email);
        }

        public IEnumerable<Product> GetAllProductsForSale()
        {
            var products = _productRepository.GetAllProducts();
            return products.Where(product => product.Status == ProductStatusEnum.OnSale);
        }

        public void BuyProduct(Guid buyerId, Guid productId)
        {
            var product = _productRepository.GetProductById(productId);
            var buyer = _userRepository.GetById(buyerId) as Buyer;
            var seller = _userRepository.GetAllSellers().FirstOrDefault(seller => seller.OwnedProducts.Contains(productId));
            var amount = product.Price + 5 * (product.Price / 100);

            if (product == null || product.Status != ProductStatusEnum.OnSale)
            {
                Console.WriteLine("Produkt nije dostupan za prodaju.");
                return;
            }

            if (buyer.Balance < product.Price)
            {
                Console.WriteLine("Nemate dovoljno sredstava na vasem racunu.");
                return;
            }

            buyer.Balance -= amount;
            product.Status = ProductStatusEnum.Sold;

            var transaction = new Transaction(product.Id, buyer.Id, seller.Id, amount);
            _transactionRepository.Add(transaction);

            Console.WriteLine($"Produkt {product.Name} uspjesno kupljen.");
        }

        public void BuyProduct(Guid buyerId, Guid productId, Guid promoCodeId)
        {
            var product = _productRepository.GetProductById(productId);
            var buyer = _userRepository.GetById(buyerId) as Buyer;
            var seller = _userRepository.GetAllSellers().FirstOrDefault(seller => seller.OwnedProducts.Contains(productId));
            var discount = _promoCodeRepository.GetById(promoCodeId);
            if (discount == null || discount.ExpirationDate < DateTime.Now)
            {
                Console.WriteLine("Nevazeci ili istekli promotivni kod.");
                return;
            }

            var amount = product.Price - discount.DiscountPercentage * (product.Price / 100) + 5 * (product.Price / 100);

            if (product == null || product.Status != ProductStatusEnum.OnSale)
            {
                Console.WriteLine("Produkt nije dostupan za prodaju.");
                return;
            }

            if (buyer.Balance < product.Price)
            {
                Console.WriteLine("Nemate dovoljno sredstava na vasem racunu.");
                return;
            }

            buyer.Balance -= amount;
            product.Status = ProductStatusEnum.Sold;

            var transaction = new Transaction(product.Id, buyer.Id, seller.Id, amount);
            _transactionRepository.Add(transaction);

            Console.WriteLine($"Produkt {product.Name} uspjesno kupljen.");
        }

        public void ReturnProduct(Guid buyerId, Guid productId)
        {
            var product = _productRepository.GetProductById(productId);
            var buyer = _userRepository.GetById(buyerId) as Buyer;
            var seller = _userRepository.GetAllSellers().FirstOrDefault(seller => seller.OwnedProducts.Contains(productId));

            if (product == null || product.Status != ProductStatusEnum.Sold || !buyer.PurchasedProducts.Contains(productId))
            {
                Console.WriteLine("Produkt nije kupljen ili je vec vracen.");
                return;
            }

            decimal refundAmount = product.Price * 0.80m;
            buyer.Balance += refundAmount;
            seller.TotalEarnings -= product.Price * 0.85m;
            product.Status = ProductStatusEnum.OnSale;

            var returnTransaction = new Transaction(product.Id, buyer.Id, seller.Id, 80);
            _transactionRepository.Add(returnTransaction);

            Console.WriteLine($"produkt {product.Name} uspjesno vracen. Povrat: {refundAmount} EUR.");
        }

        public void AddProductToFavourites(Guid buyerId, Guid productId)
        {
            var product = _productRepository.GetProductById(productId);
            var buyer = _userRepository.GetById(buyerId) as Buyer;

            if (product == null || product.Status != ProductStatusEnum.OnSale)
            {
                Console.WriteLine("Produkt nije dostupan.");
                return;
            }

            buyer.FavouriteProducts.Add(product.Id);
            Console.WriteLine($"Produkt {product.Name} dodan u favorite.");
        }

        public List<Product> GetPurchasedProducts(Guid buyerId)
        {
            var buyer = _userRepository.GetById(buyerId) as Buyer;
            var products = _productRepository.GetAllProducts();

            var purchasedProducts = products.Where(product => buyer.PurchasedProducts.Contains(product.Id)).ToList();

            return purchasedProducts;
        }

        public List<Product> GetFavouriteProducts(Guid buyerId)
        {
            var buyer = _userRepository.GetById(buyerId) as Buyer;
            var favouriteProducts = _productRepository.GetAllProducts().Where(product => buyer.FavouriteProducts.Contains(product.Id)).ToList();

            return favouriteProducts;
        }

        public void AddProductForSale(Guid sellerId, string name, string description, decimal price, Guid categoryId)
        {
            var category = _categoryRepository.GetById(categoryId);

            if (category == null)
            {
                Console.WriteLine("Kategorija nije pronadena.");
                return;
            }

            var seller = _userRepository.GetById(sellerId) as Seller;
            var product = new Product(name, description, price, categoryId);

            seller.OwnedProducts.Add(product.Id);
            _productRepository.AddProduct(product);

            Console.WriteLine($"Produkt {name} dodan na prodaju.");
        }

        public List<Product> GetSellerProducts(Guid sellerId)
        {
            return _productRepository.GetAllProducts().Where(p => p.Id == sellerId).ToList();
        }

        public decimal GetTotalSaleEarnings(Guid sellerId)
        {
            var seller = _userRepository.GetById(sellerId) as Seller;
            return seller.TotalEarnings;
        }

        public List<Product> GetSoldProductsByCategory(Guid sellerId, Guid categoryId)
        {
            var category = _categoryRepository.GetById(categoryId);

            if (category == null)
            {
                Console.WriteLine("Kategorija nije pronadena.");
                return new List<Product>();
            }

            var soldProducts = _productRepository.GetAllProducts().Where(p => p.Id == sellerId && p.Status == ProductStatusEnum.Sold && p.CategoryId == categoryId).ToList();
            return soldProducts;
        }

        public decimal GetTotalEarningsByDate(Guid sellerId, DateTime date)
        {
            var filteredTransactions = _transactionRepository.GetByUser(sellerId);
            var totalEarnings = 0m;

            foreach (var transaction in filteredTransactions)
            {
                if (transaction.Date < date)
                {
                    totalEarnings += -transaction.Amount * - 5 * (-transaction.Amount / 100);
                }
            }
            return totalEarnings;
        }
    }
}
