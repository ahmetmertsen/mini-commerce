using FluentValidation;
using MediatR;
using product_service.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product_service.Application.Features.Product.Commands.UpdateVariantStock
{
    public class UpdateProductVariantStockCommandHandler : IRequestHandler<UpdateProductVariantStockCommand, UpdateProductVariantStockCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<UpdateProductVariantStockCommand> _validator;

        public UpdateProductVariantStockCommandHandler(IProductRepository productRepository, IValidator<UpdateProductVariantStockCommand> validator)
        {
            _productRepository = productRepository;
            _validator = validator;
        }

        public async Task<UpdateProductVariantStockCommandResponse> Handle(UpdateProductVariantStockCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var variant = await _productRepository.GetVariantByIdAsync(request.VariantId);

            if (variant == null)
            {
                throw new Exception("Ürün varyantı bulunamadı.");
            }

            variant.StockQuantity = request.StockQuantity;
            await _productRepository.SaveChangesAsync();

            return new UpdateProductVariantStockCommandResponse
            {
                Succeeded = true,
                Message = "Varyant stok bilgisi başarıyla güncellendi.",
                VariantId = variant.Id
            };
        }
    }
}
