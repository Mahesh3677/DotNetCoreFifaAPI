using Fifa.Contracts;
using Fifa.Contracts.Requests;
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
            return Ok(await _countryService.GetCountriesAsync());
        }

        [HttpPost(ApiRoutes.Countries.Create)]
        public async Task<IActionResult> CreateAsync([FromBody] CountryRequest countryRequest)
        {                  

            string fileName = countryRequest.FlagImage.FileName;
            string path = Path.Combine(_hostingEnvironment.ContentRootPath, "Images/CountryFlags/" + countryRequest.Name);

            using (var stream = new FileStream(path , FileMode.Create))
            {
                await countryRequest.FlagImage.CopyToAsync(stream);
            }

            var country = new Country
            {
                Name = countryRequest.Name,
                Rank = countryRequest.Rank,
                FlagImage = fileName
            };

            await _countryService.CreateCountryAsync(country);

            return Ok(true);
        }
    }
}
