using BG.ContratacionWeb.Pyme.Dtos;
using BG.ContratacionWeb.Pyme.Helpers;

namespace BG.ContratacionWeb.Pyme.Models
{
    public class CommonModels
    {
    }

    public class DatosCliente
    {
        [DataNames("")]
        public string Cedula { get; set; }

        [DataNames("primerNombre", "firstName")]
        public string FirstName { get; set; }

        [DataNames("apellidoPaterno", "lastName")]
        public string LastName { get; set; }

        [DataNames("celular", "phoneNumber")]
        public string PhoneNumber { get; set; }

        [DataNames("correo", "email")]
        public string Email { get; set; }
    }

    public class Anyo
    {
        public string Detalle { get; set; }

        [DataNames("Año 1", "Anyo1")]
        public string Anyo1 { get; set; }

        [DataNames("DatosAnyo1", "TieneDataAnyo1")]
        public string TieneDataAnyo1 { get; set; }

        [DataNames("Año 2", "Anyo2")]
        public string Anyo2 { get; set; }

        [DataNames("DatosAnyo2", "TieneDataAnyo2")]
        public string TieneDataAnyo2 { get; set; }

        [DataNames("Año 3", "Anyo3")]
        public string Anyo3 { get; set; }

        [DataNames("Anyo21", "TieneDataAnyo3")]
        public string TieneDataAnyo3 { get; set; }

        public string Environment { get; set; }
    }

    public class DatosClientePy //: Selection
    {
        [DataNames("IDENTIFICACION", "cedula")]
        public string Cedula { get; set; }

        [DataNames("NOMBRE", "nombres")]
        public string Nombres { get; set; }

        [DataNames("CELULAR", "celular")]
        public string Celular { get; set; }

        [DataNames("EMAIL", "email")]
        public string Email { get; set; }

        [DataNames("FeCHA_NACIMIENTO", "fechaNacimiento")]
        public string FechaNacimiento { get; set; }

        [DataNames("DESC_NACIONALIDAD", "nacionalidad")]
        public string Nacionalidad { get; set; }

        [DataNames("ID_ESTADO_CIVIL", "idEstadoCivil")]
        public string IdEstadoCivil { get; set; }

        [DataNames("DESC_ESTADO_CIVIL", "estadoCivil")]
        public string EstadoCivil { get; set; }

        [DataNames("Direccion1", "direccionDomicilio")]
        public string DireccionDomicilio { get; set; }

        [DataNames("Telefono1", "telefonoDomicilio")]
        public string TelefonoDomicilio { get; set; }

        [DataNames("Direccion2", "direccionNegocio")]
        public string DireccionNegocio { get; set; }

        [DataNames("Telefono2", "telefonoNegocio")]
        public string TelefonoNegocio { get; set; }

        //*** paso1
        public FromFormSolicitudCreditoDto infoSolicitud { get; set; }
        //*** paso3
        public FromFormInfoDomicilio infoDomicilio { get; set; }

        public IvaVentasMeses IvaVentaMes { get; set; }
        //*** paso6
        public FromFormVentas infoVentas { get; set; }
        //*** paso 7
        public FromFormClientes infoClientesProveedores { get; set; }
        //*** paso8
        public FromFormInfoDirecNeg infoNegocio { get; set; }

        public FromFormNegocioCertf infoCertificaciones { get; set; }

        public FromFormCuentaParaCredito infoCuentas { get; set; }

        public EstadoCivilClienteDto estadoCivilFormRC { get; set; }

        public EstadoCivilFromCore estadoCivilFormCore { get; set; }

        public string fechaRevision { get; set; }

        public string idProceso { get; set; }

        public string ActionRoute { get; set; }
    }

    public class IvaVentasMeses
    {
        public string Mes1 { get; set; }
        public string Mes2 { get; set; }
        public string Mes3 { get; set; }
    }

    public class InfToken
    {
        public InfToken()
        {
            canal = "";
            identificacion = "";
            tipoIdentificacion = "";
            producto = "";
            aplicacion = "";
            seguridad = "";
            usuario = "";
            celular = "";
        }

        public string canal { get; set; }
        public string identificacion { get; set; }
        public string tipoIdentificacion { get; set; }
        public string producto { get; set; }
        public string aplicacion { get; set; }
        public string seguridad { get; set; }
        public string usuario { get; set; }
        public string celular { get; set; }
    }

    public class DatosEstCivilCliente
    {
        [DataNames("esCliente", "esCliente")]
        public string esCliente { get; set; }
        [DataNames("estadoCivil", "estadoCivil")]
        public int estadoCivil { get; set; }
        [DataNames("descEstadoCivil", "descEstadoCivil")]
        public string descEstadoCivil { get; set; }
        [DataNames("identificacionConyuge", "identificacionConyuge")]
        public string identificacionConyuge { get; set; }
    }

    public class DatosEstCivilCYG
    {
        [DataNames("nombresPresenta", "nombresConyuge")]
        public string nombresConyuge { get; set; }
        [DataNames("identificacionConyuge", "ConyIdentificacionConyuge")]
        public string ConyIdentificacionConyuge { get; set; }
        [DataNames("fechaNacimiento", "ConyFechaNacimiento")]
        public string ConyFechaNacimiento { get; set; }
        [DataNames("nacionalidad", "ConyNacionalidad")]
        public string ConyNacionalidad { get; set; }
        [DataNames("descNacionalidad", "ConyDescNacionalidad")]
        public string ConyDescNacionalidad { get; set; }
    }

    public class EstadoCivilFromCore
    {
        [DataNames("cyg_identificacion", "cyg_identificacion")]
        public string cyg_identificacion { get; set; }
        [DataNames("cyg_nombre", "cyg_nombre")]
        public string cyg_nombre { get; set; }
        [DataNames("cyg_nombre_presenta", "cyg_nombre_presenta")]
        public string cyg_nombre_presenta { get; set; }
        [DataNames("cyg_id_nacionalidad", "cyg_id_nacionalidad")]
        public int cyg_id_nacionalidad { get; set; }
        [DataNames("cyg_desc_nacionalidad", "cyg_desc_nacionalidad")]
        public string cyg_desc_nacionalidad { get; set; }
        [DataNames("cyg_fecha_nacimiento", "cyg_fecha_nacimiento")]
        public string cyg_fecha_nacimiento { get; set; }
        [DataNames("cyg_id_estado_civil", "cyg_id_estado_civil")]
        public int cyg_id_estado_civil { get; set; }
        [DataNames("cyg_celular", "cyg_celular")]
        public string cyg_celular { get; set; }
        [DataNames("cyg_email", "cyg_email")]
        public string cyg_email { get; set; }
    }
}