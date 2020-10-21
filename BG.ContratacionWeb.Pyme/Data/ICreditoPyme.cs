using AutoMapper;
using BG.ContratacionWeb.Pyme.Dtos;
using BG.ContratacionWeb.Pyme.Helpers;
using BG.ContratacionWeb.Pyme.Infrastructure;
using BG.ContratacionWeb.Pyme.Models;
using BG.ContratacionWeb.Pyme.ServicioContratacionProductos;
using BG.ContratacionWeb.Pyme.ServicioRatingEmpresarial;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace BG.ContratacionWeb.Pyme.Data
{
    public interface ICreditoPyme
    {
        DatosForSimuladorViewDto GetDatosInicio(string cedula, out string mensajeRetorno);
        DatosClientePy GetInfoClientePy(string cedula, out string codError, out string mensajeError);
        string CalculaCuotaMensual(FromFormSolicitudCreditoDto datosCredito, string cedula, ref string codError);
        string CalcularCuotaAlVencimiento(FromFormSolicitudCreditoDto data);
        byte[] GetTablaAmortizacion(DatosClientePy dataCliente, FromFormSolicitudCreditoDto soli);
        string RegistrarSolitud(string cedula, FromFormSolicitudCreditoDto soli);
        string ActualizarDatosCliente(DatosClientePy datosCliente);
        string RegistrarDireccionDomOrNeg(string cedula, dynamic data);

        string GetDatosForViewDireccionDomicilio(ref ForViewInfoDomicilioDto data, DatosClientePy datosCliente, ICatalogoRepository _repo);
        string GetDataForViewDireccionNegocio(ref ForViewInfoNegocioDto data, DatosClientePy datosCliente, ICatalogoRepository _repo);

        Anyo ConsultaAnyos(ref DatosClientePy dataCliente, out string codError, out string mensajeError);
        string ProcesarPdfSRI(DatosClientePy dataCliente, HttpRequestBase request, out string codigoError, out string mensajeError);
        string GenerarVariaciones_Indicadores_FlujoEfectivo(DatosClientePy dataCliente);
        MesesIva GetMesesIva();
        string ProcesarPdfIva(ref DatosClientePy dataCliente, HttpRequestBase request, out string codigoError, out string mensajeError);
        string ProcesarDeclaracionIVA(DatosClientePy dataCliente);

        string RegistrarProductos(string cedula, FromFormVentas ventas);
        string RegistrarClientesProveedores(string cedula, FromFormClientes clientesproveedores);

        //FromFormVentas GetProductos(string cedula);
        //FromFormClientes GetClientesProveedores(string cedula);
        string RegistrarCertificaciones(string cedula, FromFormNegocioCertf soli);
        string RegistrarReferenciasBancariasSolicitud(string cedula, ReferenciasBancariasPyme[] ReferenciasBancarias);

        RespConsultarCtasCliente ConsultarCuentasCliente(string cedula);

        string SaveCtasParaCredito(string cedula, FromFormCuentaParaCredito data);

        string GenerarAnalisisCualitativo(DatosClientePy dataCliente);

        string ConsultarHostRiesgosGarantias(string idProceso, string cedula, ref string CodError, ref string MensajeError);

        EstadoCivilClienteDto ConsultaEstadoCivilCliente(string cedula);
    }

    public class CreditoPyme : ICreditoPyme
    {
        private readonly ServicioContratacionProductos.WS_ContratacionProductosOnline _ContratacionProducto;
        private readonly wsConsultaExterna _ServicioRatingEmpresarial;
        
        private readonly CommonApplicationSettings _settings;

        public CreditoPyme(ServicioContratacionProductos.WS_ContratacionProductosOnline ContratacionProducto, CommonSettingsManager settings,
                                        wsConsultaExterna ServicioRatingEmpresarial)
        {
            _ContratacionProducto = ContratacionProducto;
            _settings = settings.Config;
            _ServicioRatingEmpresarial = ServicioRatingEmpresarial;
        }

        public DatosForSimuladorViewDto GetDatosInicio(string cedula, out string mensajeRetorno)
        {
            var data = new DatosForSimuladorViewDto();
            ServicioContratacionProductos.Catalogo[] catalogos;
            string resp;
            decimal TasaNominal, TasaEfectiva, FactorReajuste;
            string codigoRetorno;
            DataTable response;
            data.Rango = _settings.RangoDeCreditoPyme;

            try
            {
                //catalogos: (al vencimieto/mensualmente), (3,6,9,12,18 meses), (destino credito)
                _ContratacionProducto.ConsultarCatalogos(_settings.ProductoCP.idCanal, _settings.ProductoCP.IdProductoPadreNeo, 305,out mensajeRetorno, out catalogos);
                data.ModosDePago = catalogos;
                _ContratacionProducto.ConsultarCatalogos(_settings.ProductoCP.idCanal, _settings.ProductoCP.IdProductoPadreNeo, 306, out mensajeRetorno, out catalogos);
                if (catalogos != null && catalogos.Length > 0)
                    data.PlazosCredito = catalogos.OrderBy(c => c.IdCodigo).ToArray();
                _ContratacionProducto.ConsultarCatalogos(_settings.ProductoCP.idCanal, _settings.ProductoCP.IdProductoPadreNeo, 307, out mensajeRetorno, out catalogos);
                data.DestinosCredito = catalogos;

                resp = _ContratacionProducto.ObtenerTasaInteresProducto(_settings.ProductoCP.idCanal, _settings.ProductoCP.IdProductoNeo, _settings.PeriodicidadDias,
                                                                  out mensajeRetorno, out TasaNominal, out TasaEfectiva, out FactorReajuste);

                data.TasaEfectiva = TasaEfectiva.ToString();
                data.TasaNominal = TasaNominal.ToString();
                response = _ContratacionProducto.ConsultarCtaAhorroCorriente("C", cedula, out codigoRetorno, out mensajeRetorno);
                mensajeRetorno = "OK";
                if (response != null)
                    data.TieneCuentaAhorro = response.Select("tipoCuentaDesc = 'Ahorros'").Length == 0 ? false : true;
                else
                    data.TieneCuentaAhorro = false;

                //config producto  (ME = 2098, AD = 2099)
                data.ConfigParamProduct = GetProductParams("2098", out codigoRetorno);

            }
            catch (System.Exception)
            {
                mensajeRetorno = "503";
                return data;
            }
            
            return data;
        }

        public ConfigParamProduct[] GetProductParams(string idProducto, out string codRetorno)
        {
            string mensaje; DataSet data = null;
            ConfigParamProduct[] resp = null;
            try
            {
                data = _ContratacionProducto.ConsultaParametrosProducto(idProducto, out codRetorno, out mensaje);
            }
            catch (Exception)
            {
                codRetorno = "503";  mensaje = "";
            }

            if(data != null)
            {
                DataNamesMapper<ConfigParamProduct> mapperToConfigParam = new DataNamesMapper<ConfigParamProduct>();
                resp = mapperToConfigParam.Map(data.Tables["Table"]).ToArray();
            }

            return resp;
        }

        public DatosClientePy GetInfoClientePy(string cedula, out string codError, out string mensajeError)
        {
            DataNamesMapper<DatosClientePy> mapperToclient = new DataNamesMapper<DatosClientePy>();
            DatosClientePy infoCliente = null;
            DataSet data = null;
            try
            {
                data = _ContratacionProducto.ConsultaDatosClienteCoreAll(cedula, "C", out codError, out mensajeError);
            }
            catch (System.Exception)
            {
                codError = "503";
                mensajeError = InitialConfig.getNotiMessage(codError, _settings).paragraph;
                return null;
            }
            int paso = 0;
            int idSolicitud = 0;
            DataRow row = null;
            DataTable table = null;
            if (data.Tables.Contains("DatosBasicosCliente") && codError == "000")
            {
                row = data.Tables["DatosBasicosCliente"].Rows[0];
                infoCliente = mapperToclient.Map(row);
                var pagina = row.Field<string>("ID_PASO_SOLICITUD");
                var idSoli = row.Field<string>("ID_SOLICITUD");
                paso = pagina.ToInt();
                idSolicitud = idSoli.ToInt();
                //EstadoCivilFromCore
                DataNamesMapper<EstadoCivilFromCore> mapperToEcFromCore = new DataNamesMapper<EstadoCivilFromCore>();
                infoCliente.estadoCivilFormCore = mapperToEcFromCore.Map(row);
            }
            if (paso == 0 && idSolicitud == 0)
                return infoCliente;


            if (paso >= 1) // Simulador | datos cliente
            {
                infoCliente.infoSolicitud = Mapper.Map<FromFormSolicitudCreditoDto>(row);
                //DataNamesMapper<SolicitudSeleccion> mapperToSeleccion1 = new DataNamesMapper<SolicitudSeleccion>();
                //infoCliente.SolicitudSeleccionAnt = mapperToSeleccion1.Map(row);
            }
            if (paso >= 2)//no definido     (estado Civil)
            {

            }
            if (paso >= 3) //Direccion domicilio | Confirmacion Datos Cliente
            {
                DataNamesMapper<FromFormInfoDomicilio> mapperToSeleccion3 = new DataNamesMapper<FromFormInfoDomicilio>();
                infoCliente.infoDomicilio = mapperToSeleccion3.Map(row);
                infoCliente.ActionRoute = "InfoDomicilio";
            }
            if (paso >= 4) //Carga Balances
            {

            }
            if (paso >= 5) //Carga IVA
            {

            }
            if (paso >= 6) //Ventas Cliente
            {
                DataNamesMapper<Models.Producto> mapperToSeleccion6 = new DataNamesMapper<Models.Producto>();
                var seleccionado = new FromFormVentas();
                seleccionado.productos = mapperToSeleccion6.Map(data.Tables["Productos"]).ToList();
                seleccionado.numeroEmpleados = row["NUM_EMPLEADOS"].ToString().ToInt();
                infoCliente.infoVentas = seleccionado;
                infoCliente.ActionRoute = "TusVentas";
            }
            if (paso >= 7) //Clientes-Proveedores
            {
                List<ClientesProveedores> clientesProveedores = new List<ClientesProveedores>();
                DataNamesMapper<ClientesProveedores> mapperToSeleccion7 = new DataNamesMapper<ClientesProveedores>();
                FromFormClientes forWiew = new FromFormClientes();

                if (data.Tables.Contains("Clientes") && codError == "000")
                {
                    var t = data.Tables["Clientes"].getWithAddedCol("C");
                    clientesProveedores = mapperToSeleccion7.Map(t).ToList();
                }
                if (data.Tables.Contains("Proveedores") && codError == "000")
                {
                    var t = data.Tables["Proveedores"].getWithAddedCol("P");
                    var r = mapperToSeleccion7.Map(t).ToList();
                    if (clientesProveedores != null)
                        clientesProveedores = clientesProveedores.Concat(r).ToList();
                    else
                        clientesProveedores = r;
                }
                forWiew.clientes = clientesProveedores;
                infoCliente.infoClientesProveedores = forWiew;
                infoCliente.ActionRoute = "ClientesProveedores";
            }
            if (paso >= 8) //Direccion Negocio
            {
                DataNamesMapper<FromFormInfoDirecNeg> mapperToSeleccion8 = new DataNamesMapper<FromFormInfoDirecNeg>();
                infoCliente.infoNegocio = mapperToSeleccion8.Map(row);
                infoCliente.ActionRoute = "DireccionNegocio";
            }
            if (paso >= 9)  //Certificacion | Confirmacion Datos Negocio
            {

            }
            if (paso == 10) //Referencias Bancarias
            {

            }

            return infoCliente;
        }

        public string CalculaCuotaMensual(FromFormSolicitudCreditoDto datosCredito, string cedula, ref string codError)
        {
            string mensajeRetorno="", dividendo ="", totalCredito;

            try
            {
                var resp = _ContratacionProducto.SimulacionCreditoMate14("C", cedula, _settings.ProductoCP.IdProductoNeo.ToString(), "1", datosCredito.plazoCredito.Descripcion.GetMonths(),
                                                            datosCredito.amortizacion, datosCredito.monto + ".00", "", "0", "0", ref codError, ref mensajeRetorno, out dividendo, out totalCredito);
            }
            catch (System.Exception)
            {
                codError = "503";
            }
            
            return dividendo;
        }

        public string CalcularCuotaAlVencimiento(FromFormSolicitudCreditoDto data)
        {
            //round( (20000 *(1+ (11.23/36500)* 60 )),2)
            //round( (monto solicitado * ( 1 + (tasa de interés nominal /365000) número de días)),2)
            DateTime futureDay; int days = 60;
            DateTime now = Convert.ToDateTime(DateTime.Today.Date, new CultureInfo("es-EC"));
            if (DateTime.TryParse(data.diaDePago, CultureInfo.CreateSpecificCulture("es-EC"), DateTimeStyles.None, out futureDay))
            {
                TimeSpan ts = futureDay - now;
                days = ts.Days;
            }

            return Math.Round((int.Parse(data.monto) * (1 + (11.23 / 36500) * days)), 2).ToString();
        }
        

        public byte[] GetTablaAmortizacion(DatosClientePy dataCliente, FromFormSolicitudCreditoDto soli)
        {
            string CodigoRetorno, MensajeRetorno;
            byte[] pdf;
            try
            {
                pdf = _ContratacionProducto.SimularTablaAmortizacion("PDF", dataCliente.Cedula, "C", dataCliente.Nombres,
                    _settings.ProductoSG.IdProductoNeo.ToString(), soli.amortizacion, soli.monto + ".00", soli.plazoCredito.Descripcion.Split(' ')[0], "1", out CodigoRetorno, out MensajeRetorno);
            }
            catch (Exception)
            {
                pdf = null;
                CodigoRetorno = "503";
                MensajeRetorno = "servicio no desoinible por el momento.";
            }

            return pdf;
        }

        public string RegistrarSolitud(string cedula, FromFormSolicitudCreditoDto soli)
        {
            string resp, mensajeRetorno, codigoRetorno;
            try
            {
                resp = _ContratacionProducto.RegistrarSolicitudCreditoPyme(cedula, soli.monto, soli.destinoCredito.idCodigo, 
                                                                            soli.modoDePago.idCodigo, soli.plazoCredito.idCodigo, 
                                                                            soli.amortizacion, soli.fechaPago.ToInt(), soli.diaDePago, 
                                                                            soli.tasaNominal, soli.cuota, out codigoRetorno, out mensajeRetorno);
            }
            catch (Exception)
            {
                resp = "503";
            }

            return resp;
        }

        public string ActualizarDatosCliente(DatosClientePy datosCliente)
        {
            string codigoRetorno, mensajeRetorno, resp;
            try
            {
                //ActualizarDatosClienteSolicitudCredito
                resp = _ContratacionProducto.ActualizarDatosClienteSolicitudCredito(datosCliente.Cedula, datosCliente.Nombres, datosCliente.FechaNacimiento, datosCliente.Celular, datosCliente.Email, 0, int.Parse(datosCliente.IdEstadoCivil), out codigoRetorno, out mensajeRetorno);
            }
            catch (Exception)
            {
                codigoRetorno = "503"; mensajeRetorno="";
                resp = null;
            }

            return resp;
        }

        public string GetDatosForViewDireccionDomicilio( ref ForViewInfoDomicilioDto data, DatosClientePy datosCliente, ICatalogoRepository _repo)
        {
            
            Catalogo[] catalogos = null;
            if (datosCliente.infoDomicilio != null)
            {
                data.SeleccionAnterior = datosCliente.infoDomicilio;
                //DireccionDomicilioSeleccion seleccion = data.SeleccionAnterior;
                if (datosCliente.infoDomicilio.provincia != 0)
                {
                    catalogos = _repo.GetDataCboxDireccionBy("ciudad", datosCliente.infoDomicilio.provincia.ToString());
                }

                if (catalogos != null)
                    data.ciudad = catalogos;
                else
                    return "503";

                if (data.SeleccionAnterior.ciudad != 0)
                {
                    catalogos = _repo.GetDataCboxDireccionBy("parroquia", datosCliente.infoDomicilio.ciudad.ToString());
                }

                if (catalogos != null)
                    data.parroquia = catalogos;
                else
                    return "503";
            }

            catalogos = _repo.GetDataCboxTipoDireccion();

            if (catalogos != null)
                data.tiposDireccion = catalogos;
            else
                return "503";

            catalogos = _repo.GetDataCboxDireccionBy("provincia");

            if (catalogos != null)
                data.comboOptionLocation = catalogos;
            else
                return "503";

            data.direccion = datosCliente.DireccionDomicilio;

            return "200";
        }

        public string GetDataForViewDireccionNegocio(ref ForViewInfoNegocioDto data, DatosClientePy datosCliente, ICatalogoRepository _repo)
        {
            Catalogo[] catalogos = null;
            if (datosCliente.infoNegocio != null)
            {
                data.SeleccionAnterior = datosCliente.infoNegocio;
                //DireccionNegocioSeleccion seleccion = data.SeleccionAnterior;
                if (datosCliente.infoNegocio.provincia != 0)
                {
                    catalogos = _repo.GetDataCboxDireccionBy("ciudad", datosCliente.infoNegocio.provincia.ToString());
                }

                if (catalogos != null)
                    data.ciudad = catalogos;
                else
                    return "503";

                if (datosCliente.infoNegocio.ciudad != 0)
                {
                    catalogos = _repo.GetDataCboxDireccionBy("parroquia", datosCliente.infoNegocio.ciudad.ToString());
                }

                if (catalogos != null)
                    data.parroquia = catalogos;
                else
                    return "503";
            }

            catalogos = _repo.GetDataCboxForAdress("tipoInmueble");

            if (catalogos != null)
                data.tipoInmueble = catalogos;
            else
                return "503";

            catalogos = _repo.GetDataCboxDireccionBy("provincia");

            if (catalogos != null)
                data.comboOptionLocation = catalogos;
            else
                return "503";

            catalogos = _repo.GetDataCboxForAdress("aseguradora");

            if (catalogos != null)
                data.aseguradora = catalogos;
            else
                return "503";

            data.direccion = datosCliente.DireccionDomicilio;

            return "200";
        }


        public string RegistrarDireccionDomOrNeg(string cedula, dynamic data)
        {

            if(data.tipoDomicilio == "N" || data.tipoDomicilio == "T" || data.tipoDomicilio == "D")
                data.tipoDomicilio = "Domicilio";
            if(data.tipoDomicilio == "B")
                data.tipoDomicilio = "Ambos";

            string codigoRetorno, mensajeRetorno;
            try
            {
                if(data is FromFormInfoDomicilio)
                {
                    _ContratacionProducto.RegistrarDomicilioClienteSolicitud(cedula, data.direccion1, data.direccion2, data.referencia,
                                                                        data.telefonoDomNeg, data.provincia, data.ciudad, data.parroquia,
                                                                        "", data.tipoDomicilio, 0, 0, out codigoRetorno, out mensajeRetorno);
                }
                else
                {
                    _ContratacionProducto.RegistrarDomicilioClienteSolicitud(cedula, data.direccion1, data.direccion2, data.referencia,
                                                                        data.telefonoDomNeg, data.provincia, data.ciudad, data.parroquia,
                                                                        data.tipoInmueble, data.tipoDomicilio, data.isSecure, data.aseguradora, out codigoRetorno, out mensajeRetorno);
                }

                
            }
            catch (Exception)
            {
                return "503";
            }

            return "200";
        }

        public string ProcesarPdfSRI(DatosClientePy dataCliente, HttpRequestBase request, out string codigoError, out string mensajeError)
        {
            string anyo = "";
            var text = request.ExtractTextFromPdf(out anyo, out codigoError, out mensajeError);
            if (codigoError != "200")
                return null;

            //string cleanedText = Regex.Replace(text.ToString(), @"http[^\s]+", "");

            string codeError ="", mensaje = "";
            try
            {
                var rekujwssp = _ServicioRatingEmpresarial.ProcesarPDFImpuestoRenta(dataCliente.Cedula.Trim() + "001", anyo, text.ToString(), dataCliente.idProceso, dataCliente.fechaRevision, "usuarioWeb", ref codeError, ref mensaje);
            }
            catch (Exception e)
            {
                var r = e.Message;
                codigoError = "503";
                return null;
            }

            if (codeError != "000")
            {
                codigoError = "400";
                mensajeError = mensaje;
            }
            else
            {
                codigoError = "200";
                mensajeError = "subido correctamente";
            }

            return "";
        }

        public Anyo ConsultaAnyos(ref DatosClientePy dataCliente, out string codError, out string mensajeError)
        {
            mensajeError = "";
            string idClienteRating;
            idClienteRating = ""; string idProceso = "", fechaRevision = "";
            DataSet resp = null; codError = "200";
            try
            {
                _ServicioRatingEmpresarial.Consultar_Ins_Updt_Clientes_Rating_Neo(dataCliente.Cedula, dataCliente.Nombres, dataCliente.DireccionDomicilio, ref codError, ref mensajeError, ref idClienteRating, ref idProceso, ref fechaRevision);
                resp = _ServicioRatingEmpresarial.ConsultaAnyosWeb(dataCliente.Cedula, fechaRevision, idProceso);
                dataCliente.idProceso = idProceso;
                var fechaSplited = fechaRevision.Split('-');
                dataCliente.fechaRevision = fechaSplited[2] + "/" + fechaSplited[1] + "/" + fechaSplited[0];
            }
            catch (Exception e)
            {
                codError = "503";
                return null;
            }

            string informacion = ""; Anyo anyo = new Anyo();
            var table = resp.Tables["Table"];
            var myColumn = table.Columns.Cast<DataColumn>().SingleOrDefault(col => col.ColumnName == "Informar");
        
            if(myColumn != null)
            {
                codError = "200";
                var tableRow = table.AsEnumerable().First();
                informacion = tableRow.Field<string>(myColumn);
            }else{
                codError = "500"; return null;
            }
            if(informacion.ToLower() == "incompleto")
            {
                DataNamesMapper<Anyo> mapperToAnyo = new DataNamesMapper<Anyo>();
                anyo = mapperToAnyo.Map(resp.Tables["table1"].Rows[0]);
            }
            anyo.Detalle = informacion.ToLower();
            anyo.Environment = _settings.Environment.mode;

            return anyo;
        }

        public string GenerarVariaciones_Indicadores_FlujoEfectivo(DatosClientePy dataCliente)
        {
            string codError="", mensajeError="";
            try
            {
                _ServicioRatingEmpresarial.GenerarVariaciones_Indicadores_FlujoEfectivo(dataCliente.idProceso, "", ref codError, ref mensajeError);
            }
            catch (Exception)
            {
                return "503";
            }

            if (codError != "000")
                return "500";

            return "200";
        }

        public MesesIva GetMesesIva()
        {
            string codError="", mensajeError="";
            MesesIva mesesIva = new MesesIva();
            try
            {
                DataSet resp = _ServicioRatingEmpresarial.ConsultaMesesIva(ref codError, ref mensajeError);
                DataNamesMapper<MesesIva> mapperToAnyo = new DataNamesMapper<MesesIva>();
                mesesIva = mapperToAnyo.Map(resp.Tables["table"].Rows[0]);
                mesesIva.CodError = "200";
                mesesIva.mes1SinAnyo = mesesIva.Mes1.Replace(" ", "").Split('-')[0];
                mesesIva.mes2SinAnyo = mesesIva.Mes2.Replace(" ", "").Split('-')[0];
                mesesIva.mes3SinAnyo = mesesIva.Mes3.Replace(" ", "").Split('-')[0];
            }
            catch (Exception)
            {
                mesesIva.CodError = "503";
                return mesesIva;
            }

            if(codError != "000")
            {
                mesesIva.CodError = "500";
                return mesesIva;
            }

            mesesIva.Environment = _settings.Environment.mode;

            return mesesIva;
        }

        public string ProcesarPdfIva(ref DatosClientePy dataCliente, HttpRequestBase request, out string codigoError, out string mensajeError)
        {
            string mes = "";
            var text = request.ExtractTextFromPdf(out mes, out codigoError, out mensajeError);
            if (codigoError != "200")
                return null;

            if (dataCliente.IvaVentaMes == null)
                dataCliente.IvaVentaMes = new IvaVentasMeses();

            var doc = request.Files.AllKeys[0].Split('-')[0];
            //string cleanedText = Regex.Replace(text.ToString(), @"http[^\s]+", "");

            string codeError = "", mensaje = "", totalVentasMes = "";
            try
            {
                totalVentasMes = _ServicioRatingEmpresarial.ProcesarPDFDeclaracionIVA(dataCliente.Cedula.Trim() + "001", mes, text.ToString(), DateTime.Now.ToString("yyyy/MM/dd"), ref codeError, ref mensaje);
            }
            catch (Exception)
            {
                codigoError = "503";
                return null;
            }

            if (codeError != "000")
            {
                codigoError = "400";
                mensajeError = mensaje;
            }
            else
            {
                codigoError = "200";
                mensajeError = "subido correctamente";
            }

            if (doc == "documento1")
                dataCliente.IvaVentaMes.Mes1 = totalVentasMes;
            if (doc == "documento2")
                dataCliente.IvaVentaMes.Mes2 = totalVentasMes;
            if (doc == "documento3")
                dataCliente.IvaVentaMes.Mes3 = totalVentasMes;

            return "";
        }

        public string ProcesarDeclaracionIVA(DatosClientePy dataCliente)
        {
            string codError="", mensajeError="";
            try
            {
                //_ServicioRatingEmpresarial.ProcesoDeclaracionIVA("CAL", dataCliente.idProceso, double.Parse(dataCliente.IvaVentaMes.Mes1), double.Parse(dataCliente.IvaVentaMes.Mes2), double.Parse(dataCliente.IvaVentaMes.Mes3), ref codError, ref mensajeError);
                codError = "000";
            }
            catch (Exception)
            {
                return "503";
            }

            if (codError != "000")
                return "500";
            return "200";
        }


        public string RegistrarProductos(string cedula, FromFormVentas ventas)
        {
            string resp="200", mensajeRetorno, codigoRetorno="";

            try
            {
                var Product = AutoMapper.Mapper.Map<ProductosPyme[]>(ventas.productos.ToArray());
                resp = _ContratacionProducto.RegistrarProductosSolicitud(cedula, Product, ventas.numeroEmpleados.ToString(), out codigoRetorno, out mensajeRetorno);
            }
            catch (Exception)
            {
                resp = "503";
            }
            if (codigoRetorno == "000")
                return "200";
            else
                return "400";
        }

        /*
        public FromFormVentas GetProductos(string cedula)
        {
            string mensajeRetorno, codigoRetorno;
            DataSet infoUsuario = null;
            FromFormVentas ventas = new FromFormVentas();
            List<Models.Producto> listaProductos = new List<Models.Producto>();
            
            try
            {
                infoUsuario = _ContratacionProducto.ConsultaDatosClienteCoreAll(cedula, "C", out codigoRetorno, out mensajeRetorno);
            }
            catch (Exception e)
            {
                codigoRetorno = "503";
                mensajeRetorno = "Servicio \"ConsultaDatosClienteCoreAll\" no disponible. mas detalles: " + e.Message;
                return null;
            }

            if (infoUsuario.Tables.Contains("Productos") && codigoRetorno == "000")
            {

                DataRowCollection drpc = infoUsuario.Tables["Productos"].Rows;
                DataRow drsc = infoUsuario.Tables["DatosBasicosCliente"].Rows[0];

                foreach (DataRow dr in drpc)
                {
                    Models.Producto producto = new Models.Producto();
                    producto.nombre = dr["nombreProducto"].ToString(); 
                    producto.porcentajeVentas = int.Parse(dr["porcentajeVentas"].ToString());
                    listaProductos.Add(producto);
                }
                ventas.productos = listaProductos;
                ventas.numeroEmpleados = int.Parse(drsc["NUM_EMPLEADOS"].ToString());

                return ventas;
            }

            else
            {
                codigoRetorno = "503";
                mensajeRetorno = "Servicio \"ConsultaDatosClienteCoreAll\" no disponible.";
                return ventas;
            }

        }
        */
        /*public FromFormClientes GetClientesProveedores(string cedula)
        {
            string mensajeRetorno, codigoRetorno;
            DataSet infoUsuario = null;
            FromFormClientes clientesproveedores = new FromFormClientes();
            List<ClientesProveedores> listaClientesProveedores = new List<ClientesProveedores>();

            try
            {
                infoUsuario = _ContratacionProducto.ConsultaDatosClienteCoreAll(cedula, "C", out codigoRetorno, out mensajeRetorno);
            }
            catch (Exception e)
            {
                codigoRetorno = "503";
                mensajeRetorno = "Servicio \"ConsultaDatosClienteCoreAll\" no disponible. mas detalles: " + e.Message;
                return null;
            }

            if (infoUsuario.Tables.Contains("Clientes") && codigoRetorno == "000" || infoUsuario.Tables.Contains("Proveedores") && codigoRetorno == "000")
            {
                DataRowCollection drcc = infoUsuario.Tables["Clientes"].Rows;
                DataRowCollection drpc = infoUsuario.Tables["Proveedores"].Rows;

                if (infoUsuario.Tables.Contains("Clientes"))
                    {
                  
                    foreach (DataRow dr in drcc)
                    {
                        Models.ClientesProveedores clientes = new Models.ClientesProveedores();
                        clientes.nombre = dr["nombre"].ToString();
                        clientes.porcentaje = int.Parse(dr["porcentaje"].ToString());
                        clientes.dias = int.Parse(dr["dias"].ToString());
                        clientes.tipo = "C";
                        listaClientesProveedores.Add(clientes);
                    }
                 
                  if(infoUsuario.Tables.Contains("Proveedores"))
                    {
                        foreach(DataRow dr in drpc)
                        {
                            Models.ClientesProveedores proveedores = new Models.ClientesProveedores();
                            proveedores.nombre = dr["nombre"].ToString();
                            proveedores.porcentaje = int.Parse(dr["porcentaje"].ToString());
                            proveedores.dias = int.Parse(dr["dias"].ToString());
                            proveedores.tipo = "P";
                            listaClientesProveedores.Add(proveedores);
                        }
                    }
                
                }
                clientesproveedores.clientes = listaClientesProveedores;
                

                return clientesproveedores;
            }

            else
            {
                codigoRetorno = "503";
                mensajeRetorno = "Servicio \"ConsultaDatosClienteCoreAll\" no disponible.";
                return clientesproveedores;
            }

        }

        */
        public string RegistrarClientesProveedores(string cedula, FromFormClientes clientesproveedores)
        {
            string resp, mensajeRetorno, codigoRetorno;


            try
            {
                var ClienteProveedor = AutoMapper.Mapper.Map<ServicioContratacionProductos.ClientesPyme[]>(clientesproveedores.clientes.ToArray());
                resp = _ContratacionProducto.RegistrarClienteProveedorSolicitud(cedula, ClienteProveedor, out codigoRetorno, out mensajeRetorno);
            }
            catch (Exception)
            {
                resp = "503";
            }

            return resp;
        }

        public string RegistrarCertificaciones(string cedula, FromFormNegocioCertf soli)
        {
            String[] certfs = null;
            string otra = null, codigoRetorno, mensajeRetorno;
            List<string> tempList = new List<string>();
            if (soli.TieneCertificacion == 1)
            {
                
                foreach(var item in soli.Certificaciones)
                {
                    if (item.Descripcion != "Otra")
                        tempList.Add(item.IdCodigo.ToString());
                    else
                        otra = item.Descripcion;
                }
                certfs = tempList.ToArray();
            }
            try
            {
                var resp = _ContratacionProducto.RegistrarCertificacionesSolicitud(cedula, certfs, otra, out codigoRetorno, out mensajeRetorno);
            }
            catch (Exception)
            {
                return "503";
            }

            return "200";
        }

        public string RegistrarReferenciasBancariasSolicitud(string cedula, ReferenciasBancariasPyme[] ReferenciasBancarias)
        {
            string codigoRetorno, mensajeRetorno;
            try
            {
                _ContratacionProducto.RegistrarReferenciasBancariasSolicitud(cedula, ReferenciasBancarias, out codigoRetorno, out mensajeRetorno);
            }
            catch (Exception)
            {
                return "503";
            }

            if (codigoRetorno != "000")
                return "500";
            return "200";
        }

        public RespConsultarCtasCliente ConsultarCuentasCliente(string cedula)
        {
            string codigoRetorno, mensajeRetorno;
            DataTable resp = null;
            var toReturn = new RespConsultarCtasCliente();
            try
            {
                resp = _ContratacionProducto.ConsultarCtaAhorroCorriente("C", cedula, out codigoRetorno, out mensajeRetorno);
            }
            catch (Exception)
            {
                toReturn.errorCode = "503";
                return toReturn;
            }
            if (resp != null)
            {
                toReturn.errorCode = resp.Select("tipoCuentaDesc = 'Ahorros'").Length == 0 ? "404" : "200";
            }
            else
            {
                toReturn.errorCode = "404";
                return toReturn;
            }

            foreach(var item in resp.Select("tipoCuentaDesc = 'Ahorros'"))
            {
                toReturn.NumCuentas.Add(item.Field<string>("numeroCuenta"));
            }

            return toReturn;
        }

        public string SaveCtasParaCredito(string cedula, FromFormCuentaParaCredito data)
        {
            string codigoError, mensajeError;
            try
            {
                var resp = _ContratacionProducto.RegistrarCuentasSolicitud(cedula, data.cuentaDep, data.cuentaDeb, out codigoError, out mensajeError);
            }
            catch (Exception)
            {
                return "503";
            }

            if (codigoError != "000")
                return "500";

            return "200";
        }

        public string GenerarAnalisisCualitativo(DatosClientePy dataCliente)
        {
            string codError = "000", mensajeError= "";
            try
            {
                _ServicioRatingEmpresarial.GenerarAnalisisCualitativo(dataCliente.idProceso, dataCliente.Cedula, dataCliente.fechaRevision, "", ref codError, ref mensajeError);
            }
            catch (Exception)
            {
                return "503";
            }

            if (codError != "000")
                return "500";

            return "200";
        }

        public string ConsultarHostRiesgosGarantias(string idProceso, string cedula, ref string CodError, ref string MensajeError)
        {
            try
            {
                string tipo = "C";
                
                
                _ServicioRatingEmpresarial.ConsultaHostRiesgos(int.Parse(idProceso), tipo, cedula, 0, ref CodError, ref MensajeError);
                var resp = _ServicioRatingEmpresarial.ConsultaHostGarantias(cedula, tipo, idProceso, ref CodError, ref MensajeError);
            }
            catch (Exception)
            {
                return "503";
            }
            if (CodError != "000")
                return "500";
            return "200";
        }

        public EstadoCivilClienteDto ConsultaEstadoCivilCliente(string cedula)
        {
            string codigoRetorno, mensajeRetorno;
            DataSet resp = null;
            try
            {
                resp = _ContratacionProducto.ConsultaInformacionCliente(cedula, out codigoRetorno, out mensajeRetorno);
            }
            catch (Exception)
            {
                return new EstadoCivilClienteDto { codError = "503" };
            }

            if(codigoRetorno == "000")
            {
                DataNamesMapper<DatosEstCivilCliente> mapperToEstCivil = new DataNamesMapper<DatosEstCivilCliente>();
                DatosEstCivilCliente cliente = mapperToEstCivil.Map(resp.Tables["DatosBasicosCliente"].Rows[0]);
                var estCivil = Mapper.Map<EstadoCivilClienteDto>(cliente);

                DatosEstCivilCYG cony = null;
                if (resp.Tables["DatosBasicosConyugeCliente"].Rows.Count > 0)
                {
                    DataNamesMapper<DatosEstCivilCYG> mapperToCYG = new DataNamesMapper<DatosEstCivilCYG>();
                    cony = mapperToCYG.Map(resp.Tables["DatosBasicosConyugeCliente"].Rows[0]);
                    Mapper.Map(cony, estCivil);
                }

                estCivil.codError = "200";

                return estCivil;
            }

            return new EstadoCivilClienteDto { codError = "500" };
        }
    }

    public class RespConsultarCtasCliente
    {
        public RespConsultarCtasCliente()
        {
            NumCuentas = new List<string>();
        }

        public string errorCode { get; set; }
        public List<string> NumCuentas { get; set; }
    }

  
}
