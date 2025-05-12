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
}