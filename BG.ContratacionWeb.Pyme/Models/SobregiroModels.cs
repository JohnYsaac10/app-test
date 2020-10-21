using BG.ContratacionWeb.Pyme.Helpers;

namespace BG.ContratacionWeb.Pyme.Models
{
    public class SobregiroModels
    {
    }

    public class CuentasSobreGiro
    {
        [DataNames("NUMERO-CUENTA", "numCuentas")]
        public string NumCuenta { get; set; }

        [DataNames("TIPO-CUENTA", "tipoCuenta")]
        public string TipoCuenta { get; set; }
    }

    public class Holidays
    {
        [DataNames("Fecha", "date")]
        public string Date { get; set; }
    }
    
}