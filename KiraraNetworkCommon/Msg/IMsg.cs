using Google.Protobuf;

namespace Kirara.Network
{
    public interface IMsg : IMessage
    {
        uint CmdId { get; }
    }
}