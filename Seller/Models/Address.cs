using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Models
{
    public class Address
    {
        [BsonElement("addressLine")]
        public string AddressLine { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("state")]
        public string State { get; set; }

        [BsonElement("pin")]
        public string Pin { get; set; }
    }
}
