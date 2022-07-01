using Common.Models;
using Buyer.Repository;
using EventBus.Messages.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.QueueConsumer
{
    public class QueueConsumer : IConsumer<ProductCommandEvent>
    {
        private readonly IBuyerRepository _repo;
        public QueueConsumer(IBuyerRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task Consume(ConsumeContext<ProductCommandEvent> context)
        {
            var product = context.Message;
            var data = new Product
            {
                BidEndDate = product.BidEndDate,
                ProductId = product.ProductId,
                Name = product.Name,
                DetailedDescription = product.DetailedDescription,
                ShortDescription = product.ShortDescription,
                StartingPrice = product.StartingPrice
            };
            await _repo.InsertProduct(data);
        }
    }
}
