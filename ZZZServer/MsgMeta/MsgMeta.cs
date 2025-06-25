﻿public partial class MsgMeta : IMsgMeta
{
    public readonly Dictionary<uint, MsgMetaData> cmdIdToData;
    public readonly Dictionary<Type, MsgMetaData> typeToData;

    public bool TryGetCmdId(Type type, out uint cmdId)
    {
        if (typeToData.TryGetValue(type, out var metaData))
        {
            cmdId = metaData.cmdId;
            return true;
        }
        cmdId = 0;
        return false;
    }

    public Google.Protobuf.MessageParser GetParser(uint cmdId)
    {
        if (cmdIdToData.TryGetValue(cmdId, out var metaData))
        {
            return metaData.parser;
        }
        return null;
    }

    public bool IsRsp(uint cmdId)
    {
        if (cmdIdToData.TryGetValue(cmdId, out var metaData))
        {
            return metaData.isRsp;
        }
        return false;
    }
}