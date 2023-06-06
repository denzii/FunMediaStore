using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using FunMediaStore.Model;
using FunMediaStore.Interface;

namespace FunMediaStore.Service
{
    public class Auth: IAuth
    {
        private readonly IConfiguration _config;
        public Auth(IConfiguration config)
        {
            _config = config;
        }
        
        // This should be private and be called in the isAuthentic function
        // however, is made public so the dummy seed data provider can use it.
        public (byte[] passwordSalt, byte[] passwordHash) CreatePasswordHash(string password, byte[]? salt = null)
        {
            if (salt == null)
            {
                using RandomNumberGenerator rng = RandomNumberGenerator.Create();
                rng.GetBytes(salt ??= new byte[32]);
            }

            using var hmac = new HMACSHA512(salt);
            return (passwordSalt: salt, passwordHash: hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Role, user.Role),
            };

            if (user.Email != null) claims.Add(new Claim(ClaimTypes.Email, user.Email));
            if (user.Username != null) claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Username));
            if (user.Name != null) claims.Add(new Claim(ClaimTypes.GivenName, user.Name));
            if (user.Surname != null) claims.Add(new Claim(ClaimTypes.Surname, user.Surname));

            var token = new JwtSecurityToken(
                _config["JwtSettings:Issuer"],
                _config["JwtSettings:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        public User GetClaimedUser(IEnumerable<Claim> claims)
        {
            return new User
            {
                Email = claims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                Username = claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                Name = claims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                Surname = claims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value
            };
        }

        public bool isAuthentic(User user, byte[] passwordHash)
        {
            if (user.PasswordHash == null) return false;

            return Enumerable.SequenceEqual(user.PasswordHash, passwordHash);
        }
    }
}
