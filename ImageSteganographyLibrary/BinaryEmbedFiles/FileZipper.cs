using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;

namespace ImageSteganographyLibrary
{
    public class FileZipper
    {
        public static string ArchiveFiles(List<FileInfo> files, string password)
        {
            try
            {
                string outputPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                ZipFile fileZipper = new ZipFile(outputPath);

                if (!string.IsNullOrEmpty(password)) fileZipper.Password = password;


                foreach (FileInfo fi in files)
                {
                    fileZipper.AddFile(fi.FullName, "IConceal");
                }

                fileZipper.Save();

                return outputPath;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string ArchiveDirectory(DirectoryInfo directory, string password)
        {
            try
            {
                string outputPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                ZipFile fileZipper = new ZipFile(outputPath);

                if (!string.IsNullOrEmpty(password)) fileZipper.Password = password;

                fileZipper.AddDirectory(directory.FullName, "IConceal");

                fileZipper.Save();

                return outputPath;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
