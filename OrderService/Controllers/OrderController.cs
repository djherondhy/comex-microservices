using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService.Data.Dtos.Product;
using OrderService.Data.Dtos.RabbitMq;
using OrderService.DTOs.Order;
using OrderService.Enums;
using OrderService.ItemServiceHttpClient;
using OrderService.Models;
using OrderService.RabbitMqClient;
using OrderService.Repository;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IStockServiceHttpClient _stockServiceHttpClient;
        private readonly IMapper _mapper;
        private IRabbitMqClient _rabbitMqClient;

        public OrderController(IOrderRepository orderRepository, IStockServiceHttpClient stockServiceHttpClient, IMapper mapper, IRabbitMqClient rabbitMqClient)
        {
            _orderRepository = orderRepository;
            _stockServiceHttpClient = stockServiceHttpClient;
            _mapper = mapper;
            _rabbitMqClient = rabbitMqClient;
        }

        [HttpPost]
        public async Task<ActionResult<OrderReadDTO>> CreateOrder(OrderCreateDTO orderCreateDto)
        {

            if (string.IsNullOrWhiteSpace(orderCreateDto.Status.ToString()))
            {
                return BadRequest("Status não pode ser vazio.");
            }

            if (!Enum.TryParse<OrderStatus>(orderCreateDto.Status.ToString(), true, out var statusEnum))
            {
                return BadRequest("Status inválido.");
            }

            Order order = new Order
            {
                CreationDate = orderCreateDto.CreationDate,
                Status = statusEnum,
                OrderItems = new List<OrderItem>()
            };


            foreach (var itemDto in orderCreateDto.OrderItems)
            {
                ReadProductDto product = await _stockServiceHttpClient.GetProductById(itemDto.ProductId);

                if (product == null)
                {
                    return NotFound($"Produto com ID {itemDto.ProductId} não encontrado.");
                }

                if (product.AvailableQuantity < itemDto.Amount)
                {
                    return BadRequest($"Estoque insuficiente para o produto {product.Name}.");
                }

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Amount = itemDto.Amount,
                    UnitPrice = product.Price,
                    NameProduct = product.Name,
                    ImageProduct = product.Image
                });
            }

            order.Total = order.OrderItems.Sum(i => i.Total);

            await _orderRepository.CreateOrder(order);
            await _orderRepository.SaveChangesAsync();

            var orderReadDto = _mapper.Map<OrderReadDTO>(order);

            return CreatedAtAction(nameof(GetOrderById), new { id = orderReadDto.Id }, orderReadDto);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<OrderReadDTO>> GetOrderById(int id)
        {
            Order order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            var orderReadDto = _mapper.Map<OrderReadDTO>(order);
            return Ok(orderReadDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderReadDTO>>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();

            if (orders == null || !orders.Any())
            {
                return NotFound("Nenhum pedido encontrado.");
            }

            var orderReadDtos = _mapper.Map<IEnumerable<OrderReadDTO>>(orders);

            return Ok(orderReadDtos);
        }


        [HttpPut("/updateStatusOrderById")]
        public async Task<ActionResult<OrderReadDTO>> UpdateStatusOrderById(UpdateStatusOrderByIdDto updateStatusOrderByIdDto)
        {
            if (string.IsNullOrWhiteSpace(updateStatusOrderByIdDto.Status.ToString()))
            {
                return BadRequest("Status não pode ser vazio.");
            }

            if (!Enum.TryParse<OrderStatus>(updateStatusOrderByIdDto.Status.ToString(), true, out var statusEnum))
            {
                return BadRequest("Status inválido.");
            }

            var order = await _orderRepository.UpdateStatusOrderById(updateStatusOrderByIdDto.OrderId, statusEnum);
            var orderReadDto = _mapper.Map<OrderReadDTO>(order);

            if (updateStatusOrderByIdDto.Status == OrderStatus.Concluido)
            {
                await _orderRepository.SaveChangesAsync();

                var dataSendUpdateProduct = new List<UpdateProductQuantityInStockDto>();

                foreach (var itemDto in orderReadDto.OrderItems)
                {
                    dataSendUpdateProduct.Add(
                        new UpdateProductQuantityInStockDto
                        {
                            Amount = itemDto.Amount,
                            ProductId = itemDto.ProductId,
                            OrderId = order.Id
                        }
                    );
                }

                _rabbitMqClient.UpdateProductQuantityInStock(dataSendUpdateProduct);

                return Ok(orderReadDto);
            }
            else
            {
                await _orderRepository.SaveChangesAsync();
                return Ok(orderReadDto);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            await _orderRepository.DeleteOrderById(order);
            await _orderRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
