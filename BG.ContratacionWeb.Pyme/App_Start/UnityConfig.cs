using BG.ContratacionWeb.Pyme.Data;
using BG.ContratacionWeb.Pyme.Infrastructure;
using Calabonga.Configurations;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace BG.ContratacionWeb.Pyme
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<ICacheService, CacheService>();
            container.RegisterType<IConfigSerializer, JsonConfigSerializer>();
            container.RegisterType<CommonSettingsManager>(); 
            container.RegisterType<EnvModeSettingsManager>();
            container.RegisterType<ServicioContratacionProductos.WS_ContratacionProductosOnline>();
            //container.RegisterType<ServicioExpediente.ServicioExpedientes>();
            container.RegisterType<ServicioRatingEmpresarial.wsConsultaExterna>();
            container.RegisterType<ISobreGiroRepository, SobreGiroRepository>();
            container.RegisterType<IOtpRepository, OtpRepository>();
            container.RegisterType<ITokenRepository, TokenRepository>();
            container.RegisterType<ICreditoPyme, CreditoPyme>(); 
            container.RegisterType<ICatalogoRepository, CatalogoRepository>(); 


            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}