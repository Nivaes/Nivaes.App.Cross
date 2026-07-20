using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
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

            var id = ViewModelRequestCache.Add(request.ViewModel);
            writer.Write(id);

            if (request.ParameterValues != null) {
                Debugger.Break();
            }
            if (request.PresentationValues != null)
            {
                Debugger.Break();
            }

            return ms.ToArray();
        }

        public static ViewModelRequest Deserialize(byte[] buffer) 
        {
            using var ms = new MemoryStream(buffer);
            using var reader = new BinaryReader(ms);

            var id = reader.ReadUInt32();
            if (ViewModelRequestCache.TryGetValue(id, out var viewModel))
            {
                return new ViewModelRequest(viewModel!)
                {

                };
            }

            throw new AppException("ViewModelReques not field");
        }

        public static uint DeserializeId(byte[] buffer)
        {
            using var ms = new MemoryStream(buffer);
            using var reader = new BinaryReader(ms);

            return reader.ReadUInt32();
        }
    }
}
