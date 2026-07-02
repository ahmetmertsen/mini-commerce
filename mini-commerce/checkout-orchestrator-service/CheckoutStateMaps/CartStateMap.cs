using checkout_orchestrator_service.CheckoutStateInstances;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkout_orchestrator_service.CheckoutStateMaps
{
    public class CartStateMap : SagaClassMap<CartStateInstance>
    {
        protected override void Configure(EntityTypeBuilder<CartStateInstance> entity, ModelBuilder model)
        {
            entity.HasKey(x => x.CorrelationId);

            entity.Property(x => x.CurrentState)
                .HasMaxLength(100);

            entity.Property(x => x.CustomerId)
                .IsRequired();

            entity.Property(x => x.CartId)
                .IsRequired();

            entity.Property(x => x.TotalAmount)
                .HasPrecision(18, 2);

            entity.Property(x => x.FailureReason)
                .HasMaxLength(500);
        }
    }
}
