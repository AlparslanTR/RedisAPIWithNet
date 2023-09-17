using Microsoft.AspNetCore.Mvc;
using Net_RedisExchangeAPI.Services;
using StackExchange.Redis;

namespace Net_RedisExchangeAPI.Controllers
{
    public class ListTypesController : Controller
    {
        private readonly ILogger<StringTypesController> _logger;
        private readonly RedisService _redisService;
        private readonly string listKey = "Users";

        public ListTypesController(ILogger<StringTypesController> logger, RedisService redisService)
        {
            _logger = logger;
            _redisService = redisService;
        }
        public IActionResult Index()
        {
            List<string> list = new List<string>();
            var db = _redisService.GetDatabase(0); // Bir den fazla db ler değişkenlik gösterebilir bu yüzden statik vermiyorum.
            if (db is not null)
            {
                if (db.KeyExists(listKey))
                {
                    db.ListRange(listKey, 0, 100).ToList().ForEach(x =>
                    {
                        list.Add(x.ToString());
                    }); // 1'den 100 e kadar olan dataları getir.
                    _logger.LogInformation("Veriler getirildi");
                }
                else
                {
                    _logger.LogInformation("Sunucu da veri yok");
                }
            }
            return View(list);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            var db = _redisService.GetDatabase(0);
            db.ListRightPush(listKey, name);
            if (db is not null)
            {
                _logger.LogInformation($"{name} Adlı Ekleme İşlemi Başarılı");
            }
            else
            {
                 _logger.LogInformation("Ekleme İşlemi Başarısız Oldu");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remove(string name)
        {
            var db = _redisService.GetDatabase(0);
            db.ListRemove(listKey, name);

            if (db is not null)
            {
                _logger.LogInformation($"{name} Adlı Kaldırma İşlemi Başarılı");
            }
            else
            {
                _logger.LogInformation("Kaldırma İşlemi Başarısız Oldu");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
