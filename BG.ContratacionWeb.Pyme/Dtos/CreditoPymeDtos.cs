using BG.ContratacionWeb.Pyme.Infrastructure;
using BG.ContratacionWeb.Pyme.Models;
using BG.ContratacionWeb.Pyme.ServicioContratacionProductos;
using System.Collections.Generic;
using System;
using BG.ContratacionWeb.Pyme.Helpers;

namespace BG.ContratacionWeb.Pyme.Dtos
{
    public class CreditoPymeDtos
    {
    }

    public class Selection
    {
       public dynamic SeleccionAnterior { get; set; }
    }

    public class DatosForSimuladorViewDto : Selection
    {
        public Catalogo[] ModosDePago { get; set; }
        public Catalogo[] PlazosCredito { get; set; }
        public Catalogo[] DestinosCredito { get; set; }
        public string TasaNominal { get; set; }
        public string TasaEfectiva { get; set; }
        public bool TieneCuentaAhorro { get; set; }
        public ConfigParamProduct[] ConfigParamProduct { get; set; }
        public RangoProducto Rango { get; set; }
    }

    public class FromFormSolicitudCreditoDto
    {
        public FromFormSolicitudCreditoDto()
        {
            destinoCredito = new CatalogoReduce();
            modoDePago = new CatalogoReduce();
            plazoCredito = new CatalogoReduce();
        }
        public string monto { get; set; }
        public CatalogoReduce destinoCredito { get; set; }
        public CatalogoReduce modoDePago { get; set; }
        public CatalogoReduce plazoCredito { get; set; }
        public string amortizacion { get; set; }
        public string diaDePago { get; set; }
        public string fechaPago { get; set; }
        public string cuota { get; set; }
        public string tasaNominal { get; set; }
        public string tasaEfectiva { get; set; }
    }

    public class CatalogoReduce
    {
        public int idCodigo { get; set; }
        public string Descripcion { get; set; }
    }

    public class ForViewEstadoCivilDto
    {
        public EstadoCivilFromCore EstadoCivil { get; set; }
        public Catalogo[] estados { get; set; }
        public int IdEstadoCivil { get; set; }
        public string EstadoCivilDesc { get; set; }
    }

    public class FromFormEstadoCivilDto
    {
        public int estadoCivil { get; set; }

        public string descEstadoCivil { get; set; }

        public string identificacionConyuge { get; set; }

        public string celularConyuge { get; set; }

        public string emailConyuge { get; set; }

        public string codigoCaso { get; set; }
    }

    public class ForViewInfoDomicilioDto : Selection
    {
        public Catalogo[] tiposDireccion { get; set; }
        public Catalogo[] tipoInmueble { get; set; }
        public string direccion { get; set; }
        public Catalogo[] comboOptionLocation { get; set; }
        public Catalogo[] ciudad { get; set; }
        public Catalogo[] parroquia { get; set; }
    }

    public class FromFormInfoDomicilio
    {
        public string tipoDomicilio { get; set; }
        [DataNames("Direccion1", "direccion1")]
        public string direccion1 { get; set; }
        [DataNames("Direccion1_comp", "direccion2")]
        public string direccion2 { get; set; }
        [DataNames("Direccion1_refe", "referencia")]
        public string referencia { get; set; }
        [DataNames("Direccion1_provincia", "provincia")]
        public int provincia { get; set; }
        public string provinciaDesc { get; set; }
        [DataNames("Direccion1_ciudad", "ciudad")]
        public int ciudad { get; set; }
        public string ciudadDesc { get; set; }
        [DataNames("Direccion1_parroquia", "parroquia")]
        public int parroquia { get; set; }
        public string parroquiaDesc { get; set; }
        [DataNames("telefono1", "telefonoDomNeg")]
        public string telefonoDomNeg { get; set; }

        /*public int isSecure { get; set; }
        public string isSecureDesc { get; set; }
        public int aseguradora { get; set; }
        public string aseguradoraDesc { get; set; }
        public string tipoInmueble { get; set; }
        public string tipoInmuebleDesc { get; set; }*/
    }

    public class FromFormInfoDirecNeg
    {
        public string tipoDomicilio { get; set; }
        [DataNames("Direccion2", "direccion1")]
        public string direccion1 { get; set; }
        [DataNames("Direccion2_comp", "direccion2")]
        public string direccion2 { get; set; }
        [DataNames("Direccion2_refe", "referencia")]
        public string referencia { get; set; }
        [DataNames("Direccion2_provincia", "provincia")]
        public int provincia { get; set; }
        public string provinciaDesc { get; set; }
        [DataNames("Direccion2_ciudad", "ciudad")]
        public int ciudad { get; set; }
        public string ciudadDesc { get; set; }
        [DataNames("Direccion2_parroquia", "parroquia")]
        public int parroquia { get; set; }
        public string parroquiaDesc { get; set; }
        [DataNames("telefono2", "telefonoDomNeg")]
        public string telefonoDomNeg { get; set; }
        [DataNames("tiene_seguro", "tieneSeguro")]
        public int isSecure { get; set; }
        public string isSecureDesc { get; set; }
        [DataNames("id_catalogo_seg", "catalogoSeguro")]
        public int aseguradora { get; set; }
        public string aseguradoraDesc { get; set; }
        [DataNames("Direccion2_tipo_inm", "tipoInmueble")]
        public string tipoInmueble { get; set; }
        public string tipoInmuebleDesc { get; set; }
    }

    public class FromFormComboArea
    {
        public string AreaType { get; set; }
        public string IdSeleccionPadre { get; set; }
    }

    public class FromFormVentas : Selection
    {
        public List<Models.Producto> productos { get; set; }
        public int numeroEmpleados { get; set; }
    }

    public class FromFormClientes
    {
        public List<ClientesProveedores> clientes { get; set; }
    }

    public class ForViewInfoNegocioDto : Selection
    {
        public string direccion { get; set; }
        public Catalogo[] comboOptionLocation { get; set; }
        public Catalogo[] ciudad { get; set; }
        public Catalogo[] parroquia { get; set; }
        public Catalogo[] aseguradora { get; set; }
        public Catalogo[] tipoInmueble { get; set; }
    }

    public class FromFormNegocioCertf
    {
        public Catalogo[] Certificaciones { get; set; }
        public int TieneCertificacion { get; set; }
    }

    public class ForViewCertNegocio
    {
        public Catalogo[] Certificaciones { get; set; }
    }

    public class ForViewReferenciaBancaria
    {
        public Catalogo[] bancos { get; set; }
    }

    public class FromFormCuentaParaCredito
    {
        public string cuentaDeb { get; set; }
        public string cuentaDep { get; set; }
    }

    public class EstadoCivilClienteDto
    {
        
        public string esCliente { get; set; }
        
        public int estadoCivil { get; set; }
        
        public string descEstadoCivil { get; set; }
        
        public string identificacionConyuge { get; set; }
        // si tiene conyugue. then query table: DatosBasicosConyugeCliente
        
        public string nombresConyuge { get; set; }
        
        public string ConyIdentificacionConyuge { get; set; }
        
        public string ConyFechaNacimiento { get; set; }
        
        public string ConyNacionalidad { get; set; }
        
        public string ConyDescNacionalidad { get; set; }
        public string codError { get; set; }
    }
}