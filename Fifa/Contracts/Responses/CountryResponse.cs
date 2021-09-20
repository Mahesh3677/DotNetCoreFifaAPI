using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Fifa.Contracts.Responses
{
    public class CountryResponse
    {
        public Guid CountryId { get; set; }
        public string Name { get; set; }
        public IFormFile FlagImage { get; set; }

        public string FlagImageUrl { get; set; }
        public int Rank { get; set; }
    }
}
