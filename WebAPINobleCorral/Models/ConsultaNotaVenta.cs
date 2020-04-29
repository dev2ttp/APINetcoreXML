using System;
using System.Collections.Generic;
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
}
