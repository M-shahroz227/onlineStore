using Microsoft.IdentityModel.Tokens;
using onlineStore.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace onlineStore.Service.JwtService
{
    public class JwtService : IJwtService
    {
        public string GenerateToken(Register register)
        {
            // key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsASecretKeyForJwtTokenGeneration"));
            // credentials
            var Credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, register.Id.ToString()),
                new Claim(ClaimTypes.Name, register.Username),
                new Claim(ClaimTypes.Email, register.Email),
                new Claim(ClaimTypes.Role, register.Role)

            };
            // token
            var token = new JwtSecurityToken(
                issuer: "onlineStore",
                audience: "onlineStoreUsers",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: Credentials
                );
            // token create
            return new JwtSecurityTokenHandler().WriteToken(token);


        }

        
    }
}
