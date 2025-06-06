﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Cloudflare.HttpClient.Abstract;
using Soenneker.Cloudflare.OpenApiClient;
using Soenneker.Cloudflare.Utils.Client.Abstract;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.ValueTask;
using Soenneker.Kiota.BearerAuthenticationProvider;
using Soenneker.Utils.AsyncSingleton;
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
            System.Net.Http.HttpClient client = await httpClientUtil.Get(token).NoSync();

            var apiKey = configuration.GetValueStrict<string>("Cloudflare:ApiKey");

            var requestAdapter = new HttpClientRequestAdapter(new BearerAuthenticationProvider(apiKey), httpClient: client);

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

