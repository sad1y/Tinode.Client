namespace Tinode.Client.Response
{
    public class CreateAccountResponse
    {
        public string Desc { get; }
        public string User { get; }
        public string Token { get; }
        public string Expires { get; }

        public CreateAccountResponse(string desc, string user, string token, string expires)
        {
            Expires = expires;
            Desc = desc;
            User = user;
            Token = token;
        }
    }
}