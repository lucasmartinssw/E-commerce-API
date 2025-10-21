﻿namespace Ecommerce.Domain.Entities;

public class CartItem
{
    public long Id { get; set; }
    public int Quantity { get; set; }

   
    public long CartId { get; set; }
    public Cart Cart { get; set; } = null!;

    public long ProductId { get; set; }
    public Product Product { get; set; } = null!;
}