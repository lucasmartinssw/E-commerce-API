using System.Collections.Generic;

namespace Ecommerce.Communication.Responses;

public class ResponseOrderHistoryJson
{
    public List<ResponseOrderShortJson> Orders { get; set; } = new List<ResponseOrderShortJson>();
}