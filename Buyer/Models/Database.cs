using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Models
{
    public class Database1
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string UserCollection { get; set; } = null!;

        public string ProductCollection { get; set; } = null!;
    }
}
