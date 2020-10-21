using BG.ContratacionWeb.Pyme.Helpers;

namespace BG.ContratacionWeb.Pyme.Models
{
    public class CreditoPymeModels
    {
    }

    public class ConfigParamProduct
    {
        [DataNames("idCodigo", "idCodigo")]
        public string idCodigo { get; set; }

        [DataNames("nombreFisico", "tipoMonto")]
        public string tipoMonto { get; set; }

        [DataNames("idProducto", "idProducto")]
        public string idProducto { get; set; }

        [DataNames("valorCampo", "montoMaximoProducto")]
        public string montoMaximoProducto { get; set; }
    }

    public class Producto
    {
        [DataNames("nombreProducto", "nombre")]
        public string nombre { get; set; }

        [DataNames("porcentajeVentas", "porcentajeVentas")]
        public int porcentajeVentas { get; set; }
    }

    public class ClientesProveedores
    {
        [DataNames("nombre", "nombre")]
        public string nombre { get; set; }
        [DataNames("porcentaje", "porcentaje")]
        public int porcentaje { get; set; }
        [DataNames("dias", "dias")]
        public int dias { get; set; }
        [DataNames("tipo", "tipo")]
        public string tipo { get; set; }
    }

    //public class SolicitudSeleccion
    //{
    //    [DataNames("monto_a_financiar", "monto")]
    //    public string monto { get; set; }
    //    [DataNames("id_destinocredito", "idDestinoCredtio")]
    //    public int idDestinoCredtio { get; set; }
    //    [DataNames("id_producto", "idProducto")]
    //    public int idProducto { get; set; }
    //    [DataNames("id_plazo", "idPlazo")]
    //    public int idPlazo { get; set; }
    //    [DataNames("tabla_amortizacion", "amortizacion")]
    //    public string amortizacion { get; set; }
    //    [DataNames("fecha_pago", "fechaPago")]
    //    public string fechaPago { get; set; }
    //    [DataNames("Dia_pago", "diaPago")]
    //    public string diaPago { get; set; }
    //    [DataNames("cuota_mensual", "cuotaMensual")]
    //    public string cuotaMensual { get; set; }
    //}

    /*public class DireccionDomicilioSeleccion
    {
        [DataNames("Direccion1", "direccion1")]
        public string direccion1 { get; set; }
        [DataNames("Direccion1_comp", "direccion2")]
        public string direccion2 { get; set; }
        [DataNames("Direccion1_refe", "referencia")]
        public string referencia { get; set; }
        [DataNames("Direccion1_provincia", "provincia")]
        public int provincia { get; set; }
        [DataNames("Direccion1_ciudad", "ciudad")]
        public int ciudad { get; set; }
        [DataNames("Direccion1_parroquia", "parroquia")]
        public int parroquia { get; set; }
        [DataNames("telefono1", "telefono")]
        public string telefono { get; set; }
    }*/

    /*public class DireccionNegocioSeleccion
    {
        [DataNames("Direccion2", "direccion1")]
        public string direccion1 { get; set; }
        [DataNames("Direccion2_comp", "direccion2")]
        public string direccion2 { get; set; }
        [DataNames("Direccion2_refe", "referencia")]
        public string referencia { get; set; }
        [DataNames("Direccion2_provincia", "provincia")]
        public int provincia { get; set; }
        [DataNames("Direccion2_ciudad", "ciudad")]
        public int ciudad { get; set; }
        [DataNames("Direccion2_parroquia", "parroquia")]
        public int parroquia { get; set; }
        [DataNames("telefono2", "telefono")]
        public string telefono { get; set; }
        [DataNames("Direccion2_tipo_inm", "tipoInmueble")]
        public int tipoInmueble { get; set; }
        [DataNames("tiene_seguro", "tieneSeguro")]
        public int tieneSeguro { get; set; }
        [DataNames("id_catalogo_seg", "catalogoSeguro")]
        public int catalogoSeguro { get; set; }
    }*/

    public class MesesIva
    {
        [DataNames("mes_1", "Mes1")]
        public string Mes1 { get; set; }
        [DataNames("mes_2", "Mes2")]
        public string Mes2 { get; set; }
        [DataNames("mes_3", "Mes3")]
        public string Mes3 { get; set; }

        public string Environment { get; set; }

        public string CodError { get; set; }

        public string mes1SinAnyo { get; set; }
        public string mes2SinAnyo { get; set; }
        public string mes3SinAnyo { get; set; }
    }

 /*   enum Meses {

        Enero = 1,

        Febrero,

        Marzo,

        Abril,

        Mayo,

        Junio,

        Julio,

        Agosto,

        Septiembre,

        Octubre,

        Noviembre,

        Diciembre
    }*/

}