using ShawAndPartners.Domain.Entities.v1;

namespace ShawAndPartners.Application.DTOs
{
    public class UserUpdateDto
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string FavoriteSport { get; set; }

        public User ToUser()
        {
            return new User
            {
                City = this.City,
                Country = this.Country,
                FavoriteSport = this.FavoriteSport
            };
        }

    }
}
