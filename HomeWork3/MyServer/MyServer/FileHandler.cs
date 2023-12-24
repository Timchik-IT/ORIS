namespace MyServer;

public static class FileHandler
{
    public static void FileCheck()
    {
        //проверка на наличие папки static
        if (Directory.GetDirectories(Directory.GetCurrentDirectory())
                .FirstOrDefault(x => x.Split('/')[^1] == Configurator.StaticFilePath) == null)
        {
            Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/{Configurator.StaticFilePath}");
        }
    
        //проверка на наличие index.html в каждой папке
        if (Directory.GetDirectories(Configurator.StaticFilePath).Any(directory => Directory.GetFiles($"{directory}/")
                .FirstOrDefault(x => x.Split('/')[^1] == "index.html") == null))
        {
            throw new FileNotFoundException("index.html");
        }
    }
}