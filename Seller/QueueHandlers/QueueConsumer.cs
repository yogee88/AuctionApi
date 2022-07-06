using Common.Models;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Seller.Models;
using Seller.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Seller.QueueHandlers
{
    public class QueueConsumer : IConsumer<CommandEvent>
    {
        private readonly IRepository _repo;

        private readonly IMediator _mediator;

        public QueueConsumer(IRepository repo, IMediator mediator)
        {
            _repo = repo;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<CommandEvent> context)
        {
            var productBid = context.Message;
            var eventQueue = new ProductBid
            {
                ProductId = productBid.ProductId,
                BidAmount = productBid.BidAmount,
                CreatedDate = productBid.CreatedDate,
                UpdatedDate = productBid.UpdatedDate,
                EmailId = productBid.EmailId,
                BidderId = productBid.BidderId,
                Phone = productBid.Phone,
                Name = productBid.Name
            };
            //await _repo.CreateProductBids(eventQueue);
            var isInserted = await this._mediator.Send(eventQueue);
        }

    }
}
