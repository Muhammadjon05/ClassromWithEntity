    namespace Full_Identity.Helper;

public class FileHelper
{
    private const string WwwRoot = "wwwroot";

    private static void CheckPath(string folder)
    {
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
    }

    public static async Task<string> SaveUserFile(IFormFile file)
    {
        return await SaveFile(file, "UserPhotos");
    }

    public static async Task<string> SaveSchoolFile(IFormFile file)
    {
        return await SaveFile(file, "SchoolPhotos");
    }   
    private static async Task<string> SaveFile(IFormFile file, string folder)
    {
        CheckPath(Path.Combine(WwwRoot,folder));
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

        var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        await File.WriteAllBytesAsync($"{WwwRoot}//{folder}//{fileName}", ms.ToArray());
        return $"/{folder}/{fileName}";
    }
}