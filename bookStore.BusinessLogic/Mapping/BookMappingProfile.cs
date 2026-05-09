using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using bookStore.Domain.Entities;
using bookStore.Domain.Models.Book;

namespace bookStore.BusinessLogic.Mapping
{
    public class BookMappingProfile : Profile
    {
        public BookMappingProfile()
        {
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.Category,
                    opt => opt.MapFrom(src =>
                        string.Join(", ",
                            src.BookCategories
                                .OrderBy(bc => bc.CategoryId)
                                .Select(bc => bc.Category.Name))))
                .ForMember(dest => dest.CoverImageUrl,
                    opt => opt.MapFrom(src =>
                        src.Images.Where(i => i.IsActive).Select(i => i.Url).FirstOrDefault()
                        ?? src.Images.Select(i => i.Url).FirstOrDefault()));

            CreateMap<BookDto, Book>();
        }
    }
}
