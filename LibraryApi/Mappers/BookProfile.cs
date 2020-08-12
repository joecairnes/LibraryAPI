using AutoMapper;
using LibraryApi.Domain;
using LibraryApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Mappers
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            // PostBookCreate -> Book
            CreateMap<PostBookCreate, Book>()
                .ForMember(dest => dest.InStock, opt => opt.MapFrom((s) => true));

            // Book -< GetABookResponse
            CreateMap<Book, GetABookResponse>();

            // Book -> GetBooksResponseItem
            CreateMap<Book, GetBooksResponseItem>();
        }
    }
}
