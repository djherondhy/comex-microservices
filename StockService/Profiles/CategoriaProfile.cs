using AutoMapper;
using StockService.Data.Dtos;
using StockService.Models;

namespace StockService.Profiles;
public class CategoriaProfile : Profile
{
    public CategoriaProfile()
    {
        CreateMap<CreateCategoryDto, Categoria>();
        CreateMap<UpdateCategoryDto, Categoria>();
        CreateMap<Categoria, UpdateCategoryDto>();
        CreateMap<Categoria, ReadCategoryDto>();
    }
}