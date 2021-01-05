using System;

namespace osu2013server.Enums
{
    [Flags]
    public enum Mods
    {
        None = 0,
        NoFail = 1,
        Easy = 2,
        NoVideo = 4,
        Hidden = 8,
        HardRock = 16,
        SuddenDeath = 32,
        DoubleTime = 64,
        Relax = 128,
        HalfTime = 256,
        Flashlight = 1024,
        Autoplay = 2048,
        SpunOut = 4096,
        Relax2 = 8192,
        LastMod = 16384
    }
}