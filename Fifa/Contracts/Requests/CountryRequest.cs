using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fifa.Contracts.Requests
{
    public class CountryRequest
    {

        public Guid CountryId { get; set; }
        public string Name { get; set; }
        public IFormFile FlagImage { get; set; }

        public int Rank { get; set; }

    }
}
