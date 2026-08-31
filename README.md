[![](https://img.shields.io/nuget/v/soenneker.cloudflare.utils.client.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.cloudflare.utils.client/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.cloudflare.utils.client/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.cloudflare.utils.client/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.cloudflare.utils.client.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.cloudflare.utils.client/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.cloudflare.utils.client/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.cloudflare.utils.client/actions/workflows/codeql.yml)

# Soenneker.Cloudflare.Utils.Client

Provides thread-safe, token-keyed caching for configured `CloudflareOpenApiClient` instances.

## Installation

```bash
dotnet add package Soenneker.Cloudflare.Utils.Client
```

## Configuration

```json
{
  "Cloudflare": {
    "ApiKey": "your-api-token",
    "RequestResponseLogging": false
  }
}
```

The parameterless `Get` overload uses `Cloudflare:ApiKey`. Store API tokens in a secret provider. Request/response logging is optional; authorization headers are redacted by the underlying Cloudflare HTTP provider.

## Registration

```csharp
using Soenneker.Cloudflare.Utils.Client.Registrars;

services.AddCloudflareClientUtilAsSingleton();
```

Scoped registration is available when the generated-client cache should be scoped. In either case, the utility borrows a singleton Cloudflare HTTP provider so disposing a scoped utility does not tear down the shared HTTP clients.

## Usage

```csharp
using Soenneker.Cloudflare.Utils.Client.Abstract;

CloudflareOpenApiClient client = await clientUtil.Get(cancellationToken);

var response = await client.Zones.GetAsync(
    cancellationToken: cancellationToken);
```

`Get(apiKey)` maintains a distinct generated client and underlying authenticated `HttpClient` for each token. Use only a bounded set of long-lived tokens with a singleton utility; arbitrary one-off tokens otherwise remain cached.

`Remove(apiKey)` and `RemoveSync(apiKey)` remove the generated client and immediately remove and dispose its underlying `HttpClient`. Disposing the utility clears its generated-client cache; the separately owned singleton HTTP provider remains alive until its own container lifetime ends.

The returned generated client is shared for its cache lifetime. Do not mutate its request adapter or dispose infrastructure borrowed from it. Cloudflare API errors are surfaced through Kiota's generated exception behavior.
