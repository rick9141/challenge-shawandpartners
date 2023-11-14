using CsvHelper.Configuration.Attributes;

namespace ShawAndPartners.Domain.Entities.v1
{
    public class User
    {
        [Name("name")]
        public string Name { get; set; }

        [Name("city")]
        public string City { get; set; }

        [Name("country")]
        public string Country { get; set; }

        [Name("favorite_sport")]
        public string FavoriteSport { get; set; }
    }
}
