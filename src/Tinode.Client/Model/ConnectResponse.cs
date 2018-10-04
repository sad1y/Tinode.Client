namespace Tinode.Client
{
    public class ConnectResponse
    {
        public string Version { get; }
        public string Build { get; }

        public ConnectResponse(string version, string build)
        {
            Version = version;
            Build = build;
        }
    }
}