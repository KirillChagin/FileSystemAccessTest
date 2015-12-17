using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemAccess.Models
{
    public class FolderContentViewModel : IFileSystemItemContentViewModel
    {
        public IList<FileSystemItemBaseViewModel> ContentValue { get; set; }

        object IFileSystemItemContentViewModel.Content
        {
            get { return this.ContentValue; }
            set { this.ContentValue = (IList<FileSystemItemBaseViewModel>)value; }
        }

        public FolderContentViewModel()
        {
        }

        public FolderContentViewModel(List<FileSystemItemBaseViewModel> content)
        {
            ContentValue = content;
        }
    }
}
