using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace osu2013server.Packets
{
    public class PacketOut
    {
        private byte[] payload;
        public virtual ushort id { get; init; } = 0;

        protected virtual void WritePayload(Stream buffer) {}

        public virtual byte[] ToByteArray()
        {
            using var buffer = new MemoryStream();

            using (var writer = new BinaryWriter(buffer, Encoding.UTF8, leaveOpen: true))
            {
                writer.Write(id);
                writer.Write((byte)0);
                writer.Write(0); // buffer.Position = 4;
                
                WritePayload(buffer);

                buffer.Position = 3;
                
                writer.Write((int)buffer.Length-7);
            }

            payload = buffer.ToArray();

            return payload;
        }
    }
}