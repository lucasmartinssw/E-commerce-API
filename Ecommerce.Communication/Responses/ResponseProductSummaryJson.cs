namespace Ecommerce.Communication.Responses;

public class ResponseProductSummaryJson
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? ImageFilename { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}