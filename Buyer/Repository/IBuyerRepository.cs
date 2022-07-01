using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Repository
{
    public interface IBuyerRepository
    {
        public Task CreateAsync(User user);
        public Task<bool> InsertOrUpsert(User user);

        public Task InsertProduct(Product product);
    }
}
