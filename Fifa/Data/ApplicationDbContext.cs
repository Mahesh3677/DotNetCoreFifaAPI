using Fifa.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fifa.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Country> Country { get; set; }

        [NotMapped]
        public DbSet<ResponseCountry>  ResponseCountry { get; set; }

        public DbSet<Club> Club { get; set; }

        public DbSet<CountryClubRankRelation> CountryClubRankRelation { get; set; }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //    builder.Entity<Country>()
        //         .Ignore(c => c.Rank);
        //    builder.Entity<Club>()
        //       .Ignore(c => c.Rank);
        //}
    }
}
