using Soenneker.Cloudflare.Utils.Client.Abstract;
using Soenneker.Tests.HostedUnit;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cloudflare.Utils.Client.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class CloudflareClientUtilTests : HostedUnitTest
{
    private readonly ICloudflareClientUtil _util;

    public CloudflareClientUtilTests(Host host) : base(host)
    {
        _util = Resolve<ICloudflareClientUtil>(true);
    }

    [Test]
    public async Task Get_WithSameApiKey_ReturnsCachedClient(CancellationToken cancellationToken)
    {
        const string apiKey = "test-api-key";

        var first = await _util.Get(apiKey, cancellationToken);
        var second = await _util.Get(apiKey, cancellationToken);

        await Assert.That(second).IsSameReferenceAs(first);

        await _util.Remove(apiKey, cancellationToken);
    }
}
