using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SKinEditer.util
{
    class FileUtil
    {

        //開啟檔案所在資料夾，並選中
        public static void OpenFolderAndSelectFile(String fileFullName)
        {
            ProcessStartInfo psi = new ProcessStartInfo("Explorer.exe");
            psi.Arguments = "/e,/select," + fileFullName;
            Process.Start(psi);
        }
        //開啟檔案所在資料夾
        public static void OpenFolder(String folderName)
        {
            ProcessStartInfo psi = new ProcessStartInfo("Explorer.exe");
            psi.Arguments = folderName;
            Process.Start(psi);
        }

        public static void OpenFile(string filePath) {

            Process.Start(filePath);
        }

        public static string GetParentDirectoryPath( string folderPath, int levels)
        {
            string result = folderPath;
            for (int i = 0; i < levels; i++)
            {
                if (Directory.GetParent(result) != null)
                {
                    result = Directory.GetParent(result).FullName;
                }
                else
                {
                    return result;
                }
            }
            return result;
        }

        //檢查看裡面有沒有一個src的資料夾，如果有則表示是android application
        public static Boolean CheckIsAndroidApplication(string targetDirectory)
        {
            string[] src = Directory.GetDirectories(targetDirectory, "src", System.IO.SearchOption.TopDirectoryOnly);

            return src.Length > 0;

        }

        public static string MakeFoldernameValid(string foldername)
        {
            if (foldername == null)
                throw new ArgumentNullException();

            if (foldername.EndsWith("."))
                foldername = Regex.Replace(foldername, @"\.+$", "");

            if (foldername.Length == 0)
                throw new ArgumentException();

            if (foldername.Length > 245)
                throw new PathTooLongException();

            foreach (char c in System.IO.Path.GetInvalidPathChars())
            {
                foldername = foldername.Replace(c, '_');
            }

            return foldername;
        }

    }
}
