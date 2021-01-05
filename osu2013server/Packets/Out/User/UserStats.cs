using System.IO;
using System.Text;
using osu2013server.Objects;

namespace osu2013server.Packets.Out
{
    public class UserStats : PacketOut
    {
        public override ushort id => 5;
        public Player Player { get; init; }

        protected override void WritePayload(Stream buffer)
        {
            using var writer = new BinaryWriter(buffer, Encoding.UTF8, leaveOpen: true);
            
            writer.Write(Player.ID);
            writer.Write(Player.Stats.Status);
            writer.Write(Player.Stats.Beatmap);
            writer.Write(Player.Stats.BeatmapMD5);
            writer.Write(Player.Stats.Mods);
            writer.Write(Player.Stats.Gamemode);
            writer.Write(Player.Stats.MapID);
            writer.Write(Player.Stats.RankedScore);
            writer.Write(Player.Stats.Accuracy);
            writer.Write(Player.Stats.PlayCount);
            writer.Write(Player.Stats.TotalScore);
            writer.Write(Player.Stats.RankedScore);
            writer.Write(Player.Stats.PerformancePoints);
        }
    }
}