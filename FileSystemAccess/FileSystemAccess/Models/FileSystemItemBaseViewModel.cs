using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using FileSystemAccess.Converters;
using Newtonsoft.Json;

namespace FileSystemAccess.Models
{
    [KnownType(typeof(FileContentViewModel))]
    [KnownType(typeof(FolderContentViewModel))]
    public class FileSystemItemBaseViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parentPath")]
        public string ParentPath { get; set; }

        [JsonProperty("isFolder")]
        public bool IsFolder { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("content")]
        [JsonConverter(typeof(ContentConverter))]
        public IFileSystemItemContentViewModel Content { get; set; }
    }
}
