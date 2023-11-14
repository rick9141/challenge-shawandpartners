using CsvHelper;
using ShawAndPartners.Application.Contracts.Services.v1;
using ShawAndPartners.Application.DTOs;
using ShawAndPartners.Domain.Contracts.Repositories;
using ShawAndPartners.Domain.Entities.v1;
using System.Globalization;
using System.Text;

namespace ShawAndPartners.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task UploadCsvAsync(Stream csvStream)
        {
            using var reader = new StreamReader(csvStream, Encoding.UTF8);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var users = csv.GetRecords<User>().ToList();

            foreach (var user in users)
            {
                var existingUser = await _userRepository.GetByNameAsync(user.Name);
                if (existingUser != null)
                {
                    await _userRepository.UpdateAsync(user);
                }
                else
                {
                    await _userRepository.InsertAsync(user);
                }
            }
        }

        public async Task<IEnumerable<User>> SearchAsync(string searchTerm)
        {
            return await _userRepository.SearchAsync(searchTerm);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }
        public async Task<bool> UpdateUserAsync(string name, UserUpdateDto userDto)
        {
            var userToUpdate = await _userRepository.GetByNameAsync(name);
            if (userToUpdate == null)
            {
                return false;
            }

            userToUpdate.City = userDto.City;
            userToUpdate.Country = userDto.Country;
            userToUpdate.FavoriteSport = userDto.FavoriteSport;

            var updatedRows = await _userRepository.UpdateAsync(userToUpdate);

            return updatedRows > 0;
        }

        public async Task<int> DeleteByNameAsync(string name)
        {
            return await _userRepository.DeleteByNameAsync(name);
        }
    }
}
