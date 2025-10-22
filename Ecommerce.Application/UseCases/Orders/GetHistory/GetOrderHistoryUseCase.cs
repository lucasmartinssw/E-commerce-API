using Ecommerce.Communication.Responses;
using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Application.UseCases.Orders.GetHistory;

public class GetOrderHistoryUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;

    public GetOrderHistoryUseCase(IUserRepository userRepository, IOrderRepository orderRepository)
    {
        _userRepository = userRepository;
        _orderRepository = orderRepository;
    }

    public async Task<ResponseOrderHistoryJson> Execute(string userEmail)
    {
        var user = await _userRepository.GetByEmail(userEmail);
        if (user == null)
        {
            throw new ValidationErrorsException(new List<string> { "Usuário não autenticado." });
        }

        var orders = await _orderRepository.GetAllByUserId(user.Id);
        
        return new ResponseOrderHistoryJson
        {
            Orders = orders.Select(order => new ResponseOrderShortJson
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount
            }).ToList()
        };
    }
}