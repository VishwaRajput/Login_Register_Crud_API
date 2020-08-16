using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using assignmentAPI.Context;
using assignmentAPI.Model;

namespace assignmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsGridController : ControllerBase
    {
        private readonly DataContext _context;

        public StudentsGridController(DataContext context)
        {
            _context = context;
        }

        // GET: api/StudentsGrid
        [HttpGet]
        public IEnumerable<StudentsGridModel> GetStudents()
        {
            return _context.Students;
        }

        // GET: api/StudentsGrid/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentsGridModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentsGridModel = await _context.Students.FindAsync(id);

            if (studentsGridModel == null)
            {
                return NotFound();
            }

            return Ok(studentsGridModel);
        }

        // PUT: api/StudentsGrid/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentsGridModel([FromRoute] int id, [FromBody] StudentsGridModel studentsGridModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != studentsGridModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(studentsGridModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentsGridModelExists(id))
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

        // POST: api/StudentsGrid
        [HttpPost]
        public async Task<IActionResult> PostStudentsGridModel([FromBody] StudentsGridModel studentsGridModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Students.Add(studentsGridModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentsGridModel", new { id = studentsGridModel.Id }, studentsGridModel);
        }

        // DELETE: api/StudentsGrid/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentsGridModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentsGridModel = await _context.Students.FindAsync(id);
            if (studentsGridModel == null)
            {
                return NotFound();
            }

            _context.Students.Remove(studentsGridModel);
            await _context.SaveChangesAsync();

            return Ok(studentsGridModel);
        }

        private bool StudentsGridModelExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}