using System.Collections.Generic;

namespace FileSystemAccess.Domain.Models
{
    public class FolderContentModel : IFileSystemItemContentModel
    {
        public IList<FileSystemItemModelBase> ContentValue { get; set; }

        object IFileSystemItemContentModel.Content
        {
            get { return this.ContentValue; }
            set { this.ContentValue = (IList<FileSystemItemModelBase>)value; }
        }

        public FolderContentModel()
        {
            ContentValue = new List<FileSystemItemModelBase>();
        }

        public FolderContentModel(List<FileSystemItemModelBase> content)
        {
            ContentValue = content;
        }
    }
}
