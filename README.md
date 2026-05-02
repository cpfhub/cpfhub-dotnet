# cpfhub-dotnet: .NET SDK for CPFHub.io

🇺🇸 **English** | [🇧🇷 Português](#português)

**Official .NET SDK for [CPFHub.io](https://cpfhub.io) — Brazilian CPF Lookup API**

[![NuGet](https://img.shields.io/nuget/v/CPFHub)](https://www.nuget.org/packages/CPFHub)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

---

## What is CPFHub.io?

CPFHub.io is a REST API that returns name, gender, and date of birth from any Brazilian CPF number — in ~300ms, with 99.9% uptime and full LGPD compliance.

**10M+ CPFs queried · 1,300+ active companies · 99.9% uptime**

---

## Installation

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

---

## curl Example

```bash
curl -X GET "https://api.cpfhub.io/cpf/12345678909" \
  -H "x-api-key: YOUR_API_KEY"
```

**Response:**

```json
{
  "success": true,
  "data": {
    "cpf": "12345678909",
    "name": "Fulano de Tal",
    "nameUpper": "FULANO DE TAL",
    "gender": "M",
    "birthDate": "15/06/1990",
    "day": 15,
    "month": 6,
    "year": 1990
  }
}
```

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

Looks up a CPF and returns the associated identity data.

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

## Rate Limits

| Plan | Limit |
|---|---|
| Free | 1 request every 2 seconds · 50 requests/month |
| Pro | 1 request per second · 1,000 requests/month |
| Corporate | Custom |

The SDK automatically retries on `429` with exponential backoff (up to 3 attempts).

---

## Plans & Pricing

| Plan | Price | Included | Extra |
|------|-------|----------|-------|
| **Free** | R$ 0/month | 50 lookups | — |
| **Pro** | R$ 149/month | 1,000 lookups | R$ 0,15/lookup |
| **Corporate** | Custom | Custom | Custom |

[View full pricing at cpfhub.io →](https://cpfhub.io#pricing)

---

## Requirements

- .NET 6+
- Compatible with C#, F#, and VB.NET

---

## Links

- [Documentation](https://cpfhub.io/documentacao)
- [Dashboard](https://app.cpfhub.io)
- [NuGet Package](https://www.nuget.org/packages/CPFHub)
- [Status Page](https://app.cpfhub.io/status)
- [Pricing](https://cpfhub.io#pricing)
- [LGPD Compliance](https://cpfhub.io/lgpd)
- [OpenAPI Specification](https://github.com/cpfhub/cpfhub-openapi/blob/main/openapi.yaml)
- [MCP Server (AI Agents)](https://github.com/cpfhub/cpfhub-mcp)

---

## License

MIT © [CPFHub.io](https://cpfhub.io)

---

# Português

[🇺🇸 English](#cpfhub-dotnet-net-sdk-for-cpfhubio) | 🇧🇷 **Português**

**SDK .NET oficial para [CPFHub.io](https://cpfhub.io) — API de Consulta de CPF Brasileiro**

---

## O que é o CPFHub.io?

O CPFHub.io é uma API REST que retorna nome, gênero e data de nascimento de qualquer CPF brasileiro — em ~300ms, com 99,9% de uptime e total conformidade com a LGPD.

**10M+ CPFs consultados · 1.300+ empresas ativas · 99,9% uptime**

---

## Instalação

```bash
dotnet add package CPFHub
```

Ou via o NuGet Package Manager:

```
Install-Package CPFHub
```

---

## Início Rápido

```csharp
using CPFHub;

var client = new CPFHubClient("SUA_CHAVE_DE_API");

var result = await client.LookupAsync("00000000000");

Console.WriteLine(result.Name);      // "Fulano de Tal"
Console.WriteLine(result.Gender);    // "M"
Console.WriteLine(result.BirthDate); // "15/06/1990"
```

Obtenha sua chave de API gratuita em [app.cpfhub.io](https://app.cpfhub.io) — sem cartão de crédito.

---

## Exemplo curl

```bash
curl -X GET "https://api.cpfhub.io/cpf/12345678909" \
  -H "x-api-key: SUA_CHAVE_DE_API"
```

**Resposta:**

```json
{
  "success": true,
  "data": {
    "cpf": "12345678909",
    "name": "Fulano de Tal",
    "nameUpper": "FULANO DE TAL",
    "gender": "M",
    "birthDate": "15/06/1990",
    "day": 15,
    "month": 6,
    "year": 1990
  }
}
```

---

## Referência da API

### `new CPFHubClient(string apiKey, CPFHubOptions? options = null)`

```csharp
var options = new CPFHubOptions
{
    Timeout = TimeSpan.FromSeconds(5),
    BaseUrl = "https://api.cpfhub.io"
};

var client = new CPFHubClient("SUA_CHAVE_DE_API", options);
```

### `client.LookupAsync(string cpf, CancellationToken ct = default) → Task<CPFResult>`

Consulta um CPF e retorna os dados de identidade associados.

Aceita CPF com ou sem formatação (`000.000.000-00` ou `00000000000`).

#### Propriedades de `CPFResult`

| Propriedade | Tipo | Descrição |
|-------------|------|-----------|
| `Cpf` | `string` | CPF (apenas dígitos) |
| `Name` | `string` | Nome completo — `"Fulano de Tal"` |
| `NameUpper` | `string` | Nome completo em maiúsculas |
| `Gender` | `string` | `"M"` ou `"F"` |
| `BirthDate` | `string` | Data de nascimento — `"DD/MM/YYYY"` |
| `Day` | `int` | Dia de nascimento |
| `Month` | `int` | Mês de nascimento |
| `Year` | `int` | Ano de nascimento |

---

## Tratamento de Erros

```csharp
using CPFHub;
using CPFHub.Exceptions;

var client = new CPFHubClient("SUA_CHAVE_DE_API");

try
{
    var result = await client.LookupAsync("00000000000");
    Console.WriteLine(result.Name);
}
catch (CPFHubException ex)
{
    Console.WriteLine($"Erro {ex.StatusCode}: {ex.Message}");
    // 400 — Formato de CPF inválido
    // 401 — Chave de API inválida ou ausente
    // 404 — CPF não encontrado
    // 429 — Limite de requisições excedido
    // 500 — Erro no servidor
    // 503 — Serviço temporariamente indisponível
}
```

---

## Exemplos

### ASP.NET Core — Injeção de Dependência

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
    "ApiKey": "SUA_CHAVE_DE_API"
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

let client = CPFHubClient("SUA_CHAVE_DE_API")
let result = client.LookupAsync("00000000000") |> Async.AwaitTask |> Async.RunSynchronously
printfn "%s" result.Name
```

---

## Limites de Requisição

| Plano | Limite |
|---|---|
| Gratuito | 1 requisição a cada 2 segundos · 50 requisições/mês |
| Pro | 1 requisição por segundo · 1.000 requisições/mês |
| Corporativo | Personalizado |

O SDK faz retry automático no erro `429` com backoff exponencial (até 3 tentativas).

---

## Planos e Preços

| Plano | Preço | Incluído | Extra |
|-------|-------|----------|-------|
| **Gratuito** | R$ 0/mês | 50 consultas | — |
| **Pro** | R$ 149/mês | 1.000 consultas | R$ 0,15/consulta |
| **Corporativo** | Personalizado | Personalizado | Personalizado |

[Ver preços completos em cpfhub.io →](https://cpfhub.io#pricing)

---

## Requisitos

- .NET 6+
- Compatível com C#, F# e VB.NET

---

## Links

- [Documentação](https://cpfhub.io/documentacao)
- [Dashboard](https://app.cpfhub.io)
- [NuGet Package](https://www.nuget.org/packages/CPFHub)
- [Página de Status](https://app.cpfhub.io/status)
- [Preços](https://cpfhub.io#pricing)
- [Conformidade LGPD](https://cpfhub.io/lgpd)
- [Especificação OpenAPI](https://github.com/cpfhub/cpfhub-openapi/blob/main/openapi.yaml)
- [Servidor MCP (Agentes de IA)](https://github.com/cpfhub/cpfhub-mcp)

---

## Licença

MIT © [CPFHub.io](https://cpfhub.io)
