using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PKO_BOT.Business
{
    public static class PcapUtilities
    {
        public static async Task<int> GetHighestTrafficDevice(CaptureDeviceList devices, int timeoutMs)
        {
            System.Collections.Concurrent.ConcurrentDictionary<int, int> bag = new System.Collections.Concurrent.ConcurrentDictionary<int, int>();

            var isCapturing = true;
            var tasks = new List<Task>();

            for (var index = 0; index < devices.Count; ++index)
            {
                devices[index].Open(DeviceMode.Promiscuous, timeoutMs);
                bag[index] = 0;

                var currentIndex = index;

                tasks.Add(new Task(() =>
                {
                    while (isCapturing)
                    {
                        var rawCapture = devices[currentIndex].GetNextPacket();
                        
                        if (rawCapture != null && bag.ContainsKey(currentIndex))
                        {
                            ++bag[currentIndex];
                        }
                    }
                }));
            }

            foreach (Task task in tasks)
            {
                task.Start();
            }

            await Task.Run(() =>
            {
                Thread.Sleep(timeoutMs);
                isCapturing = false;
            });

            await Task.WhenAll(tasks);

            foreach (var device in devices)
            {
                device.Close();
            }

            return bag.First(item => item.Value == bag.Max(currentItem => currentItem.Value)).Key;
        }
    }
}
