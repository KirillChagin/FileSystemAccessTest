namespace FileSystemAccess.Domain.Models
{
    public interface IFileSystemItemContentModel
    {
        /// <summary>
        /// File or folder content
        /// </summary>
        object Content { get; set; }
    }
}
