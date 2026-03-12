using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Cloudflare.HttpClient.Registrars;
using Soenneker.Cloudflare.Utils.Client.Abstract;
using Soenneker.Utils.HttpClientCache.Registrar;

namespace Soenneker.Cloudflare.Utils.Client.Registrars;

/// <summary>
/// An async thread-safe singleton for the Cloudflare OpenApiClient
/// </summary>
public static class CloudflareClientUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="ICloudflareClientUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddCloudflareClientUtilAsSingleton(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton().AddCloudflareHttpClientAsSingleton().TryAddSingleton<ICloudflareClientUtil, CloudflareClientUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="ICloudflareClientUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddCloudflareClientUtilAsScoped(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton().AddCloudflareHttpClientAsSingleton().TryAddScoped<ICloudflareClientUtil, CloudflareClientUtil>();

        return services;
    }
}
