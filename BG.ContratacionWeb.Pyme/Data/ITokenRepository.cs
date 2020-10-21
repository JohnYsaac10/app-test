using BG.ContratacionWeb.Pyme.Dtos;
using System;

namespace BG.ContratacionWeb.Pyme.Data
{
    public interface ITokenRepository
    {
        ServicioToken.TokenValida RecuperarInfToken(string token, string canal, string app, out string codigoRetorno);

        ServicioToken.TokenObtiene GenerarToken(GenerateTokenFormDto data, out string errorCode);
    }



    public class TokenRepository : ITokenRepository
    {
        public ServicioToken.TokenValida RecuperarInfToken(string token, string canal, string app, out string codigoRetorno)
        {
            ServicioToken.TokenValida tokenValida = null;
            codigoRetorno = "000";
            try
            {
                ServicioToken.TokenServiceClient _servicioToken = new ServicioToken.TokenServiceClient();
                tokenValida = _servicioToken.RecuperarInfToken(token, canal, app);
            }
            catch (Exception e)
            {
                codigoRetorno = "503";
            }
            return tokenValida;
        }

        public ServicioToken.TokenObtiene GenerarToken(GenerateTokenFormDto data, out string errorCode)
        {
            try
            {
                errorCode = "000";
                ServicioToken.TokenServiceClient _servicioToken = new ServicioToken.TokenServiceClient();
                ServicioToken.TokenObtiene dataToken;
                dataToken = _servicioToken.GenerarToken(data.Canal, data.Aplicacion, data.MetaJson);
                return dataToken;
            }
            catch (Exception e)
            {
                errorCode = "503";
            }

            return null;
        }
    }
}