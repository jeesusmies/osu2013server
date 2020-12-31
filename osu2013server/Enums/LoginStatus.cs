namespace osu2013server.Enums
{
    public enum LoginStatus
    {
        TestBuildButNotSupporter = -6,
        ServerSideError = -5,
        AccountNotActivated = -4,
        Banned = -3,
        TooOldVersion = -2,
        AuthenticationFailed = -1,
        AuthenticationSuccessful = 0
    }
}