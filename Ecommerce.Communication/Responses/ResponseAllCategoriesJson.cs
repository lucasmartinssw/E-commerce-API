using System.Collections.Generic;

namespace Ecommerce.Communication.Responses;

public class ResponseAllCategoriesJson
{
    public List<ResponseCategoryJson> Categories { get; set; } = new List<ResponseCategoryJson>();
}