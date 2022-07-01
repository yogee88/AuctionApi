//using Identity.Model;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Repository
{
    public interface IAccountRepository
    {
        public Task<List<User>> GetAsync(string emailId);
    }
}
