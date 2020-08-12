using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public interface ILookupDevelopers
    {
        Task<string> GetCurrentOnCallDeveloper();
    }
}