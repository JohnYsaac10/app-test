using BG.ContratacionWeb.Pyme.Models;

namespace BG.ContratacionWeb.Pyme.Dtos
{
    public class SobreGiroDtos
    {
    }

    public class SobreGiroDetalleSolicitudDto
    {
        public SobreGiroDetalleSolicitudDto()
        {
            SobreGiroElegido = new SobreGiroElegido();
            SobreGiroElegido.Nombre = "SobreGiro";
            SobreGiroElegido.Detalle = "Se solicitará fondos que no tienes disponibles en tu cuenta.";
        }

        private string _tipoSobreGiro;
        public string Cuenta { get; set; }
        public string Cantidad { get; set; }
        public string FechaPago { get; set; }
        public string celular { get; set; }
        public string TasaInteres { get; set; }
        public SobreGiroElegido SobreGiroElegido { get; set; }
        public string TipoSobregiro
        {
            get { return _tipoSobreGiro; }
            set
            {
                _tipoSobreGiro = value;
                if(_tipoSobreGiro == "85")
                {
                    SobreGiroElegido.Nombre = "Pago sobre depósito";
                    SobreGiroElegido.Detalle = "Se realizará un adelanto de fondos sobre cheques depositados en tu cuenta.";
                }
            }
        }
    }

    public class SobreGiroElegido
    {
        public string Nombre { get; set; }
        public string Detalle { get; set; }
    }

    public class DatosForSulicitudViewDto
    {
        public CuentasSobreGiro[] Cuentas { get; set; }
        public Holidays[] Holidays { get; set; }
        public string MontoRango { get; set; }
        public SobreGiroDetalleSolicitudDto DataPreset { get; set; }
        public string mensajeRetorno { get; set; }
        public string codigoRetorno { get; set; }
    }

    public class DataToken
    {
        public string token { get; set; }
        public string canal { get; set; }
        public string aplicacion { get; set; }
    }

    public class GenerateTokenFormDto
    {
        public string Canal { get; set; }
        public string Aplicacion { get; set; }
        public string MetaJson { get; set; }
    }
}