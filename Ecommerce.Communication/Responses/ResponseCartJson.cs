using System.Collections.Generic;

namespace Ecommerce.Communication.Responses;

public class ResponseCartJson
{
    public long Id { get; set; } 
    public List<ResponseCartItemDetailJson> Items { get; set; } = new List<ResponseCartItemDetailJson>();
    public decimal TotalAmount { get; set; } 
}