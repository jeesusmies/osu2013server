using System.IO;
using System.Text;
using osu2013server.Objects;

namespace osu2013server.Packets.Out
{
    public class UserStats : PacketOut
    {
        public override ushort id => 5;
        public PlayerStats Status { get; init; }

        protected override void WritePayload(Stream buffer)
        {
            using var writer = new BinaryWriter(buffer, Encoding.UTF8, leaveOpen: true);
            
            writer.Write(Status);
        }
    }
}