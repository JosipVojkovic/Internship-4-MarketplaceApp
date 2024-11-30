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

        public bool RegisterBuyer(string name, string email, decimal balance)
        {
            if(_userRepository.GetUser(email) != null)
            {
                return false;
            }

            var buyer = new Buyer(name, email, balance);
            _userRepository.Add(buyer);
            return true;
        }

        public bool RegisterSeller(string name, string email)
        {
            if (_userRepository.GetUser(email) != null)
            {
                return false;
            }

            var seller = new Seller(name, email);

            _userRepository.Add(seller);
            return true;
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
                Console.WriteLine("Proizvod nije dostupan za prodaju.\n");
                return;
            }

            if (buyer.Balance < product.Price)
            {
                Console.WriteLine("Nemate dovoljno sredstava na vasem racunu.\n");
                return;
            }

            buyer.Balance -= amount;
            product.Status = ProductStatusEnum.Sold;
            buyer.PurchasedProducts.Add(product.Id);

            var transaction = new Transaction(product.Id, buyer.Id, seller.Id, -amount);
            _transactionRepository.Add(transaction);

            Console.WriteLine($"Proizvod {product.Name} uspjesno kupljen.\n");
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
                Console.WriteLine("Proizvod nije dostupan za prodaju.\n");
                return;
            }

            if (buyer.Balance < product.Price)
            {
                Console.WriteLine("Nemate dovoljno sredstava na vasem racunu.\n");
                return;
            }

            buyer.Balance -= amount;
            product.Status = ProductStatusEnum.Sold;
            buyer.PurchasedProducts.Add(product.Id);

            var transaction = new Transaction(product.Id, buyer.Id, seller.Id, -amount);
            _transactionRepository.Add(transaction);
            

            Console.WriteLine($"Proizvod {product.Name} uspjesno kupljen.\n");
        }

        public void ReturnProduct(Guid buyerId, Guid productId)
        {
            var product = _productRepository.GetProductById(productId);
            var buyer = _userRepository.GetById(buyerId) as Buyer;
            var seller = _userRepository.GetAllSellers().FirstOrDefault(seller => seller.OwnedProducts.Contains(productId));

            if (product == null || product.Status != ProductStatusEnum.Sold || !buyer.PurchasedProducts.Contains(productId))
            {
                Console.WriteLine("Proizvod nije kupljen ili je vec vracen.\n");
                return;
            }

            decimal refundAmount = Math.Round(product.Price * 0.80m, 2);
            buyer.Balance += refundAmount;
            seller.TotalEarnings -= Math.Round(product.Price * 0.85m, 2);
            product.Status = ProductStatusEnum.OnSale;
            buyer.PurchasedProducts.Remove(product.Id);

            var returnTransaction = new Transaction(product.Id, buyer.Id, seller.Id, 80);
            _transactionRepository.Add(returnTransaction);

            Console.WriteLine($"Proizvod {product.Name} uspjesno vracen. Povrat: {refundAmount} EUR.\n");
        }

        public void AddProductToFavourites(Guid buyerId, Guid productId)
        {
            var product = _productRepository.GetProductById(productId);
            var buyer = _userRepository.GetById(buyerId) as Buyer;

            if (product == null || product.Status != ProductStatusEnum.OnSale)
            {
                Console.WriteLine("Proizvod nije dostupan.");
                return;
            }

            buyer.FavouriteProducts.Add(product.Id);
            Console.WriteLine($"Proizvod {product.Name} dodan u favorite.\n");
        }

        public List<Product> GetPurchasedProducts(Guid buyerId)
        {
            var buyer = _userRepository.GetById(buyerId) as Buyer;
            var products = _productRepository.GetAllProducts();

            var purchasedProducts = products.Where(product => buyer.PurchasedProducts.Contains(product.Id)).ToList();

            return purchasedProducts;
        }

        public List<Transaction> GetBuyerTransactions(Guid buyerId)
        {
            return _transactionRepository.GetByUser(buyerId);
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
                Console.WriteLine("Kategorija nije pronadena.\n");
                return;
            }

            var seller = _userRepository.GetById(sellerId) as Seller;
            var product = new Product(name, description, price, categoryId);

            seller.OwnedProducts.Add(product.Id);
            _productRepository.AddProduct(product);

            Console.WriteLine($"Proizvod {name} dodan na prodaju.\n");
        }

        public List<Product> GetSellerProducts(Guid sellerId)
        {
            var seller = _userRepository.GetById(sellerId) as Seller;
            var products = _productRepository.GetAllProducts().Where(p => seller.OwnedProducts.Contains(p.Id)).ToList();

            return products;
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

            var soldProducts = GetSellerProducts(sellerId).FindAll(p => p.Status == ProductStatusEnum.Sold && p.CategoryId == categoryId);
            return soldProducts;
        }

        public decimal GetTotalEarningsByDate(Guid sellerId, DateTime startDate, DateTime endDate)
        {
            var filteredTransactions = _transactionRepository.GetByUser(sellerId);
            var totalEarnings = 0m;

            foreach (var transaction in filteredTransactions)
            {
                if (transaction.Date > startDate && transaction.Date < endDate)
                {
                    totalEarnings += transaction.Amount * - 5 * (transaction.Amount / 100);
                }
            }
            return Math.Round(Math.Abs(totalEarnings), 2);
        }

        public List<PromoCode> GetPromoCodes(Guid productId)
        {
            var productCategoryId = _productRepository.GetProductById(productId).CategoryId;
            var promoCodes = _categoryRepository.GetById(productCategoryId).PromoCodes;
            return promoCodes;
        }

        public List<Category> GetCategories()
        {
            return _categoryRepository.GetAll().ToList();
        }
    }
}
