using System.Text.Json.Serialization;

namespace Ecommerce.Communication.Responses;
public class ResponseViaCepJson
{
    [JsonPropertyName("cep")]
    public string Cep { get; set; } = string.Empty;

    [JsonPropertyName("logradouro")]
    public string Street { get; set; } = string.Empty;

    [JsonPropertyName("complemento")]
    public string? ApiComplement { get; set; }

    [JsonPropertyName("bairro")]
    public string Neighborhood { get; set; } = string.Empty;

    [JsonPropertyName("localidade")]
    public string City { get; set; } = string.Empty;

    [JsonPropertyName("uf")]
    public string State { get; set; } = string.Empty;

    [JsonPropertyName("erro")]
    public bool Error { get; set; } = false;
}