using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Standard.DynMvp.Base
{
    public class FileHelper
    {
        public static void SafeSave(string srcFileName, string bakFileName, string destFileName)
        {
            File.Delete(bakFileName);

            if (File.Exists(destFileName) == true)
                File.Move(destFileName, bakFileName);

            if (File.Exists(srcFileName) == true)
                File.Move(srcFileName, destFileName);
        }

        public static void Move(string srcFileName, string destFileName)
        {

            if (File.Exists(destFileName) == true)
                File.Delete(destFileName);

            if (File.Exists(srcFileName) == true)
                File.Move(srcFileName, destFileName);
        }

        public static void ClearFolder(string folderName, params string[] excludeFileNames)
        {
            if (Directory.Exists(folderName) == false)
                return;

            DirectoryInfo dir = new DirectoryInfo(folderName);

            foreach (FileInfo fi in dir.GetFiles())
            {
                bool exist = excludeFileNames != null && Array.Exists(excludeFileNames, f => f == fi.Name);
                if (exist == false)
                {
                    fi.IsReadOnly = false;
                    fi.Delete();
                }
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                ClearFolder(di.FullName);
                di.Delete();
            }
        }

        public static void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs, bool overrite)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            //if (!dir.Exists)
            //{
            //    throw new DirectoryNotFoundException(
            //        "Source directory does not exist or could not be found: "
            //        + sourceDirName);
            //}

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                if (File.Exists(tempPath) == true && overrite == false)
                    continue;

                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    CopyDirectory(subdir.FullName, temppath, copySubDirs, overrite);
                }
            }
        }

        public static bool CopyFile(string srcCommonFile, string dstCommonFile, bool overWrite)
        {
            if (File.Exists(srcCommonFile) == false)
                return false;

            try
            {
                File.Copy(srcCommonFile, dstCommonFile, overWrite);
                return true;
            }
            catch (IOException)
            { return false; }
        }
    }
}
