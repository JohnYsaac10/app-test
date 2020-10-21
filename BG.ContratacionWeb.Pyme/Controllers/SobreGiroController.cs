using BG.ContratacionWeb.Pyme.Data;
using BG.ContratacionWeb.Pyme.Dtos;
using BG.ContratacionWeb.Pyme.Filters;
using BG.ContratacionWeb.Pyme.Helpers;
using BG.ContratacionWeb.Pyme.Infrastructure;
using BG.ContratacionWeb.Pyme.Models;
using System.Web.Mvc;

namespace BG.ContratacionWeb.Pyme.Controllers
{
    public class SobreGiroController : Controller
    {
        private readonly ISobreGiroRepository _sobregiroRepo;
        private readonly IOtpRepository _OtpRepo;
        private readonly CommonApplicationSettings _settings;

        public SobreGiroController(ISobreGiroRepository sobregiroRepo, IOtpRepository OtpRepo, CommonSettingsManager settings)
        {
            _sobregiroRepo = sobregiroRepo;
            _OtpRepo = OtpRepo;
            _settings = settings.Config;
        }

        [ProtectGet]
        public ActionResult Solicitud()
        {
            DatosCliente dataClient = Session["datosCliente"] as DatosCliente;

            var data = _sobregiroRepo.GetDatosInicio(ref dataClient);

            if (data.codigoRetorno != null)
                return RedirectToAction("Notificacion", "ErrorHandler", new { id = data.codigoRetorno });

            //max and min
            data.MontoRango  = _settings.RangoDeSobregiro.valorMin.ToString()+ "-" + _settings.RangoDeSobregiro.valorMax.ToString();

            Session["datosCliente"] = dataClient;

            Session["saludo"] = dataClient.getSaludo();

            data.DataPreset = Session["solicitud"] as SobreGiroDetalleSolicitudDto;

            return View(data);
        }

        [HttpPost]
        [ProtectPost]
        public ActionResult Solicitud(SobreGiroDetalleSolicitudDto data)
        {
            string mensaje = "No envió datos.";
            if (data != null && data.isValidateRequisition(_settings, out mensaje))
            {
                Session["solicitud"] = data;
                return Json(new { codError = "200" });
            }
            return Json(new { codError = "400", mensaje });
        }

        [ProtectGet]
        public ActionResult DetalleSolicitud()
        {
            if (Session["solicitud"] == null)
                return RedirectToAction("Solicitud");


            decimal tasaNominal, tasaEfectiva, FactorReajuste;
            string mensajeRetorno;

            var dataSession = Session["solicitud"] as SobreGiroDetalleSolicitudDto;
            var clt = Session["datosCliente"] as DatosCliente;
            dataSession.celular = clt.PhoneNumber;


            var tasaInteres = _sobregiroRepo.ObtenerTasaInteresProducto(out mensajeRetorno, out tasaNominal, out tasaEfectiva, out FactorReajuste);

            dataSession.TasaInteres = tasaEfectiva.ToString();

            Session["solicitud"] = dataSession;

            var dataClient = Session["datosCliente"] as DatosCliente;

            return View(dataSession);
        }

        [HttpPost]
        [ProtectPost]
        public ActionResult CreaCodigoOTP(string data)
        {
            string mensajeRetorno;
            var dataClient = Session["datosCliente"] as DatosCliente;
            var resp = _OtpRepo.GenerarOTP(dataClient.Cedula, dataClient.PhoneNumber, out mensajeRetorno);

            return Json(new { codError = resp, mensaje = mensajeRetorno }); ;
        }

        [HttpPost]
        [ProtectPost]
        public ActionResult ValidaOTP(string data)
        {
            long idExpediente;
            string codigoRetorno;
            string mensajeRetorno;

            var dataClient = Session["datosCliente"] as DatosCliente;
            var dataSession = Session["solicitud"] as SobreGiroDetalleSolicitudDto;

            var resp = _OtpRepo.ValidarOTP(dataSession, dataClient.Cedula, data, out idExpediente, out codigoRetorno, out mensajeRetorno);

            if(codigoRetorno == "200")
                Session.Clear();
            
            return Json(new { codError = codigoRetorno, mensaje = mensajeRetorno, redirectCode = resp });
        }
        
    }
}