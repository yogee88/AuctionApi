using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Seller.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("firstName")]
        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        [Required]
        [MinLength(5)]
        [MaxLength(25)]
        public string SecondName { get; set; }

        [BsonElement("email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [BsonElement("phone")]
        [Required]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; }

        [BsonElement("address")]
        public Address Address { get; set; }
                
        public Product[] Products { get; set; }
    }
}
