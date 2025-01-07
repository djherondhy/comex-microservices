using AutoMapper;
using StockService.Data.Dtos;
using StockService.Models;

namespace StockService.Profiles;
public class ProdutoProfile : Profile
{
    public ProdutoProfile()
    {
        CreateMap<CreateProductDto, Produto>();
        CreateMap<UpdateProductDto, Produto>();
        CreateMap<Produto, UpdateProductDto>();
        CreateMap<Produto, ReadProductDto>();
        CreateMap<Produto, ProductResponseDto>();
    }
}