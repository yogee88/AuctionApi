using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Models
{
    public class ProductBid
    {
        [BsonElement("productId")]
        public int ProductId { get; set; }

        [BsonElement("bidAmount")]
        public int BidAmount { get; set; }

        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("updatedDate")]
        public DateTime? UpdatedDate { get; set; }
    }
}
