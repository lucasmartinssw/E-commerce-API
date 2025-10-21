using System.Collections.Generic;

namespace Ecommerce.Communication.Responses;

public class ResponseAllAddressesJson
{
    public List<ResponseAddressJson> Addresses { get; set; } = new List<ResponseAddressJson>();
}