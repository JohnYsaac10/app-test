using BG.ContratacionWeb.Pyme.Data;
using BG.ContratacionWeb.Pyme.Dtos;
using BG.ContratacionWeb.Pyme.Filters;
using BG.ContratacionWeb.Pyme.Infrastructure;
using System;
using System.Web.Mvc;

namespace BG.ContratacionWeb.Pyme.Controllers
{
    public class TestController : Controller
    {
        private readonly CommonApplicationSettings _settings;
        private readonly ITokenRepository _tokenRepo;
        public TestController(CommonSettingsManager settings, ITokenRepository tokenRepo)
        {
            _settings = settings.Config;
            _tokenRepo = tokenRepo;
        }

        public ActionResult Index()
        {
            if (!(_settings.Environment.mode.Equals("development", StringComparison.InvariantCultureIgnoreCase)))
                return RedirectToAction("Notificacion", "ErrorHandler", new { id = "404" });
            
            return View("GenerateToken");
        }

        [HttpPost]
        public ActionResult GenerateToken(GenerateTokenFormDto data)
        {
            if (!(_settings.Environment.mode.Equals("development", StringComparison.InvariantCultureIgnoreCase)))
                return Json(new { errorCode = "401" });

            string errorCode;
            var resp = _tokenRepo.GenerarToken(data, out errorCode);

            if (resp == null || errorCode == "503")
            {
                return Json(new { errorCode });
            }

            if(resp.meta.Code == "000")
            {
                var token = resp.data.token;
                return Json(new { errorCode = "200", data = $"?token={token}&canal={data.Canal}&aplicacion={data.Aplicacion}" });
            }

            return Json(new { errorCode = "400" });
        }
    }
}