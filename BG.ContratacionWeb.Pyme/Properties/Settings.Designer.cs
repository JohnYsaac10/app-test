﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BG.ContratacionWeb.Pyme.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://172.26.60.71:88/WS_ContratacionProductosOnline/WS_ContratacionProductosOnl" +
            "ine.asmx")]
        public string BG_ContratacionWeb_Pyme_ServicioContratacionProductos_WS_ContratacionProductosOnline {
            get {
                return ((string)(this["BG_ContratacionWeb_Pyme_ServicioContratacionProductos_WS_ContratacionProductosOnl" +
                    "ine"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://172.26.60.71:88/BG.Neo.Ws.Expedientes/ServicioExpedientes.asmx")]
        public string BG_ContratacionWeb_Pyme_ServicioExpediente_ServicioExpedientes {
            get {
                return ((string)(this["BG_ContratacionWeb_Pyme_ServicioExpediente_ServicioExpedientes"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://172.26.60.144/RatingEmpresarial/wsRating/wsconsultaexterna.asmx")]
        public string BG_ContratacionWeb_Pyme_ServicioRatingEmpresarial_wsConsultaExterna {
            get {
                return ((string)(this["BG_ContratacionWeb_Pyme_ServicioRatingEmpresarial_wsConsultaExterna"]));
            }
        }
    }
}
