using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Fifa.Domain
{
    public class Country
    {
      
        [Key]
        public Guid CountryId { get; set; }
        public string Name { get; set; }
        public string FlagImage { get; set; }

        public virtual ICollection<Club> Clubs { get; set; }

        [NotMapped]
        public int Rank { get; set; }
    }   
    
    public class ResponseCountry
    {
        public Guid CountryId { get; set; }
        public string Name { get; set; }
        public string FlagImage { get; set; }       
        public int Rank { get; set; }
    }
}
