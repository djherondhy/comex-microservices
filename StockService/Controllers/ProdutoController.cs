using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using AutoMapper.QueryableExtensions;
using StockService.Data;
using StockService.Data.Dtos;
using StockService.Models;
using StockService.ItemServiceHttpClient;
// using StockService.RabbitMqClient;

namespace StockService.Controllers;

[ApiController]
[Route("[controller]")]
public class ProdutoController : ControllerBase
{
    private ProdutoContext _context;
    private IMapper _mapper;

    private IOrderServiceHttpClient _orderServiceHttpClient;
    // private IRabbitMqClient _rabbitMqClient;

    public ProdutoController(ProdutoContext context, IMapper mapper, IOrderServiceHttpClient orderServiceHttpClient)
    {
        _context = context;
        _mapper = mapper;
        _orderServiceHttpClient = orderServiceHttpClient;
        // _rabbitMqClient = rabbitMqClient;
    }

   

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AddProduct([FromBody] CreateProductDto productDto)
    {
        var category = _context.Categories.FirstOrDefault(c => c.Id == productDto.CategoryId);
        if (category == null)
        {
            return NotFound("Categoria não encontrada.");
        }

        Produto product = _mapper.Map<Produto>(productDto);
        _context.Products.Add(product);
        _context.SaveChanges();

        var productResponseDto = _mapper.Map<ProductResponseDto>(product);
        var readProductDto = _mapper.Map<ReadProductDto>(product);

        _orderServiceHttpClient.SendProductToOrderService(readProductDto);

        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, productResponseDto);
    }

   

    [HttpGet]
    public IEnumerable<ReadProductDto> GetAllProducts([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        return _context.Products
            .OrderBy(p => p.Id)
            .Skip(skip)
            .Take(take)
            .ProjectTo<ReadProductDto>(_mapper.ConfigurationProvider)
            .ToList();
    }


 
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReadProductDto))]
    public IActionResult GetProductById(int id)
    {
        var product = _context.Products.FirstOrDefault(product => product.Id == id);
        if (product == null) return NotFound();
        var productDto = _mapper.Map<ReadProductDto>(product);
        return Ok(productDto);
    }

   
   
    public IActionResult UpdateProduct(int id, [FromBody] UpdateProductDto productDto)
    {
        var product = _context.Products.FirstOrDefault(product => product.Id == id);
        if (product == null) return NotFound();
        _mapper.Map(productDto, product);
        _context.SaveChanges();
        return NoContent();
    }

  
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteProduct(int id)
    {
        var product = _context.Products.FirstOrDefault(product => product.Id == id);
        if (product == null) return NotFound();
        _context.Remove(product);
        _context.SaveChanges();
        return NoContent();
    }

}
