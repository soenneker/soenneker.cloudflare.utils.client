using Soenneker.Cloudflare.OpenApiClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cloudflare.Utils.Client.Abstract;

/// <summary>
/// An async thread-safe singleton for the Cloudflare OpenApiClient
/// </summary>
public interface ICloudflareClientUtil : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Gets a configured Cloudflare OpenAPI client instance
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A configured Cloudflare OpenAPI client</returns>
    ValueTask<CloudflareOpenApiClient> Get(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a configured Cloudflare OpenAPI client instance for a specific API token.
    /// </summary>
    /// <param name="apiKey">Cloudflare API token used as the bearer token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A configured Cloudflare OpenAPI client</returns>
    ValueTask<CloudflareOpenApiClient> Get(string apiKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes and disposes the configured Cloudflare OpenAPI client instance.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see langword="true"/> if a cached client was removed</returns>
    ValueTask<bool> Remove(CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes and disposes the Cloudflare OpenAPI client instance for a specific API token.
    /// </summary>
    /// <param name="apiKey">Cloudflare API token used as the bearer token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><see langword="true"/> if a cached client was removed</returns>
    ValueTask<bool> Remove(string apiKey, CancellationToken cancellationToken = default);

    bool RemoveSync(CancellationToken cancellationToken = default);

    bool RemoveSync(string apiKey, CancellationToken cancellationToken = default);
}
