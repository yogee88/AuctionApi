using MediatR;

namespace Seller.Models
{
    public class ProductBidsRequest : IRequest<ProductBidsResponse>
    {
        public int ProductId { get; set; }
    }
}
