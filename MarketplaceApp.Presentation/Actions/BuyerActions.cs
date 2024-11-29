using MarketplaceApp.Domain;
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


    }
}
