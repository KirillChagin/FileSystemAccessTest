using System;

namespace FileSystemAccess.Domain.Exceptions
{
    public class PathNotExistsException : Exception
    {
        public PathNotExistsException()
        {
        }

        public PathNotExistsException(string message)
            : base(message)
        {
        }

        public PathNotExistsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
