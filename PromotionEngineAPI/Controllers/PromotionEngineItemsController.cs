using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromotionEngineAPI.Models;
using Promotion.Engine.Library;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionEngineItemsController : ControllerBase
    {
        private readonly PromotionEngineContext _context;

        public PromotionEngineItemsController(PromotionEngineContext context)
        {
            _context = context;
        }

        // GET: api/PromotionEngineItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromotionEngineItem>>> GetPromotionEngineItems()
        {
            return await _context.PromotionEngineItems.ToListAsync();
        }

        // GET: api/PromotionEngineItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PromotionEngineItem>> GetPromotionEngineItem(long id)
        {
            var promotionEngineItem = await _context.PromotionEngineItems.FindAsync(id);

            if (promotionEngineItem == null)
            {
                return NotFound();
            }

            return promotionEngineItem;
        }

        // PUT: api/PromotionEngineItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPromotionEngineItem(long id, PromotionEngineItem promotionEngineItem)
        {
            if (id != promotionEngineItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(promotionEngineItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PromotionEngineItemExists(id))
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

        private static void Add3PromotionRules(List<PromotionRule> promotionRules)
        {
            // Create Promotion rule
            int nItems = 3;
            int price = nItems*PromotionEngineLibrary.PriceA - PromotionEngineLibrary.Promotion3AsSaving;
            string item_i = "A";
            promotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);

            // Create Promotion rule
            nItems = 2;
            var priceRule2Bs = nItems*PromotionEngineLibrary.PriceB - PromotionEngineLibrary.Promotion2BsSaving;
            item_i = "B";
            promotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, priceRule2Bs);

            // Create Promotion rule
            price = PromotionEngineLibrary.PromotionCandD;
            item_i = "C";
            string item_j = "D";
            promotionRules.CreatePromotion2ItemsForFixedPrice(item_i, item_j, price);
        }

        // POST: api/PromotionEngineItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PromotionEngineItem>> PostPromotionEngineItem(PromotionEngineItem promotionEngineItem)
        {
            // Todo: Based on InputSKU compute the total price and store in in-memory database
            try {
                var _input = promotionEngineItem.InputSKU;
                if (string.IsNullOrEmpty(_input))
                    throw new ArgumentNullException("Parameter needs to be set", nameof(_input));
                var stockKeepingUnits = new List<string>(_input.Split(","));
                var _counts = stockKeepingUnits.CountSKU();
                
                List<PromotionRule> _promotionRules = new List<PromotionRule>();
                Add3PromotionRules(_promotionRules);
                var _totalPrice = _counts.TotalPriceUsingPromotionRules(_promotionRules);

                promotionEngineItem.TotalPrice = _totalPrice;

            } catch (ArgumentNullException) {}

            _context.PromotionEngineItems.Add(promotionEngineItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPromotionEngineItem), new { id = promotionEngineItem.Id }, promotionEngineItem);
        }

        // DELETE: api/PromotionEngineItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromotionEngineItem(long id)
        {
            var promotionEngineItem = await _context.PromotionEngineItems.FindAsync(id);
            if (promotionEngineItem == null)
            {
                return NotFound();
            }

            _context.PromotionEngineItems.Remove(promotionEngineItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PromotionEngineItemExists(long id)
        {
            return _context.PromotionEngineItems.Any(e => e.Id == id);
        }
    }
}
