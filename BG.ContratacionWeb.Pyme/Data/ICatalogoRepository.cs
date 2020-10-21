using BG.ContratacionWeb.Pyme.Infrastructure;
using System;
using System.Linq;
using BG.ContratacionWeb.Pyme.ServicioContratacionProductos;

namespace BG.ContratacionWeb.Pyme.Data
{
    public interface ICatalogoRepository
    {
        Catalogo[] GetDataCboxEstadoCivil();
        Catalogo[] GetDataCboxTipoDireccion();
        Catalogo[] GetDataCboxDireccionBy(string tipo, string idSeleccionPadre = "0");
        Catalogo[] GetDataCboxForAdress(string catalogo);
        Catalogo[] GetDataCboxCertificaciones();
        Catalogo[] GetDataCboxBancos();
        Catalogo[] GetCatalogoFor(int idCatalogo);
    }

    public class CatalogoRepository : ICatalogoRepository
    {

        private readonly WS_ContratacionProductosOnline _ContratacionProducto;
        private readonly CommonApplicationSettings _settings;

        public CatalogoRepository(WS_ContratacionProductosOnline ContratacionProducto,
                                  CommonSettingsManager settings)
        {
            _ContratacionProducto = ContratacionProducto;
            _settings = settings.Config;
        }

        public Catalogo[] GetDataCboxEstadoCivil()
        {
            Catalogo[] catalogos;
            string mensajeRetorno = "";

            try
            {
                _ContratacionProducto.ConsultarCatalogos(_settings.ProductoCP.idCanal, _settings.ProductoCP.IdProductoPadreNeo, 92, out mensajeRetorno, out catalogos);
            }
            catch (System.Exception)
            {
                catalogos = null;
                mensajeRetorno = "503";
            }

            return catalogos;
        }

        public Catalogo[] GetDataCboxTipoDireccion()
        {
            Catalogo[] catalogos;
            string mensajeRetorno = "";

            try
            {
                _ContratacionProducto.ConsultarCatalogos(_settings.ProductoCP.idCanal, _settings.ProductoCP.IdProductoPadreNeo, 97, out mensajeRetorno, out catalogos);
            }
            catch (Exception)
            {
                catalogos = null;
                mensajeRetorno = "503";
            }

            if (catalogos != null && catalogos.Length > 0)
            {
                catalogos = Array.FindAll(catalogos, element =>
                           element.Descripcion.Equals("TRABAJO", StringComparison.InvariantCultureIgnoreCase) ||
                           element.Descripcion.Equals("DOMICILIO", StringComparison.InvariantCultureIgnoreCase));
            }

            if (catalogos != null && catalogos.Length > 0)
            {
                Catalogo[] newCatalogos = new Catalogo[]
                {
                    new Catalogo { CodigoHost = "B", IdCodigo = 2, Descripcion = "AMBOS" },
                    new Catalogo { CodigoHost = "N", IdCodigo = 0, Descripcion = "NINGUNO" }
                };
                catalogos = catalogos.Concat(newCatalogos).ToArray();
            }


            return catalogos;
        }

        public Catalogo[] GetDataCboxDireccionBy(string tipo, string idSeleccionPadre = "0")
        {
            int idCatalogo = 0;
            if (string.IsNullOrEmpty(tipo))
                return null;
            if (tipo.ToLower() == "provincia")
                idCatalogo = 4;
            if (tipo.ToLower() == "ciudad")
                idCatalogo = 5;
            if (tipo.ToLower() == "parroquia")
                idCatalogo = 115;

            Catalogo[] catalogos;
            string mensajeRetorno = "";
            try
            {
                var resp = _ContratacionProducto.ConsultarCatalogos(_settings.ProductoCP.idCanal, int.Parse(idSeleccionPadre), idCatalogo, out mensajeRetorno, out catalogos);
            }
            catch (Exception)
            {
                return null;
            }
            return catalogos;
        }

        public Catalogo[] GetDataCboxForAdress(string catalogo)  //igual
        {
            Catalogo[] catalogos;
            string mensajeRetorno = ""; int idCatalogo = 0;

            if (catalogo == "tipoInmueble")
                idCatalogo = 46;
            if (catalogo == "aseguradora")
                idCatalogo = 91;

            try
            {
                var resp = _ContratacionProducto.ConsultarCatalogos(_settings.ProductoCP.idCanal, 0, idCatalogo, out mensajeRetorno, out catalogos);
            }
            catch (Exception)
            {
                return null;
            }

            return catalogos;

        }

        public Catalogo[] GetDataCboxCertificaciones()  //igual
        {
            string mensajeRetorno;
            Catalogo[] certfs = null;
            try
            {
                _ContratacionProducto.ConsultarCatalogos(_settings.ProductoCP.idCanal, 0, 312, out mensajeRetorno, out certfs);
            }
            catch (Exception)
            {
                return null;
            }

            return certfs;
        }

        public Catalogo[] GetDataCboxBancos()  //igual
        {
            string mensajeRetorno;
            Catalogo[] bancos = null;
            try
            {
                _ContratacionProducto.ConsultarCatalogos(_settings.ProductoCP.idCanal, 0, 30, out mensajeRetorno, out bancos);
            }
            catch (Exception)
            {
                return null;
            }

            return bancos;
        }

        public Catalogo[] GetDataCboxTipoCta() //igual
        {
            string mensajeRetorno;
            Catalogo[] bancos = null;
            try
            {
                _ContratacionProducto.ConsultarCatalogos(_settings.ProductoCP.idCanal, 0, 57, out mensajeRetorno, out bancos);
            }
            catch (Exception)
            {
                return null;
            }

            return bancos;
        }

        public Catalogo[] GetCatalogoFor(int idCatalogo) //igual
        {
            string mensajeRetorno;
            Catalogo[] bancos = null;
            try
            {
                _ContratacionProducto.ConsultarCatalogos(_settings.ProductoCP.idCanal, 0, idCatalogo, out mensajeRetorno, out bancos);
            }
            catch (Exception)
            {
                return null;
            }

            return bancos;
        }
    }

    public enum Catalogos
    {
        TipoInmueble = 46,
        Aseguradora = 91,
        Certificaciones = 312,
        Bancos = 30, 
        TipoCuenta = 57
    }
}
