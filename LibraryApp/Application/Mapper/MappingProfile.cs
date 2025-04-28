using LibraryApp.Models;
using AutoMapper;
using LibraryApp.Application.DTO.Author;
using LibraryApp.Application.DTO.Book;

namespace LibraryApp.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Book, BookDto>()
               .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.FirstName + ' ' + src.Author.LastName))
               .ReverseMap()
               .ForMember(dest => dest.Author, opt => opt.Ignore());

            CreateMap<Author, AuthorDto>().ReverseMap();

            CreateMap<CreateUpdateBookDto, Book>()
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId)) 
        .ForMember(dest => dest.CoverImage, opt => opt.Ignore()); 
      
        }
       
    }
}
