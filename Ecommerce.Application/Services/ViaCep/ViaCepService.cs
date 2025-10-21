using Ecommerce.Communication.Responses;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.ViaCep;

public class ViaCepService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ViaCepService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ResponseViaCepJson?> GetAddressByCepAsync(string cep)
    {
        var cleanCep = cep.Replace("-", "").Replace(".", "");

        var httpClient = _httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri("https://viacep.com.br/");

        try
        {
            var response = await httpClient.GetFromJsonAsync<ResponseViaCepJson>($"ws/{cleanCep}/json/");
            if (response != null && !response.Error)
            {
                return response;
            }
            return null; 
        }
        catch (HttpRequestException)
        {
            return null; 
        }
    }
}