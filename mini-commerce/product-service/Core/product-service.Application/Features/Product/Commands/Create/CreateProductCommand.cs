using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product_service.Application.Features.Product.Commands.Create
{
    public class CreateProductCommand : IRequest<CreateProductCommandResponse>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
    }
}
