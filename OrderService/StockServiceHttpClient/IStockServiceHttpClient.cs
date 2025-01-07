using System.Text.Json;
using System.Text;
using OrderService.ItemServiceHttpClient;
using OrderService.Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using OrderService.Data.Dtos.Product;

namespace StockService.ItemServiceHttpClient
{
    public class StockServiceHttpClient : IStockServiceHttpClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public StockServiceHttpClient(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        public async Task<List<ReadProductDto>> GetAllProducts(int skip = 0, int take = 50)
        {

            var url = $"{_configuration["StockService"]}/Product?skip={skip}&take={take}";


            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var products = JsonSerializer.Deserialize<List<ReadProductDto>>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return products;
            }


            return new List<ReadProductDto>();
        }

        public async Task<ReadProductDto> GetProductById(int id)
        {
            var url = $"{_configuration["StockService"]}/Product/{id}";

            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var product = JsonSerializer.Deserialize<ReadProductDto>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return product;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception($"Produto com ID {id} não encontrado.");
            }


            throw new Exception($"Falha ao buscar o produto com ID {id}: {response}");
        }
    }
}
