using ShawAndPartners.Application.DTOs;
using ShawAndPartners.Domain.Entities.v1;

namespace ShawAndPartners.Application.Contracts.Services.v1
{
    public interface IUserService
    {
        Task UploadCsvAsync(Stream csvStream);
        Task<IEnumerable<User>> SearchAsync(string searchTerm);
        Task<IEnumerable<User>> GetAllAsync();
        Task<bool> UpdateUserAsync(string name, UserUpdateDto userDto);
        Task<int> DeleteByNameAsync(string name);
    }
}
