using BG.ContratacionWeb.Pyme.Dtos;
using BG.ContratacionWeb.Pyme.Infrastructure;
using BG.ContratacionWeb.Pyme.Models;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace BG.ContratacionWeb.Pyme.Helpers
{
    public static class Extentions
    {
        public static string ToDollarWithoutDecimal(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            CultureInfo myCI = new CultureInfo("en-US", false);

            var d = Double.Parse(value).ToString("C2", myCI);

            return d.Replace(".00", "");
        }

        public static string ToDolllars(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            CultureInfo myCI = new CultureInfo("en-US", false);

            return Double.Parse(value).ToString("C2", myCI);
        }

        public static string ToDolllars(this int value)
        {
            if (value == 0)
                return "";
            var valueString = value.ToString();

            if (string.IsNullOrEmpty(valueString))
                return "";
            CultureInfo myCI = new CultureInfo("en-US", false);

            return Double.Parse(valueString).ToString("C2", myCI);
        }

        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            if (!(email.ToLower().Contains("@") && email.ToLower().Contains(".")))
                return false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static string ToNumber(this string value)
        {
            if (value.Contains("$"))
            {
                value = value.Replace("$", "").Replace(",", "");
            }
            return value;
        }

        public static DataTable getWithAddedCol(this DataTable value, string defaultValue)
        {
            DataColumn newColumn = new DataColumn("tipo", typeof(String));
            newColumn.DefaultValue = defaultValue;
            value.Columns.Add(newColumn);
            return value;
        }

        public static int ToInt(this string value)
        {
            if (value.Contains("/"))
                return 0;
            if (string.IsNullOrEmpty(value))
                return 0;
            if (value.Any(char.IsLetter))
                return 0;

            return int.Parse(value);
        }

        public static string ToSaludo(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            value = value.ToUpperFirstLetter();
            var nombre = value.Split(' ');

            var length = nombre.Length;
            if(length == 3)
                return "Hola, " + nombre[nombre.Length - 1] + " " + nombre[0];

            return "Hola, " + nombre[nombre.Length - 2] + " " + nombre[0];
        }

        public static StringBuilder ExtractTextFromPdf(this HttpRequestBase Request, out string anyo, out string codError, out string mensaje)
        {
            codError = ""; mensaje = ""; anyo = "";
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        var stream = fileContent.InputStream;
                        anyo  = Request.Files.AllKeys[0].Split('-')[1];
                        var fileExt = System.IO.Path.GetExtension(fileContent.FileName);
                        if (fileExt.ToLower() != ".pdf")
                        {
                            codError = "400"; mensaje = "solo se admiten formatos con extención .pdf";
                            return null;
                        }
                        else
                        {
                            PdfReader reader = new PdfReader(stream);
                            StringBuilder text = new StringBuilder();

                            for (int i = 1; i <= reader.NumberOfPages; i++)
                            {
                                text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                            }
                            codError = "200"; mensaje = "";
                            return text;
                        }
                    }
                    else
                    {
                        codError = "400"; mensaje = "Todos los archivos son requeridos.";
                        return null;
                    }
                }
            }
            catch
            {
                codError = "400"; mensaje = "Upps! error al procesar el archivo .pdf";
                return null;
            }
            codError = "500"; mensaje = "Algo salió mal procesando el archivo.";
            return new StringBuilder("");
        }

        //added by ysaac
        //mask th email
        //usage: "string".hidingEmail()
        public static string maskEmail(this string correo)
        {
            if (string.IsNullOrEmpty(correo))
                return string.Empty;
            string pattern = @"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@)";
            string result = Regex.Replace(correo, pattern, m => new string('*', m.Length));
            return result;
        }
        //added by ysaac
        // Mask the mobile.
        // Usage: "strign".MaskMobile() => "134****876"
        public static string maskMobile(this string mobile)
        {
            int startIndex = 3; string mask = "****";
            if (string.IsNullOrEmpty(mobile))
                return string.Empty;

            string result = mobile;
            int starLengh = mask.Length;


            if (mobile.Length >= startIndex)
            {
                result = mobile.Insert(startIndex, mask);
                if (result.Length >= (startIndex + starLengh * 2))
                    result = result.Remove((startIndex + starLengh), starLengh);
                else
                    result = result.Remove((startIndex + starLengh), result.Length - (startIndex + starLengh));

            }

            return result;
        }

        public static string ToUpperFirstLetter(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            source = source.ToLower();
            var palabras = source.Split(' ');

            palabras = Array.FindAll(palabras, p => p != "").ToArray();

            if (palabras.Length == 1)
            {
                char[] letters = source.ToCharArray();

                letters[0] = char.ToUpper(letters[0]);

                return new string(letters);
            }
            else
            {
                string wordConverted = "";
                char[] letters;
                foreach (var palabra in palabras)
                {
                    letters = palabra.ToCharArray();

                    letters[0] = char.ToUpper(letters[0]);

                    wordConverted += new string(letters) + " ";
                }

                return wordConverted.Trim();
            }
        }

        public static string getSaludo(this DatosCliente data)
        {
            if (data == null) return "";
            return "Hola, " + data.FirstName.ToUpperFirstLetter() + " " + data.LastName.ToUpperFirstLetter();
        }

        public static bool isValidateRequisition(this SobreGiroDetalleSolicitudDto myObject, CommonApplicationSettings _settings, out string mensaje)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if (string.IsNullOrEmpty(value) && (pi.Name != "TasaInteres" && pi.Name != "celular"))
                    {
                        mensaje = pi.Name + " no fué ingresado";
                        return false;
                    }
                }
            }

            var cantidad = myObject.Cantidad.Replace(",", "");


            if (cantidad.Any(char.IsLetter))
            {
                mensaje = "la cantidad contenía letras";
                return false;
            }

            if (cantidad.ToInt() < _settings.RangoDeSobregiro.valorMin || cantidad.ToInt() > _settings.RangoDeSobregiro.valorMax)
            {
                mensaje = cantidad + $" no está en el rango de ({_settings.RangoDeSobregiro.valorMin} - {_settings.RangoDeSobregiro.valorMax})";
                return false;
            }

            var cuenta = myObject.Cuenta.Split('-');

            if (cuenta[0].Any(char.IsLetter))
            {
                mensaje = "la cuenta contiene letras";
                return false;
            }


            if (myObject.TipoSobregiro != "82" && myObject.TipoSobregiro != "85")
            {
                mensaje = "Tipo Sobregiro no válido";
                return false;
            }


            DateTime temp;
            if (!(DateTime.TryParse(myObject.FechaPago, CultureInfo.CreateSpecificCulture("es-EC"), DateTimeStyles.None, out temp)))
            {
                mensaje = "fecha no válida. formato fecha: " + String.Format("{0:dd/MM/yyyy}", DateTime.Today);
                return false;
            }

            var hoy = Convert.ToDateTime(DateTime.Today.Date, new CultureInfo("es-EC"));
            
            if(hoy.Month != temp.Month)
            {
                mensaje = "\n  Mes erroneo. \n fecha actual: " + String.Format("{0:dd/MM/yyyy}", hoy) + "\n" + "fecha introducida: " + String.Format("{0:dd/MM/yyyy}", temp);
                return false;
            }

            if (hoy.Year != temp.Year)
            {
                mensaje = "\n  Año erroneo. \n fecha actual: " + String.Format("{0:dd/MM/yyyy}", hoy) + "\n" + "fecha introducida: " + String.Format("{0:dd/MM/yyyy}", temp);
                return false;
            }

            
            if ((temp.DayOfWeek == DayOfWeek.Saturday) || (temp.DayOfWeek == DayOfWeek.Sunday))
            {
                string dateToday = hoy.ToString("d");
                mensaje = "\n No se permiten Sábado y Domingos";
                return false;
            }

            mensaje = "";

            return true;
        }

        //method  
        public static bool IsAnyNullOrEmpty(this object myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if (string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsAnyNullOrEmpty(this object myObject, string except)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string) && !pi.Name.Contains(except) )
                {
                    string value = (string)pi.GetValue(myObject);
                    if (string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static string GetMonths(this string value)
        {
            if (value == null)
                return "";
            var arr = value.Split(' ');
            return arr[0];
        }

        public static string GetSessionName(this string value)
        {
            if (value == "CreditoPyme")
                return "datosClienteCP";
            return "datosCliente";
        }

        public static bool IsValidModelSimulador(this FromFormSolicitudCreditoDto value, CommonApplicationSettings _settings, out string mensaje)
        {
            if(value == null){
                mensaje = "Petición Erronea";
                return false;
            }
            mensaje = "";
            if (string.IsNullOrEmpty(value.monto)) {
                mensaje = "Debes Ingresar un monto para acceder a su crédito";
                return false;
            }
            if (string.IsNullOrEmpty(value.destinoCredito.Descripcion)) {
                mensaje = "Debes seleccionar el destino de su crédito";
                return false;
            }
            if (string.IsNullOrEmpty(value.modoDePago.Descripcion)) {
                mensaje = "Debes seleccionar el modo de pago de su crédito";
                return false;
            }
            if (value.monto.Any(char.IsLetter)) {
                mensaje = "El campo monto contiene Letras";
                return false;
            }
            /*var valorSoli = int.Parse(value.Monto);
            if (valorSoli > _settings.RangoDeSobregiro.valorMax || valorSoli < _settings.RangoDeSobregiro.valorMin){
                mensaje = "Debes ingresar un monto entre [1,000 - 40,000]";
                return false;
            }*/
            
            if (value.modoDePago.idCodigo == 12167) //al vencimiento
            {
                DateTime temp;
                if (!(DateTime.TryParse(value.fechaPago, CultureInfo.CreateSpecificCulture("es-EC"), DateTimeStyles.None, out temp)))
                {
                    mensaje = "fecha no válida. formato fecha: " + String.Format("{0:dd/MM/yyyy}", DateTime.Today);
                    return false;
                }

                if ((temp.DayOfWeek == DayOfWeek.Saturday) || (temp.DayOfWeek == DayOfWeek.Sunday))
                {
                    mensaje = "\n No se permiten Sábado y Domingos";
                    return false;
                }

                var hoy = Convert.ToDateTime(DateTime.Today.Date, new CultureInfo("es-EC"));
                var MinDate = hoy.AddDays(_settings.RangoDeCreditoPyme.minDias);
                var MaxDate = MinDate.AddDays(_settings.RangoDeCreditoPyme.maxDias);

                if (temp > MaxDate)
                {
                    mensaje = "\n  Fecha erroneo. Fecha Permitida hasta el " + String.Format("{0:dd/MM/yyyy}", MaxDate) + "\n fecha actual: " + String.Format("{0:dd/MM/yyyy}", hoy) + "\n" + "fecha introducida: " + String.Format("{0:dd/MM/yyyy}", temp);
                    return false;
                }

                if (temp < MinDate)
                {
                    mensaje = "\n  Fecha erroneo. Fecha minima de selección: " + String.Format("{0:dd/MM/yyyy}", MinDate) + "\n fecha actual: " + String.Format("{0:dd/MM/yyyy}", hoy) + "\n" + "fecha introducida: " + String.Format("{0:dd/MM/yyyy}", temp);
                    return false;
                }
            }

            if(value.modoDePago.idCodigo == 12168) //mensualmente
            {
                if (string.IsNullOrEmpty(value.plazoCredito.Descripcion)){
                    mensaje = "Debes seleccionar un plazo de su crédito";
                    return false;
                }
                if (string.IsNullOrEmpty(value.amortizacion)){
                    mensaje = "Debes seleccionar el tipo de amortización";
                    return false;
                }
                if (string.IsNullOrEmpty(value.diaDePago)){
                    mensaje = "Debes ingresar el dia de pago de su crédito";
                    return false;
                }
                var dia = int.Parse(value.diaDePago);
                if (dia > 30 || dia < 1){
                    mensaje = "Debes ingresar un dia entre [1-30]";
                    return false;
                }
                if (value.amortizacion != "FRANCESA" && value.amortizacion != "ALEMANA")
                {
                    mensaje = "Tipo de amortizacion no válida";
                    return false;
                }
            }
            return true;
        }

        public static bool isValidInfoDomicilio(this FromFormInfoDirecNeg model, out string mensaje)
        {
            mensaje = "";
            if (model == null)
                return false;
            if (model.tipoDomicilio == "N" || model.tipoDomicilio == "T" || model.tipoDomicilio == "Negocio") {

                if (string.IsNullOrEmpty(model.direccion1)) {
                    mensaje = "Olvidates ingresar dirección";
                    return false;
                }
                
            }

            if (string.IsNullOrEmpty(model.direccion2))
            {
                mensaje = "Olvidates ingresar referencia";
                return false;
            }

            if (string.IsNullOrEmpty(model.referencia))
            {
                mensaje = "Olvidates ingresar referencia";
                return false;
            }

            if (model.provincia == 0) {
                mensaje = "olvidates seleccionar provincia";
                return false;
            }

            if (model.ciudad == 0)
            {
                mensaje = "olvidates seleccionar ciudad";
                return false;
            }
            if (model.parroquia == 0)
            {
                mensaje = "olvidates seleccionar parroquia";
                return false;
            }

            if (string.IsNullOrEmpty(model.telefonoDomNeg)) {
                mensaje = "Falta ingresar el teléfono.";
                return false;
            }
            if (model.telefonoDomNeg.Length < 9)
            {
                mensaje = "Teléfono incompleto.";
                return false;
            }

            if (model.tipoDomicilio == "Negocio") {

                if (string.IsNullOrEmpty(model.tipoInmueble))
                {
                    mensaje = "Olvidaste selecionar tipo de Inmueble";
                    return false;
                }
            }
            

            return true;
        }
    }
}