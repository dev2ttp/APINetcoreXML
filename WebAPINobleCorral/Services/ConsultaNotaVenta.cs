using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPINobleCorral.Models;
using System.Linq;
using System.Xml;
using NLog;
using Newtonsoft.Json;

namespace WebAPINobleCorral.Services
{
    public class ConsultaNotaVenta
    {
        private static readonly NLog.ILogger log = NLog.LogManager.GetCurrentClassLogger();
        
        public ResponseNotaventa ConsultarNota()
        {
            ResponseNotaventa response = new ResponseNotaventa();
            List<ConsultaNVDetalle> cs = new List<ConsultaNVDetalle>();
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("c:/ConsultaDocumentos.xml");
                var doc = xDoc.InnerXml;
                var xml = System.Xml.Linq.XElement.Parse(doc);
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
                                       producto     = (string)el.Element("Producto") ?? "",
                                       cantidad     = (string)el.Element("Cantidad") ?? "",
                                       totalProd    = (string)el.Element("Total") ?? "",
                                   }).ToArray();

                var num = dataDetalle.Count();

                if (dataEncabezado !=null)
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
    }
}
