using System.IO;
using System.Text;

namespace osu2013server.Packets.Out
{
    public class Login : PacketOut
    {
        public override ushort id => 5;
        public int Status { get; set; }

        protected virtual void WritePayload(Stream buffer)
        {
            using var writer = new BinaryWriter(buffer, Encoding.UTF8, leaveOpen: true);
            
            writer.Write(Status);
        }
    }
}