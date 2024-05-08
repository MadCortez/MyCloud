using MyCloud.Models;

namespace MyCloud.Helpers
{
    public static class GetFilesHelper
    {
        public static List<FileData> GetUserFiles(string path)
        {
            var directory = new DirectoryInfo(path);
            List<FileData> returnFiles = new List<FileData>();
            if (directory.Exists)
            {
                var files = directory.GetFiles();
                foreach (var file in files)
                {
                    FileData toAdd = new FileData();
                    toAdd.FileName = file.Name.Remove(file.Name.Length - 4, 4);
                    toAdd.Path = file.ToString();
                    toAdd.Weight = file.Length;
                    toAdd.CreateDate = file.LastAccessTime;
                    toAdd.AllMemory = file.Length;
                    returnFiles.Add(toAdd);
                }
            }
            return returnFiles;
        }
    }
}
