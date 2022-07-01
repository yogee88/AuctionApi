using Common.Models;
using Seller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Repository
{
    public interface IRepository
    {
        public Task<List<User>> GetAsync();

        public Task<List<Product>> GetAsync(int id);

        public Task CreateAsync(User user);

        public Task InsertOrUpsert(User user);

        public Task<bool> DeleteProduct(int id);

        public Task CreateProductBids(ProductBid bids);

        public Task<ProductBidsResponse> GetProductBids(int productId);
    }
}
