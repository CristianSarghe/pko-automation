using PKO_BOT.Packets.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PKO_BOT.Packets
{
    public class PacketManager
    {
        private NamedPipeServerStream pipeIn;
        private NamedPipeClientStream pipeOut;
        private Thread readingThread;

        private readonly string dllRoute = Directory.GetCurrentDirectory() + "\\WSPE.dat";

        private int processId = 0;
        private string socket = null;

        private Encoding ae = Encoding.GetEncoding(28591);

        private Dictionary<byte, Action<RecordedPacket>> packetFilterActions = new Dictionary<byte, Action<RecordedPacket>>();

        // DLL Imports
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
           uint dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

        public void AddFilterAction(byte code, Action<RecordedPacket> action)
        {
            if (!this.packetFilterActions.ContainsKey(code))
            {
                this.packetFilterActions.Add(code, action);
            }
        }

        public void RemoveFilterAction(byte code)
        {
            if (!this.packetFilterActions.ContainsKey(code))
            {
                this.packetFilterActions.Remove(code);
            }
        }

        public void AttachToProcess(int processId)
        {
            this.processId = processId;

            if (processId != 0)
            {
                InitializePipes();
                StartReading();
            }
        }

        public void WriteMessage(string socket, byte[] payload)
        {
            PipeHeader message = new PipeHeader
            {
                command = Constants.CMD_INJECT,
                function = Constants.FUNC_SEND,
                datasize = payload.Length,
                sockid = int.Parse(socket, System.Globalization.NumberStyles.HexNumber)
            };

            pipeOut.Write(Utilities.RawSerializeEx(message), 0, Marshal.SizeOf(message));
            pipeOut.Write(payload, 0, message.datasize);
        }

        public void Dismiss()
        {
            if (this.pipeOut != null && this.pipeOut.IsConnected == true)
            {
                var header = new PipeHeader();
                header.command = Constants.CMD_UNLOAD_DLL;

                try
                {
                    WriteHeader(header);
                }
                catch { }

                if (readingThread != null && readingThread.IsAlive)
                {
                    readingThread.Abort();
                }

                this.pipeOut.Close();
            }

            if (this.pipeIn != null && this.pipeIn.IsConnected == true)
            {
                this.pipeIn.Close();
            }

            this.processId = 0;
        }

        private bool StartReading()
        {
            readingThread = new Thread(() =>
            {
                try
                {
                    PipeRead();
                }
                catch(ThreadAbortException e) { Console.WriteLine(e.Message); }
            })
            {
                IsBackground = true
            };
            readingThread.Start();

            return true;
        }

        private void StopReading()
        {
            if (readingThread != null && readingThread.IsAlive)
            {
                readingThread.Interrupt();
            }
        }

        private void InitializePipes()
        {
            if (pipeOut == null)
            {
                pipeOut = new NamedPipeClientStream(".", "wspe.send." + this.processId.ToString("X8"), PipeDirection.Out, PipeOptions.Asynchronous);
            }

            try
            {
                pipeIn = new NamedPipeServerStream("wspe.recv." + this.processId.ToString("X8"), PipeDirection.In, 1, PipeTransmissionMode.Message);
            }
            catch
            {
                Console.WriteLine("Can not attach to process! A previous session might still be active!");
                this.processId = 0;
                return;
            }

            // Inject WSPE.dat from current directory
            IntPtr hProc = OpenProcess(ProcessAccessFlags.All, false, this.processId);
            IntPtr ptrLoadLib = GetProcAddress(GetModuleHandle("KERNEL32.DLL"), "LoadLibraryA");

            if (hProc == IntPtr.Zero)
            {
                Console.WriteLine("Cannot open process!");
                return;
            }

            IntPtr ptrMem = VirtualAllocEx(hProc, (IntPtr)0, (uint)dllRoute.Length, AllocationType.COMMIT, MemoryProtection.EXECUTE_READ);
            if (ptrMem == IntPtr.Zero)
            {
                MessageBox.Show("Cannot allocate process memory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte[] dbDLL = ae.GetBytes(dllRoute);

            if (!WriteProcessMemory(hProc, ptrMem, dbDLL, (uint)dbDLL.Length, out int ipTmp))
            {
                MessageBox.Show("Cannot write to process memory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CreateRemoteThread(hProc, IntPtr.Zero, 0, ptrLoadLib, ptrMem, 0, IntPtr.Zero);

            string RegName = "PacketEditor.com";
            string RegKey = "7007C8466C99901EF555008BF90D0C0F11C2005CE042C84B7C1E2C0050DF305647026513";

            pipeIn.WaitForConnection();

            if (!pipeOut.IsConnected)
            {
                pipeOut.Connect();
                pipeOut.Write(BitConverter.GetBytes(RegName.Length), 0, 1);
                pipeOut.Write(ae.GetBytes(RegName), 0, RegName.Length);
                pipeOut.Write(ae.GetBytes(RegKey), 0, RegKey.Length);
            }
        }

        private void PipeRead()
        {
            byte[] packetHeaderBytes = new byte[14];

            while (pipeIn.IsConnected)
            {
                while (pipeIn.Read(packetHeaderBytes, 0, 14) != 0)
                {
                    var packetHeaderObject = (PipeHeader)Utilities.RawDeserializeEx(packetHeaderBytes, typeof(PipeHeader));

                    if (packetHeaderObject.datasize != 0)
                    {
                        this.socket = packetHeaderObject.sockid.ToString("X4");

                        var packetData = new byte[packetHeaderObject.datasize];
                        pipeIn.Read(packetData, 0, packetData.Length);

                        switch (packetHeaderObject.function)
                        {
                            case Constants.FUNC_SEND:
                                Console.WriteLine("Sent: " + packetData.Length);
                                if(this.packetFilterActions.ContainsKey(packetData[3]))
                                {
                                    packetFilterActions[packetData[3]](new RecordedPacket { Data = packetData, Header = packetHeaderObject });
                                }
                                break;
                            case Constants.FUNC_RECV:
                                Console.WriteLine("Received: " + packetData.Length);
                                if (this.packetFilterActions.ContainsKey(packetData[3]))
                                {
                                    packetFilterActions[packetData[3]](new RecordedPacket { Data = packetData, Header = packetHeaderObject });
                                }
                                break;
                            default:
                                Console.WriteLine("Other type of message");
                                break;
                        }
                    }
                    else
                    {
                        if (packetHeaderObject.command == Constants.CMD_INIT)
                        {
                            if (packetHeaderObject.function == Constants.INIT_DECRYPT)
                                if (packetHeaderObject.extra == 0)
                                {
                                    Console.WriteLine("Failed.");
                                    continue;
                                }
                                else
                                {
                                    PipeHeader message = new PipeHeader
                                    {
                                        datasize = 0,
                                        command = Constants.CMD_ENABLE_MONITOR
                                    };

                                    WriteHeader(message);
                                }
                        }
                    }
                }
            }
        }

        private void WriteHeader(PipeHeader packet)
        {
            pipeOut.Write(Utilities.RawSerializeEx(packet), 0, Marshal.SizeOf(packet));
        }
    }
}
