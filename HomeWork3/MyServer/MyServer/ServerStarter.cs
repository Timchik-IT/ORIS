using System.Net;

namespace MyServer;

public class ServerStarter
{
    private void FileChecker()
    {
        //проверка на наличие папки static
        if (Directory.GetDirectories(Directory.GetCurrentDirectory())
                .FirstOrDefault(x => x.Split('/')[^1] == Configurator.StaticFilePath) == null)
        {
            Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/{Configurator.StaticFilePath}");
        }
    
        //проверка на наличие index.html
        if (Directory.GetFiles(Directory.GetCurrentDirectory() + $"/{Configurator.StaticFilePath}")
                .FirstOrDefault(x => x.Split('/')[^1] == "index.html") == null)
        {
            throw new FileNotFoundException();
        }
    }
    
    public void ServerStart()
    {
        HttpListener server = new HttpListener();
        
        try
        {
            FileChecker();
    
            server.Prefixes.Add($"http://{Configurator.Address}:{Configurator.Port}/");
            server.Start(); // начинаем прослушивать входящие подключения
            Console.WriteLine("Server start");
            
            while (server.IsListening)
            {
                // получаем контекст
                var context = server.GetContext();
                var response = context.Response;
                
                var url = context.Request.Url?.AbsolutePath;
                Console.WriteLine($"url is: {url}");

                if (url?.Split(".")[^1] == "html" && Directory.GetFiles($"{Configurator.StaticFilePath}/")
                        .FirstOrDefault(x => x.Split('/')[^1] == url.Trim('/')) == null)
                {
                    url = "/404.html";
                }
                
                // отправляемый в ответ код htmlвозвращает
                byte[] readFile = File.ReadAllBytes($"{Configurator.StaticFilePath}//{(url is "" or "/" ? "index.html" : url)}");
        
                // получаем поток ответа и пишем в него ответ
                response.ContentLength64 = readFile.Length;
                using Stream output = response.OutputStream;
                
                // отправляем данные
                output.WriteAsync(readFile); 
                output.FlushAsync();    
            }
    
            Console.WriteLine("Запрос обработан");
            server.Stop();
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