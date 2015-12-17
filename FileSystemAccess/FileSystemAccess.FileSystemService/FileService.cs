using System;
using System.IO;
using FileSystemAccess.Domain.Exceptions;
using FileSystemAccess.Domain.Models;
using FileSystemAccess.FileSystemServiceContract;

namespace FileSystemAccess.FileSystemService
{
    public class FileService : IFileService
    {
        /// <summary>
        /// Get a file/folder info for specified path
        /// </summary>
        /// <param name="path">System path</param>
        /// <returns><see cref="FileSystemItemModelBase"/>file info model</returns>
        /// <exception cref="ArgumentException">if <see cref="path"/> is empty</exception>
        /// <exception cref="PathNotExistsException">if <see cref="path"/> not exists</exception>
        public FileSystemItemModelBase GetFileOrFolder(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path could not be empty");    
            }

            if (!Directory.Exists(path) && !File.Exists(path))
            {
                throw new PathNotExistsException("No file or folder with specified path");
            }

            var attributes = File.GetAttributes(path);
            if (attributes.HasFlag(FileAttributes.Directory))
            {
                return FileSystemUtils.GetFolder(path);
            }
            return FileSystemUtils.GetFile(path);
        }

        /// <summary>
        /// Delete file/folder
        /// </summary>
        /// <param name="path">System path</param>
        /// <exception cref="ArgumentException">if <see cref="path"/> is empty</exception>
        /// <exception cref="PathNotExistsException">if <see cref="path"/> not exists or already deleted</exception>
        /// <exception cref="UnauthorizedAccessException"> if access denied</exception>
        public void DeleteFileOrFolder(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path could not be empty");
            }

            if (!Directory.Exists(path) && !File.Exists(path))
            {
                throw new PathNotExistsException("No file or folder with specified path");
            }

            var attributes = File.GetAttributes(path);
            if (attributes.HasFlag(FileAttributes.Directory))
            {
                try
                {
                    FileSystemUtils.DeleteFolder(path);
                }
                catch (DirectoryNotFoundException)
                {
                    throw new PathNotExistsException("No file or folder with specified path");
                }
            }

            try
            {
                FileSystemUtils.DeleteFile(path);
            }
            catch (FileNotFoundException)
            {
                throw new PathNotExistsException("No file or folder with specified path");
            }
        }

        /// <summary>
        /// Create a new file
        /// </summary>
        /// <param name="folderPath">Folder path where a new file should be created</param>
        /// <param name="fileSystemItem"><see cref="CreateOrUpdateFileModel"/> - file info</param>
        /// <exception cref="ArgumentException">if <see cref="fileSystemItem"/> is empty</exception>
        /// <exception cref="PathNotExistsException">if <see cref="fileSystemItem"/> not exists or already deleted</exception>
        /// <exception cref="FileAlreadyExistsException">if file already exists</exception>
        /// <exception cref="UnauthorizedAccessException"> if access denied</exception>
        public void CreateFile(string folderPath, CreateOrUpdateFileModel fileSystemItem)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                throw new ArgumentException("Path could not be empty");
            }

            if (fileSystemItem == null)
            {
                throw new ArgumentException("fileSystemItem could not be null");
            }
            if (!Directory.Exists(folderPath))
            {
                throw new PathNotExistsException("No folder with specified path");
            }

            try
            {
                bool isNew;
                FileSystemUtils.CreateOrReplaceFile(folderPath, fileSystemItem, out isNew);
            }
            catch (FileAlreadyExistsException e)
            {
                throw;
            }
            catch (Exception ex)
            {
                //TODO: rethrow with more info
                throw;
            }
        }

        /// <summary>
        /// Create or update file
        /// </summary>
        /// <param name="folderPath">Folder path where a new file should be created/updated</param>
        /// <param name="fileSystemItem"><see cref="CreateOrUpdateFileModel"/> - file info</param>
        /// <param name="isCreated">Determines whether a file is created, or updated</param>
        /// <exception cref="ArgumentException">if <see cref="folderPath"/> is empty or <see cref="fileSystemItem"/> is null</exception>
        /// <exception cref="FileAlreadyExistsException">if file already exists</exception>
        /// <exception cref="UnauthorizedAccessException"> if access denied</exception>
        public void UpdateOrCreateFile(string folderPath, CreateOrUpdateFileModel fileSystemItem, out bool isCreated)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                throw new ArgumentException("Path could not be empty");
            }

            if (fileSystemItem == null)
            {
                throw new ArgumentException("fileSystemItem could not be null");
            }

            try
            {
                FileSystemUtils.CreateOrReplaceFile(folderPath, fileSystemItem, out isCreated, true);
            }
            catch (Exception ex)
            {
                //TODO: rethrow with more info
                throw;
            }
        }
    }
}
