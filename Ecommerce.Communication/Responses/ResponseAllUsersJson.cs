using System.Collections.Generic;

namespace Ecommerce.Communication.Responses;

public class ResponseAllUsersJson
{
    public List<ResponseUserJson> Users { get; set; } = new List<ResponseUserJson>();
}