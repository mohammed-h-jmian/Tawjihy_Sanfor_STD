//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Caching.Memory;

//namespace tawjihy.Areas.Admin.Controllers
//{
//    public class CacheController : BaseController
//    {
//        private IMemoryCache _memoryCache;

//        public CacheController(IMemoryCache memoryCache)
//        {
//            _memoryCache = memoryCache;
//        }

//        public IActionResult ClearCache()
//        {
//            _memoryCache.Dispose();
//            _memoryCache = new MemoryCache(new MemoryCacheOptions());
//            return Redirect("/Admin/Home/Index");
//        }
//        public IActionResult ShowCache()
//        {
//            var cachedItems = _memoryCache.Get<List<string>>("CachedItems");
//            return View(cachedItems);
//        }
        

//    }
//}
