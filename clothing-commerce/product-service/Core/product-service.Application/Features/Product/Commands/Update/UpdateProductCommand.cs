using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product_service.Application.Features.Product.Commands.Update
{
    public class UpdateProductCommand : IRequest<UpdateProductCommandResponse>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
    }
}
