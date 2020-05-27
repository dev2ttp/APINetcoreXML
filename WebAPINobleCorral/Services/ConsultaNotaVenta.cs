using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPINobleCorral.Models;
using System.Linq;
using System.Xml;
using NLog;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace WebAPINobleCorral.Services
{
    public class ConsultaNotaVenta
    {
        private static readonly NLog.ILogger log = NLog.LogManager.GetCurrentClassLogger();
        private static readonly HttpClient client = new HttpClient();

        public ResponseNotaventa ConsultarNota()
        {
            ResponseNotaventa response = new ResponseNotaventa();
            List<ConsultaNVDetalle> cs = new List<ConsultaNVDetalle>();
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("c:/Users/Chicho/Documents/totalpack/proyectos/noble corral/Consulta Documentos/prueba.xml");
                var doc = xDoc.InnerXml;
                var xml = System.Xml.Linq.XElement.Parse(doc);

                var dataEncabezado4 = (from el in xml.Descendants("MENSAJE")
                                      select new RespuestaDocNoEncontrado()
                                      {
                                          tipo = (string)el.Element("TIPO") ?? "",
                                          estado = (string)el.Element("ESTADO") ?? "",
                                          descripcion = (string)el.Element("DESCRIPCION") ?? ""
                                      }).ToArray();


                var dataEncabezado = (from el in xml.Descendants("ENCABEZADO")
                                      select new ConsultaNVCabezera()
                                      {
                                          correlativo = (string)el.Element("Correlativo") ?? "",
                                          fecha = (string)el.Element("Fecha") ?? "",
                                          cliente = (string)el.Element("Cliente") ?? "",
                                          total = (string)el.Element("Total") ?? "",
                                      }).FirstOrDefault();

                var dataDetalle = (from el in xml.Descendants("DETALLE")
                                   select new ConsultaNVDetalle()
                                   {
                                       producto = (string)el.Element("Producto") ?? "",
                                       cantidad = (string)el.Element("Cantidad") ?? "",
                                       totalProd = (string)el.Element("Total") ?? "",
                                   }).ToArray();

                var num = dataDetalle.Count();

                if (dataEncabezado != null)
                {
                    log.Info("R: Nota de venta: " + JsonConvert.SerializeObject(dataEncabezado));
                    response.dataC = dataEncabezado;
                    for (int i = 0; i < num; i++)
                    {
                        cs.Add(dataDetalle[i]);
                    }
                    response.dataD = cs;
                    response.status = true;
                    response.code = 200;
                    response.message = "OK";
                }
                else
                {
                    log.Info("E: Nota de venta: " + JsonConvert.SerializeObject(dataEncabezado));
                    response.status = false;
                    response.code = 410;
                    response.message = "";
                }
                return response;
            }
            catch (Exception e)
            {
                log.Error("E: Nota de venta: " + e.ToString());
                response.status = false;
                response.code = 804;
                response.message = e.Message;
                return response;
            }

        }

        public async Task<RespuestaDocNoEncontrado> RealizarConsulta()
        {
            //var url = string.Format("{0}/vitacora", Settings.Default.UrlApi);
            GenerarDocumento documento = new GenerarDocumento();
            var url = "http://200.72.199.83//Ws%20Documento/Documento.asmx/GeneraDocumento";
            documento.empresa = "E01";
            documento.tipodocto = "NOTA DE VENTA";
            documento.Numero = "0000000085";
            documento.iTimeOut = "30";
            var keyValues = documento.ToKeyValue();

            var nvc = new List<KeyValuePair<string, string>>();
            var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(keyValues) };

            try
            {
                using (var response = await client.SendAsync(req))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    content = content.Replace("&lt;", "<");
                    content = content.Replace("&gt;", ">");
                    var xml = System.Xml.Linq.XElement.Parse(content);
                    XNamespace ns = "http://tempuri.org/";
                    var dataEncabezado4 = (from el in xml.Descendants(ns + "MENSAJE")
                                           select new RespuestaDocNoEncontrado()
                                           {
                                               tipo = (string)el.Element(ns + "TIPO") ?? "",
                                               estado = (string)el.Element(ns + "ESTADO") ?? "",
                                               descripcion = (string)el.Element(ns + "DESCRIPCION") ?? ""
                                           }).ToArray();
                    log.Info(content);
                    RespuestaDocNoEncontrado response2 = new RespuestaDocNoEncontrado();
                    return response2;
                }
            }
            catch (Exception ex)
            {

                log.Info(ex);
            }
            RespuestaDocNoEncontrado response3 = new RespuestaDocNoEncontrado();
            return response3;
        }
    }
}
