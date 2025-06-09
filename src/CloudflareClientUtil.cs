using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Cloudflare.HttpClient.Abstract;
using Soenneker.Cloudflare.OpenApiClient;
using Soenneker.Cloudflare.Utils.Client.Abstract;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.ValueTask;
using Soenneker.HttpClients.LoggingHandler;
using Soenneker.Kiota.BearerAuthenticationProvider;
using Soenneker.Utils.AsyncSingleton;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cloudflare.Utils.Client;

/// <inheritdoc cref="ICloudflareClientUtil"/>
public sealed class CloudflareClientUtil : ICloudflareClientUtil
{
    private readonly AsyncSingleton<CloudflareOpenApiClient> _client;

    private System.Net.Http.HttpClient? _httpClient;

    public CloudflareClientUtil(ICloudflareHttpClient httpClientUtil, IConfiguration configuration, ILogger<CloudflareClientUtil> logger)
    {
        _client = new AsyncSingleton<CloudflareOpenApiClient>(async (token, _) =>
        {
            var apiKey = configuration.GetValueStrict<string>("Cloudflare:ApiKey");

            var logging = configuration.GetValue<bool>("Cloudflare:RequestResponseLogging");

            if (logging)
            {
                var loggingHandler = new HttpClientLoggingHandler(logger, new HttpClientLoggingOptions
                {
                    LogLevel = LogLevel.Debug
                });

                loggingHandler.InnerHandler = new HttpClientHandler();

                _httpClient = new System.Net.Http.HttpClient(loggingHandler);
            }
            else
            {
                _httpClient = await httpClientUtil.Get(token).NoSync();
            }

            var requestAdapter = new HttpClientRequestAdapter(new BearerAuthenticationProvider(apiKey), httpClient: _httpClient);

            return new CloudflareOpenApiClient(requestAdapter);
        });
    }

    public ValueTask<CloudflareOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        return _client.Get(cancellationToken);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();

        _client.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        _httpClient?.Dispose();

        return _client.DisposeAsync();
    }
}