namespace FlowBoard.Frontend.Domain.Constants;

public static class AuthConstants
{
    public const string SchemeName = "UserAuth";

    public static class JsMethods
    {
        public const string Initialize = "microsoftAuth.initialize";
        public const string LoginPopup = "microsoftAuth.loginPopup";
    }
}