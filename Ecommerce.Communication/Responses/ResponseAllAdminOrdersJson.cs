using System.Collections.Generic;

namespace Ecommerce.Communication.Responses;

public class ResponseAllAdminOrdersJson
{
    public List<ResponseAdminOrderSummaryJson> Orders { get; set; } = new List<ResponseAdminOrderSummaryJson>();
}