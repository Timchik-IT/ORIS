using System.Text.Json;
using MyServer.Configuration;

namespace MyServer;


public class Configurator
{
    private static readonly Configurator instance = new Configurator();
    
    private Appsettings Configuration { get; set; }
    
    public static string Address { get; private set; }
    public static int Port { get; private set; }
    public static string StaticFilePath { get; private set; }
    
    private Configurator()
    {
        Configuration = GetConfig();
        Address = Configuration.Address;
        Port = Configuration.Port;
        StaticFilePath = Configuration.StaticFilePath;
    }

    private static Appsettings GetConfig()
    {
        using var file = File.OpenRead("appsettings.json");
        Console.WriteLine("Deserialize done");
        return JsonSerializer.Deserialize<Appsettings>(file) ?? throw new Exception();
    }
}