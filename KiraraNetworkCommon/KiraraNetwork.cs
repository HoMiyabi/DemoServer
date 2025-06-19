using System.Reflection;

namespace Kirara.Network
{
    public static class KiraraNetwork
    {
        public static IMsgMeta MsgMeta { get; private set; }
        public static int SessionTimeoutMs { get; private set; } = 8000;

        public static void Init(IMsgMeta msgMeta, Assembly assembly)
        {
            MsgMeta = msgMeta;
            NetMessageWorker.Scan(assembly);
        }
    }
}