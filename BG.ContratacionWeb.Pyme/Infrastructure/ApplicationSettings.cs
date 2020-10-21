namespace BG.ContratacionWeb.Pyme.Infrastructure
{
    public class ApplicationSettings
    {
        
    }

    public class CommonApplicationSettings
    {
        public EnvioSMS EnvioSMS { get; set; }
        public EnvioOTP EnvioOTP { get; set; }
        public Producto ProductoSG { get; set; }
        public Producto ProductoCP { get; set; }
        public RangoProducto RangoDeSobregiro { get; set; }
        public RangoProducto RangoDeCreditoPyme { get; set; }
        public Environment Environment { get; set; }
        public Notificaciones Notificaciones { get; set; }
        public ParametroProductoCode ParametroProducto { get; set; }

        public int PeriodicidadDias { get; set; }
    }

    // -------- clases complementarias -----------
    public class EnvioSMS
    {
        public string idPlantilla_otp_SMS { get; set; }
        
    }

    public class EnvioOTP
    {
        public string opidUsuario { get; set; }

    }

    public class Producto
    {
        public int idCanal { get; set; }
        public int IdProductoPadreNeo { get; set; }
        public int IdProductoNeo { get; set; }
        public string NombreProducto { get; set; }
    }

    public class Token
    {
        public string aplicacion { get; set; }
        public string canal { get; set; }
    }

    public class RangoProducto
    {
        public int valorMin { get; set; }
        public int valorMax { get; set; }
        public int maxDias { get; set; }
        public int minDias { get; set; }
    }

    public class Environment
    {
        public string mode { get; set; }
    }

    public class Notificaciones
    {
        public string UrlDefaultPageToRedirect { get; set; }
        public Mensaje[] mensajes { get; set; }
    }

    public class Mensaje
    {
        public string code { get; set; }
        public string title { get; set; }
        public string paragraph { get; set; }
        public string redirectTo { get; set; }
    }

    public class ParametroProductoCode
    {
        public int ME { get; set; }
        public int AD { get; set; }
    }
}