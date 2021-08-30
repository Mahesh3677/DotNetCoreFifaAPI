using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fifa.Domain
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }

        [Required]        
        public string Name { get; set; }

        public string UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public IdentityUser User { get; set; }
    }
}
