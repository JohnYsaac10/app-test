using BG.ContratacionWeb.Pyme.Helpers;
using BG.ContratacionWeb.Pyme.Infrastructure;
using BG.ContratacionWeb.Pyme.Models;
using Calabonga.Configurations;
using System;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace BG.ContratacionWeb.Pyme.Filters
{
    public class AuthorizationFilters
    {
    }

    //filtro para cuando entre sin autorizacion y se necesite responder con json version 1.0
    /*public class ProtectPost : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.RouteData.Values["controller"].ToString();
            if (filterContext.HttpContext.Session[controller.GetSessionName()] == null)
            {
                filterContext.Result = new JsonResult
                {
                    Data = new { codError = "401", mensaje = "Tu session ha expirado" }
                };
            }
        }
    }*/

    //filtro para cuando entre sin autorizacion y se necesite responder con json version 2.0
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ProtectPostAttribute : FilterAttribute, IAuthorizationFilter
    {
        public string UseAFT { get; set; }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            var httpContext = filterContext.HttpContext;

            if (string.IsNullOrEmpty(UseAFT))
                UseAFT = "yes";

            if (UseAFT.Equals("yes", StringComparison.InvariantCultureIgnoreCase))
                try
                {
                    var cookie = httpContext.Request.Cookies[AntiForgeryConfig.CookieName];
                    AntiForgery.Validate(cookie != null ? cookie.Value : null, httpContext.Request.Headers["__RequestVerificationToken"]);
                }
                catch (Exception)
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new { codError = "405", mensaje = "recargando la pagina..." }
                    };
                }

            if (filterContext.Result == null)
            {
                var controller = filterContext.RouteData.Values["controller"].ToString();
                if (filterContext.HttpContext.Session[controller.GetSessionName()] == null)
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new { codError = "401", mensaje = "Tu session ha expirado" }
                    };
                }
            }
        }
    }


    //filtro para cuando entre sin autorizacion y se necesite redireccionar
    public class ProtectGet : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.RouteData.Values["controller"].ToString();

            if (filterContext.HttpContext.Session[controller.GetSessionName()] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "ErrorHandler" },
                    { "action", "Notificacion" },
                    { "id", "401" }
                });
            }
        }
    }

    public class SkipToken : ActionFilterAttribute
    {
        private readonly string _mode;
        private CommonSettingsManager conf;
        public SkipToken()
        {
            conf = new CommonSettingsManager(new JsonConfigSerializer(), new CacheService());
            _mode = (conf.Config).Environment.mode;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var param = filterContext.HttpContext.Request.Params["aplicacion"]; param = param.ToLower();
            var token = filterContext.HttpContext.Request.Params["token"];
            var s = token.Split(' ');

            var isInDevelopment = (_mode.Equals("development", StringComparison.InvariantCultureIgnoreCase));

            if (isInDevelopment && (s.Length > 1))
            {
                if (param == "sobregiro")
                {
                    DatosCliente datosCliente = new DatosCliente();
                    datosCliente.Cedula = s[1];

                    filterContext.HttpContext.Session.Add("datosCliente", datosCliente);
                    
                    filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                       { "controller", "SobreGiro" },
                       { "action", "Solicitud" }
                    });
                }

                if (param == "creditopyme")
                {
                    DatosClientePy datosCliente = new DatosClientePy();
                    datosCliente.Cedula = s[1];

                    filterContext.HttpContext.Session.Add("datosClienteCP", datosCliente);

                    filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                       { "controller", "CreditoPyme" },
                       { "action", "Simulador" }
                    });
                }
            }


        }
    }
    
}