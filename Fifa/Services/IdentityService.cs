using Fifa.Domain;
using Fifa.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fifa.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var existingUSer = await _userManager.FindByEmailAsync(email);

            if (existingUSer != null)
            {
                return new AuthenticationResult { Errors = new[] { "Email already exists" } };
            }

            var newUSer = new IdentityUser
            {
                Email = email,
                UserName = email
            };

            var createdUSer = await _userManager.CreateAsync(newUSer, password);
            if (!createdUSer.Succeeded)
            {
                return new AuthenticationResult { Errors = createdUSer.Errors.Select(x => x.Description) };
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, newUSer.Email),
                       new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                          new Claim(JwtRegisteredClaimNames.Email, newUSer.Email),
                          new Claim("id",newUSer.Id)
                }),
                Expires =DateTime.Now.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult 
            { 
               Token =tokenHandler.WriteToken(token),
               Success = true
            };
        }
    }
}
