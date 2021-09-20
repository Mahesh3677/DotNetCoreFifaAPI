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

        
        public DbSet<ResponseCountry>  ResponseCountry { get; set; }

        public DbSet<Club> Club { get; set; }

        public DbSet<CountryClubRankRelation> CountryClubRankRelation { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
          
            builder.Entity<ResponseCountry>().HasNoKey().ToView(null);
        }


    }
}
