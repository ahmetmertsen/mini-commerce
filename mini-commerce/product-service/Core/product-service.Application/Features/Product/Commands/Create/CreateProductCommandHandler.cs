using FluentValidation;
using MediatR;
using product_service.Application.Repositories;
using product_service.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product_service.Application.Features.Product.Commands.Create
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<CreateProductCommand> _validator;

        public CreateProductCommandHandler(IProductRepository productRepository, IValidator<CreateProductCommand> validator)
        {
            _productRepository = productRepository;
            _validator = validator;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var product = new Domain.Entities.Product(request.Name, request.Description, request.Brand, request.CategoryName);

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return new CreateProductCommandResponse
            {
                Succeeded = true,
                Message = "Ürün başarıyla oluşturuldu",
                ProductId = product.Id
            };
        }
    }
}
