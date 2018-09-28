using Pbx;

namespace Tinode.Client.Exceptions
{
    public class TinodeUnauthorizedExcpetion : TinodeSeverExcpetion
    {
        public TinodeUnauthorizedExcpetion(ServerMsg msg) : base(msg)
        {
        }
    }
}