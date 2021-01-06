using System;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace StarFinder.Utils
{
    public interface IMyLogger : IDisposable
    {
        IMyLogger For<T>();
        IMyLogger For(Type type);
        void Trace(string template, params object[] values);
        void Debug(string template, params object[] values);
        void Info(string template, params object[] values);
        void Warning(string template, params object[] values);
        void Error(string template, params object[] values);
        void Fatal(string template, params object[] values);
    }

    public class MyLogger : IMyLogger
    {
        public MyLogger()
        {
            const string logTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}";

            LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch {MinimumLevel = LogEventLevel.Debug};

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .WriteTo.Console(outputTemplate: logTemplate)
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(@"Logs\Info-.log", rollingInterval: RollingInterval.Day, outputTemplate: logTemplate))
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.File(@"Logs\Debug-.log", rollingInterval: RollingInterval.Day, outputTemplate: logTemplate))
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(@"Logs\Warning-.log", rollingInterval: RollingInterval.Day, outputTemplate: logTemplate))
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.File(@"Logs\Error-.log", rollingInterval: RollingInterval.Day, outputTemplate: logTemplate))
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.File(@"Logs\Fatal-.log", rollingInterval: RollingInterval.Day, outputTemplate: logTemplate))
                .WriteTo.File(@"Logs\All-.log", rollingInterval: RollingInterval.Day, outputTemplate: logTemplate)
                .CreateLogger();
        }
        public static IMyLogger For<T>()
        {
            MyLogger myLogger = new MyLogger(Log.ForContext<T>());
            return myLogger;
        }

        public static IMyLogger For(Type type)
        {
            MyLogger myLogger = new MyLogger(Log.ForContext(type));
            return myLogger;
        }

        private Serilog.ILogger _inner;

        private MyLogger(Serilog.ILogger inner)
        {
            _inner = inner;
        }

        IMyLogger IMyLogger.For<T>()
        {
            return For<T>();
        }

        IMyLogger IMyLogger.For(Type type)
        {
            return For(type);
        }

        public void Trace(string template, params object[] values)
        {
            if (_inner.IsEnabled(LogEventLevel.Verbose))
            {
                _inner.Verbose(template, values);
            }
        }
        public void Debug(string template, params object[] values)
        {
            if (_inner.IsEnabled(LogEventLevel.Debug))
            {
                _inner.Debug(template, values);
            }
        }
        public void Info(string template, params object[] values)
        {
            if (_inner.IsEnabled(LogEventLevel.Information))
            {
                _inner.Information(template, values);
            }
        }
        public void Warning(string template, params object[] values)
        {
            if (_inner.IsEnabled(LogEventLevel.Warning))
            {
                _inner.Warning(template, values);
            }
        }
        public void Error(string template, params object[] values)
        {
            if (_inner.IsEnabled(LogEventLevel.Error))
            {
                _inner.Error(template, values);
            }
        }
        public void Fatal(string template, params object[] values)
        {
            if (_inner.IsEnabled(LogEventLevel.Fatal))
            {
                _inner.Fatal(template, values);
            }
        }
        public void Dispose()
        {
            Log.CloseAndFlush();
        }
    }
}