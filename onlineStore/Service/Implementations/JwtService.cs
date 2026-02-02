using Microsoft.IdentityModel.Tokens;
using onlineStore.Model;
using onlineStore.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace onlineStore.Service.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
{
           new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
};


            // 🔥 Add user features as claims
            if (user.UserFeatures != null)
            {
                foreach (var uf in user.UserFeatures)
                {
                    if (uf.Feature != null && !string.IsNullOrEmpty(uf.Feature.Code))
                        claims.Add(new Claim("feature", uf.Feature.Code));
                }
            }

            // 🔑 Key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JwtSettings:Secret"]));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
