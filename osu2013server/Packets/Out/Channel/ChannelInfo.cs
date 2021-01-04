using System.IO;
using System.Text;

namespace osu2013server.Packets.Out.Channel
{
    public class ChannelInfo : PacketOut
    {
        public override ushort id => 24;
        public Channel Channel { get; init; }

        protected override void WritePayload(Stream buffer)
        {
            using var writer = new BinaryWriter(buffer, Encoding.UTF8, leaveOpen: true);
            
            writer.WriteBString(Channel.Name);
            writer.WriteBString(Channel.Topic);
            writer.WriteBString(Channel.Topic);
        }
    }
}