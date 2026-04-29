using Microsoft.Extensions.DependencyInjection;

namespace CPFHub;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCPFHub(this IServiceCollection services, string apiKey)
    {
        services.AddHttpClient<ICPFHubClient, CPFHubClient>(client =>
        {
            // Configuration is handled in the constructor
        })
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            // Optional: Add custom handler configuration here
        });

        services.AddSingleton<ICPFHubClient>(sp => 
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(nameof(ICPFHubClient));
            return new CPFHubClient(httpClient, apiKey);
        });

        return services;
    }
}
