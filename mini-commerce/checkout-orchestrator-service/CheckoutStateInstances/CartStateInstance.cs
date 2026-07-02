using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkout_orchestrator_service.CheckoutStateInstances
{
    public class CartStateInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; } = null!;

        public Guid CartId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalAmount { get; set; }

        public Guid? PaymentId { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? InvoiceId { get; set; }
        public Guid? ShipmentId { get; set; }

        public string? FailureReason { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
