using Fifa.Contracts;
using Fifa.Contracts.Requests;
using Fifa.Contracts.Responses;
using Fifa.Domain;
using Fifa.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fifa.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CountriesController : Controller
    {
        private readonly ICountryService _countryService;

        private IWebHostEnvironment _hostingEnvironment;

        public CountriesController(ICountryService countryService , IWebHostEnvironment hostingEnvironment)
        {
            _countryService = countryService;

            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet(ApiRoutes.Countries.Getall)]
        //[Authorize(Roles ="Admin")]
        //[Authorize(Policy = "MustWorkFor")]
        public async Task<IActionResult> GetAllAsync()
        {
            
            List <ResponseCountry> response= await _countryService.GetCountriesAsync();
            string uploadsFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "images");           

            List<CountryResponse> countries = response.Select(x => new CountryResponse
            {
               CountryId =x.CountryId,
               FlagImageUrl = EncodeFile(Path.Combine(uploadsFolder, x.FlagImage)),
               Name = x.Name ,
               Rank = x.Rank
            }).ToList();

            return Ok(countries);
        }

        [HttpGet(ApiRoutes.Countries.Get)]        
        public async Task<IActionResult> GetAsync([FromQuery(Name = "countryId")] Guid countryId)
        {

           List<ResponseCountry> responseData = await _countryService.GetCountryAsync(countryId);

            ResponseCountry response = responseData.FirstOrDefault();
            string uploadsFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "images");

            CountryResponse countries = new CountryResponse
            {
                CountryId = response.CountryId,
                FlagImageUrl = EncodeFile(Path.Combine(uploadsFolder, response.FlagImage)),
                Name = response.Name,
                Rank = response.Rank
            };

            return Ok(countries);
        }

        [HttpDelete(ApiRoutes.Countries.Delete)]
        public async Task<IActionResult> DeleteAsync([FromQuery(Name = "countryId")] Guid countryId)
        {
            bool responseData = await _countryService.DeleteCountryAsync(countryId);         

            return Ok(responseData);
        }

        private string EncodeFile(string fileName)
        {     
            string path = fileName;
            byte[] b = System.IO.File.ReadAllBytes(path);
            return "data:image/jpeg;base64," + Convert.ToBase64String(b);
        }

        [HttpPost(ApiRoutes.Countries.Create)]
        public async Task<IActionResult> CreateAsync([FromForm] CountryRequest countryRequest)
        {                  

            string fileName = countryRequest.FlagImage.FileName;
            var countryID = Guid.NewGuid();
            string uploadsFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "images");
            string  uniqueFileName = countryID.ToString() + "_" + countryRequest.Name+ ".jpeg";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                countryRequest.FlagImage.CopyTo(fileStream);
            }

            var country = new Country
            {
                CountryId = countryID,
                Name = countryRequest.Name,
                Rank = countryRequest.Rank,
                FlagImage = uniqueFileName
            };

            await _countryService.CreateCountryAsync(country);

            return Ok(true);
        }

        [HttpPut(ApiRoutes.Countries.Update)]
        public async Task<IActionResult> UpdateAsync([FromForm] CountryRequest countryRequest)
        {
            var country = new Country
            {
                CountryId = new Guid(countryRequest.CountryId),
                Name = countryRequest.Name,
                Rank = countryRequest.Rank               
            };

            await _countryService.EditCountryAsync(country);

            return Ok(true);
        }

    }
}
