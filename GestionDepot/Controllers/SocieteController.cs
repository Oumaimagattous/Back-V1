using GestionDepot.Data;
using GestionDepot.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionDepot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocieteController : ControllerBase
    {
        private readonly GestionDBContext dbcontext;
        public SocieteController(GestionDBContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var Allobj = dbcontext.Societes.ToList();
            return Ok(Allobj);
        }
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            var item = dbcontext.Societes.Find(id);
            if (item == null)
                return NotFound();
            else
                return Ok(item);

        }
        [HttpPost]
        public IActionResult AddItem(SocieteDto obj)
        {
            var dbobj = new Societe
            {
                Name = obj.Name,
                Adresse = obj.Adresse,
                MF = obj.MF,

            };

            dbcontext.Societes.Add(dbobj);
            dbcontext.SaveChanges();
            return Ok(dbobj);


        }
        // PUT: api/Societe/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSociete(int id, SocieteDto societeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var societe = await dbcontext.Societes.FindAsync(id);
            if (societe == null)
            {
                return NotFound();
            }

            // Update the Societe entity
            societe.Name = societeDto.Name;
            societe.Adresse = societeDto.Adresse;
            societe.MF = societeDto.MF;

            dbcontext.Entry(societe).State = EntityState.Modified;

            try
            {
                await dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SocieteExists(id))
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
        // DELETE: api/Societe/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSociete(int id)
        {
            var societe = await dbcontext.Societes.FindAsync(id);
            if (societe == null)
            {
                return NotFound();
            }

            dbcontext.Societes.Remove(societe);
            await dbcontext.SaveChangesAsync();

            return NoContent();
        }

        private bool SocieteExists(int id)
        {
            return dbcontext.Societes.Any(e => e.Id == id);
        }


    }

}

