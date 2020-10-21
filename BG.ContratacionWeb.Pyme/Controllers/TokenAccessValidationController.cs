using BG.ContratacionWeb.Pyme.App_Start;
using BG.ContratacionWeb.Pyme.Data;
using BG.ContratacionWeb.Pyme.Dtos;
using BG.ContratacionWeb.Pyme.Filters;
using BG.ContratacionWeb.Pyme.Helpers;
using BG.ContratacionWeb.Pyme.Infrastructure;
using BG.ContratacionWeb.Pyme.Models;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace BG.ContratacionWeb.Pyme.Controllers
{
    public class TokenAccessValidationController : Controller
    {
        private readonly CommonApplicationSettings _settings;
        private readonly ITokenRepository _tokenRepo;

        public TokenAccessValidationController(ITokenRepository tokenRepo, CommonSettingsManager settings)
        {
            _settings = settings.Config;
            _tokenRepo = tokenRepo;
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [SkipToken]
        public ActionResult Index(DataToken data)
        {
            if (data.IsAnyNullOrEmpty())
                return RedirectToAction("Notificacion", "ErrorHandler", new { id = "401" });

            Session.Clear();

            string codigoRetorno;

            ServicioToken.TokenValida tokenValida = _tokenRepo.RecuperarInfToken(data.token, data.canal, data.aplicacion, out codigoRetorno);

            if(tokenValida.meta.Code == "000")
            {
                string str_json = tokenValida.data.metajson;
                InfToken json = JsonConvert.DeserializeObject<InfToken>(str_json);

                DatosCliente datosCliente = new DatosCliente();
                datosCliente.Cedula = json.identificacion;

                if(json.producto == _settings.ProductoSG.IdProductoNeo.ToString())
                {
                    Session["datosCliente"] = datosCliente;
                    return RedirectToAction("Solicitud", "SobreGiro");
                }

                if(json.producto == _settings.ProductoCP.IdProductoNeo.ToString())
                {
                    var obj = new DatosClientePy(); obj.Cedula = json.identificacion;
                    Session["datosClienteCP"] = obj;
                    return RedirectToAction("Simulador", "CreditoPyme");
                }
                    
            }


            return RedirectToAction("Notificacion", "ErrorHandler", new { id = codigoRetorno });
        }
    }
}