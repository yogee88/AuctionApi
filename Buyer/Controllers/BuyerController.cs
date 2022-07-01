using Common.Models;
using Buyer.Repository;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BuyerController : ControllerBase
    {
        private readonly IBuyerRepository _repo;
        private readonly IPublishEndpoint _publishEndPoint;

        public BuyerController(IBuyerRepository repo, IPublishEndpoint publishEndPoint)
        {
            _repo = repo;
            _publishEndPoint = publishEndPoint;
        }

        //placebid
        [Route("place-bid", Name = "Place Bid")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            try
            {
                if (await _repo.InsertOrUpsert(user))
                {
                    var productBid = user.ProductBids.FirstOrDefault();
                    var eventQueue = new CommandEvent
                    {
                        ProductId = productBid.ProductId,
                        BidAmount = productBid.BidAmount,
                        CreatedDate = productBid.CreatedDate,
                        UpdatedDate = productBid.UpdatedDate,
                        BidderId = user.Id,
                        EmailId = user.Email,
                        Phone = user.Phone,
                        Name = string.Format("{0} {1}", user.FirstName, user.LastName)
                    };
                    await _publishEndPoint.Publish(eventQueue);
                    return Accepted();
                }
                return BadRequest("Bid Date Expired or Bid Already Exists");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
 
        }

        //update-bid/{productId}/{buyerEmailld}/{ newBidAmount}
        [Route("update-bid/{productId}/{buyerEmailId}/{newBidAmount}", Name = "Update Bid")]
        [HttpPut]
        public async Task<IActionResult> Update(int productId, string buyerEmailId, int newBidAmount)
        {
            try
            {
                var user = new User
                {
                    Email = buyerEmailId,
                    ProductBids = new[]
                    {
                        new ProductBid
                        {
                            BidAmount  = newBidAmount,
                            ProductId = productId,
                            UpdatedDate = DateTime.Now
                        }
                    }
                };
                if (ModelState.IsValid)
                {
                    await _repo.InsertOrUpsert(user);
                    return Ok();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return BadRequest("Bid Date Expired");
        }
    }
}
