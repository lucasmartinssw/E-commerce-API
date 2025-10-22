using Ecommerce.Communication.Requests;
using Ecommerce.Communication.Responses;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Repositories;
using Ecommerce.Exceptions;
using Ecommerce.Exceptions.ExceptionsBase;
using Ecommerce.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore; 
using System.Collections.Generic;
using System.Linq;

namespace Ecommerce.Application.UseCases.Orders.Checkout;

public class CheckoutUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly EcommerceDbContext _dbContext; 

    public CheckoutUseCase(
        IUserRepository userRepository, ICartRepository cartRepository,
        IAddressRepository addressRepository, IProductRepository productRepository,
        IOrderRepository orderRepository, EcommerceDbContext dbContext) 
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
        _addressRepository = addressRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _dbContext = dbContext;
    }

    public async Task<ResponseOrderJson> Execute(string userEmail, RequestCreateOrderJson request)
    {
        var user = await _userRepository.GetByEmail(userEmail);
        var cart = await _cartRepository.GetByUserId(user!.Id); 

        if (user == null || cart == null || !cart.Items.Any())
        {
            throw new ValidationErrorsException(new List<string> { "Carrinho vazio ou usuário inválido." });
        }

        
        var shippingAddress = await _addressRepository.GetById(request.ShippingAddressId);
        if (shippingAddress == null || shippingAddress.UserId != user.Id)
        {
            throw new ValidationErrorsException(new List<string> { "Endereço de entrega inválido." });
        }

        var productIds = cart.Items.Select(i => i.ProductId).ToList();
        var productsInCart = await _dbContext.Products
                                     .Where(p => productIds.Contains(p.Id))
                                     .ToListAsync();

        var orderItems = new List<OrderItem>();
        decimal totalAmount = 0;

        foreach (var item in cart.Items)
        {
            var product = productsInCart.FirstOrDefault(p => p.Id == item.ProductId);
            if (product == null || product.StockQuantity < item.Quantity)
            {
                throw new ValidationErrorsException(new List<string> { $"Estoque insuficiente para o produto: {product?.Name ?? "ID " + item.ProductId}" });
            }

            product.StockQuantity -= item.Quantity;
            product.UpdatedAt = DateTime.UtcNow;

            orderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                PriceAtPurchase = product.Price
            });

            totalAmount += item.Quantity * product.Price;
        }

        
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            
            var order = new Order
            {
                UserId = user.Id,
                ShippingAddressId = request.ShippingAddressId,
                TotalAmount = totalAmount,
                Status = OrderStatusType.pending, 
                OrderDate = DateTime.UtcNow,
                OrderItems = orderItems
            };
            await _orderRepository.Add(order);

            _dbContext.Products.UpdateRange(productsInCart); 

            await _cartRepository.ClearItems(cart.Id);


            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();

            
            return new ResponseOrderJson
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount
            };
        }
        catch (Exception ex)
        {
            
            await transaction.RollbackAsync();
            
            throw new ValidationErrorsException(new List<string> { "Erro ao processar o pedido. Tente novamente." });
        }
    }
}