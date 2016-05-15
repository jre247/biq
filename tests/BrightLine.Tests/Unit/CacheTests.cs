using NUnit.Framework;


namespace BrightLine.Tests.Unit
{
    [TestFixture]
    public class CacheTests
    {
        //private ICache GetCache()
        //{
        //    var cache = new Cache();
        //    cache.Remove("r1");
        //    cache.Remove("r2");
        //    return cache;
        //}
        //
        //
        //[Test]
        //public void CanAddStaticItem()
        //{
        //    var cache = GetCache();
        //    cache.Add("r1", new ResourceDoc() { Name = "res 1" }, 5.Minutes());
        //    var item = cache.Get<ResourceDoc>("r1");
        //    Assert.AreEqual("res 1", item.Name);
        //}
        //
        //
        //[Test]
        //public void CanAddRefeshItem()
        //{
        //    var cache = GetCache();
        //    cache.Add("r2", null, () => new ResourceDoc() { Name = "res 2" }, 5.Minutes());
        //    var item = cache.Get<ResourceDoc>("r2");
        //    Assert.AreEqual("res 2", item.Name);
        //}
        //
        //
        //[Test]
        //public void CanGetEntries()
        //{
        //    var cache = GetCache();
        //    cache.Add("r1", new ResourceDoc() { Name = "res 1" }, 5.Minutes());
        //    cache.Add("r2", null, () => new ResourceDoc() { Name = "res 2" }, 5.Minutes());
        //    var entries = cache.GetCacheEntries();
        //    Assert.AreEqual(2, entries.Count);
        //}
    }
}
