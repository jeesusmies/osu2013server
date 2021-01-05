using System.IO;
using System.Text;
using osu2013server.Objects;

namespace osu2013server.Packets.Out
{
    public class UserStats : PacketOut
    {
        public override ushort id => 11;
        public Player Player { get; init; }

        protected override void WritePayload(Stream buffer)
        {
            using var writer = new BinaryWriter(buffer, Encoding.UTF8, leaveOpen: true);
            
            writer.Write(Player.ID);
            writer.Write((byte)Player.Stats.Status);
            writer.Write(Player.Stats.Action);
            writer.Write(Player.Stats.ActionMD5);
            writer.Write((int)Player.Stats.Mods);
            writer.Write((byte)Player.Stats.Gamemode);
            writer.Write(Player.Stats.MapID);
            writer.Write(Player.Stats.RankedScore);
            writer.Write(Player.Stats.Accuracy);
            writer.Write(Player.Stats.PlayCount);
            writer.Write(Player.Stats.Score);
            writer.Write(Player.Stats.Rank);
            writer.Write(Player.Stats.PerformancePoints);
        }
    }
}