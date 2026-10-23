using Soenneker.Cloudflare.OpenApiClient;
using System;
using Microsoft.Kiota.Abstractions;
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
    /// Gets the cached client's authenticated request adapter for responses the generated client cannot represent.
    /// The adapter shares the client's request/response logging and lifetime and must not be disposed by callers.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The configured client's request adapter.</returns>
    ValueTask<IRequestAdapter> GetRequestAdapter(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the cached client's authenticated request adapter for a specific API token.
    /// The adapter shares the client's request/response logging and lifetime and must not be disposed by callers.
    /// </summary>
    /// <param name="apiKey">The Cloudflare API token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The token-specific client's request adapter.</returns>
    ValueTask<IRequestAdapter> GetRequestAdapter(string apiKey, CancellationToken cancellationToken = default);

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

    /// <summary>
    /// Removes sync.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A value indicating whether the operation succeeded.</returns>
    bool RemoveSync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes sync.
    /// </summary>
    /// <param name="apiKey">The API key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A value indicating whether the operation succeeded.</returns>
    bool RemoveSync(string apiKey, CancellationToken cancellationToken = default);
}
