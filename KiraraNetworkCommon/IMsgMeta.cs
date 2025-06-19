public interface IMsgMeta
{
    Google.Protobuf.MessageParser GetParser(uint cmdId);
}

// type => id
// id => parser