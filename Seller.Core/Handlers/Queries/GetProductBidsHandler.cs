using MediatR;
using Seller.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Seller.Core.Handlers.Queries
{
    public class GetProductBidsHandler : IRequestHandler<ProductBidsRequest, ProductBid[]>
    {
        public async Task<ProductBid[]> Handle(ProductBidsRequest request, CancellationToken cancellationToken)
        {
            return new[] { new ProductBid { } };
        }
    }
}
