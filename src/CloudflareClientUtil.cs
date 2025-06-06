using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Cloudflare.HttpClient.Abstract;
using Soenneker.Cloudflare.OpenApiClient;
using Soenneker.Cloudflare.Utils.Client.Abstract;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.ValueTask;
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

    public CloudflareClientUtil(ICloudflareHttpClient httpClientUtil, IConfiguration configuration, ILogger<CloudflareClientUtil> logger)
    {
        _client = new AsyncSingleton<CloudflareOpenApiClient>(async (token, _) =>
        {
            var loggingHandler = new LoggingHandler(logger) { InnerHandler = new HttpClientHandler() };

            var httpClient = new System.Net.Http.HttpClient(loggingHandler);

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

    public ValueTask DisposeAsync()
    {
        return _client.DisposeAsync();
    }
}

public class LoggingHandler : DelegatingHandler
{
    private readonly ILogger _logger;

    public LoggingHandler(ILogger logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Log outgoing request
        _logger.LogInformation("Request: {Method} {Uri}", request.Method, request.RequestUri);
        foreach (var header in request.Headers)
            _logger.LogInformation("Request Header: {Key}: {Value}", header.Key, string.Join(", ", header.Value));
        if (request.Content != null)
        {
            var content = await request.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogInformation("Request Content: {Content}", content);
        }

        var response = await base.SendAsync(request, cancellationToken);

        // Log incoming response
        _logger.LogInformation("Response: {StatusCode}", response.StatusCode);
        foreach (var header in response.Headers)
            _logger.LogInformation("Response Header: {Key}: {Value}", header.Key, string.Join(", ", header.Value));
        if (response.Content != null)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogInformation("Response Content: {Content}", content);
        }

        return response;
    }
}