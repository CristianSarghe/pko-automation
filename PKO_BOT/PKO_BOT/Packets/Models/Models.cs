using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PKO_BOT.Packets.Models
{
    public class RecordedPacket
    {
        public PipeHeader Header { get; set; }
        public byte[] Data { get; set; }

        public string Socket
        {
            get
            {
                return Header.sockid.ToString("X4");
            }
        }

        public byte[] GetNextPosX()
        {
            if(this.Data[3] == 0x33)    // Attack packet
            {
                return new byte[] { this.Data[35], this.Data[36], this.Data[37] };
            }

            if (this.Data[3] == 0x25)
            {
                return new byte[] { this.Data[33], this.Data[34], this.Data[35] };
            }

            return null;
        }

        public byte[] GetNextPosY()
        {
            if (this.Data[3] == 0x33)    // Attack packet
            {
                return new byte[] { this.Data[31], this.Data[32], this.Data[33] };
            }

            if(this.Data[3] == 0x25)
            {
                return new byte[] { this.Data[29], this.Data[30], this.Data[31] };
            }

            return null;
        }

        public void SetNextPosX(byte[] bytes)
        {
            if (bytes == null || bytes.Length != 3) return;

            if (this.Data[3] == 0x33)    // Attack packet
            {
                this.Data[35] = bytes[0];
                this.Data[36] = bytes[1];
                this.Data[37] = bytes[2];
            }

            if (this.Data[3] == 0x25)
            {
                this.Data[33] = bytes[0];
                this.Data[34] = bytes[1];
                this.Data[35] = bytes[2];
            }
        }

        public void SetNextPosY(byte[] bytes)
        {
            if (bytes == null || bytes.Length != 3) return;

            if (this.Data[3] == 0x33)    // Attack packet
            {
                this.Data[31] = bytes[0];
                this.Data[32] = bytes[1];
                this.Data[33] = bytes[2];
            }

            if (this.Data[3] == 0x25)
            {
                this.Data[29] = bytes[0];
                this.Data[30] = bytes[1];
                this.Data[31] = bytes[2];
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct PipeHeader
    {
        [MarshalAs(UnmanagedType.I1)]
        public byte command;
        public byte function;
        [MarshalAs(UnmanagedType.I4)]
        public int sockid;
        public int datasize;
        public int extra;
    }

    //Classes
    public class SockInfo
    {
        public string[] afamily = new string[] { "UNSPEC", "UNIX", "INET", "IMPLINK", "PUP", "CHAOS", "NS", "ISO", "ECMA", "DATAKIT", "CCITT", "SNA", "DECnet", "DLI", "LAT", "HYLINK", "APPLETALK", "NETBIOS", "MAX" };
        public string[] atype = new string[] { "", "STREAM", "DGRAM", "RAW", "RDM", "SEQPACKET" };
        public string[] sdhow = new string[] { "RECEIVE", "SEND", "BOTH" };
        public string sockidfmt = "X4";

        public string proto(int proto)
        {
            switch (proto)
            {
                case 0:
                    return "IP";
                case 1:
                    return "ICMP";
                case 2:
                    return "GGP";
                case 6:
                    return "TCP";
                case 12:
                    return "PUP";
                case 17:
                    return "UDP";
                case 22:
                    return "IDP";
                case 77:
                    return "ND";
                case 255:
                    return "RAW";
                case 256:
                    return "MAX";
                default:
                    return "UNKNOWN";
            }
        }

        public string type(int type)
        {
            switch (type)
            {
                case 0:
                    return "IP";
                case 1:
                    return "ICMP";
                case 2:
                    return "GGP";
                case 6:
                    return "TCP";
                case 12:
                    return "PUP";
                case 17:
                    return "UDP";
                case 22:
                    return "IDP";
                case 77:
                    return "ND";
                case 255:
                    return "RAW";
                case 256:
                    return "MAX";
                default:
                    return "UNKNOWN";
            }
        }

        public string msg(int function)
        {
            switch (function)
            {
                case Constants.FUNC_SEND:
                    return "send()";
                case Constants.FUNC_SENDTO:
                    return "sendto()";
                case Constants.FUNC_WSASEND:
                    return "WSASend()";
                case Constants.FUNC_WSASENDTO:
                    return "WSASendTo()";
                case Constants.FUNC_WSASENDDISCONNECT:
                    return "WSASendDisconnect()";
                case Constants.FUNC_RECV:
                    return "recv()";
                case Constants.FUNC_RECVFROM:
                    return "recvfrom()";
                case Constants.FUNC_WSARECV:
                    return "WSARecv()";
                case Constants.FUNC_WSARECVFROM:
                    return "WSARecvFrom()";
                case Constants.FUNC_WSARECVDISCONNECT:
                    return "WSARecvDisconnect()";
                default:
                    return "";
            }
        }

        public string api(int function)
        {
            switch (function)
            {
                case Constants.FUNC_WSAACCEPT:
                    return "WSAAccept()";
                case Constants.FUNC_ACCEPT:
                    return "accept()";
                case Constants.FUNC_WSACONNECT:
                    return "WSAConnect()";
                case Constants.FUNC_CONNECT:
                    return "connect()";
                case Constants.FUNC_WSASOCKETW_IN:
                case Constants.FUNC_WSASOCKETW_OUT:
                    return "WSASocket()";
                case Constants.FUNC_BIND:
                    return "bind()";
                case Constants.CONN_WSASENDTO:
                    return "WSASendTo()";
                case Constants.CONN_WSARECVFROM:
                    return "WSARecvFrom()";
                case Constants.CONN_SENDTO:
                    return "sendto()";
                case Constants.CONN_RECVFROM:
                    return "recvfrom()";
                case Constants.FUNC_SOCKET_IN:
                case Constants.FUNC_SOCKET_OUT:
                    return "socket()";
                case Constants.FUNC_CLOSESOCKET:
                    return "closesocket()";
                case Constants.FUNC_LISTEN:
                    return "listen()";
                case Constants.FUNC_SHUTDOWN:
                    return "shutdown()";
                case Constants.FUNC_WSASENDDISCONNECT:
                    return "WSASendDisconnect()";
                case Constants.FUNC_WSARECVDISCONNECT:
                    return "WSARecvDisconnect()";
                case Constants.DNS_GETHOSTNAME:
                    return "gethostname()";
                case Constants.DNS_GETHOSTBYADDR_IN:
                case Constants.DNS_GETHOSTBYADDR_OUT:
                    return "gethostbyaddr()";
                case Constants.DNS_GETHOSTBYNAME_IN:
                case Constants.DNS_GETHOSTBYNAME_OUT:
                    return "gethostbyname()";
                default:
                    return "";
            }
        }

        public byte msgnum(string name)
        {
            switch (name)
            {
                case "send()":
                    return Constants.FUNC_SEND;
                case "sendto()":
                    return Constants.FUNC_SENDTO;
                case "WSASend()":
                    return Constants.FUNC_WSASEND;
                case "WSASendTo()":
                    return Constants.FUNC_WSASENDTO;
                case "WSASendDisconnect()":
                    return Constants.FUNC_WSASENDDISCONNECT;
                case "recv()":
                    return Constants.FUNC_RECV;
                case "recvfrom()":
                    return Constants.FUNC_RECVFROM;
                case "WSARecv()":
                    return Constants.FUNC_WSARECV;
                case "WSARecvFrom()":
                    return Constants.FUNC_WSARECVFROM;
                case "WSARecvDisconnect()":
                    return Constants.FUNC_WSARECVDISCONNECT;
                default:
                    return 0;
            }
        }

        public byte apinum(string name)
        {
            switch (name)
            {
                case "WSAAccept()":
                    return Constants.FUNC_WSAACCEPT;
                case "accept()":
                    return Constants.FUNC_ACCEPT;
                case "WSAConnect()":
                    return Constants.FUNC_WSACONNECT;
                case "connect()":
                    return Constants.FUNC_CONNECT;
                case "WSASocket()":
                    return Constants.FUNC_WSASOCKETW_IN;
                case "bind()":
                    return Constants.FUNC_BIND;
                case "WSASendTo()":
                    return Constants.CONN_WSASENDTO;
                case "WSARecvFrom()":
                    return Constants.CONN_WSARECVFROM;
                case "sendto()":
                    return Constants.CONN_SENDTO;
                case "recvfrom()":
                    return Constants.CONN_RECVFROM;
                case "socket()":
                    return Constants.FUNC_SOCKET_IN;
                case "closesocket()":
                    return Constants.FUNC_CLOSESOCKET;
                case "listen()":
                    return Constants.FUNC_LISTEN;
                case "shutdown()":
                    return Constants.FUNC_SHUTDOWN;
                case "gethostname()":
                    return Constants.DNS_GETHOSTNAME;
                case "gethostbyname()":
                    return Constants.DNS_GETHOSTBYNAME_OUT;
                case "gethostbyaddr()":
                    return Constants.DNS_GETHOSTBYADDR_OUT;
                default:
                    return 0;
            }
        }

        public int errornum(string name)
        {
            switch (name)
            {
                case "WSA_IO_PENDING":
                    return 10035;
                case "WSA_OPERATION_ABORTED":
                    return 10004;
                case "WSAEACCES":
                    return 10013;
                case "WSAEADDRINUSE":
                    return 10048;
                case "WSAEADDRNOTAVAIL":
                    return 10049;
                case "WSAEAFNOSUPPORT":
                    return 10047;
                case "WSAEALREADY":
                    return 10037;
                case "WSAECONNABORTED":
                    return 10053;
                case "WSAECONNREFUSED":
                    return 10061;
                case "WSAECONNRESET":
                    return 10054;
                case "WSAEDESTADDRREQ":
                    return 10039;
                case "WSAEDISCON":
                    return 10101;
                case "WSAEFAULT":
                    return 10014;
                case "WSAEHOSTUNREACH":
                    return 10065;
                case "WSAEINPROGRESS":
                    return 10036;
                case "WSAEINTR":
                    return 10004;
                case "WSAEINVAL":
                    return 10022;
                case "WSAEISCONN":
                    return 10056;
                case "WSAEMFILE":
                    return 10024;
                case "WSAEMSGSIZE":
                    return 10040;
                case "WSAENETDOWN":
                    return 10050;
                case "WSAENETRESET":
                    return 10052;
                case "WSAENETUNREACH":
                    return 10051;
                case "WSAENOBUFS":
                    return 10055;
                case "WSAENOPROTOOPT":
                    return 10042;
                case "WSAENOTCONN":
                    return 10057;
                case "WSAENOTSOCK":
                    return 10038;
                case "WSAEOPNOTSUPP":
                    return 10045;
                case "WSAEPROTONOSUPPORT":
                    return 10043;
                case "WSAEPROTOTYPE":
                    return 10041;
                case "WSAESHUTDOWN":
                    return 10058;
                case "WSAESOCKTNOSUPPORT":
                    return 10044;
                case "WSAETIMEDOUT":
                    return 10060;
                case "WSAEWOULDBLOCK":
                    return 10035;
                case "WSAHOST_NOT_FOUND":
                    return 11001;
                case "WSANO_DATA":
                    return 11004;
                case "WSANO_RECOVERY":
                    return 11003;
                case "WSANOTINITIALISED":
                    return 10093;
                case "WSATRY_AGAIN":
                    return 11002;
                case "NO_ERROR":
                default:
                    return 0;
            }
        }

        public string error(int error)
        {
            switch (error)
            {
                case 10013:
                    return "WSAEACCES";
                case 10048:
                    return "WSAEADDRINUSE";
                case 10049:
                    return "WSAEADDRNOTAVAIL";
                case 10047:
                    return "WSAEAFNOSUPPORT";
                case 10037:
                    return "WSAEALREADY";
                case 10053:
                    return "WSAECONNABORTED";
                case 10061:
                    return "WSAECONNREFUSED";
                case 10054:
                    return "WSAECONNRESET";
                case 10039:
                    return "WSAEDESTADDRREQ";
                case 10101:
                    return "WSAEDISCON";
                case 10014:
                    return "WSAEFAULT";
                case 10065:
                    return "WSAEHOSTUNREACH";
                case 10036:
                    return "WSAEINPROGRESS";
                case 10004:
                    return "WSAEINTR";
                case 10022:
                    return "WSAEINVAL";
                case 10056:
                    return "WSAEISCONN";
                case 10024:
                    return "WSAEMFILE";
                case 10040:
                    return "WSAEMSGSIZE";
                case 10050:
                    return "WSAENETDOWN";
                case 10052:
                    return "WSAENETRESET";
                case 10051:
                    return "WSAENETUNREACH";
                case 10055:
                    return "WSAENOBUFS";
                case 10042:
                    return "WSAENOPROTOOPT";
                case 10057:
                    return "WSAENOTCONN";
                case 10038:
                    return "WSAENOTSOCK";
                case 10045:
                    return "WSAEOPNOTSUPP";
                case 10043:
                    return "WSAEPROTONOSUPPORT";
                case 10041:
                    return "WSAEPROTOTYPE";
                case 10058:
                    return "WSAESHUTDOWN";
                case 10044:
                    return "WSAESOCKTNOSUPPORT";
                case 10060:
                    return "WSAETIMEDOUT";
                case 10035:
                    return "WSAEWOULDBLOCK";
                case 11001:
                    return "WSAHOST_NOT_FOUND";
                case 11004:
                    return "WSANO_DATA";
                case 11003:
                    return "WSANO_RECOVERY";
                case 10093:
                    return "WSANOTINITIALISED";
                case 11002:
                    return "WSATRY_AGAIN";
                case 0:
                default:
                    return "NO_ERROR";
            }
        }
    }
}
