using Microsoft.Kiota.Abstractions;
using Soenneker.Cloudflare.OpenApiClient;

namespace Soenneker.Cloudflare.Utils.Client;

internal sealed class CloudflareClient(IRequestAdapter adapter) : CloudflareOpenApiClient(adapter)
{
    internal IRequestAdapter Adapter => RequestAdapter;
}
