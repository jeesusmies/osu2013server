using System.IO;
using System.Text;
using osu2013server.Enums;

namespace osu2013server.Packets.Out
{
    public class BanchoPrivileges : PacketOut
    {
        public override ushort id => 71;
        public Privileges Privileges { get; init; }

        protected override void WritePayload(Stream buffer)
        {
            using var writer = new BinaryWriter(buffer, Encoding.UTF8, leaveOpen: true);
            
            writer.Write((int)Privileges);
        }
    }
}