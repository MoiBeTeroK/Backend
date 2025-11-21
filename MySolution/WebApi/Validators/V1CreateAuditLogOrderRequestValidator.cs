using FluentValidation;
using Models.Dto.V1.Requests;


public class V1CreateAuditLogOrderRequestValidator : AbstractValidator<V1AuditLogOrderRequest>
{
    public V1CreateAuditLogOrderRequestValidator()
    {
        RuleFor(x => x.Orders)
            .NotNull()
            .Must(orders => orders.All(o => o.OrderId > 0))
            .WithMessage("Все OrderId должны быть больше 0")
            .Must(orders => orders.All(o => o.OrderItemId > 0))
            .WithMessage("Все OrderItemId должны быть больше 0")
            .Must(orders => orders.All(o => o.CustomerId > 0))
            .WithMessage("Все CustomerId должны быть больше 0")
            .Must(orders => orders.All(o => !string.IsNullOrEmpty(o.OrderStatus)))
            .WithMessage("Все OrderStatus должны быть заполнены");
    }
}