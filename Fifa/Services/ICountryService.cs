using Fifa.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifa.Services
{
    public interface ICountryService
    {
        Task<List<ResponseCountry>> GetCountriesAsync();

        Task<Country> GetCountryAsync(Guid countryId);

        Task<bool> CreateCountryAsync(Country country);

        Task<bool> EditCountryAsync(Country country);

        Task<bool> DeleteCountryAsync(Guid countryId);
    }
}
