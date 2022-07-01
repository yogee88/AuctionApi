using Identity.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
        }

        public string Generate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.EmailId),
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Aud, _config["Token:Audience"]),
                new Claim(JwtRegisteredClaimNames.Iss, _config["Token:Issuer"])
            };

            var sign = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = sign,
                Issuer = _config["Token:Issuer"]
            };

            var handler = new JwtSecurityTokenHandler();

            var token = handler.CreateToken(tokenDesc);

            return handler.WriteToken(token);
        }
    }
}
