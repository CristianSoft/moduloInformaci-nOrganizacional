using backendfepon.Data;
using backendfepon.DTOs.FacultyDTOs;
using backendfepon.DTOs.StateDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FacultyController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public FacultyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacultyDTO>>> GetFaculties()
        {
            try
            {
                var faculties = await _context.Faculties
                    .Select(s => new FacultyDTO
                    {
                        Faculty_Name = s.Faculty_Name
                    })
                    .ToListAsync();

                return Ok(faculties);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener la facultad"));
            }
        }

        // GET: api/Faculty/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FacultyDTO>> GetFaculty(int id)
        {
            try
            {
                var faculty = await _context.Faculties
                    .Where(p => p.Faculty_Id == id)
                    .Select(p => new FacultyDTO
                    {
                        Faculty_Name = p.Faculty_Name
                    })
                    .FirstOrDefaultAsync();

                if (faculty == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Facultad no encontrada."));
                }

                return Ok(faculty);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener la facultad"));
            }
        }
    }
}
