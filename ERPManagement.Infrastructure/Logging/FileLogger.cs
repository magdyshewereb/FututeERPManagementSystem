using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace ERPManagement.Infrastructure.Logging
{
    public class FileLogger : ILogger
    {
        private readonly FileLoggerProvider _loggerFileProvider;
        private static readonly object _lock = new object();

        public FileLogger([NotNull] FileLoggerProvider loggerFileProvider)
        {
            _loggerFileProvider = loggerFileProvider ?? throw new ArgumentNullException(nameof(loggerFileProvider));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NullLoggerScope.Instance; // تفادي Warnings
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            string fullFilePath = GetLogFilePath(logLevel);
            string logRecord = GetLogRecord(logLevel, state, exception, formatter);

            lock (_lock)
            {
                using var fileStream = new FileStream(fullFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                using var streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine(logRecord);
            }
        }

        private string GetLogFilePath(LogLevel logLevel)
        {
            string basePath = logLevel switch
            {
                LogLevel.Trace => _loggerFileProvider.Options.TraceFolderPath,
                LogLevel.Information => _loggerFileProvider.Options.InfoFolderPath,
                LogLevel.Error => _loggerFileProvider.Options.ErrorFolderPath,
                _ => _loggerFileProvider.Options.FolderPath
            };

            string fileName = _loggerFileProvider.Options.FilePath.Replace("{date}", DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
            return Path.Combine(basePath, fileName);
        }

        private string GetLogRecord<TState>(LogLevel logLevel, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            return string.Format("{0} [{1}] {2} {3}",
                "[" + DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss+00:00") + "]",
                logLevel.ToString(),
                formatter(state, exception),
                exception != null ? exception.StackTrace : "");
        }

        // Null Scope لتفادي الخطأ في الـ BeginScope
        private class NullLoggerScope : IDisposable
        {
            public static NullLoggerScope Instance { get; } = new NullLoggerScope();
            public void Dispose() { }
        }
    }
}
