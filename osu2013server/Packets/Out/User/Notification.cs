using System.IO;
using System.Text;

namespace osu2013server.Packets.Out
{
    public class Notification : PacketOut
    {
        public override ushort id => 24;
        public string Message { get; init; }

        protected override void WritePayload(Stream buffer)
        {
            using var writer = new BinaryWriter(buffer, Encoding.UTF8, leaveOpen: true);
            
            writer.Write(Message);
        }
    }
}