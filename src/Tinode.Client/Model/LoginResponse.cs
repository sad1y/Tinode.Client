namespace Tinode.Client
{
    public class LoginResponse
    {
        public string User { get; }
        public string AuthLevel { get; }
        public string Token { get; }
        public string Expires { get; }

        public LoginResponse(string authLevel, string user, string token, string expires)
        {
            AuthLevel = authLevel;
            User = user;
            Token = token;
            Expires = expires;
        }
    }
}