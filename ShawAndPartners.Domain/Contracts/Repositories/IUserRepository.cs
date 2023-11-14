using ShawAndPartners.Domain.Entities.v1;

namespace ShawAndPartners.Domain.Contracts.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByNameAsync(string id);
        Task<int> InsertAsync(User user);
        Task<int> UpdateAsync(User user);
        Task<int> DeleteByNameAsync(string name);
        Task<IEnumerable<User>> SearchAsync(string searchTerm);
    }
}
