using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog.Events;
using SharpConfig;

namespace StarFinder
{
    public static class Config
    {
        public static Configuration GeneralConfig;

        public static LogEventLevel LogLevel => GetLogLevel();
        public static string AddressCSVPath => GeneralConfig["General"]["Address CSV"].StringValue;

        public static void ReloadConfig()
        {
            if(!File.Exists("Config.ini"))
            {
                CreateDefaultConfig();
            }
            GeneralConfig = Configuration.LoadFromFile("Config.ini");
        }

        private static void CreateDefaultConfig()
        {
            Configuration myConfig = new Configuration();
            myConfig["General"]["Log Level"].StringValue = "Debug";
            myConfig["General"]["Address CSV"].StringValue = @".\btc_balance_sorted.csv";

            myConfig.SaveToFile("Config.ini");
        }

        public static LogEventLevel GetLogLevel()
        {
            switch (GeneralConfig["General"]["Log Level"].StringValue)
            {
                case "Verbose":
                case "Trace":
                    return LogEventLevel.Verbose;
                case "Debug":
                    return LogEventLevel.Debug;
                case "Info":
                case "Information":
                    return LogEventLevel.Information;
                case "Warning":
                    return LogEventLevel.Warning;
                case "Error":
                    return LogEventLevel.Error;

                default:
                    return LogEventLevel.Information;
            }
        }
    }
}