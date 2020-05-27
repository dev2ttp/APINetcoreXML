using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPINobleCorral.Models
{
    public class ConsultaNotaVenta
    {
        public string numeroDocumento { get; set; }
    }
    public class ConsultaNVCabezera
    {
        public string correlativo { get; set; }
        public string fecha { get; set; }
        public string cliente { get; set; }
        public string total { get; set; }
    }
    public class ConsultaNVDetalle
    {
        public string producto { get; set; }
        public string cantidad { get; set; }
        public string totalProd { get; set; }
    }
    public class ResponseNotaventa
    {
        public ConsultaNVCabezera dataC { get; set; }
        public List<ConsultaNVDetalle> dataD { get; set; }
        public bool status { get; set; }
        public int code { get; set; }
        public string message { get; set; }
    }

    public class GenerarDocumento
    {
        public string empresa { get; set; }
        public string tipodocto { get; set; }
        public string Numero { get; set; }
        public string iTimeOut { get; set; }
    }

    public class RespuestaDocNoEncontrado
    {
        public string tipo { get; set; }
        public string estado { get; set; }
        public string descripcion { get; set; }
    }
    public static class ObjectExtensions
    {
        public static IDictionary<string, string> ToKeyValue(this object metaToken)
        {
            if (metaToken == null)
            {
                return null;
            }

            JToken token = metaToken as JToken;
            if (token == null)
            {
                return ToKeyValue(JObject.FromObject(metaToken));
            }

            if (token.HasValues)
            {
                var contentData = new Dictionary<string, string>();
                foreach (var child in token.Children().ToList())
                {
                    var childContent = child.ToKeyValue();
                    if (childContent != null)
                    {
                        contentData = contentData.Concat(childContent)
                            .ToDictionary(k => k.Key, v => v.Value);
                    }
                }

                return contentData;
            }

            var jValue = token as JValue;
            if (jValue?.Value == null)
            {
                return null;
            }

            var value = jValue?.Type == JTokenType.Date ?
                jValue?.ToString("o", CultureInfo.InvariantCulture) :
                jValue?.ToString(CultureInfo.InvariantCulture);

            return new Dictionary<string, string> { { token.Path, value } };
        }
    }
}
