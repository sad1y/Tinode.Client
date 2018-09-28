using Pbx;

namespace Tinode.Client.Exceptions
{
    public class TinodeSeverExcpetion : TinodeExcpetion
    {
        private readonly ServerMsg _msg;

        public int Code => _msg.Ctrl.Code;
        public string Text => _msg.Ctrl.Text;

        public string What => _msg.Ctrl.Params.TryGetValue("what", out var value) ? value.ToStringUtf8() : null;

        public TinodeSeverExcpetion(ServerMsg msg) => _msg = msg;
    }
}