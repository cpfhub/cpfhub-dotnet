# CPFHub

**Official .NET SDK for [CPFHub.io](https://cpfhub.io) — Brazilian CPF Lookup API**

> SDK oficial .NET para a [CPFHub.io](https://cpfhub.io) — API de consulta de CPF

[![NuGet Version](https://img.shields.io/nuget/v/CPFHub)](https://www.nuget.org/packages/CPFHub)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

---

## What is CPFHub.io?

CPFHub.io is a REST API that returns name, gender, and date of birth from any Brazilian CPF number — in ~300ms, with 99.9% uptime, and full LGPD compliance.

> CPFHub.io é uma API REST que retorna nome, gênero e data de nascimento a partir de qualquer CPF brasileiro — em ~300ms, com 99,9% de uptime e total conformidade com a LGPD.

**10M+ CPFs queried · 1,300+ active companies · 99.9% uptime**

---

## Installation / Instalação

```bash
dotnet add package CPFHub
```

Or via the NuGet Package Manager:

```
Install-Package CPFHub
```

---

## Quick Start

```csharp
using CPFHub;

var client = new CPFHubClient("YOUR_API_KEY");

var result = await client.LookupAsync("00000000000");

Console.WriteLine(result.Name);      // "Fulano de Tal"
Console.WriteLine(result.Gender);    // "M"
Console.WriteLine(result.BirthDate); // "15/06/1990"
```

Get your free API key at [app.cpfhub.io](https://app.cpfhub.io) — no credit card required.

> Obtenha sua chave gratuita em [app.cpfhub.io](https://app.cpfhub.io) — sem cartão de crédito.

---

## API Reference

### `new CPFHubClient(string apiKey, CPFHubOptions? options = null)`

```csharp
var options = new CPFHubOptions
{
    Timeout = TimeSpan.FromSeconds(5),
    BaseUrl = "https://api.cpfhub.io"
};

var client = new CPFHubClient("YOUR_API_KEY", options);
```

### `client.LookupAsync(string cpf, CancellationToken ct = default) → Task<CPFResult>`

Accepts CPF with or without formatting (`000.000.000-00` or `00000000000`).

#### `CPFResult` properties

| Property | Type | Description |
|----------|------|-------------|
| `Cpf` | `string` | CPF number (digits only) |
| `Name` | `string` | Full name — `"Fulano de Tal"` |
| `NameUpper` | `string` | Full name in uppercase |
| `Gender` | `string` | `"M"` or `"F"` |
| `BirthDate` | `string` | Date of birth — `"DD/MM/YYYY"` |
| `Day` | `int` | Birth day |
| `Month` | `int` | Birth month |
| `Year` | `int` | Birth year |

---

## Error Handling

```csharp
using CPFHub;
using CPFHub.Exceptions;

var client = new CPFHubClient("YOUR_API_KEY");

try
{
    var result = await client.LookupAsync("00000000000");
    Console.WriteLine(result.Name);
}
catch (CPFHubException ex)
{
    Console.WriteLine($"Error {ex.StatusCode}: {ex.Message}");
    // 400 — Invalid CPF format
    // 401 — Invalid or missing API key
    // 404 — CPF not found
    // 429 — Rate limit exceeded
    // 500 — Server error
    // 503 — Service temporarily unavailable
}
```

---

## Examples

### ASP.NET Core — Dependency Injection

```csharp
// Program.cs
builder.Services.AddCPFHub(options =>
{
    options.ApiKey = builder.Configuration["CPFHub:ApiKey"];
});
```

```csharp
// OnboardingController.cs
[ApiController]
[Route("[controller]")]
public class OnboardingController : ControllerBase
{
    private readonly ICPFHubClient _cpfHub;

    public OnboardingController(ICPFHubClient cpfHub)
    {
        _cpfHub = cpfHub;
    }

    [HttpGet("{cpf}")]
    public async Task<IActionResult> Verify(string cpf, CancellationToken ct)
    {
        var result = await _cpfHub.LookupAsync(cpf, ct);
        return Ok(new { result.Name, result.Gender });
    }
}
```

```json
// appsettings.json
{
  "CPFHub": {
    "ApiKey": "YOUR_API_KEY"
  }
}
```

### Minimal API

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCPFHub(o => o.ApiKey = builder.Configuration["CPFHub:ApiKey"]);

var app = builder.Build();

app.MapGet("/cpf/{cpf}", async (string cpf, ICPFHubClient client) =>
{
    var result = await client.LookupAsync(cpf);
    return Results.Ok(result);
});

app.Run();
```

### F#

```fsharp
open CPFHub

let client = CPFHubClient("YOUR_API_KEY")

let result = client.LookupAsync("00000000000") |> Async.AwaitTask |> Async.RunSynchronously

printfn "%s" result.Name
```

---

## Rate Limits / Limites de Requisição

| Plan / Plano | Limit / Limite |
|---|---|
| Free / Grátis | 1 request every 2 seconds · 50 requests/month |
| Pro | 1 request per second · 1,000 requests/month |
| Corporate / Corporativo | Custom / Personalizado |

The SDK automatically retries on `429` with exponential backoff (up to 3 attempts).

---

## Plans & Pricing / Planos e Preços

| Plan | Price | Included | Extra |
|------|-------|----------|-------|
| **Free** | R$ 0/month | 50 lookups | — |
| **Pro** | R$ 149/month | 1,000 lookups | R$ 0,15/lookup |
| **Corporate** | Custom | Custom | Custom |

[View full pricing at cpfhub.io →](https://cpfhub.io#pricing)

---

## Requirements / Requisitos

- .NET 6+
- Compatible with C#, F#, and VB.NET

---

## Links

- [Documentation / Documentação](https://cpfhub.io/documentacao)
- [Dashboard / Painel](https://app.cpfhub.io)
- [NuGet Package](https://www.nuget.org/packages/CPFHub)
- [Status Page](https://app.cpfhub.io/status)
- [LGPD Compliance](https://cpfhub.io/lgpd)

---

## License / Licença

MIT © [CPFHub.io](https://cpfhub.io)
