using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Curso.Context;
using Curso.Models;

namespace Curso.Controllers
{
    
        [ApiController]
        [Route("api/[controller]")]
        public class ClientesController : ControllerBase
        {
            private readonly MyDbContext _context;
            public ClientesController(MyDbContext context, ILogger<ClientesController> logger)
            {
                _context = context;
            _logger = logger;
        }
        private readonly ILogger<ClientesController> _logger;


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
            _logger.LogInformation("Se solicito un Get para el cliente: |{0}",id);
            return cliente;
            }
            [HttpPost]
            public ActionResult<Cliente> Create(Cliente cliente)
            {
                _context.Clientes.Add(cliente);
                _context.SaveChanges();
            _logger.LogInformation("Se creo un nuevo cliente: {0}", cliente.Id);
            return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
            }
            [HttpPut("{id}")]
            public ActionResult Update(int id, Cliente client)
            {
                if (id != client.Id)
                {
                    return BadRequest("No debe cambiar el ID del registro");
                }
                _context.Entry(client).State = EntityState.Modified;
                _context.SaveChanges();
            _logger.LogInformation("Se actualizo el cliente: {0}", client.Id);
            return NoContent();
            }
            [HttpDelete("{id}")]
            public ActionResult<Cliente> Delete(int id)
            {
                var client = _context.Clientes.Find(id);
                if (client == null)
                {
                    return NotFound();
                }
                _context.Clientes.Remove(client);
                _context.SaveChanges();
            _logger.LogInformation("El cliente {0} ha sido eliminado", client.Id);
            return client;
            }
        }
    
}
