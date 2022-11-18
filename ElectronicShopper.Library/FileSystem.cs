namespace ElectronicShopper.Library;

public class FileSystem : IFileSystem
{
    

    public async Task Save(string path, Stream stream)
    {
        await using var fileStream = File.Create(path);
       stream.Position = 0;
       await stream.CopyToAsync(fileStream);
    }


    public void DeleteDirectory(string path, bool recursive = false)
    {
        Directory.Delete(path, recursive);
    }

    public void DeleteFile(string path)
    {
        File.Delete(path);
    }

    public void CreateDirectory(string path)
    {
        Directory.CreateDirectory(path);
    }

    public bool Exists(string path)
    {
        return Directory.Exists(path) || File.Exists(path);
    }
}