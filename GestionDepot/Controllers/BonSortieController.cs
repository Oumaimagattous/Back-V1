using GestionDepot.Data;
using GestionDepot.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionDepot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BonSortieController : ControllerBase
    {
        private readonly GestionDBContext _context;
        private readonly ILogger<BonSortieController> _logger;

        public BonSortieController(GestionDBContext context, ILogger<BonSortieController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/BonEntree
        // GET: api/BonEntree
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BonSortie>>> GetBonSorties()
        {
            var bonSorties = await _context.BonSorties
                .Include(b => b.Client)
                .Include(b => b.Produit)
                .Include(b => b.Chambre)
                .Include(b => b.Societe)
                .ToListAsync();

            // Fix invalid date values
            foreach (var bonSortie in bonSorties)
            {
                if (bonSortie.Date == DateTime.MinValue)
                {
                    bonSortie.Date = new DateTime(1970, 1, 1); // Set a default date if needed
                }
            }

            return bonSorties;
        }


        // POST: api/BonSortie
        [HttpPost]
        public async Task<ActionResult<BonSortie>> CreateBonSortis(BonSortieDto bonSortieDto)
        {
            _logger.LogInformation("Received POST request to create BonSortie");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid");
                return BadRequest(ModelState);
            }

            // Validate foreign keys
            if (bonSortieDto.IdClient.HasValue)
            {
                var clientExists = await _context.Clients.AnyAsync(f => f.Id == bonSortieDto.IdClient);
                if (!clientExists)
                {
                    _logger.LogWarning($"Invalid Clinet Id: {bonSortieDto.IdClient}");
                    return BadRequest("Invalid Clinet Id");
                }
            }

            if (bonSortieDto.IdProduit.HasValue)
            {
                var produitExists = await _context.Produits.AnyAsync(p => p.Id == bonSortieDto.IdProduit);
                if (!produitExists)
                {
                    _logger.LogWarning($"Invalid Produit Id: {bonSortieDto.IdProduit}");
                    return BadRequest("Invalid Produit Id");
                }
            }

            if (bonSortieDto.IdChambre.HasValue)
            {
                var chambreExists = await _context.Chambres.AnyAsync(c => c.Id == bonSortieDto.IdChambre);
                if (!chambreExists)
                {
                    _logger.LogWarning($"Invalid Chambre Id: {bonSortieDto.IdChambre}");
                    return BadRequest("Invalid Chambre Id");
                }
            }

            if (bonSortieDto.IdSociete.HasValue)
            {
                var societeExists = await _context.Societes.AnyAsync(s => s.Id == bonSortieDto.IdSociete);
                if (!societeExists)
                {
                    _logger.LogWarning($"Invalid Societe Id: {bonSortieDto.IdSociete}");
                    return BadRequest("Invalid Societe Id");
                }
            }

            var bonSortie = new BonSortie
            {
                Date = bonSortieDto.Date,
                Qte = bonSortieDto.Qte,
                IdClient = bonSortieDto.IdClient,
                IdProduit = bonSortieDto.IdProduit,
                IdChambre = bonSortieDto.IdChambre,
                IdSociete = bonSortieDto.IdSociete,
            };

            _context.BonSorties.Add(bonSortie);
            await _context.SaveChangesAsync();

            // Inclure les propriétés de navigation
            var createdBonSortie = await _context.BonSorties
                .Include(b => b.Client)
                .Include(b => b.Produit)
                .Include(b => b.Chambre)
                .Include(b => b.Societe)
                .FirstOrDefaultAsync(b => b.Id == bonSortie.Id);

            _logger.LogInformation($"Created BonEntree with Id: {createdBonSortie.Id}");

            return CreatedAtAction(nameof(GetBonSorties), new { id = createdBonSortie.Id }, createdBonSortie);
        }

        // GET: api/BonEntree/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BonSortie>> GetBonSorties(int id)
        {
            var bonSortie = await _context.BonSorties
                .Include(b => b.Client)
                .Include(b => b.Produit)
                .Include(b => b.Chambre)
                .Include(b => b.Societe)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bonSortie == null)
            {
                return NotFound();
            }

            return bonSortie;
        }

        // PUT: api/BonEntree/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBonSortie(int id, BonSortieDto bonSortieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bonSortie = await _context.BonSorties.FindAsync(id);
            if (bonSortie == null)
            {
                return NotFound();
            }

            // Update the BonEntree entity
            bonSortie.Date = bonSortieDto.Date;
            bonSortie.Qte = bonSortieDto.Qte;
            bonSortie.IdClient = bonSortieDto.IdClient;
            bonSortie.IdProduit = bonSortieDto.IdProduit;
            bonSortie.IdChambre = bonSortieDto.IdChambre;
            bonSortie.IdSociete = bonSortieDto.IdSociete;

            _context.Entry(bonSortie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BonSortieExists(id))
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

        // DELETE: api/BonEntree/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBonSortie(int id)
        {
            var bonSortie = await _context.BonSorties.FindAsync(id);
            if (bonSortie == null)
            {
                return NotFound();
            }

            _context.BonSorties.Remove(bonSortie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BonSortieExists(int id)
        {
            return _context.BonSorties.Any(e => e.Id == id);
        }
    }
}
