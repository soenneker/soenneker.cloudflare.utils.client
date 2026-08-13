using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Cloudflare.HttpClient.Abstract;
using Soenneker.Cloudflare.OpenApiClient;
using Soenneker.Cloudflare.Utils.Client.Abstract;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.ValueTask;
using Soenneker.Kiota.BearerAuthenticationProvider;
using Soenneker.Dictionaries.Singletons;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cloudflare.Utils.Client;

/// <inheritdoc cref="ICloudflareClientUtil"/>
public sealed class CloudflareClientUtil : ICloudflareClientUtil
{
    private readonly SingletonDictionary<CloudflareOpenApiClient> _clients;

    private readonly ICloudflareHttpClient _httpClientUtil;
    private readonly IConfiguration _configuration;

    public CloudflareClientUtil(ICloudflareHttpClient httpClientUtil, IConfiguration configuration)
    {
        _httpClientUtil = httpClientUtil;
        _configuration = configuration;
        // Method group => no closure allocation
        _clients = new SingletonDictionary<CloudflareOpenApiClient>(CreateClient);
    }

    private async ValueTask<CloudflareOpenApiClient> CreateClient(string apiKey, CancellationToken token)
    {
        System.Net.Http.HttpClient httpClient = await _httpClientUtil.Get(apiKey, token)
                                                                     .NoSync();

        var requestAdapter = new HttpClientRequestAdapter(new BearerAuthenticationProvider(apiKey), httpClient: httpClient);

        return new CloudflareOpenApiClient(requestAdapter);
    }

    public ValueTask<CloudflareOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        var apiKey = _configuration.GetValueStrict<string>("Cloudflare:ApiKey");
        return Get(apiKey, cancellationToken);
    }

    public ValueTask<CloudflareOpenApiClient> Get(string apiKey, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        return _clients.Get(apiKey, cancellationToken);
    }

    public ValueTask<bool> Remove(CancellationToken cancellationToken = default)
    {
        var apiKey = _configuration.GetValueStrict<string>("Cloudflare:ApiKey");
        return Remove(apiKey, cancellationToken);
    }

    public async ValueTask<bool> Remove(string apiKey, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        bool removed = await _clients.Remove(apiKey, cancellationToken)
                                     .NoSync();

        return removed;
    }

    public bool RemoveSync(CancellationToken cancellationToken = default)
    {
        var apiKey = _configuration.GetValueStrict<string>("Cloudflare:ApiKey");
        return RemoveSync(apiKey, cancellationToken);
    }

    public bool RemoveSync(string apiKey, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        bool removed = _clients.RemoveSync(apiKey, cancellationToken);
        return removed;
    }

    /// <summary>
    /// Releases resources used by the current instance.
    /// </summary>
    public void Dispose()
    {
        _clients.Dispose();
    }

    /// <summary>
    /// Asynchronously releases resources used by the current instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask DisposeAsync()
    {
        await _clients.DisposeAsync()
                      .NoSync();
    }
}
