using AutoMapper;
using BG.ContratacionWeb.Pyme.Dtos;
using BG.ContratacionWeb.Pyme.Helpers;
using BG.ContratacionWeb.Pyme.Infrastructure;
using System;

namespace BG.ContratacionWeb.Pyme.Data
{
    public interface IOtpRepository
    {
        string GenerarOTP(string cedula, string celular, out string mensajeRetorno);

        string ValidarOTP(SobreGiroDetalleSolicitudDto requisition,string cedula, string posibleOtp, out long idExpediente, out string codigoRetorno, out string mensajeRetorno);

        string ValidarOTPCP(string cedula, string posibleOtp, out string codigoRetorno, out string mensajeRetorno);
    }



    public class OtpRepository : IOtpRepository
    {
        private readonly ServicioContratacionProductos.WS_ContratacionProductosOnline _ContratacionProducto;
        private readonly CommonApplicationSettings _settings;

        public OtpRepository(ServicioContratacionProductos.WS_ContratacionProductosOnline ContratacionProducto
                                    , CommonSettingsManager settings)
        {
            _ContratacionProducto = ContratacionProducto;
            _settings = settings.Config;
        }


        public string GenerarOTP(string cedula, string celular, out string mensajeRetorno)
        {
            string resp = null;
            mensajeRetorno = "";
            try
            {
                resp = _ContratacionProducto.GeneraOtp(cedula, celular, _settings.EnvioOTP.opidUsuario, _settings.ProductoSG.NombreProducto, out mensajeRetorno);
            }
            catch (Exception e)
            {
                resp = "503";
                mensajeRetorno = InitialConfig.getNotiMessage("503", _settings).paragraph;
            }
            if (resp == "0000")
                resp = "200";
            else
                resp = "400";

            return resp;
        }


        public string ValidarOTP(SobreGiroDetalleSolicitudDto requisition, string cedula, string posibleOtp, out long idExpediente, out string codigoRetorno, out string mensajeRetorno)
        {
            mensajeRetorno = "";
            string resp;
            idExpediente = 0;
            codigoRetorno = "0000";
            try
            {
                ServicioContratacionProductos.Solicitud solicitud = new ServicioContratacionProductos.Solicitud();

                Mapper.Map(requisition, solicitud);
                Mapper.Map(_settings, solicitud);
                solicitud.IdentificacionCliente = cedula;

                var resp1 = _ContratacionProducto.ValidarOtpSobregiro(cedula, posibleOtp, _settings.ProductoSG.IdProductoNeo,
                                                                   solicitud, out idExpediente, out codigoRetorno, out mensajeRetorno);
            }
            catch (Exception e)
            {
                codigoRetorno = "503";
                mensajeRetorno = InitialConfig.getNotiMessage("503", _settings).paragraph;
                return "0";
            }

            if (codigoRetorno == "099")
            {
                codigoRetorno = "200";
                return "099";
            }
            if (codigoRetorno == "000")
            {
                codigoRetorno = "200";
                return "000";
            }
            if (codigoRetorno == "0001" || codigoRetorno == "9999")
            {
                codigoRetorno = "400";
                mensajeRetorno = "El código ingresado está incorrecto. Por favor ingrésalo nuevamente";
                return "";
            }

            return "";
        }

        public string ValidarOTPCP(string cedula, string posibleOtp, out string codigoRetorno, out string mensajeRetorno)
        {
            mensajeRetorno = "";
            string resp;
            codigoRetorno = "0000";
            try
            {
                resp = _ContratacionProducto.ValidaOTP(cedula, posibleOtp, out mensajeRetorno);

            }
            catch (Exception e)
            {
                resp = "503";
                codigoRetorno = "503";

            }
            if (resp == "0000")
                resp = "200";
            else
                resp = "400";

            return resp;
        }


    }
}
