using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSystemAccess.Domain.Exceptions;
using FileSystemAccess.Domain.Models;

namespace FileSystemAccess.FileSystemService
{
    internal class FileSystemUtils
    {
        /// <summary>
        /// Returns Folder info
        /// </summary>
        /// <param name="path">Full folder path</param>
        /// <returns><see cref="FileSystemItemModelBase"/> - folder info</returns>
        public static FileSystemItemModelBase GetFolder(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            if (directoryInfo.Exists)
            {
                var folder = new FileSystemItemModelBase()
                {
                    IsFolder = true,
                    Name = directoryInfo.Name,
                    ParentPath = directoryInfo.Parent?.FullName
                };

                var folderContent = new FolderContentModel();

                foreach (var subDirectory in directoryInfo.GetDirectories())
                {
                    folderContent.ContentValue.Add(new FileSystemItemModelBase()
                    {
                        Name = subDirectory.Name,
                        IsFolder = true
                    });
                }

                foreach (var subFile in directoryInfo.GetFiles())
                {
                    folderContent.ContentValue.Add(new FileSystemItemModelBase()
                    {
                        Name = subFile.Name,
                        IsFolder = false,
                        Size = subFile.Length
                    });
                }

                folder.Content = folderContent;

                return folder;
            }

            return null;
        }

        /// <summary>
        /// Returns file by path
        /// </summary>
        /// <param name="path">Full file path</param>
        /// <returns><see cref="FileSystemItemModelBase"/> - file info</returns>
        public static FileSystemItemModelBase GetFile(string path)
        {
            var fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                var bytes = File.ReadAllBytes(path);
                var content = Convert.ToBase64String(bytes);

                return new FileSystemItemModelBase()
                {
                    Name = fileInfo.Name,
                    ParentPath = fileInfo.DirectoryName,
                    IsFolder = false,
                    Size = fileInfo.Length,
                    Content = new FileContentModel(content)
                };
            }
            return null;
        }

        /// <summary>
        /// Deletes file
        /// </summary>
        /// <param name="path">Full file path</param>
        public static void DeleteFile(string path)
        {
            var fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                File.Delete(fileInfo.FullName);
            }
            else
            {
                throw new FileNotFoundException("File not found");
            }
        }

        /// <summary>
        /// Recurively deletes folder
        /// </summary>
        /// <param name="path">Folder full path</param>
        public static void DeleteFolder(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            if (directoryInfo.Exists)
            {
                Directory.Delete(directoryInfo.FullName, true);
            }
            else
            {
                throw new DirectoryNotFoundException("Directory");
            }
        }

        /// <summary>
        /// Create or update file
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="fileModel"></param>
        /// <param name="isNewFile"></param>
        /// <param name="replaceIfExists"></param>
        public static void CreateOrReplaceFile(string folderPath, CreateOrUpdateFileModel fileModel, out bool isNewFile, bool replaceIfExists = false)
        {
            isNewFile = true;

            var path = $"{folderPath.TrimEnd('/')}/{fileModel.Name}";

            if (File.Exists(path))
            {
                if (replaceIfExists)
                {
                    isNewFile = false;
                    File.Delete(path);
                }
                else
                {
                    throw new FileAlreadyExistsException("File already exists");
                }
            }

            try
            {
                using (var fs = File.Create(path))
                {
                    if (fileModel.Content != null)
                    {
                        var info = Convert.FromBase64String(fileModel.Content);
                        fs.Write(info, 0, info.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
