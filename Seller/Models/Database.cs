using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Models
{
    public class Database
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string UserCollection { get; set; } = null!;
        public string ProductBidsCollection { get; set; } = null!;
    }
}
