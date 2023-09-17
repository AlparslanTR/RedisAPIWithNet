using Microsoft.AspNetCore.Mvc;
using Net_RedisExchangeAPI.Services;
using StackExchange.Redis;

namespace Net_RedisExchangeAPI.Controllers
{
    public class HashSetTypesController : Controller
    {
        private readonly ILogger<HashSetTypesController> _logger;
        private readonly RedisService _redisService;
        private readonly string redisKey = "hashname";

        public HashSetTypesController(ILogger<HashSetTypesController> logger, RedisService redisService)
        {
            _logger = logger;
            _redisService = redisService;
        }

        public IActionResult Index()
        {
            var db = _redisService.GetDatabase(0);
            HashSet<string> list = new HashSet<string>();
            if (redisKey  is not null)
            {
                if (db.KeyExists(redisKey))
                {
                    db.SetMembers(redisKey).ToList().ForEach(x =>
                    {
                        list.Add(x.ToString());
                    });
                    _logger.LogInformation("Veriler Getirildi");
                }             
                 else
                {
                    _logger.LogInformation("Sunucuda veri yok");
                }
            }
           
            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            var db = _redisService.GetDatabase(0);
            db.SetAdd(redisKey, name);
            db.KeyExpire(redisKey, DateTime.Now.AddMinutes(5)); // Çalıştığı andan itibaren her seferin de 5 dakika eklenecek.
            return RedirectToAction(nameof(Index));
        }
    }
}
