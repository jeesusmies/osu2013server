using osu2013server.Enums;

namespace osu2013server.Objects
{
    public class PlayerStats
    {
        public long Score { get; set; }
        public long RankedScore { get; set; }
        public long TotalScore { get; set; }
        
        public short PerformancePoints { get; set; }
        public Status Status { get; set; }
        public int PlayCount { get; set; }
        public float Accuracy { get; set; }
        public int Rank { get; set; }
        public string Action { get; set; }
        public string ActionMD5 { get; set; }
        public Gamemode Gamemode { get; set; }
        public Mods Mods { get; set; }
        public int MapID { get; set; }
    }
}