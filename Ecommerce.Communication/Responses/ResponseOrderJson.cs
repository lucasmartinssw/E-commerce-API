using System;
using System.Collections.Generic;

namespace Ecommerce.Communication.Responses;

public class ResponseOrderJson
{
    public long Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty; 
    public decimal TotalAmount { get; set; }
}