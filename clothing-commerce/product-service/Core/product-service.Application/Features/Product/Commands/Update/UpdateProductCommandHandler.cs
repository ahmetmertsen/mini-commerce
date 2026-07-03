using FluentValidation;
using MediatR;
using product_service.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product_service.Application.Features.Product.Commands.Update
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, UpdateProductCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<UpdateProductCommand> _validator;

        public UpdateProductCommandHandler(IProductRepository productRepository, IValidator<UpdateProductCommand> validator)
        {
            _productRepository = productRepository;
            _validator = validator;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null)
            {
                throw new Exception("Ürün bulunamadı.");
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Brand = request.Brand;
            product.CategoryName = request.CategoryName;
            product.UpdatedDate = DateTime.UtcNow;

            await _productRepository.SaveChangesAsync();
            return new UpdateProductCommandResponse
            {
                Succeeded = true,
                Message = "Ürün başarıyla güncellendi.",
                ProductId = product.Id
            };
        }
    }
}
