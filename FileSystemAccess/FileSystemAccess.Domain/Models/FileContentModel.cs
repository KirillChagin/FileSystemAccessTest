namespace FileSystemAccess.Domain.Models
{
    public class FileContentModel : IFileSystemItemContentModel
    {
        public string ContentValue { get; set; }

        object IFileSystemItemContentModel.Content
        {
            get { return this.ContentValue; }
            set { this.ContentValue = (string)value; }
        }
        public FileContentModel()
        {
        }

        public FileContentModel(string content)
        {
            ContentValue = content;
        }
    }
}
