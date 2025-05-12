﻿using Soenneker.Cloudflare.Utils.Client.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Cloudflare.Utils.Client.Tests;

[Collection("Collection")]
public class CloudflareClientUtilTests : FixturedUnitTest
{
    private readonly ICloudflareClientUtil _util;

    public CloudflareClientUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<ICloudflareClientUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
