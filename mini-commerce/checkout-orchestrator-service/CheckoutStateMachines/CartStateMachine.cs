using checkout_orchestrator_service.CheckoutStateInstances;
using MassTransit;
using Shared.Events.CartEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkout_orchestrator_service.CheckoutStateMachines
{
    public class CartStateMachine : MassTransitStateMachine<CartStateInstance>
    {
        public Event<CartConfirmationEvent> CartConfirmationEvent { get; private set; }
        

        public State CartConfirmed { get; private set; }


        public CartStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => CartConfirmationEvent, x =>
            {
                x.CorrelateById(context => context.Message.CorrelationId);
                x.SelectId(context => context.Message.CorrelationId);
            });

            Initially(
                When(CartConfirmationEvent)
                .Then(context =>
                {
                    context.Saga.CartId = context.Message.CartId;
                    context.Saga.CustomerId = context.Message.CustomerId;
                    context.Saga.TotalAmount = context.Message.TotalAmount;
                    context.Saga.CreatedDate = DateTime.UtcNow;
                    context.Saga.UpdatedDate = DateTime.UtcNow;
                })
                .TransitionTo(CartConfirmed));
                
        }
    }
}
