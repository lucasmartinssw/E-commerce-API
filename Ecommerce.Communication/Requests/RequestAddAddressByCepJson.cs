namespace Ecommerce.Communication.Requests;

public class RequestAddAddressByCepJson
{
    public string ZipCode { get; set; } = string.Empty;
    public string? Number { get; set; }
    public string? Complement { get; set; }
}