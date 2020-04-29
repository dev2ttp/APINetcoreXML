using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPINobleCorral.Models;
using WebAPINobleCorral.Services;

namespace WebAPINobleCorral.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotaVentaController : ControllerBase
    {
        Services.ConsultaNotaVenta cn = new Services.ConsultaNotaVenta();

        // GET: api/NotaVenta
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/NotaVenta/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/NotaVenta
        [HttpPost("ConsultaNota")]
        public ResponseNotaventa ConsultarNota(Models.ConsultaNotaVenta numeroDocumento)
        {
            var response = cn.ConsultarNota();
            return response;
        }

        // PUT: api/NotaVenta/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
