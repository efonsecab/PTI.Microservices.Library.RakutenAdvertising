# PTI.Microservices.Library.RakutenAdvertising

This is part of PTI.Microservices.Library set of packages

The purpose of this package is to facilitate calls to Rakuten Advertising APIs, while maintaining a consistent usage pattern among the different services in the group

**Examples:**

## Search Product
    RakutenAdvertisingService rakutenAdvertisingService =
        new RakutenAdvertisingService(null,
        this.RakutenAdvertisingConfiguration,
        new Microservices.Library.Interceptors.CustomHttpClient(
            new Microservices.Library.Interceptors.CustomHttpClientHandler(null)));
    var pageResult = await rakutenAdvertisingService.SearchProductAsync("lego", "13923");
    Assert.IsNotNull(pageResult);
    do
    {
        pageResult = await rakutenAdvertisingService.SearchProductAsync("lego", "13923", pageResult.PageNumber + 1);
        Assert.IsNotNull(pageResult);
    } while (pageResult.PageNumber < pageResult.TotalPages);