using checkout_orchestrator_service.CheckoutStateMaps;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkout_orchestrator_service.CheckoutDbContext
{
    public class CheckoutSagaDbContext : SagaDbContext
    {
        public CheckoutSagaDbContext(DbContextOptions<CheckoutSagaDbContext> options) : base(options) { }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get
            {
                yield return new CartStateMap();
            }
        }

    }
}
