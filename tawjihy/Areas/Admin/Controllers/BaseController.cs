using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace tawjihy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public class BaseController : Controller
    {
        private IMemoryCache _memoryCache;

        public BaseController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult ClearCache()
        {
            //_memoryCache.Dispose();
            _memoryCache.Remove("CachedItems");
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            return Redirect("/Admin/Home/Index");
        }
        public IActionResult ErrorUpload()
        {
            return View();
        }
        public IActionResult ErrorDelete()
        {
            return View();
        }
        public bool ClearCacheMethod()
        {
            try
            {
                _memoryCache.Remove("CachedItems");
                //_memoryCache.Dispose();
                _memoryCache = new MemoryCache(new MemoryCacheOptions());
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

    }
}
