using System;

namespace Ecommerce.Communication.Responses;

public class ResponseAdminOrderSummaryJson
{
    public long Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public long UserId { get; set; } 
    public string UserName { get; set; } = string.Empty; 
    public string UserEmail { get; set; } = string.Empty;
}