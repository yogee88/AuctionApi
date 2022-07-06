using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
//using MongoDB.Driver;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Seller.Models;
using Microsoft.EntityFrameworkCore;

namespace Seller.Repository
{
    public class Repository : IRepository
    {
        private readonly CoreDbContext coreDbContext;

        public Repository(CoreDbContext coreDbContext)
        {
            this.coreDbContext = coreDbContext;
        }

        public async Task<List<User>> GetAsync() =>
            await this.coreDbContext.User.ToListAsync();

        public async Task<List<Product>> GetAsync(int id) =>
            await this.coreDbContext.Product.Where(x => x.UserId == id).ToListAsync();
        public async Task<User> GetUserAsyncByEmail(string email) =>
           await this.coreDbContext.User.Where(x => x.Email == email).FirstOrDefaultAsync();

        public async Task<Product> GetAsyncByName(string name) =>
           await this.coreDbContext.Product.Where(x => x.Name == name).FirstOrDefaultAsync();

        public async Task CreateAsync(User user)
        {
            this.coreDbContext.User.Add(user);
            await this.coreDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, User user)
        {
            this.coreDbContext.User.Update(user);
            await this.coreDbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var user = this.coreDbContext.User.Where(x => x.Id == id).FirstOrDefault();
            this.coreDbContext.User.Remove(user);
            await this.coreDbContext.SaveChangesAsync();
        }

        public async Task InsertOrUpsert(ProductBid bid)
        {
            if (bid.ProductId > 0)
            {

                var productBid = await this.coreDbContext.ProductBid.Where(x => x.ProductId == bid.ProductId && x.BidderId == bid.BidderId).FirstOrDefaultAsync();

                if (productBid != null && productBid.ProductBidId > 0)
                {
                    productBid.BidAmount = bid.BidAmount;
                    productBid.UpdatedDate = DateTime.UtcNow;
                    productBid.Phone = bid.Phone;

                    this.coreDbContext.ProductBid.Update(productBid);
                    await this.coreDbContext.SaveChangesAsync();
                }
                else
                {
                    await CreateProductBids(bid);
                }
            }
        }

        public async Task<bool> DeleteProduct(int id)
        {
            //if (id > 0)
            //{
            //    var productDetails = await _collection.Find(x => x.Products.Any(x => x.ProductId == id && x.BidEndDate <= DateTime.Today)).FirstOrDefaultAsync();
            //    if (productDetails != null && productDetails.Products.Any())
            //    {
            //        return false;
            //    }

            //    var bids = await _bidscollection.Find(x => x.ProductId == id).ToListAsync();
            //    if (bids.Any())
            //    {
            //        return false;
            //    }
            //    // Update
            //    var update = Builders<User>.Update.PullFilter(p => p.Products, f => f.ProductId == id);
            //    var result = await _collection.FindOneAndUpdateAsync(p => p.Products.Any(x => x.ProductId == id), update);
            //    return true;
            //}

            return false;
        }

        public async Task CreateProductBids(ProductBid bids)
        {
            bids.CreatedDate = DateTime.UtcNow;
            await this.coreDbContext.ProductBid.AddAsync(bids);
            await this.coreDbContext.SaveChangesAsync();
            return;
        }

        public async Task<ProductBidsResponse> GetProductBids(int productId)
        {
            //var user = await _collection.Find(x => x.Products.Any(x => x.ProductId == productId)).FirstOrDefaultAsync();
            var product = this.coreDbContext.Product.Where(x => x.ProductId == productId).FirstOrDefault();
            var bids = await this.coreDbContext.ProductBid.Where(x => x.ProductId == productId).ToListAsync();
            return new ProductBidsResponse
            {
                Product = product,
                ProductBids = bids
            };
        }
    }
}
