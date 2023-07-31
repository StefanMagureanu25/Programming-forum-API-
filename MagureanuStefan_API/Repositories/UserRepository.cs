using MagureanuStefan_API.DataContext;
using MagureanuStefan_API.Models.Authentication;
using MagureanuStefan_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MagureanuStefan_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ClubLibraDataContext _context;
        private readonly IConfiguration _configuration;
        public UserRepository(ClubLibraDataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<AuthenticationResponse> Authenticate(AuthenticateRequest request)
        {
            var user = await _context.Members.SingleOrDefaultAsync(x => x.Username == request.Username && x.Password == request.Password);
            if (user == null)
            {
                return null;
            }
            var token = await GenerateJwtToken(request);

            return new AuthenticationResponse(token);
        }
        private async Task<string> GenerateJwtToken(AuthenticateRequest request)
        {
            var securityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                (_configuration.GetValue<string>("Authentication:Secret")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration.GetValue<string>("Authentication:Domain"),
                _configuration.GetValue<string>("Authentication:Audience"),
                null,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
