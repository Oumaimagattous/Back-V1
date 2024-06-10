using GestionDepot.Data;
using GestionDepot.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionDepot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly GestionDBContext dbcontext;
        public ClientController(GestionDBContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var Allobj = dbcontext.Clients.ToList();
            return Ok(Allobj);
        }
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            var item = dbcontext.Clients.Find(id);
            if (item == null)
                return NotFound();
            else
                return Ok(item);

        }
        [HttpPost]
        public IActionResult AddItem(ClientDto obj)
        {
            var dbobj = new Client
            {
                Name = obj.Name,
                Adresse = obj.Adresse,
                Type = obj.Type,

            };

            dbcontext.Clients.Add(dbobj);
            dbcontext.SaveChanges();
            return Ok(dbobj);


        }
        // PUT: api/Client/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, ClientDto clientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = await dbcontext.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            // Update the Client entity
            client.Name = clientDto.Name;
            client.Adresse = clientDto.Adresse;
            client.Type = clientDto.Type;

            dbcontext.Entry(client).State = EntityState.Modified;

            try
            {
                await dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Client/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await dbcontext.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            dbcontext.Clients.Remove(client);
            await dbcontext.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(int id)
        {
            return dbcontext.Clients.Any(e => e.Id == id);
        }
    }

}


