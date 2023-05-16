using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Curso.Context;
using Curso.Models;
using Curso.Producer;
using System.Text.Json;

namespace Curso.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ILogger<ClientesController> _logger;
        private readonly IProducer _producer;

        public ClientesController(MyDbContext context, ILogger<ClientesController> logger, IProducer producer)
        {
            _context = context;
            _logger = logger;
            _producer = producer;
        }

        [HttpGet(Name = "GetCliente")]
        public ActionResult<IEnumerable<Cliente>> GetAll()
        {
            _logger.LogInformation("Se solicito un GetAll...");
            return _context.Clientes.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Cliente> GetById(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null)
            {
                return NotFound();
            }
            _producer.ProduceMessage("Cliente", $"Se consultó por id el cliente {JsonSerializer.Serialize(cliente)}");
            _logger.LogInformation("Se solicito un Get por id para el cliente: |{0}", id);
            return cliente;
        }

        [HttpGet("Cuil/{cuil}")]
        public ActionResult<Cliente> GetByCuil(string cuil)
        {
            var cliente = _context.Clientes.FirstOrDefault(cliente => cliente.cuil == cuil);
            if (cliente == null)
            {
                return NotFound();
            }
            _producer.ProduceMessage("Cliente", $"Se consultó por cuil el cliente {JsonSerializer.Serialize(cliente)}");
            _logger.LogInformation("Se solicito un Get por cuil para el cliente: |{0}", cuil);
            return cliente;
        }

        [HttpPost]
        public ActionResult<Cliente> Create(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
            _logger.LogInformation("Se creo un nuevo cliente: {0}", cliente.Id);
            _producer.ProduceMessage("Cliente", $"Se creó el cliente {JsonSerializer.Serialize(cliente)}");
            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest("No debe cambiar el ID del registro");
            }
            _context.Entry(cliente).State = EntityState.Modified;
            _context.SaveChanges();
            _logger.LogInformation("Se actualizo el cliente: {0}", cliente.Id);
            _producer.ProduceMessage("Cliente", $"Se actualizó el cliente {JsonSerializer.Serialize(cliente)}");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<Cliente> Delete(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null)
            {
                return NotFound();
            }
            _context.Clientes.Remove(cliente);
            _context.SaveChanges();
            _logger.LogInformation("El cliente {0} ha sido eliminado", cliente.Id);
            _producer.ProduceMessage("Cliente", $"Se eliminó el cliente {JsonSerializer.Serialize(cliente)}");
            return cliente;
        }
    }
}
