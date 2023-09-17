using Microsoft.AspNetCore.Mvc;
using Net_RedisExchangeAPI.Services;

namespace Net_RedisExchangeAPI.Controllers
{
    public class StringTypesController : Controller
    {
        private readonly ILogger<StringTypesController> _logger;
        private readonly RedisService _redisService;

        public StringTypesController(ILogger<StringTypesController> logger, RedisService redisService)
        {
            _logger = logger;
            _redisService = redisService;
        }

        public IActionResult Index()
        {
            var db = _redisService.GetDatabase(0);
            db.StringSet("Name", "Alparslan Akbaş");
            db.StringSet("Name2", "Gazi Hataş");

            if (db is not null)
            {
                _logger.LogInformation("Veri Eklendi");
            }
            else
            {
                _logger.LogInformation("Veri Eklenemedi");
            }
            return View();
        }              
    }
}
