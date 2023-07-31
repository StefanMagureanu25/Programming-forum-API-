using MagureanuStefan_API.DataContext;
using MagureanuStefan_API.Models.Authentication;

namespace MagureanuStefan_API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<AuthenticationResponse> Authenticate(AuthenticateRequest request);

    }
}
