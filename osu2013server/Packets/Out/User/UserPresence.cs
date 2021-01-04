using System.IO;
using System.Text;
using osu2013server.Enums;

namespace osu2013server.Packets.Out
{
    public class UserPresence : PacketOut
    {
        public override ushort id => 83;
        
        public int ID { get; init; }
        public string Username { get; init; }
        public string UTC_Offset { get; init; }
        public byte Country { get; init; }
        public Priviliges Privilige { get; init; }
        public float Longitude { get; init; }
        public float Latitude { get; init; }

        protected override void WritePayload(Stream buffer)
        {
            using var writer = new BinaryWriter(buffer, Encoding.UTF8, leaveOpen: true);
            
            writer.Write(ID);
            writer.Write(Username);
            writer.Write(UTC_Offset);
            writer.Write(Country);
            writer.Write((byte)Privilige);
            writer.Write(Longitude);
            writer.Write(Latitude);
        }
    }
}