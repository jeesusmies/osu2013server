using System.IO;
using System.Text;
using osu2013server.Enums;

namespace osu2013server.Packets.Out
{
    public class UserPresence : PacketOut
    {
        public override ushort id => 83;
        
        // Should maybe make this into an object?
        // Not sure cuz its not being used anywhere else...
        public int ID { get; init; }
        public string Username { get; init; }
        public byte UtcOffset { get; init; }
        public byte Country { get; init; }
        public Privileges Privilege { get; init; }
        public float Longitude { get; init; }
        public float Latitude { get; init; }

        protected override void WritePayload(Stream buffer)
        {
            using var writer = new BinaryWriter(buffer, Encoding.UTF8, leaveOpen: true);
            
            writer.Write(ID);
            writer.Write(Username);
            writer.Write(UtcOffset);
            writer.Write(Country);
            writer.Write((byte)Privilege);
            writer.Write(Longitude);
            writer.Write(Latitude);
        }
    }
}