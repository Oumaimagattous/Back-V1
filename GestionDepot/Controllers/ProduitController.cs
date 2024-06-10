using GestionDepot.Data;
using GestionDepot.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionDepot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduitController : ControllerBase
    {
        private readonly GestionDBContext dbcontext;
        public ProduitController(GestionDBContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var Allobj = dbcontext.Produits.ToList();
            return Ok(Allobj);
        }
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            var item = dbcontext.Produits.Find(id);
            if (item == null)
                return NotFound();
            else
                return Ok(item);

        }

        [HttpPost]
        public IActionResult AddItem(ProduitDto obj)
        {
            var dbobj = new Produit
            {
                Name = obj.Name, 

            };

            dbcontext.Produits.Add(dbobj);
            dbcontext.SaveChanges();
            return Ok(dbobj);


        }
        // PUT: api/Produit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduit(int id, ProduitDto produitDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var produit = await dbcontext.Produits.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }

            // Update the Produit entity
            produit.Name = produitDto.Name;

            dbcontext.Entry(produit).State = EntityState.Modified;

            try
            {
                await dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProduitExists(id))
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

        // DELETE: api/Produit/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduit(int id)
        {
            var produit = await dbcontext.Produits.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }

            dbcontext.Produits.Remove(produit);
            await dbcontext.SaveChangesAsync();

            return NoContent();
        }

        private bool ProduitExists(int id)
        {
            return dbcontext.Produits.Any(e => e.Id == id);
        }
    }

}