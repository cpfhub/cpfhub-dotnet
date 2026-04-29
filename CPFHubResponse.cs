using System.Text.Json.Serialization;

namespace CPFHub;

public record CPFHubResponse
{
    [JsonPropertyName("cpf")]
    public string Cpf { get; init; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("nameUpper")]
    public string NameUpper { get; init; } = string.Empty;

    [JsonPropertyName("gender")]
    public string Gender { get; init; } = string.Empty;

    [JsonPropertyName("birthDate")]
    public string BirthDate { get; init; } = string.Empty;

    [JsonPropertyName("day")]
    public int Day { get; init; }

    [JsonPropertyName("month")]
    public int Month { get; init; }

    [JsonPropertyName("year")]
    public int Year { get; init; }
}
