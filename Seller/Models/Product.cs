using Common.Attributes;
using Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Seller.Models
{
    public class Product
    {
        [BsonElement("ProductId")]
        public string ProductId { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string DetailedDescription { get; set; }

        [RequiredEnum(ErrorMessage = "Category is Required")]
        public Category Category { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Starting Price must be number")]
        public double StartingPrice { get; set; }

        [DateValidation(IsFutureDate = true)]
        [Display(Name = "Bid End Date")]
        public DateTime BidEndDate { get; set; }            
    }
}
