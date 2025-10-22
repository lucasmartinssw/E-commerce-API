using Ecommerce.Communication.Responses;
using Ecommerce.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.Orders.GetAllAdmin;

public class GetAllOrdersAdminUseCase
{
    private readonly IOrderRepository _orderRepository;

    public GetAllOrdersAdminUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<ResponseAllAdminOrdersJson> Execute()
    {
        var orders = await _orderRepository.GetAllAdmin();

        return new ResponseAllAdminOrdersJson
        {
            Orders = orders.Select(order => new ResponseAdminOrderSummaryJson
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                UserId = order.UserId,
                UserName = order.User.Name, 
                UserEmail = order.User.Email 
            }).ToList()
        };
    }
}