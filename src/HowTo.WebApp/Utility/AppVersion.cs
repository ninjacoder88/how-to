﻿namespace HowTo.WebApp.Utility
{
    public static class AppVersion
    {
        public const int Major = 1;
        public const int Minor = 0;
        public static string Version = $"{Major}.{Minor}";
        public static string QueryString = $"v={Version}";
    }
}
