using AutoMapper;
using bookStore.Domain.Entities;
using bookStore.Domain.Models.Book;

namespace bookStore.Api.Mappings
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles() {
            CreateMap<BookDto, Book>();

            CreateMap<BookDto, Book>().ReverseMap();
        }
        
    }
}
