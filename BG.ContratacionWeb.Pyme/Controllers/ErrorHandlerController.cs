using BG.ContratacionWeb.Pyme.Helpers;
using BG.ContratacionWeb.Pyme.Infrastructure;
using System.Web.Mvc;

namespace BG.ContratacionWeb.Pyme.Controllers
{
    public class ErrorHandlerController : Controller
    {
        private readonly CommonApplicationSettings _settings;

        public ErrorHandlerController(CommonSettingsManager settings)
        {
            _settings = settings.Config;
        }

        // GET: ErrorHandler
        public ActionResult Notificacion(string id)
        {
            var mensaje = InitialConfig.getNotiMessage(id, _settings);

            if (id == "000" || id == "099" || id == "001")
                Session.Clear();

            return View(mensaje);
        }

        public ActionResult info(Mensaje id)
        {
            return View(id);
        }
    }
}