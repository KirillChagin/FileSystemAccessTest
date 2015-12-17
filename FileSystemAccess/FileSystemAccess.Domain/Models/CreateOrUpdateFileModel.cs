using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAccess.Domain.Models
{
    /// <summary>
    /// Represents a file info entity for creating or updating file
    /// </summary>
    public class CreateOrUpdateFileModel
    {
        /// <summary>
        /// File name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Base64 string of file content
        /// </summary>
        public string Content { get; set; }
    }
}
