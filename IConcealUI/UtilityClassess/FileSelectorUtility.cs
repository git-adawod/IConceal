using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace IConcealUI
{
    class FileSelectorUtility
    {
        public static List<FileInfo> GetFiles()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All files (*.*)|*.*";
            ofd.Title = "Select File(s)";
            ofd.InitialDirectory = @"C:\";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                List<FileInfo> files = new List<FileInfo>();
                foreach (string path in ofd.FileNames)
                {
                    files.Add(new FileInfo(path));
                }
                return files;
            }

            return null;
        }

        public static FileInfo GetImage()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "png, jpg, jpeg|*.png;*.jpg;*.jpeg";
            ofd.Title = "Select an Image";
            ofd.InitialDirectory = @"C:\";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                return new FileInfo(ofd.FileName);
            }

            return null;
        }

        public static DirectoryInfo GetDirectory()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                return new DirectoryInfo(fbd.SelectedPath);
            }

            return null;
        }
    }
}
