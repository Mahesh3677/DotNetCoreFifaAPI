using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fifa.Domain
{
    public class CountryClubRankRelation
    {
        [Key]
        [Required]
        public Guid RankId { get; set; }

        public Guid? CountryID { get; set; }

        public Guid? ClubID { get; set; }

        [Required]
        public int Rank { get; set; }

        [ForeignKey(nameof(CountryID))]
        public virtual Country Country { get; set; }

        [ForeignKey(nameof(ClubID))]
        public virtual Club Club { get; set; }
    }
}
