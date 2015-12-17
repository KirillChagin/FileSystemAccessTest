using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FileSystemAccess.Domain.Models;
using FileSystemAccess.Models;

namespace FileSystemAccess
{
    public class AutoMapperConfig
    {
        /// <summary>
        /// Mappings registration
        /// </summary>
        public static void RegisterMappings()
        {
            Mapper.CreateMap<FileSystemItemModelBase, FileSystemItemBaseViewModel>();
            Mapper.CreateMap<FileContentModel, FileContentViewModel>().ForMember(fvm => fvm.ContentValue, f => f.MapFrom(src => src.ContentValue));
            Mapper.CreateMap<FolderContentModel, FolderContentViewModel>().ForMember(fvm => fvm.ContentValue, f => f.MapFrom(src => src.ContentValue));
            Mapper.CreateMap<IFileSystemItemContentModel, IFileSystemItemContentViewModel>();
            Mapper.CreateMap<CreateOrUpdateFileViewModel, CreateOrUpdateFileModel>();
        }
    }
}
