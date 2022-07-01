using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Models
{
    public class ProductBid : IRequest<bool>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("productId")]
        public string ProductId { get; set; }

        [BsonElement("bidAmount")]
        public int BidAmount { get; set; }

        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("updatedDate")]
        public DateTime? UpdatedDate { get; set; }

        [BsonElement("emailId")]
        public string EmailId { get; set; }

        [BsonElement("bidderId")]
        public string BidderId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }
    }
}
