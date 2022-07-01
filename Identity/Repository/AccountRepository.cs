using Common.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Identity.Repository
{
    public class AccountRepository : IAccountRepository
    {
        //private readonly IMongoCollection<User> _collection;
        private readonly CoreDbContext coreDbContext;

        public AccountRepository(CoreDbContext coreDbContext)
        {
            this.coreDbContext = coreDbContext;
        }
        public async Task<List<User>> GetAsync(string emailId)
        {
            var users = await coreDbContext.User.Where(x => x.Email == emailId).ToListAsync();

            return users;
        }
    }
}
