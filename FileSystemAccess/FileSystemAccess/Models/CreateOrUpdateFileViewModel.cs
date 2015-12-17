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
    public class CreateOrUpdateFileViewModel
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
