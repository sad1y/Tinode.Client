using System;
using Pbx;

namespace Tinode.Client.Extensions
{
    public static class ServerMsgExtensions
    {
        public static string GetId(this ServerMsg msg)
        {
            switch (msg.MessageCase)
            {
                case ServerMsg.MessageOneofCase.Ctrl: return msg.Ctrl.Id;
                case ServerMsg.MessageOneofCase.Meta: return msg.Meta.Id;
                case ServerMsg.MessageOneofCase.Pres: return string.Empty;
                default:
                    throw new NotSupportedException("cannot get message id of type " + msg.MessageCase);
            }
        }

        public static int GetCode(this ServerMsg msg)
        {
            switch (msg.MessageCase)
            {
                case ServerMsg.MessageOneofCase.Ctrl: return msg.Ctrl.Code;
                case ServerMsg.MessageOneofCase.Meta: return 0;
                default:
                    throw new NotSupportedException("cannot get message Code of type " + msg.MessageCase);
            }
        }
    }
}