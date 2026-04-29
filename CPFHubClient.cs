using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace CPFHub;

public interface ICPFHubClient
{
    Task<CPFHubResponse?> LookupAsync(string cpf, CancellationToken cancellationToken = default);
}

public class CPFHubClient : ICPFHubClient
{
    private readonly HttpClient _httpClient;
    private static readonly Regex CpfRegex = new(@"\D", RegexOptions.Compiled);

    public CPFHubClient(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.cpfhub.io/v1/");
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "CPFHub-DotNet-SDK/1.0.0");
    }

    public async Task<CPFHubResponse?> LookupAsync(string cpf, CancellationToken cancellationToken = default)
    {
        var cleanCpf = CpfRegex.Replace(cpf, "");

        if (cleanCpf.Length != 11)
        {
            throw new ArgumentException("Invalid CPF format. Must have 11 digits.", nameof(cpf));
        }

        var response = await _httpClient.GetAsync($"cpf/{cleanCpf}", cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CPFHubResponse>(cancellationToken: cancellationToken);
        }

        return response.StatusCode switch
        {
            System.Net.HttpStatusCode.NotFound => null,
            System.Net.HttpStatusCode.Unauthorized => throw new UnauthorizedAccessException("Invalid or missing API key."),
            System.Net.HttpStatusCode.BadRequest => throw new ArgumentException("Invalid CPF format."),
            _ => throw new HttpRequestException($"CPFHub API returned an error: {response.StatusCode}")
        };
    }
}
