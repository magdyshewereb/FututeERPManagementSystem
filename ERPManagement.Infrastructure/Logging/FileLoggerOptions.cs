namespace ERPManagement.Infrastructure.Logging
{
    public class FileLoggerOptions
    {
        public virtual string FilePath { get; set; }

        public virtual string FolderPath { get; set; }
        public virtual string ErrorFolderPath { get; set; }
        public virtual string InfoFolderPath { get; set; }
        public virtual string TraceFolderPath { get; set; }
    }
}
