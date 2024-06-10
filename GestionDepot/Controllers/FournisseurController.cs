using GestionDepot.Data;
using GestionDepot.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionDepot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FournisseurController : ControllerBase
    {
        private readonly GestionDBContext dbcontext;
        public FournisseurController(GestionDBContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var Allobj = dbcontext.Fournisseurs.ToList();
            return Ok(Allobj);
        }
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            var item = dbcontext.Fournisseurs.Find(id);
            if (item == null)
                return NotFound();
            else
                return Ok(item);

        }
        [HttpPost]
        public IActionResult AddItem(FournisseurDto obj)
        {
            var dbobj = new Fournisseur
            {
                Name = obj.Name,
                Adresse = obj.Adresse,
             

            };

            dbcontext.Fournisseurs.Add(dbobj);
            dbcontext.SaveChanges();
            return Ok(dbobj);


        }
        // PUT: api/Fournisseur/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFournisseur(int id, FournisseurDto fournisseurDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fournisseur = await dbcontext.Fournisseurs.FindAsync(id);
            if (fournisseur == null)
            {
                return NotFound();
            }

            // Update the Fournisseur entity
            fournisseur.Name = fournisseurDto.Name;
            fournisseur.Adresse = fournisseurDto.Adresse;

            dbcontext.Entry(fournisseur).State = EntityState.Modified;

            try
            {
                await dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FournisseurExists(id))
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

        // DELETE: api/Fournisseur/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFournisseur(int id)
        {
            var fournisseur = await dbcontext.Fournisseurs.FindAsync(id);
            if (fournisseur == null)
            {
                return NotFound();
            }

            dbcontext.Fournisseurs.Remove(fournisseur);
            await dbcontext.SaveChangesAsync();

            return NoContent();
        }

        private bool FournisseurExists(int id)
        {
            return dbcontext.Fournisseurs.Any(e => e.Id == id);
        }
    }
}

