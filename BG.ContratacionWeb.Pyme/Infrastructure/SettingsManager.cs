using Calabonga.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BG.ContratacionWeb.Pyme.Infrastructure
{

    public class EnvModeSettingsManager : Configuration<ApplicationSettings>
    {
        public EnvModeSettingsManager(IConfigSerializer serializer, ICacheService cacheService)
            : base(serializer, cacheService)
        {
        }

        public override string DirectoryName
        {
            get { return HttpContext.Current.Server.MapPath("~/Configurations"); }
        }

        public override string FileName
        {
#if !DEBUG
            get { return "appsettings.production.json"; }
#else
            get { return "appsettings.local.json"; }
#endif
        }
    }

    public class CommonSettingsManager : Configuration<CommonApplicationSettings>
    {
        public CommonSettingsManager(IConfigSerializer serializer, ICacheService cacheService)
            : base(serializer, cacheService)
        {
        }

        public override string DirectoryName
        {
            get { return HttpContext.Current.Server.MapPath("~/Configurations"); }
        }

        public override string FileName
        {
            get { return "appsettings.json"; }
        }
    }
}