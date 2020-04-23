using System;

namespace PKO_BOT.Packets.Models
{
    // Flags
    [Flags]
    enum ProcessAccessFlags : uint
    {
        All = 0x001F0FFF,
        Terminate = 0x00000001,
        CreateThread = 0x00000002,
        VMOperation = 0x00000008,
        VMRead = 0x00000010,
        VMWrite = 0x00000020,
        DupHandle = 0x00000040,
        SetInformation = 0x00000200,
        QueryInformation = 0x00000400,
        Synchronize = 0x00100000
    }

    [Flags()]
    enum AllocationType : uint
    {
        COMMIT = 0x1000,
        RESERVE = 0x2000,
        RESET = 0x80000,
        LARGE_PAGES = 0x20000000,
        PHYSICAL = 0x400000,
        TOP_DOWN = 0x100000,
        WRITE_WATCH = 0x200000
    }

    [Flags()]
    enum MemoryProtection : uint
    {
        EXECUTE = 0x10,
        EXECUTE_READ = 0x20,
        EXECUTE_READWRITE = 0x40,
        EXECUTE_WRITECOPY = 0x80,
        NOACCESS = 0x01,
        READONLY = 0x02,
        READWRITE = 0x04,
        WRITECOPY = 0x08,
        GUARD_Modifierflag = 0x100,
        NOCACHE_Modifierflag = 0x200,
        WRITECOMBINE_Modifierflag = 0x400
    }

    public class Constants
    {
        public const byte CMD_DEINIT = 9;
        public const byte CMD_INIT = 8;
        public const byte CMD_DNS_STRUCTDATA = 7;
        public const byte CMD_DNS_DATA = 6;
        public const byte CMD_NODATA = 5;
        public const byte CMD_NOFILTERSTRUCTDATA = 4;
        public const byte CMD_NOFILTERDATA = 3;
        public const byte CMD_STRUCTDATA = 2;
        public const byte CMD_DATA = 1;
        public const byte CMD_UNLOAD_DLL = 255;
        public const byte CMD_ENABLE_MONITOR = 254;
        public const byte CMD_DISABLE_MONITOR = 253;
        public const byte CMD_ENABLE_FILTER = 252;
        public const byte CMD_DISABLE_FILTER = 251;
        public const byte CMD_INJECT = 250;
        public const byte CMD_RECV = 249;
        public const byte CMD_FILTER = 248;
        public const byte CMD_FREEZE = 247;
        public const byte CMD_UNFREEZE = 246;
        public const byte CMD_QUERY = 245;

        public const byte INIT_DECRYPT = 1;

        public const byte FUNC_NULL = 0;
        public const byte FUNC_WSASEND = 1;
        public const byte FUNC_WSARECV = 2;
        public const byte FUNC_SEND = 3;
        public const byte FUNC_RECV = 4;
        public const byte FUNC_WSASENDTO = 5;
        public const byte FUNC_WSARECVFROM = 6;
        public const byte FUNC_SENDTO = 7;
        public const byte FUNC_RECVFROM = 8;
        public const byte FUNC_WSASENDDISCONNECT = 9;
        public const byte FUNC_WSARECVDISCONNECT = 10;
        public const byte FUNC_WSAACCEPT = 11;
        public const byte FUNC_ACCEPT = 12;
        public const byte FUNC_WSACONNECT = 13;
        public const byte FUNC_CONNECT = 14;
        public const byte FUNC_WSASOCKETW_IN = 15;
        public const byte FUNC_WSASOCKETW_OUT = 16;
        public const byte FUNC_BIND = 17;
        public const byte FUNC_CLOSESOCKET = 18;
        public const byte FUNC_LISTEN = 19;
        public const byte FUNC_SHUTDOWN = 20;
        public const byte CONN_WSASENDTO = 21;
        public const byte CONN_WSARECVFROM = 22;
        public const byte CONN_SENDTO = 23;
        public const byte CONN_RECVFROM = 24;
        public const byte DNS_GETHOSTBYNAME_OUT = 25;
        public const byte DNS_GETHOSTBYNAME_IN = 26;
        public const byte DNS_GETHOSTBYADDR_OUT = 27;
        public const byte DNS_GETHOSTBYADDR_IN = 28;
        public const byte DNS_WSAASYNCGETHOSTBYNAME_OUT = 29;
        public const byte DNS_WSAASYNCGETHOSTBYNAME_IN = 30;
        public const byte DNS_WSAASYNCGETHOSTBYADDR_OUT = 31;
        public const byte DNS_WSAASYNCGETHOSTBYADDR_IN = 32;
        public const byte DNS_GETHOSTNAME = 33;
        public const byte FUNC_WSACLEANUP = 34;
        public const byte FUNC_SOCKET_IN = 35;
        public const byte FUNC_SOCKET_OUT = 36;
        public const byte FUNC_GETSOCKNAME = 37;
        public const byte FUNC_GETPEERNAME = 38;

        public const byte ActionReplaceString = 0;
        public const byte ActionReplaceStringH = 1;
        public const byte ActionError = 2;
        public const byte ActionErrorH = 3;
    }
}
