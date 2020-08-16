using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using assignmentAPI.Context;
using assignmentAPI.Model;

namespace assignmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuControlController : ControllerBase
    {
        private readonly DataContext _context;

        public MenuControlController(DataContext context)
        {
            _context = context;
        }

        // GET: api/MenuControl
        [HttpGet]
        public IEnumerable<MenuControlModel> GetMenu()
        {
            return _context.Menu;
        }

        // GET: api/MenuControl/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenuControlModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var menuControlModel = await _context.Menu.FindAsync(id);
            
            if (menuControlModel == null)
            {
                return NotFound();
            }

            return Ok(menuControlModel);
        }

        // PUT: api/MenuControl/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMenuControlModel([FromRoute] int id, [FromBody] MenuControlModel menuControlModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != menuControlModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(menuControlModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuControlModelExists(id))
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


        // POST: api/MenuControl
        [HttpPost]
        public async Task<IActionResult> PostMenuControlModel([FromBody] MenuControlModel menuControlModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Menu.Add(menuControlModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMenuControlModel", new { id = menuControlModel.Id }, menuControlModel);
        }

        // DELETE: api/MenuControl/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuControlModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var menuControlModel = await _context.Menu.FindAsync(id);
            if (menuControlModel == null)
            {
                return NotFound();
            }

            _context.Menu.Remove(menuControlModel);
            await _context.SaveChangesAsync();

            return Ok(menuControlModel);
        }

        private bool MenuControlModelExists(int id)
        {
            return _context.Menu.Any(e => e.Id == id);
        }
    }
}