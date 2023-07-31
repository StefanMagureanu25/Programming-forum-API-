namespace MagureanuStefan_API.Models.Authentication
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }

        public AuthenticationResponse(string token)
        {
            Token = token;
        }
    }
}
