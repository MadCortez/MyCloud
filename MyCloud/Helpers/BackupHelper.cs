using System.IO.Compression;

public class BackupHelper
{
    public void CreateBackup(string sourceDirectory, string backupDirectory)
    {
        sourceDirectory = "\\MyCloud\\MyCloud\\" + sourceDirectory;
        backupDirectory = "\\MyCloud\\MyCloud\\" + backupDirectory;
        string backupFileName = $"backup_{DateTime.Now:yyyyMMddHHmmss}.zip";
        string backupFilePath = Path.Combine(backupDirectory, backupFileName);

        try
        {
            ZipFile.CreateFromDirectory(sourceDirectory, backupFilePath);
            Console.WriteLine("Резервная копия успешно создана: " + backupFilePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при создании резервной копии: " + ex.Message);
        }
    }
}