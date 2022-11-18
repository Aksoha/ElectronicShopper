namespace ElectronicShopper.Library;

public interface IFileSystem
{
    
    /// <param name="path">The path and name of the file to create.</param>
    /// <param name="stream">The stream to which the contents of the current stream will be copied.</param>
    /// <inheritdoc cref="File.Create(string)" path="/exception"/>
    Task Save(string path, Stream stream);
    
    
    /// <inheritdoc cref="Directory.Delete(string, bool)"/>
    void DeleteDirectory(string path, bool recursive = false);
    
    
    /// <inheritdoc cref="File.Delete"/>
    void DeleteFile(string path);

    
    /// <inheritdoc cref="Directory.CreateDirectory"/>
    void CreateDirectory(string path);

    
    /// <summary>
    /// Determines whether the given path refers to an existing directory or file.
    /// </summary>
    /// <param name="path">Path to test.</param>
    /// <returns><see langword="true"/> if given path or directory exists, otherwise <see langword="false"/> .</returns>
    bool Exists(string path);
}