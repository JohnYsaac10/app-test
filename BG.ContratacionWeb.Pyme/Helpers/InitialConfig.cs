using BG.ContratacionWeb.Pyme.Infrastructure;
using Calabonga.Configurations;
using Unity;

namespace BG.ContratacionWeb.Pyme.Helpers
{
    public class InitialConfig
    {
        public static Environment getEnvironment()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<ICacheService, CacheService>();
            container.RegisterType<IConfigSerializer, JsonConfigSerializer>();
            container.RegisterType<CommonSettingsManager>();

            var settings = container.Resolve<CommonSettingsManager>();

            return settings.Config.Environment;
        }

        public static Mensaje getNotiMessage(string id, CommonApplicationSettings _settings)
        {
            var msj = System.Array.Find(_settings.Notificaciones.mensajes, el => el.code == id);
            if (msj == null)
                msj = System.Array.Find(_settings.Notificaciones.mensajes, el => el.code == null);


            if (msj.redirectTo == "")
                msj.redirectTo = _settings.Notificaciones.UrlDefaultPageToRedirect;
            
            return msj;
        }
    }
}