using System.Text.Json;
using System.Text;
using StockService.Data.Dtos;


namespace StockService.ItemServiceHttpClient
{
    public class OrderServiceHttpClient : IOrderServiceHttpClient
    {

        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public OrderServiceHttpClient(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        public async void SendProductToOrderService(ReadProductDto readProductDto)
        {
            var conteudoHttp = new StringContent
                    (
                      JsonSerializer.Serialize(readProductDto),
                        Encoding.UTF8,
                        "application/json"
                    );

            await _client.PostAsync(_configuration["OrderService"], conteudoHttp);
        }
    }
}
