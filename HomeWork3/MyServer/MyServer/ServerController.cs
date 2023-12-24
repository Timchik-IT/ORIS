using System.Net;

namespace MyServer;

public class ServerController
{
    private readonly HttpListener _server;

    public ServerController()
    {
        _server = new HttpListener();
        _server.Prefixes
            .Add($"http://{Configurator.Address}:{Configurator.Port}/");
    }
    
    public void ServerStart()
    {
        _server.Start();
        ServerLifeCycle();
        _server.Stop();
    }

    private void ServerLifeCycle()
    {
        while (_server.IsListening)
        {
            var context = _server.GetContext();
            var url = UrlReader(context);

            var response = context.Response;
            var request = context.Request;
            
            UpdateServer(response, url);
        }
    }

    private async void UpdateServer(HttpListenerResponse response, string url)
    {
        var file = await File.ReadAllBytesAsync($"{Configurator.StaticFilePath}/{url}");

        response.ContentLength64 = file.Length;
        await using var output = response.OutputStream;
        await output.WriteAsync(file);
        await output.FlushAsync();
    }
    
    private string UrlReader(HttpListenerContext context)
    {
        var URL = context.Request.Url?.AbsolutePath.TrimEnd('/');

        if (URL == null) throw new ArgumentNullException(URL);

        if (URL.Split('/')[^1] == "html")
        {
            if (Directory.GetFiles($"{Configurator.StaticFilePath}/{URL.Split('/')[^2]}")
                    .FirstOrDefault(x => x.Split('/')[^1] == URL.Split('/')[^1]) == null)
            {
                URL = $"/{URL.Split('/')[^2]}" + "/404.html";
            }
        }
        else if (Directory.GetDirectories(Configurator.StaticFilePath)
                     .FirstOrDefault(x => x.Split('/')[^1] == URL.Split('/')[^1]) != null)
        {
                URL += "/index.html";
        }
        
        return URL;
    }
}