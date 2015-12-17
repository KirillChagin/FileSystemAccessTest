using FileSystemAccess.Domain.Models;

namespace FileSystemAccess.FileSystemServiceContract
{
    public interface IFileService
    {
        /// <summary>
        /// Get a file/folder info for specified path
        /// </summary>
        /// <param name="path">System path</param>
        /// <returns><see cref="FileSystemItemModelBase"/>file info model</returns>
        FileSystemItemModelBase GetFileOrFolder(string path);

        /// <summary>
        /// Delete file/folder
        /// </summary>
        /// <param name="path">System path</param>
        void DeleteFileOrFolder(string path);

        /// <summary>
        /// Create a new file
        /// </summary>
        /// <param name="folderPath">Folder path where a new file should be created</param>
        /// <param name="fileSystemItem"><see cref="CreateOrUpdateFileModel"/> - file info</param>
        void CreateFile(string folderPath, CreateOrUpdateFileModel fileSystemItem);

        /// <summary>
        /// Create or update file
        /// </summary>
        /// <param name="folderPath">Folder path where a new file should be created/updated</param>
        /// <param name="fileSystemItem"><see cref="CreateOrUpdateFileModel"/> - file info</param>
        /// <param name="isCreated">Determines whether a file is created, or updated</param>
        void UpdateOrCreateFile(string folderPath, CreateOrUpdateFileModel fileSystemItem, out bool isCreated);
    }
}
