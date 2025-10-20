
using Ecommerce.Domain.Enums;
using System;

namespace Ecommerce.Domain.Entities;

public class Payment
{
    public long OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public string PaymentMethod { get; set; } = string.Empty;
    public PaymentStatusType Status { get; set; } = PaymentStatusType.pending;
    public string? TransactionId { get; set; }
    public DateTime? PaidAt { get; set; }
}