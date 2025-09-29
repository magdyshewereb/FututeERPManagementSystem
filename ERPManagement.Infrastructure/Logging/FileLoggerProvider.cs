using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ERPManagement.Infrastructure.Logging;

namespace ERPManagement.Infrastructure.Logging
{
    [ProviderAlias("FileLoggerProvider")]
    public class FileLoggerProvider : ILoggerProvider
    {
        public readonly FileLoggerOptions Options;

        public FileLoggerProvider(IOptions<FileLoggerOptions> _options)
        {
            Options = _options.Value;

            if (!Directory.Exists(Options.FolderPath))
            {
                Directory.CreateDirectory(Options.FolderPath);
            }

            if (!Directory.Exists(Options.ErrorFolderPath))
            {
                Directory.CreateDirectory(Options.ErrorFolderPath);
            }

            if (!Directory.Exists(Options.InfoFolderPath))
            {
                Directory.CreateDirectory(Options.InfoFolderPath);
            }

            if (!Directory.Exists(Options.TraceFolderPath))
            {
                Directory.CreateDirectory(Options.TraceFolderPath);
            }

        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(this);
        }

        public void Dispose()
        {
        }
    }
}
