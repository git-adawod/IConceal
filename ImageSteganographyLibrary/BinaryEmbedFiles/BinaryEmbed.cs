using System;
using System.IO;

namespace ImageSteganographyLibrary
{
    public class BinaryEmbed
    {
        public static string StandardEmbed(FileInfo CarrierImage, FileInfo ZipFile, bool? DeleteOriginals)
        {
            string outputPath = $@"{CarrierImage.DirectoryName}\IConceal - {CarrierImage.Name}";

            try
            {
                //Copy the image
                File.Copy(CarrierImage.FullName, outputPath, true);

                using(FileStream SteganoImage = new FileStream(outputPath, FileMode.Append))
                {
                    using(FileStream ZipStream = new FileStream(ZipFile.FullName, FileMode.Open, FileAccess.Read))
                    {
                        byte[] bytes = new byte[32 * 1024];
                        int block;

                        while((block = ZipStream.Read(bytes, 0, bytes.Length)) > 0)
                        {
                            SteganoImage.Write(bytes, 0, block);
                        }
                    }
                }

                if(DeleteOriginals == true)
                {
                    File.Delete(CarrierImage.FullName);
                }

                File.Delete(ZipFile.FullName);

                return outputPath;
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }

        public static string EmbedWithCommandPrompt(FileInfo CarrierImage, FileInfo ZipFile, bool? DeleteOriginals)
        {
            try
            {
                string outputPath = $@"{CarrierImage.DirectoryName}\IConceal - {CarrierImage.Name}";
                string command = $" /C copy /b {CarrierImage.FullName} + {ZipFile.FullName} + {outputPath}";

                System.Diagnostics.Process.Start("CMD.exe", command);

                if (DeleteOriginals == true)
                {
                    File.Delete(CarrierImage.FullName);
                }

                File.Delete(ZipFile.FullName);

                return outputPath;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
