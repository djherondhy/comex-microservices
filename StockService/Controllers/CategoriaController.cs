using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StockService.Data;
using StockService.Data.Dtos;
using StockService.Models;

namespace StockService.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriaController : ControllerBase
{
    private ProdutoContext _context;
    private IMapper _mapper;

    public CategoriaController(ProdutoContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

   
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult CreateCategory([FromBody] CreateCategoryDto categoryDto)
    {
        Categoria category = _mapper.Map<Categoria>(categoryDto);
        _context.Categories.Add(category);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
    }

  
    [HttpGet]
    public IEnumerable<ReadCategoryDto> GetAllCategories([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        return _context.Categories
            .OrderBy(c => c.Id)
            .Skip(skip)
            .Take(take)
            .ProjectTo<ReadCategoryDto>(_mapper.ConfigurationProvider)
            .ToList();
    }

    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReadCategoryDto))]
    public IActionResult GetCategoryById(int id)
    {
        var category = _context.Categories.FirstOrDefault(category => category.Id == id);
        if (category == null) return NotFound();
        var categoryDto = _mapper.Map<ReadCategoryDto>(category);
        return Ok(categoryDto);
    }

  
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateCategory(int id, [FromBody] UpdateCategoryDto categoryDto)
    {
        var category = _context.Categories.FirstOrDefault(categoria => categoria.Id == id);
        if (category == null) return NotFound();
        _mapper.Map(categoryDto, category);
        _context.SaveChanges();
        return NoContent();
    }

   
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteCategory(int id)
    {
        var category = _context.Categories.FirstOrDefault(category => category.Id == id);
        if (category == null) return NotFound();

        try
        {
            _context.Remove(category);
            _context.SaveChanges();
            return NoContent();
        }
        catch (DbUpdateException ex)
        {

            if (ex.InnerException != null && ex.InnerException.Message.Contains("FK_"))
            {
                return BadRequest("Não é possível deletar a categoria porque ela está em uso.");
            }
            throw;
        }
    }

}
