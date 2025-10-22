using System;

namespace Ecommerce.Communication.Responses;

public class ResponseOrderShortJson
{
    public long Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}