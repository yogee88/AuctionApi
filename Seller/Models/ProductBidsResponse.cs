using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Models
{
    public class ProductBidsResponse
    {
        public Product Product { get; set; }

        public List<ProductBid> ProductBids { get; set; }

    }
}
