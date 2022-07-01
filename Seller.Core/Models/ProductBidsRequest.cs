using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seller.Core.Models
{
    public class ProductBidsRequest : IRequest<ProductBid[]>
    {
        public string ProductId { get; set; }
    }
}
