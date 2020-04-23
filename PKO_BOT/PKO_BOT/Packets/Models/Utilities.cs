using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PKO_BOT.Packets.Models
{
    public static class Utilities
    {
        public static byte[] RawSerializeEx(object payload)
        {
            var size = Marshal.SizeOf(payload);
            byte[] rawPayload = new byte[size];
            GCHandle handle = GCHandle.Alloc(rawPayload, GCHandleType.Pinned);
            IntPtr buffer = handle.AddrOfPinnedObject();
            Marshal.StructureToPtr(payload, buffer, false);

            handle.Free();

            return rawPayload;
        }

        public static object RawDeserializeEx(byte[] rawPayload, Type type)
        {
            int rawsize = Marshal.SizeOf(type);

            if (rawsize > rawPayload.Length)
            {
                return null;
            }

            GCHandle handle = GCHandle.Alloc(rawPayload, GCHandleType.Pinned);
            IntPtr buffer = handle.AddrOfPinnedObject();
            object retobj = Marshal.PtrToStructure(buffer, type);
            handle.Free();

            return retobj;
        }
    }
}
