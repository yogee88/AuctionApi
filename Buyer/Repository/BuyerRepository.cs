using Common.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Buyer.Repository
{
    public class BuyerRepository : IBuyerRepository
    {
        private readonly CoreDbContext coreDbContext;

        public BuyerRepository(CoreDbContext coreDbContext)
        {
            this.coreDbContext = coreDbContext;
        }

        public async Task CreateAsync(User user)
        {
            this.coreDbContext.ProductBid.Add(user.ProductBids.First());
            await this.coreDbContext.SaveChangesAsync();
        }

        public async Task<User> GetAsync(int id) =>
           await this.coreDbContext.User.Where(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<bool> InsertOrUpsert(User user)
        {
            if (user.Id > 0 || !string.IsNullOrWhiteSpace(user.Email))
            {
                // Update
                var existingBids = await this.coreDbContext.User.Where(x => x.Email == user.Email).FirstOrDefaultAsync();
                if (existingBids?.ProductBids != null && existingBids.ProductBids.Any(x => x.ProductId == user.ProductBids.FirstOrDefault().ProductId))
                {
                    if (await isValidBidEndDate(user.ProductBids.FirstOrDefault().ProductId))
                    {
                        //var filter = Builders<User>.Filter.Where(x => x.Email == user.Email && x.ProductBids.Any(p => p.ProductId == user.ProductBids.FirstOrDefault().ProductId));
                        //var update = existingBids.ProductBids.Length == 0 ?
                        //    Builders<User>.Update.AddToSet<ProductBid>("ProductBids", user.ProductBids.FirstOrDefault())
                        //    : Builders<User>.Update.Combine(
                        //        Builders<User>.Update.Set(x => x.ProductBids[-1].BidAmount, user.ProductBids.FirstOrDefault().BidAmount),
                        //        Builders<User>.Update.Set(x => x.ProductBids[-1].UpdatedDate, user.ProductBids.FirstOrDefault().UpdatedDate));
                        //var result = await _collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
                        //if (result != null)
                        //{
                        return true;
                        // }
                    }
                }
                if (await isValidBidEndDate(user.ProductBids.FirstOrDefault().ProductId))
                {
                    if (existingBids != null)
                    {
                        //var filter = Builders<User>.Filter.Where(x => x.Id == user.Id);
                        //var update = Builders<User>.Update.AddToSet<ProductBid>("ProductBids", user.ProductBids.FirstOrDefault());
                        //var result = await _collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
                        //if (result != null)
                        //{
                        return true;
                        /// }
                    }
                    else
                    {
                        await CreateAsync(user);
                        return true;
                    }
                }
            }
            else if (user.Id > 0)
            {
                await CreateAsync(user);
                return true;
            }
            return false;
        }

        public async Task InsertProduct(Product product)
        {
            await this.coreDbContext.Product.AddAsync(product);
            await this.coreDbContext.SaveChangesAsync();
        }

        private async Task<bool> isValidBidEndDate(int id)
        {
            var product = await this.coreDbContext.Product.Where(x => x.ProductId == id).FirstOrDefaultAsync();
            return product != null && product.BidEndDate > DateTime.Today;
        }
    }
}
