using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("ProductId")]
        public int ProductId { get; set; }
        
        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string DetailedDescription { get; set; }
        
        public double StartingPrice { get; set; }
        
        public DateTime BidEndDate { get; set; }
    }
}
