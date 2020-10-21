using BG.ContratacionWeb.Pyme.Dtos;
using BG.ContratacionWeb.Pyme.Helpers;
using BG.ContratacionWeb.Pyme.Infrastructure;
using BG.ContratacionWeb.Pyme.Models;
using System;
using System.Data;
using System.Linq;

namespace BG.ContratacionWeb.Pyme.Data
{
    public interface ISobreGiroRepository
    {
        string ObtenerTasaInteresProducto(out string mensajeRetorno, out decimal TasaNominal, out decimal TasaEfectiva, out decimal FactorReajuste);

        DatosForSulicitudViewDto GetDatosInicio(ref DatosCliente datosCliente);
    }



    public class SobreGiroRepository : ISobreGiroRepository
    {
        private readonly ServicioContratacionProductos.WS_ContratacionProductosOnline _ContratacionProducto;
        private readonly CommonApplicationSettings _settings;

        public SobreGiroRepository(ServicioContratacionProductos.WS_ContratacionProductosOnline ContratacionProducto
                                    , CommonSettingsManager settings)
        {
            _ContratacionProducto = ContratacionProducto;
            _settings = settings.Config;
        }


        public DatosForSulicitudViewDto GetDatosInicio(ref DatosCliente datosCliente)
        {
            var cedula = datosCliente.Cedula;
            string codigoRetorno = "000", mensajeRetorno = "";

            DataNamesMapper<DatosCliente> mapperToclient = new DataNamesMapper<DatosCliente>();
            DataNamesMapper<CuentasSobreGiro> mapperToCtas = new DataNamesMapper<CuentasSobreGiro>();
            DataNamesMapper<Holidays> mapperToHolidays = new DataNamesMapper<Holidays>();
            DatosForSulicitudViewDto data = new DatosForSulicitudViewDto();

            //obterner informacion del cliente
            DataSet infoCliente = null;
            try
            {
                infoCliente = _ContratacionProducto.ConsultaInformacionClienteCore("C", cedula, out codigoRetorno, out mensajeRetorno);
            }
            catch (Exception e)
            {
                data.codigoRetorno = "503";
                data.mensajeRetorno = "Servicio \"ConsultaInformacionClienteCore\" no disponible. mas detalles: " + e.Message;
                return data;
            }

            if (infoCliente.Tables.Contains("DatosBasicosCliente") && codigoRetorno == "000")
            {
                datosCliente = mapperToclient.Map(infoCliente.Tables["DatosBasicosCliente"].Rows[0]);
                datosCliente.Cedula = cedula;
            }
            else
            {
                data.codigoRetorno = "500";
                data.mensajeRetorno = "Algo salió mal, Servicio \"ConsultaInformacionClienteCore\" no retornó informacion. mas detalles: " + mensajeRetorno;
                return data;
            }

            //obtener cuentas para sobregiro del cliente
            DataSet infoCtas = null;
            try
            {
                infoCtas = _ContratacionProducto.ObtenerCuentasCCSobregiro("C", cedula, out codigoRetorno, out mensajeRetorno);
            }
            catch (Exception e)
            {
                data.codigoRetorno = "503";
                data.mensajeRetorno = "Servicio \"ObtenerCuentasCCSobregiro\" no disponible. mas detalles: " + e.Message;
                return data;
            }

            if (infoCtas.Tables.Contains("CUENTASOBREGIRO") && infoCtas.Tables["CUENTASOBREGIRO"].Rows.Count > 0 && codigoRetorno == "000")
            {
                data.Cuentas = mapperToCtas.Map(infoCtas.Tables["CUENTASOBREGIRO"]).ToArray();
            }
            else
            {
                data.codigoRetorno = "500";
                data.mensajeRetorno = "Algo salió mal, Servicio \"ObtenerCuentasCCSobregiro\" no retornó informacion. mas detalles: " + mensajeRetorno;
                return data;
            }

            //obtener feriados

            DataSet infoDias = null;
            try
            {
                infoDias = _ContratacionProducto.ObtenerDiasFeriado("1", out codigoRetorno, out mensajeRetorno);
            }
            catch (Exception e)
            {
                data.codigoRetorno = "503";
                data.mensajeRetorno = "Servicio \"ObtenerDiasFeriado\" no disponible. mas detalles: " + e.Message;
                return data;
            }

            if (infoDias.Tables.Contains("DiasFeriados") && infoDias.Tables["DiasFeriados"].Rows.Count > 0 && codigoRetorno == "000")
            {
                data.Holidays = mapperToHolidays.Map(infoDias.Tables["DiasFeriados"]).ToArray();
            }

            return data;
        }




        public string ObtenerTasaInteresProducto(out string mensajeRetorno, out decimal TasaNominal, out decimal TasaEfectiva, out decimal FactorReajuste)
        {
            mensajeRetorno = "";
            TasaNominal = 0;
            TasaEfectiva = 0;
            FactorReajuste = 0;
            string resp = "";
            try
            {
                resp = _ContratacionProducto.ObtenerTasaInteresProducto(_settings.ProductoSG.idCanal, _settings.ProductoSG.IdProductoNeo, _settings.PeriodicidadDias,
                                                                  out mensajeRetorno, out TasaNominal, out TasaEfectiva, out FactorReajuste);
            }
            catch (Exception e)
            {
                mensajeRetorno = "503";
            }

            return resp;
        }

    }
}
