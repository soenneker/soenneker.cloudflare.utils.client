using Soenneker.Cloudflare.Utils.Client.Abstract;
using Soenneker.Tests.HostedUnit;

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
    public void Default()
    {

    }
}
