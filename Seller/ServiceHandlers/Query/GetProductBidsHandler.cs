using MediatR;
using Seller.Models;
using Seller.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Seller.ServiceHandlers.Query
{
    public class GetProductBidsHandler : IRequestHandler<ProductBidsRequest, ProductBidsResponse>
    {
        private readonly IRepository _repo;

        public GetProductBidsHandler(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<ProductBidsResponse> Handle(ProductBidsRequest request, CancellationToken cancellationToken)
        {
            var bids = await _repo.GetProductBids(request.ProductId);
            return bids;
        }
    }
}
