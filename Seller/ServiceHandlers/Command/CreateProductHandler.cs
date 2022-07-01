using Common.Models;
using MediatR;
using Seller.Models;
using Seller.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Seller.ServiceHandlers.Command
{
    public class CreateProductHandler : IRequestHandler<ProductBid, bool>
    {
        private readonly IRepository _repo;

        public CreateProductHandler(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(ProductBid request, CancellationToken cancellationToken)
        {
            try
            {
                await _repo.CreateProductBids(request);
                return true;
            }
            catch 
            {
                return false;
            }            
        }
    }
}
