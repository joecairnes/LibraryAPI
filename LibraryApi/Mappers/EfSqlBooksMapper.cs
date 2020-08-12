using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryApi.Domain;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace LibraryApi.Mappers
{
    public class EfSqlBooksMapper : IMapBooks
    {
        LibraryDataContext Context;
        IMapper Mapper;
        MapperConfiguration Config;

        public EfSqlBooksMapper(LibraryDataContext context, IMapper mapper, MapperConfiguration config)
        {
            Context = context;
            Mapper = mapper;
            Config = config;
        }

        public async Task<GetABookResponse> AddABook(PostBookCreate bookToAdd)
        {
            // PostBookCreate -> Book
            var book = Mapper.Map<Book>(bookToAdd);
            Context.Books.Add(book); // I have no Id!
            await Context.SaveChangesAsync(); // Suddenly I have an ID! 
            //  Book -> GetABookResponse
            var response = Mapper.Map<GetABookResponse>(book);

            return response;
        }

        public async Task<GetBooksResponse> GetBooks(string genre)
        {
            var books = Context.Books
                .Where(b => b.InStock);
                // Book -> GetBooksResponseItem


            if (genre != null)
            {
                books = books.Where(b => b.Genre == genre);
            }

            var booksList = await books
                .ProjectTo<GetBooksResponseItem>(Config)
                .ToListAsync();

            var response = new GetBooksResponse
            {
                Books = booksList,
                GenreFilter = genre,
                NumberOfBooks = booksList.Count
            };

            return response;
        }
    }
}
