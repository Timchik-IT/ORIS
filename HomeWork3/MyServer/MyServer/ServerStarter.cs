using System.Net;
using System.Xml.Serialization;

namespace MyServer;

public class ServerStarter
{
    public void ServerStart()
    {
        try
        {
            FileHandler.FileCheck();
            Console.WriteLine("Files checked");

            var server = new ServerController();
            server.ServerStart();
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine("File not found!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            Console.WriteLine("Server stop!");
        }
    }
}