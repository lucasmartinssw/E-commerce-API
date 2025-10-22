namespace Ecommerce.Communication.Responses;

public class ResponseCartItemDetailJson
{
    public long Id { get; set; }
    public int Quantity { get; set; }

    public ResponseProductSummaryJson Product { get; set; } = null!;
}