using Azure.Messaging.ServiceBus;
using Common.Models;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
//using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Seller.Models;
using Seller.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Seller.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndPoint;
        private readonly IQueueClient _queueClient;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _clientSender;
        private const string queueName = "from-rabbitmq";
        ///private readonly CoreDbContext coreDbContext;
        public SellerController(IRepository repo, IMediator mediator, IPublishEndpoint publishEndPoint, IConfiguration configuration)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _publishEndPoint = publishEndPoint ?? throw new ArgumentNullException(nameof(publishEndPoint));
            //const string connectionString = "";
            var connectionString = configuration.GetConnectionString("ServiceBusConnectionString");
            _client = new ServiceBusClient(connectionString);
            _clientSender = _client.CreateSender(queueName);

            _queueClient = new QueueClient(connectionString, queueName);
        }

        [Route("api/Seller/AddProduct")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            try
            {
                await _repo.InsertOrUpsert(user);
                var product = user.Products.FirstOrDefault();
                if (product != null)
                {
                    var command = new ProductCommandEvent
                    {
                        BidEndDate = product.BidEndDate,
                        ProductId = product.ProductId,
                        Name = product.Name,
                        DetailedDescription = product.DetailedDescription,
                        ShortDescription = product.ShortDescription,
                        StartingPrice = product.StartingPrice
                    };
                    await _publishEndPoint.Publish(command);

                    //string messagePayload = JsonSerializer.Serialize(command);
                    string messagePayload = JsonConvert.SerializeObject(command);
                    ServiceBusMessage message = new ServiceBusMessage(messagePayload);
                    await _clientSender.SendMessageAsync(message).ConfigureAwait(false);
                }


                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [Route("api/Seller/DeleteProduct/{productId}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int productId)
        {
            try
            {
                var isDeleted = await _repo.DeleteProduct(productId);
                if (isDeleted)
                {
                    return Ok();
                }
                return BadRequest("Delete Failed - Bid End Date Crossed or Bids Available");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [Authorize]
        [Route("api/Seller/ShowBids/{productId}")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetUser(int productId)
        {
            try
            {
                var result = await _mediator.Send(new ProductBidsRequest { ProductId = productId });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [Route("api/Seller/Products/{userId}")]
        [HttpGet]
        public async Task<Product[]> Products(string userId)
        {
            try
            {
                var products = await _repo.GetAsync(Convert.ToInt32(userId));
                return products.ToArray();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [NonAction]
        public async void SendMessageAsync(string queueName, string messageItem, string messageLabel = null, Dictionary<string, object> messageProperties = null)
        {
            try
            {
                Message message = BuildMessage(messageItem, messageLabel, messageProperties);
                await _queueClient.SendAsync(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private Message BuildMessage(string messageItem, string messageLabel, Dictionary<string, object> messageProperties)
        {
            Message message = new(Encoding.UTF8.GetBytes(messageItem));
            message.UserProperties["TimeStampUTC"] = DateTime.UtcNow;
            message.Label = messageLabel;
            if (messageProperties != null)
            {
                foreach (var messageProperty in messageProperties)
                {
                    message.UserProperties[messageProperty.Key] = messageProperty.Value;
                }
            }
            return message;
        }
    }
}
