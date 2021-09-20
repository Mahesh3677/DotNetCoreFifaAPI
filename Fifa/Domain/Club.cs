using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fifa.Domain
{
    public class Club
    {
        [Key]
        public Guid ClubId { get; set; }
        public int Name { get; set; }
        public string LogoImage { get; set; }
        public Guid CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }

    }
}
