namespace PowerPages.SolutionComponents.Console.Constants
{
    public static class Config
    {
        public const string ConnectionStringTemplate = "AuthType=OAuth;Url={0};Interactive=True;RedirectUri=http://localhost;TokenCacheStorePath=./TokenCache";
        public const string TokenCachePath = "./TokenCache";
        public const string RedirectUri = "http://localhost";
    }
}
