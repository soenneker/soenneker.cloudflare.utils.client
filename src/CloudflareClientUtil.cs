using Microsoft.Extensions.Configuration;
using Soenneker.Cloudflare.Utils.Client.Abstract;
using Soenneker.Utils.AsyncSingleton;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Cloudflare.OpenApiClient;
using Soenneker.Extensions.Configuration;
using Soenneker.Kiota.BearerAuthenticationProvider;
using Soenneker.Cloudflare.HttpClient.Abstract;
using Soenneker.Extensions.ValueTask;

namespace Soenneker.Cloudflare.Utils.Client;

/// <inheritdoc cref="ICloudflareClientUtil"/>
public sealed class CloudflareClientUtil : ICloudflareClientUtil
{
    private readonly AsyncSingleton<CloudflareOpenApiClient> _client;

    public CloudflareClientUtil(ICloudflareHttpClient httpClientUtil, IConfiguration configuration)
    {
        _client = new AsyncSingleton<CloudflareOpenApiClient>(async (token, _) =>
        {
            System.Net.Http.HttpClient httpClient = await httpClientUtil.Get(token).NoSync();

            var apiKey = configuration.GetValueStrict<string>("Cloudflare:ApiKey");

            var requestAdapter = new HttpClientRequestAdapter(new BearerAuthenticationProvider(apiKey), httpClient: httpClient);

            return new CloudflareOpenApiClient(requestAdapter);
        });
    }

    public ValueTask<CloudflareOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        return _client.Get(cancellationToken);
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _client.DisposeAsync();
    }
}