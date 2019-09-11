using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSteganographyLibrary
{
    public struct BinaryEmbedStruct
    {
        public FileInfo Image;
        public List<FileInfo> Files;
        public DirectoryInfo Directory;
        public FileInfo ZipFileLocation;
    }
}
