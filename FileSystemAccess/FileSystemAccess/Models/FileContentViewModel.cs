using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAccess.Models
{
    public class FileContentViewModel : IFileSystemItemContentViewModel
    {
        public string ContentValue { get; set; }

        object IFileSystemItemContentViewModel.Content
        {
            get { return this.ContentValue; }
            set { this.ContentValue = (string)value; }
        }
        public FileContentViewModel()
        {
        }

        public FileContentViewModel(string content)
        {
            ContentValue = content;
        }
    }
}
