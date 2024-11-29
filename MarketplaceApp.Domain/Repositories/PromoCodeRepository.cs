using MarketplaceApp.Data;
using MarketplaceApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Domain.Repositories
{
    public class PromoCodeRepository
    {
        private readonly MarketplaceContext _marketplaceContext;

        public PromoCodeRepository(MarketplaceContext marketplaceContext)
        {
            _marketplaceContext = marketplaceContext;
        }
        public IEnumerable<PromoCode> GetAll()
        {
            return _marketplaceContext.PromoCodes;
        }

        public PromoCode GetById(Guid id)
        {
            return _marketplaceContext.PromoCodes.FirstOrDefault(p => p.Id == id);
        }

        public PromoCode GetByCode(string code)
        {
            return _marketplaceContext.PromoCodes.FirstOrDefault(p => p.Code == code);
        }

        public void Add(PromoCode promoCode)
        {
            _marketplaceContext.PromoCodes.Add(promoCode);
        }

        public void Update(PromoCode promoCode)
        {
            var existingPromoCode = _marketplaceContext.PromoCodes.FirstOrDefault(p => p.Id == promoCode.Id);
            if (existingPromoCode != null)
            {
                existingPromoCode.Code = promoCode.Code;
                existingPromoCode.DiscountPercentage = promoCode.DiscountPercentage;
                existingPromoCode.ExpirationDate = promoCode.ExpirationDate;
            }
        }

        public void Remove(Guid id)
        {
            var promoCode = _marketplaceContext.PromoCodes.FirstOrDefault(p => p.Id == id);
            if (promoCode != null)
            {
                _marketplaceContext.PromoCodes.Remove(promoCode);
            }
        }
    }
}
