using Microsoft.Extensions.Logging;

namespace ERPManagement.Infrastructure.Logging
{
    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder, Action<FileLoggerOptions> configure)
        {
            var options = new FileLoggerOptions();
            configure(options);

            //builder.Services.AddSingleton<ILoggerProvider, FileLoggerProvider>(serviceProvider => new FileLoggerProvider(options));

            return builder;
        }
    }
}
