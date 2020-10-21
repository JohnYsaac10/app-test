using BG.ContratacionWeb.Pyme.Data;
using BG.ContratacionWeb.Pyme.Dtos;
using BG.ContratacionWeb.Pyme.Filters;
using BG.ContratacionWeb.Pyme.Helpers;
using BG.ContratacionWeb.Pyme.Infrastructure;
using BG.ContratacionWeb.Pyme.Models;
using BG.ContratacionWeb.Pyme.ServicioContratacionProductos;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace BG.ContratacionWeb.Pyme.Controllers
{
    public class CreditoPymeController : Controller
    {
        private readonly ICreditoPyme _repoCreditoPyme;
        private readonly ICatalogoRepository _repoCatalogo;
        private readonly CommonApplicationSettings _settings;
        private readonly IOtpRepository _OtpRepo;
        public CreditoPymeController(ICreditoPyme repoCreditoPyme, CommonSettingsManager settings, ICatalogoRepository repoCatalogo, IOtpRepository OtpRepo)
        {
            _repoCreditoPyme = repoCreditoPyme;
            _settings = settings.Config;
            _repoCatalogo = repoCatalogo;
            _OtpRepo = OtpRepo;

        }

        [ProtectGet]
        public ActionResult Simulador()
        {
            string codError = "", mensajeError, mensaje = "OK";

            var infoCliente = Session["datosClienteCP"] as DatosClientePy;
            if(infoCliente.infoSolicitud != null)
                return View(Session["datosCatSimulacion"] as DatosForSimuladorViewDto);

            if (string.IsNullOrEmpty(infoCliente.Nombres))
                infoCliente = _repoCreditoPyme.GetInfoClientePy(infoCliente.Cedula, out codError, out mensajeError);

            var infoCatalogo = Session["datosCatSimulacion"] as DatosForSimuladorViewDto;
            if (infoCatalogo == null)
                infoCatalogo = _repoCreditoPyme.GetDatosInicio(infoCliente.Cedula, out mensaje);

            if (codError == "503" || mensaje == "503") {
                infoCatalogo = null;
                return RedirectToAction("Notificacion", "ErrorHandler", new { id = "503" });
            }

            if (infoCliente == null || mensaje != "OK")
                return RedirectToAction("Notificacion", "ErrorHandler", new { id = "500" });

            Session["datosClienteCP"] = infoCliente;
            infoCatalogo.SeleccionAnterior = infoCliente.infoSolicitud;

            Session["datosCatSimulacion"] = infoCatalogo;

            Session["saludo"] = infoCliente.Nombres.ToSaludo();

            if (!string.IsNullOrEmpty(infoCliente.ActionRoute))
                return RedirectToAction(infoCliente.ActionRoute, "CreditoPyme");

            return View(infoCatalogo);
        }

        [HttpPost]
        [ProtectPost]
        public JsonResult Calcular(string dataJson)    //mejorar esto
        {
            if(string.IsNullOrEmpty(dataJson))
                return Json(new { codError = "400", mensaje = "Olvidaste ingresar algún campo" });
            FromFormSolicitudCreditoDto data = null;
            try
            {
                data = JsonConvert.DeserializeObject<FromFormSolicitudCreditoDto>(dataJson);
            }
            catch (System.Exception)
            {
                return Json(new { codError = "400", mensaje = "Petición erronea" });
            }

            string mensaje;
            var infoCliente = Session["datosClienteCP"] as DatosClientePy;

            var infoCatalogo = Session["datosCatSimulacion"] as DatosForSimuladorViewDto;
            if (infoCatalogo == null)
                return Json(new { codError = "405" });

            if (!data.IsValidModelSimulador(_settings, out mensaje))
                return Json(new { codError = "400", mensaje });

            //aqui va servicio calcula    ******** MEJORAR ESTO
            string codError = "", resp = null;
            if (data.modoDePago.idCodigo == 12167)
            {
                resp = _repoCreditoPyme.CalcularCuotaAlVencimiento(data);
            }
            else
            {
                resp = _repoCreditoPyme.CalculaCuotaMensual(data, infoCliente.Cedula, ref codError);
            }
            if (resp == null)
                return Json(new { codError = "503", mensaje = InitialConfig.getNotiMessage("503", _settings).paragraph });

            data.tasaEfectiva = infoCatalogo.TasaEfectiva;
            data.tasaNominal = infoCatalogo.TasaNominal;
            data.cuota = resp;
            infoCliente.infoSolicitud = data;
            infoCatalogo.SeleccionAnterior= data;
            Session["datosCatSimulacion"] = infoCatalogo;
            Session["datosClienteCP"] = infoCliente;

            return Json(new { codError = "200", cuota = resp });
        }

        [ProtectGet]
        public ActionResult PdfDownload()
        {
            Session["datosCatSimulacion"] = null;
            var infoCliente = Session["datosClienteCP"] as DatosClientePy;
            
            if (infoCliente.infoSolicitud == null)
                return RedirectToAction("Simulador", "CreditoPyme");
            var pdf = _repoCreditoPyme.GetTablaAmortizacion(infoCliente, infoCliente.infoSolicitud);

            if (pdf == null)
                return RedirectToAction("Notificacion", "ErrorHandler", new { id = "503" });
            return File(pdf, "application/pdf", "tablaAmortizacionBG.pdf");
        }

        [ProtectGet]
        public ActionResult InfoPersonal()
        {
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;
            if (dataCliente.infoSolicitud == null)
                return RedirectToAction("Simulador", "CreditoPyme");

            var resp = _repoCreditoPyme.RegistrarSolitud(dataCliente.Cedula, dataCliente.infoSolicitud);
            if (resp != "000")
                return RedirectToAction("Notificacion", "ErrorHandler", new { id = resp == "503" ? "503" : "500" });

            if (string.IsNullOrEmpty(dataCliente.Email) || dataCliente.Email.ToLower().Contains("notiene"))
                return RedirectToAction("Notificacion", "ErrorHandler", new { id = "001" });

            return View(dataCliente);
        }

        /*
        [HttpPost]
        [ProtectPost]
        public JsonResult SaveEmail(string email)
        {
            if (!email.IsValidEmail())
                return Json(new { codError = "400", mensaje = "correo inválido" });

            var dataCliente = Session["datosClienteCP"] as DatosClientePy;
            dataCliente.Email = email;
            _repoCreditoPyme.ActualizarDatosCliente(dataCliente);
            Session["datosClienteCP"] = dataCliente;

            return Json(new { codError = "200" });
        }*/

        //[ProtectGet]
        public ActionResult EstadoCivil()
        {
            /*var dataCliente = Session["datosClienteCP"] as DatosClientePy;

            var data = new ForViewEstadoCivilDto();
            data.EstadoCivil = dataCliente.EstadoCivil.ToUpperFirstLetter();
            data.estados = _repoCatalogo.GetDataCboxEstadoCivil();

            return View(data);*/

            var dataCliente = Session["datosClienteCP"] as DatosClientePy;

            var data = new ForViewEstadoCivilDto();

            var cbox = _repoCatalogo.GetDataCboxEstadoCivil();

            data.EstadoCivil = dataCliente.estadoCivilFormCore;
            data.estados = cbox;
            data.IdEstadoCivil = int.Parse(dataCliente.IdEstadoCivil);
            data.EstadoCivilDesc = dataCliente.EstadoCivil;

            return View(data);
        }

        public JsonResult validarEstadoCivil(FromFormEstadoCivilDto data)
        {
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;
            if(dataCliente.estadoCivilFormRC == null)
            {
                var resp = _repoCreditoPyme.ConsultaEstadoCivilCliente(dataCliente.Cedula);
                if (resp.codError != "200")
                {
                    return Json(new { resp.codError, mensaje = InitialConfig.getNotiMessage(resp.codError, _settings).paragraph });
                }
                    //return Json("Notificacion", "ErrorHandler", new { id = resp.codError });
            }

            //if()

            return null;
        }

        /*          INFO ESTADO CIVIL A POSTEAR
        [HttpPost]
        public ActionResult EstadoCivil(string data)
        {
            if (Session["datosClienteCP"] == null)
                return RedirectToAction("Notificacion", "ErrorHandler", new { id = "401" });

            if(string.IsNullOrEmpty( data ))
                return RedirectToAction("Notificacion", "ErrorHandler", new { id = "400" });

            //validar y guardar estado civil

            return Json(new { errorCode = "200" });
        }*/

        [ProtectGet]
        public ActionResult InfoDomicilio()
        {
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;

            var data = new ForViewInfoDomicilioDto();

            //data.SeleccionAnterior = dataCliente.infoDomicilio;

            var resp = _repoCreditoPyme.GetDatosForViewDireccionDomicilio(ref data, dataCliente, _repoCatalogo);

            if(resp != "200")
                return RedirectToAction("Notificacion", "ErrorHandler", new { id = resp });

            return View(data);
        }

        [HttpPost]
        [ProtectPost]
        public JsonResult GetComboForArea(FromFormComboArea data)
        {
            var catalogo = _repoCatalogo.GetDataCboxDireccionBy(data.AreaType, data.IdSeleccionPadre);

            if(catalogo == null)
                return Json(new { codError = "503", mensaje = InitialConfig.getNotiMessage("503", _settings) });

            return Json(new { catalogo, codError = "200" } );
        }

        [HttpPost]
        [ProtectPost]
        public JsonResult PostDireccionDomYneg(FromFormInfoDirecNeg data/*FromFormInfoDomicilio data*/)
        {
            string mensaje;
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;

            if(!data.isValidInfoDomicilio(out mensaje))
                return Json(new { codError = "400", mensaje });

            if (data.tipoDomicilio == "B")
                dataCliente.DireccionNegocio = dataCliente.DireccionDomicilio;
            if (data.tipoDomicilio == "T")
            {
                if(  !(string.IsNullOrEmpty(dataCliente.DireccionNegocio)))
                {
                    var tempDireccion = dataCliente.DireccionDomicilio;
                    dataCliente.DireccionDomicilio = dataCliente.DireccionNegocio;
                    dataCliente.DireccionNegocio = tempDireccion;
                }
            }

            if (data.tipoDomicilio != "Negocio")
            {
                dataCliente.infoDomicilio = AutoMapper.Mapper.Map<FromFormInfoDomicilio>(data);
                dataCliente.TelefonoDomicilio = data.telefonoDomNeg;
            }
            else
            {
                dataCliente.infoNegocio = data;
            }

            Session["datosClienteCP"] = dataCliente;

            //aqui se deben enviar a servicio
            var codError = _repoCreditoPyme.RegistrarDireccionDomOrNeg(dataCliente.Cedula, data);

            return Json(new { codError, mensaje = InitialConfig.getNotiMessage(codError, _settings) });
        }

        [ProtectGet]
        public ActionResult ResumenInfoPersonal()
        {
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;

            return View(dataCliente);
        }

        [ProtectGet]
        public ActionResult TusBalances()
        {
            string codError = "", mensajeError ="";
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;

            var resp = _repoCreditoPyme.ConsultaAnyos(ref dataCliente, out codError,out mensajeError);

            Session["datosClienteCP"] = dataCliente;

            /*if(resp.Detalle == "completo") {
                Session["pasoVariaciones"] = "true";
                return RedirectToAction("DeclaracionIva", "CreditoPyme");
            } */

            if (codError != "200")
                return RedirectToAction("Notificacion", "ErrorHandler", new { id = codError });

            return View(resp);
        }

        [HttpPost]
        [ProtectPost]
        public JsonResult PostBalances()
        {
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;

            string mensaje = "", codigoError = "";

            var resp = _repoCreditoPyme.ProcesarPdfSRI(dataCliente, Request, out codigoError, out mensaje);
            
            if (string.IsNullOrEmpty(mensaje))
                mensaje = InitialConfig.getNotiMessage(codigoError, _settings).paragraph;

            return Json(new { codError = codigoError, mensaje });
                   
        }

        [ProtectGet]
        public ActionResult GenerarVariacionesIndicadoresFlujoEfectivo()
        {
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;
            var resp = _repoCreditoPyme.GenerarVariaciones_Indicadores_FlujoEfectivo(dataCliente);

            if(resp != "200")
                return RedirectToAction("Notificacion", "ErrorHandler", new { id = resp });

            ///if (!(_settings.Environment.mode.Equals("development", StringComparison.InvariantCultureIgnoreCase)))
                Session["pasoVariaciones"] = "true";

            return RedirectToAction("DeclaracionIva", "CreditoPyme");
        }


        [ProtectGet]
        public ActionResult DeclaracionIva()
        {
            var b = Session["pasoVariaciones"] as string;
            if (!(_settings.Environment.mode.Equals("development", StringComparison.InvariantCultureIgnoreCase)) && b != "true")
                return RedirectToAction("TusBalances", "CreditoPyme");

            var resp = _repoCreditoPyme.GetMesesIva();

            if(resp.CodError != "200")
                return RedirectToAction("Notificacion", "ErrorHandler", new { id = resp.CodError });

            return View(resp);
        }

        [HttpPost]
        [ProtectPost]
        public JsonResult SaveDeclaracionIva()
        {
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;

            string mensaje = "", codigoError = "";

            var resp = _repoCreditoPyme.ProcesarPdfIva(ref dataCliente, Request, out codigoError, out mensaje);

            Session["datosClienteCP"] = dataCliente;

            if (string.IsNullOrEmpty(mensaje))
                mensaje = InitialConfig.getNotiMessage(codigoError, _settings).paragraph;

            return Json(new { codError = codigoError, mensaje });
        }

        [ProtectGet]
        public ActionResult ProcesoDeclaracionIVA()
        {
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;

            var resp = _repoCreditoPyme.ProcesarDeclaracionIVA(dataCliente);

            if(resp != "200")
                return RedirectToAction("Notificacion", "ErrorHandler", new { id = resp });

            return RedirectToAction("TusVentas", "CreditoPyme", new { id = resp });
        }


        [ProtectGet]
        public ActionResult TusVentas()
        {
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;
            var dataForView = new FromFormVentas();
            if(dataCliente.infoVentas != null)
            {
                dataForView = dataCliente.infoVentas; 
            }
            
            //var resp = _repoCreditoPyme.GetProductos(dataCliente.Cedula);
            return View(dataForView);
        }

        [HttpPost]
        [ProtectPost]
        public JsonResult SendVentas(FromFormVentas data)
        {
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;
            dataCliente.infoVentas = data;
            Session["datosClienteCP"] = dataCliente;

            var resp = _repoCreditoPyme.RegistrarProductos(dataCliente.Cedula, data);

            return Json(new { codError = resp, mensaje = InitialConfig.getNotiMessage(resp, _settings).paragraph });
        }

        [ProtectGet]
        public ActionResult ClientesProveedores()
        {
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;
            if(dataCliente.infoClientesProveedores!= null)
            {
                return View(dataCliente.infoClientesProveedores);
            }
            //var resp = _repoCreditoPyme.GetClientesProveedores(dataCliente.Cedula);
            return View(new FromFormClientes());
        }

        [HttpPost]
        [ProtectPost]
        public JsonResult SendClientesProveedores(FromFormClientes data)
        {
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;
            dataCliente.infoClientesProveedores = data;
            Session["datosClienteCP"] = dataCliente;

            _repoCreditoPyme.RegistrarClientesProveedores(dataCliente.Cedula, data);
            return Json(new { codError = "200" });
        }


        [ProtectGet]
        public ActionResult DireccionNegocio()
        {
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;
            var data = new ForViewInfoNegocioDto();

            //data.SeleccionAnterior = dataCliente.infoNegocio;

            var resp = _repoCreditoPyme.GetDataForViewDireccionNegocio(ref data, dataCliente, _repoCatalogo);

            if (resp != "200")
                return RedirectToAction("Notificacion", "ErrorHandler", new { id = resp });

            return View(data);
        }

        [ProtectGet]
        public ActionResult CertificacionNegocio()
        {
            ForViewCertNegocio resp = new ForViewCertNegocio();

            resp.Certificaciones = _repoCatalogo.GetDataCboxCertificaciones();

            if (resp == null)
                return RedirectToAction("Notificacion", "ErrorHandler", new { id = "503" });

            return View(resp);
        }

        [HttpPost]
        public JsonResult SaveCertificaciones(string data)
        {
            if (string.IsNullOrEmpty(data))
                return Json(new { codError = "400", mensaje = "Petición incorrecta" });
            FromFormNegocioCertf info = null;

            try {info = JsonConvert.DeserializeObject<FromFormNegocioCertf>(data);}
            catch (System.Exception){
                return Json(new { codError = "500", mensaje = "Petición incorrecta" });}

            var dataCliente = Session["datosClienteCP"] as DatosClientePy;
            dataCliente.infoCertificaciones = info;
            Session["datosClienteCP"] = dataCliente;
            //aqui va metodo registrar cert
            var resp = _repoCreditoPyme.RegistrarCertificaciones(dataCliente.Cedula, info);

            return Json(new { codError = resp, mensaje = InitialConfig.getNotiMessage(resp, _settings).paragraph });
        }

        [ProtectGet]
        public ActionResult ResumenNegocio()
        {
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;


            return View(dataCliente);
        }

        [ProtectGet]
        public ActionResult ReferenciaBancaria()
        {
            var objForView = new ForViewReferenciaBancaria();
            objForView.bancos = _repoCatalogo.GetDataCboxBancos();

            if (objForView.bancos == null)
                return RedirectToAction("Notificacion", "ErrorHandler", new { id = "503" });
            return View(objForView);
        }

        [HttpPost]
        [ProtectPost]
        public JsonResult SaveReferenciaBancaria(ReferenciasBancariasPyme[] data)
        {
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;
            var resp = _repoCreditoPyme.RegistrarReferenciasBancariasSolicitud(dataCliente.Cedula, data);

            if (resp != "200")
                return Json(new { codError = resp, mensaje = InitialConfig.getNotiMessage(resp, _settings).paragraph });

            var resp2 = _repoCreditoPyme.ConsultarCuentasCliente(dataCliente.Cedula);
            

            return Json(new { codError = resp2.errorCode, mensaje = InitialConfig.getNotiMessage(resp2.errorCode, _settings).paragraph, numCtas = resp2.NumCuentas });
        }

        [HttpPost]
        [ProtectPost]
        public JsonResult SaveCuentaToDebDep(FromFormCuentaParaCredito data)
        {
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;

            FromFormCuentaParaCredito info = null;
            info = data;
            dataCliente.infoCuentas = info;
            Session["datosClienteCP"] = dataCliente;
            var resp = _repoCreditoPyme.SaveCtasParaCredito(dataCliente.Cedula, data);

            return Json(new { codError = resp, mensaje = InitialConfig.getNotiMessage(resp, _settings).paragraph });
        }

        [HttpGet]
        public ActionResult ResumenFinal()
        {
           
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;
            var resp = _repoCreditoPyme.GenerarAnalisisCualitativo(dataCliente);

            if (resp != "200")
                return RedirectToAction("Notificacion", "ErrorHandler", new { id = resp });


            if (dataCliente?.infoCuentas == null)
            { //si no guardo cuentas
                FromFormCuentaParaCredito info = new FromFormCuentaParaCredito();
                info.cuentaDeb = "Cuenta nueva 00000000";
                info.cuentaDep = "Cuenta nueva 00000000";
                dataCliente.infoCuentas = info;
            }
            return View(dataCliente);
            
        }

        [HttpPost]
        [ProtectPost]
        public JsonResult ConsultaHostResumenFinal(object obj)
        {
            string CodError = "" ;
            string MensajeError = "";
           
            var dataCliente = Session["datosClienteCP"] as DatosClientePy;
            var resp = _repoCreditoPyme.ConsultarHostRiesgosGarantias(dataCliente.idProceso, dataCliente.Cedula, ref CodError, ref MensajeError);

            return Json(new { codError = "200", mensaje = InitialConfig.getNotiMessage("we", _settings).paragraph });
        }

        [HttpPost]
        [ProtectPost]
        public ActionResult CreaCodigoOTP(string data)
        {
            string mensajeRetorno;
            var dataClient = Session["datosClienteCP"] as DatosClientePy;
            var resp = _OtpRepo.GenerarOTP(dataClient.Cedula, dataClient.Celular, out mensajeRetorno);

            return Json(new { codError = resp, mensaje = mensajeRetorno, celular = Extentions.maskMobile(dataClient.Celular) }); 
        }

        [HttpPost]
        [ProtectPost]
        public ActionResult ValidaOTP(string data)
        {
            string codigoRetorno;
            string mensajeRetorno;

            var dataClient = Session["datosClienteCP"] as DatosClientePy;

            var resp = _OtpRepo.ValidarOTPCP(dataClient.Cedula, data, out codigoRetorno, out mensajeRetorno);


            if (codigoRetorno == "503")
                return Json(new { codError = "503", mensaje = InitialConfig.getNotiMessage(codigoRetorno, _settings).paragraph });

            if (codigoRetorno == "099")
                Session.Clear();

            return Json(new { codError = resp, mensaje = mensajeRetorno });
        }

        [HttpGet]
        public ActionResult AprobacionSolicitud()
        {

            var dataCliente = Session["datosClienteCP"] as DatosClientePy;

            
            return View(dataCliente);

        }

    }
}