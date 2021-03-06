﻿using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using FileSystemAccess.ActionFilters;
using FileSystemAccess.Domain.Exceptions;
using FileSystemAccess.Domain.Models;
using FileSystemAccess.FileSystemServiceContract;
using FileSystemAccess.Models;
using FileSystemAccess.Resources;

namespace FileSystemAccess.Controllers
{
    /// <summary>
    /// Controller for working with files
    /// </summary>
    [Authorize]
    public class FileSystemController : ApiController
    {
        private IFileService _fileService;

        private string _rootFolderPath;

        /// <summary>
        /// Constructor for controller
        /// </summary>
        /// <param name="service"></param>
        public FileSystemController(IFileService service)
        {
            _fileService = service;
            _rootFolderPath = ConfigurationManager.AppSettings["rootFolder"].TrimEnd('/','\\');
        }

        // GET api/FileSystem

        /// <summary>
        /// API method for getting file or folder info
        /// </summary>
        /// <param name="path">File or folder path</param>
        /// <example>GET api/FileSystem/Files/test.txt</example>
        /// <returns>Json with file/folder information</returns>
        public IHttpActionResult Get(string path)
        {
            var fullPath = _rootFolderPath;
            try
            {
                fullPath = new Uri($"{fullPath}\\{path?.TrimEnd('/')}").LocalPath;
            }
            catch (UriFormatException)
            {
                return BadRequest(ErrorMessages.InvalidFilePathFormatErrorMessage);
            }

            try
            {
                var fileSystemItem = _fileService.GetFileOrFolder(fullPath);
                var fileSystemItemViewModel = Mapper.Map(fileSystemItem, typeof (FileSystemItemModelBase),
                    typeof (FileSystemItemBaseViewModel));
                return Ok(fileSystemItemViewModel);
            }
            catch (PathNotExistsException)
            {
                return NotFound();
            }
            catch (ArgumentException)
            {
                return BadRequest(ErrorMessages.InvalidFilePathFormatErrorMessage);
            }
            catch (UnauthorizedAccessException e)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    Content = new StringContent(ErrorMessages.AccessDeniedErrorMessage)
                };
                throw new HttpResponseException(response);
            }
        }

        // DELETE api/FileSystem

        /// <summary>
        /// API method for deleting file or folder info
        /// </summary>
        /// <param name="path">File or folder path</param>
        /// <example>DELETE api/FileSystem/Files/test.txt</example>
        /// <returns></returns>
        [ExclusiveAction]
        public HttpResponseMessage Delete(string path)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ErrorMessages.DeleteRootFolderErrorMessage)
                };
                throw new HttpResponseException(response);
            }
            var fullPath = _rootFolderPath;
            try
            {
                fullPath = new Uri($"{fullPath}\\{path?.TrimEnd('/')}").LocalPath;
            }
            catch (UriFormatException)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ErrorMessages.InvalidFilePathFormatErrorMessage)
                };
                throw new HttpResponseException(response);
            }

            try
            {
                _fileService.DeleteFileOrFolder(fullPath);
                return new HttpResponseMessage(HttpStatusCode.Accepted);
            }
            catch (PathNotExistsException)
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(ErrorMessages.NoFileOrFolederErrorMessage)
                };
                throw new HttpResponseException(response);
            }
            catch (ArgumentException)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ErrorMessages.InvalidFilePathFormatErrorMessage)
                };
                throw new HttpResponseException(response);
            }
            catch (UnauthorizedAccessException e)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    Content = new StringContent(ErrorMessages.AccessDeniedErrorMessage)
                };
                throw new HttpResponseException(response);
            }
            catch (Exception)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ErrorMessages.InternalServerErrorMessage)
                };
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// API method for creating file
        /// </summary>
        /// <param name="path">Folder path where file should be created</param>
        /// <param name="fileInfo"></param>
        /// <example>POST api/FileSystem/Files
        /// Request Body:
        /// {
        ///   "name": "test1.txt",
        ///   "content": "YXNkZg=="
        /// }
        /// </example>
        /// <returns></returns>
        [ExclusiveAction]
        public HttpResponseMessage Post(string path, [FromBody]CreateOrUpdateFileViewModel fileInfo)
        {
            if (fileInfo == null)
            {
                var response = new HttpResponseMessage((HttpStatusCode)422)
                {
                    Content = new StringContent(ErrorMessages.WrongRequestBodyErrorMessage)
                };
                throw new HttpResponseException(response);
            }
            var fullPath = _rootFolderPath;
            try
            {
                fullPath = new Uri($"{fullPath}\\{path?.TrimEnd('/')}").LocalPath;
            }
            catch (UriFormatException)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ErrorMessages.InvalidFilePathFormatErrorMessage)
                };
                throw new HttpResponseException(response);
            }

            try
            {
                var model = (CreateOrUpdateFileModel) Mapper.Map(fileInfo, typeof (CreateOrUpdateFileViewModel),
                    typeof (CreateOrUpdateFileModel));
                _fileService.CreateFile(fullPath, model);

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (PathNotExistsException)
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(ErrorMessages.NoFileOrFolederErrorMessage)
                };
                throw new HttpResponseException(response);
            }
            catch (ArgumentException)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                throw new HttpResponseException(response);
            }
            catch (FileAlreadyExistsException e)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Conflict)
                {
                    Content = new StringContent(ErrorMessages.FileAlreadyExistsErrorMessage)
                };
                throw new HttpResponseException(response);
            }
            catch (UnauthorizedAccessException e)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    Content = new StringContent(ErrorMessages.AccessDeniedErrorMessage)
                };
                throw new HttpResponseException(response);
            }
            catch (Exception e)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ErrorMessages.InternalServerErrorMessage)
                };
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// API method for updating file. If file not exists, it will be created
        /// </summary>
        /// <param name="path">File location from base URI</param>
        /// <param name="fileInfo">file name and content in JSON format</param>
        /// <example>POST api/FileSystem/Files
        /// Request Body:
        /// {
        ///   "name": "test1.txt",
        ///   "content": "YXNkZg=="
        /// }
        /// </example>
        /// <returns></returns>
        [HttpPut]
        [ExclusiveAction]
        public HttpResponseMessage Put(string path, [FromBody]CreateOrUpdateFileViewModel fileInfo)
        {
            if (fileInfo == null || string.IsNullOrWhiteSpace(fileInfo.Name))
            {
                var response = new HttpResponseMessage((HttpStatusCode)422)
                {
                    Content = new StringContent(ErrorMessages.WrongRequestBodyErrorMessage)
                };
                throw new HttpResponseException(response);
            }
            var fullPath = _rootFolderPath;
            try
            {
                fullPath = new Uri($"{fullPath}\\{path?.TrimEnd('/')}").LocalPath;
            }
            catch (UriFormatException)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ErrorMessages.InvalidFilePathFormatErrorMessage)
                };
                throw new HttpResponseException(response);
            }

            try
            {
                var model = (CreateOrUpdateFileModel)Mapper.Map(fileInfo, typeof(CreateOrUpdateFileViewModel),
                    typeof(CreateOrUpdateFileModel));
                bool isCreated;
                _fileService.UpdateOrCreateFile(fullPath, model, out isCreated);

                if (isCreated)
                {
                    return new HttpResponseMessage(HttpStatusCode.Created);
                }
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (ArgumentException)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                throw new HttpResponseException(response);
            }
            catch (UnauthorizedAccessException e)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    Content = new StringContent(ErrorMessages.AccessDeniedErrorMessage)
                };
                throw new HttpResponseException(response);
            }
            catch (Exception e)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ErrorMessages.InternalServerErrorMessage)
                };
                throw new HttpResponseException(response);
            }
        }
    }
}
