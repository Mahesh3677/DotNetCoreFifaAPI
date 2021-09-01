using Fifa.Data;
using Fifa.Domain;
using Fifa.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly DataContext _context;

        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings
            , TokenValidationParameters tokenValidationParameters,DataContext context )
        {

            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _context = context;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
         
            var user = await _userManager.FindByEmailAsync(email);

         

            if (user == null)
            {
                return new AuthenticationResult { Errors = new[] { "Email does not exists" } };
            }


            var validPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!validPassword)
            {
                return new AuthenticationResult { Errors = new[] { "email/passwprd wrong" } };
            }
            return await GenerateAuthenticationResultforUserAsync(user);
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

            await _userManager.AddClaimAsync(newUSer, new Claim("post.delete", "true"));
            if (!createdUSer.Succeeded)
            {
                return new AuthenticationResult { Errors = createdUSer.Errors.Select(x => x.Description) };
            }
            return await GenerateAuthenticationResultforUserAsync(newUSer);
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultforUserAsync(IdentityUser newUSer)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var roles = await _userManager.GetRolesAsync(newUSer);
           
            var claims = new List<Claim> {
                    new Claim(JwtRegisteredClaimNames.Sub, newUSer.Email),
                       new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                          new Claim(JwtRegisteredClaimNames.Email, newUSer.Email),
                          new Claim("id",newUSer.Id)

                };

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var userClaims = await _userManager.GetClaimsAsync(newUSer);
           
            claims.AddRange(userClaims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.Add(_jwtSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserID = newUSer.Id,
                CreationDate =DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await _context.RefreshTokens.AddAsync(refreshToken);

            await _context.SaveChangesAsync();



            return new AuthenticationResult
            {
                Token = tokenHandler.WriteToken(token),
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }

       

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!IsJwtwithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                else
                {
                    return principal;
                }

            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtwithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if(validatedToken == null)
            {
                return new AuthenticationResult { Errors = new[] { "invalid token" } };
            }
            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUTC = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if(expiryDateTimeUTC > DateTime.UtcNow)
            {
                return new AuthenticationResult { Errors = new[] { "token did not expire" } };
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);
            if(storedRefreshToken == null)
            {
                return new AuthenticationResult { Errors = new[] { "this refresh token does not exist" } };
            }

            if (storedRefreshToken.ExpiryDate < DateTime.UtcNow)
            {
                return new AuthenticationResult { Errors = new[] { "this  refresh token expired" } };
            }
            if (storedRefreshToken.Invalidated)
            {
                return new AuthenticationResult { Errors = new[] { "this refresh token invalidated" } };
            }
            if (storedRefreshToken.Used)
            {
                return new AuthenticationResult { Errors = new[] { "this refresh token used" } };
            }
            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthenticationResult { Errors = new[] { "this refresh token does not match this jwt" } };
            }
            storedRefreshToken.Used = true;

            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);

            return await GenerateAuthenticationResultforUserAsync(user);

        }
    }
}
