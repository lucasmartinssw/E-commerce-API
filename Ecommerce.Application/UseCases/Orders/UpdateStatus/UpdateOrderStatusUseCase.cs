using Ecommerce.Communication.Requests;
using Ecommerce.Domain.Enums; 
using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.Orders.UpdateStatus;

public class UpdateOrderStatusUseCase
{
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderStatusUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Execute(long orderId, RequestUpdateOrderStatusJson request)
    {
        
        if (!Enum.TryParse<OrderStatusType>(request.NewStatus, true, out var newStatus))
        {
            throw new ValidationErrorsException(new List<string> { $"Status inválido: '{request.NewStatus}'. Os valores permitidos são: {string.Join(", ", Enum.GetNames<OrderStatusType>())}" });
        }

        var order = await _orderRepository.GetByIdTracked(orderId);
        if (order == null)
        {
            throw new ValidationErrorsException(new List<string> { "Pedido não encontrado." });
        }

    
        order.Status = newStatus;
        await _orderRepository.Update(order);
    }
}