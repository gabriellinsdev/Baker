using System.Web.Http;

public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        // Configuração do CORS
        var cors = new System.Web.Http.Cors.EnableCorsAttribute("http://127.0.0.1:5500", "*", "*");
        config.EnableCors(cors);

        // Outras configurações da Web API...
    }
}
