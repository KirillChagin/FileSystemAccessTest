using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSystemAccess.FileSystemServiceContract;
using FileSystemAccess.FileSystemService;
using Microsoft.Practices.Unity;

namespace FileSystemAccess.Bootstrapper
{
    public class Bootstrapper
    {
        /// <summary>
        /// Services Registration
        /// </summary>
        /// <param name="container"></param>
        public static void Initialize(IUnityContainer container)
        {
            container.RegisterType<IFileService, FileService>();
        }
    }
}
