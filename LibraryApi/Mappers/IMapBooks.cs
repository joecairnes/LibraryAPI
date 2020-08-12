using LibraryApi.Models;
using System.Threading.Tasks;

namespace LibraryApi.Mappers
{
    public interface IMapBooks
    {
        Task<GetABookResponse> AddABook(PostBookCreate bookToAdd);
        Task<GetBooksResponse> GetBooks(string genre);
    }
}