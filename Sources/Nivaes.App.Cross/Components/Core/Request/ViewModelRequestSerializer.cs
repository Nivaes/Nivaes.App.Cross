using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace Nivaes.App.Cross
{
    internal static class ViewModelRequestSerializer
    {
        public static byte[] Serializer(ViewModelRequest request)
        {           
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);

            //writer.Write(request.);

            return ms.ToArray();
        }

        public static ViewModelRequest Deserialize(byte[] buffer) 
        {
            using var ms = new MemoryStream(buffer);
            using var reader = new BinaryReader(ms);
            
            return new ViewModelRequest
            {

            };
        }
    }
}
