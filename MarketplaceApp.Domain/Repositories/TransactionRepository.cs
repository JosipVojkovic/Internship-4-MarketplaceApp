using MarketplaceApp.Data;
using MarketplaceApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MarketplaceApp.Domain.Repositories
{
    public class TransactionRepository
    {
        private readonly MarketplaceContext _marketplaceContext;

        public TransactionRepository(MarketplaceContext marketplaceContext)
        {
            _marketplaceContext = marketplaceContext;
        }

        public void Add(MarketplaceApp.Data.Entities.Transaction transaction)
        {
            _marketplaceContext.Transactions.Add(transaction);
        }

        public List<MarketplaceApp.Data.Entities.Transaction> GetAll()
        {
            return _marketplaceContext.Transactions;
        }

        public MarketplaceApp.Data.Entities.Transaction GetById(Guid transactionId)
        {
            return _marketplaceContext.Transactions.FirstOrDefault(t => t.Id == transactionId);
        }

        public List<MarketplaceApp.Data.Entities.Transaction> GetByUser(Guid userId)
        {
            return _marketplaceContext.Transactions.Where(t => t.BuyerId == userId || t.SellerId == userId).ToList();
        }
    }
}
