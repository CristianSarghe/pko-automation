using PacketDotNet;
using SharpPcap;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PKO_BOT.Business
{
    public class PcapScanner
    {
        private bool isCapturing = true;

        public async Task<List<Packet>> ScanPackets(Form form, ICaptureDevice selectedDevice, List<byte[]> containedFilterList)
        {
            var capturedPackets = new List<Packet>();
            this.isCapturing = true;

            var task = Task.Run(() =>
            {
                selectedDevice.Open(DeviceMode.Promiscuous, 1000);

                while (this.isCapturing)
                {
                    var rawCapture = selectedDevice.GetNextPacket();

                    if (rawCapture == null)
                    {
                        continue;
                    }

                    var packet = Packet.ParsePacket(rawCapture.LinkLayerType, rawCapture.Data);

                    containedFilterList.ForEach(byteArray =>
                    {
                        if (packet.Bytes.ContainsSameOrder(byteArray))
                        {
                            capturedPackets.Add(packet);
                        }
                    });
                }

                selectedDevice.Close();
            });

            await task;

            return capturedPackets;
        }

        public void StopScanning()
        {
            this.isCapturing = false;
        }

        public void PlayPacket(byte[] payload)
        {
            // Should play packet here
        }
    }
}
