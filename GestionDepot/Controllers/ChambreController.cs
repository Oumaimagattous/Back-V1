using GestionDepot.Data;
using GestionDepot.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionDepot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChambreController : ControllerBase
    {
        private readonly GestionDBContext dbcontext;
        public ChambreController(GestionDBContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var Allobj = dbcontext.Chambres.ToList();
            return Ok(Allobj);
        }
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            var item = dbcontext.Chambres.Find(id);
            if (item == null)
                return NotFound();
            else
                return Ok(item);

        }
        [HttpPost]
        public IActionResult AddItem(ChambreDto obj)
        {
            var dbobj = new Chambre
            {
                Name = obj.Name,

            };

            dbcontext.Chambres.Add(dbobj);
            dbcontext.SaveChanges();
            return Ok(dbobj);


        }
        // PUT: api/Chambre/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChambre(int id, ChambreDto chambreDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var chambre = await dbcontext.Chambres.FindAsync(id);
            if (chambre == null)
            {
                return NotFound();
            }

            // Update the Chambre entity
            chambre.Name = chambreDto.Name;

            dbcontext.Entry(chambre).State = EntityState.Modified;

            try
            {
                await dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChambreExists(id))
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

        // DELETE: api/Chambre/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChambre(int id)
        {
            var chambre = await dbcontext.Chambres.FindAsync(id);
            if (chambre == null)
            {
                return NotFound();
            }

            dbcontext.Chambres.Remove(chambre);
            await dbcontext.SaveChangesAsync();

            return NoContent();
        }

        private bool ChambreExists(int id)
        {
            return dbcontext.Chambres.Any(e => e.Id == id);
        }
    }

}