using Fifa.Data;
using Fifa.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifa.Services
{
    public class CountryService : ICountryService
    {
        private readonly DataContext _dataContext;

        public CountryService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreateCountryAsync(Country country)
        {
            var countryId = new SqlParameter("@CountryId", country.CountryId);
            var name = new SqlParameter("@Name", country.Name);
            var flagImage = new SqlParameter("@FlagImage", country.FlagImage);
            var rank = new SqlParameter("@Rank", country.Rank);
            var data = await _dataContext.Database.ExecuteSqlRawAsync("CountryInsert @CountryId, @Name , @FlagImage , @Rank",
                countryId, name, flagImage, rank);
            return data > 0;
        }

        public async Task<bool> DeleteCountryAsync(Guid countryId)
        {
            var id = new SqlParameter("@CountryId", countryId);

            var data = await _dataContext.Database.ExecuteSqlRawAsync("DeleteCountry @CountryId", id);
            return data > 0;
        }

        public async Task<bool> EditCountryAsync(Country country)
        {
            var countryId = new SqlParameter("@CountryId", country.CountryId);
            var name = new SqlParameter("@Name", country.Name);
            var rank = new SqlParameter("@Rank", country.Rank);
            var data = await _dataContext.Database.ExecuteSqlRawAsync("CountryUpdate @CountryId, @Name  , @Rank",
                countryId, name, rank);
            return data > 0;
        }

        public async Task<List<ResponseCountry>> GetCountriesAsync()
        {
            //var countries= await _dataContext.Database.sqlq ("dbo.SelectCountries").ToListAsync();
            return await _dataContext.ResponseCountry.FromSqlRaw("dbo.SelectCountries").ToListAsync();
        }

        public async Task<List<ResponseCountry>> GetCountryAsync(Guid countryId)
        {
            var Id = new SqlParameter("@CountryId", countryId);
            return await _dataContext.ResponseCountry.FromSqlRaw("dbo.SelectCountry @CountryId", Id).ToListAsync();
        }
    }
}
