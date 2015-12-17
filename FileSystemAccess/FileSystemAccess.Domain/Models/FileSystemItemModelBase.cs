namespace FileSystemAccess.Domain.Models
{
    public class FileSystemItemModelBase
    {
        /// <summary>
        /// File or folder name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// File or folder parent path
        /// </summary>
        public string ParentPath { get; set; }

        /// <summary>
        /// True if item is foler
        /// </summary>
        public bool IsFolder { get; set; }

        /// <summary>
        /// File size
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// File or folder content
        /// </summary>
        public IFileSystemItemContentModel Content { get; set; }
    }
}
