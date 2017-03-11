using System;
using System.IO;

namespace TiqUtils.Utils
{
    public static class Logging
    {
        public static void ErrorLog(string msg) => File.AppendAllText(@"error.log", $"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] - {msg}" + Environment.NewLine);

        public static void StatsLog(string msg) => File.AppendAllText(@"stats.log", $"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] - {msg}" + Environment.NewLine);

    }
}
