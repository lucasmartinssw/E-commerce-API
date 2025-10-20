using Ecommerce.Domain.Enums; 
using System;
using System.Collections.Generic;

namespace Ecommerce.Domain.Entities;

public class Order
{
    public long Id { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatusType Status { get; set; } = OrderStatusType.pending;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;


    public long UserId { get; set; }
    public User User { get; set; } = null!;


    public long ShippingAddressId { get; set; }
    public Address ShippingAddress { get; set; } = null!;

  
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();


    public Payment? Payment { get; set; }
}