using Microsoft.VisualStudio.TestTools.UnitTesting;

using Yaoya.Core.Services;

namespace Yaoya.Core.Tests.MSTest;

[TestClass]
public class SampleDataServiceTests
{
    public SampleDataServiceTests()
    {
    }

    // Remove or update this once your app is using real data and not the SampleDataService.
    // This test serves only as a demonstration of testing functionality in the Core library.
    [TestMethod]
    public async Task EnsureSampleDataServiceReturnsContentGridDataAsync()
    {
        var sampleDataService = new SampleDataService();

        var data = await sampleDataService.GetContentGridDataAsync();

        Assert.IsTrue(data.Any());
    }
}
