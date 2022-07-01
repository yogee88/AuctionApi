using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Model
{
    public class User
    {
        public string EmailId { get; set; }        

        public string Password { get; set; }

        public string Token { get; set; }
        public int Id { get; set; }

        public bool IsAuthenticated { get; set; }
    }
}
