using System;
using System.IO.Compression;
using MyCloud.Models;
using NLog.Web.LayoutRenderers;

namespace MyCloud.Helpers
{
    public class ZipHelper
    {
        public static void CreateZip(string fileName, string userName)
        {
            string rootPath = "D:\\MyCloud\\MyCloud\\wwwroot\\Files\\" + userName;
            string path = rootPath + "\\toZip";
            string filePath = rootPath + "\\" + fileName;
            Directory.CreateDirectory(path);
            File.Move(filePath, path + "\\" + fileName);
            string zipPath = filePath + ".zip";
            ZipFile.CreateFromDirectory(path, zipPath);
            Directory.Delete(path, true);
        }

        public static void ExtractZip(string fileName, string userName)
        {
            string rootPath = "D:\\MyCloud\\MyCloud\\wwwroot\\Files\\" + userName;
            string path = rootPath + "\\toExtract";
            string filePath = rootPath + "\\" + fileName;
            ZipFile.ExtractToDirectory(filePath + ".zip", path);
            File.Move(rootPath + "\\toExtract\\" + fileName, filePath);
            Directory.Delete(path, true);
            File.Delete(filePath + ".zip");
        }
    }
}
