using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSystemAccess.Models;
using Newtonsoft.Json;

namespace FileSystemAccess.Converters
{
    public class ContentConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var fileContent = value as IFileSystemItemContentViewModel;
            if (fileContent != null)
            {
                var type = fileContent.Content.GetType();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(writer, fileContent.Content, type);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IFileSystemItemContentViewModel);
        }
    }
}
