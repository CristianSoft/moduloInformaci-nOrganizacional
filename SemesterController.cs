using backendfepon.Data;
using backendfepon.DTOs.EventExpenseDTO;
using backendfepon.DTOs.SemesterDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SemesterController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public SemesterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SemesterDTO>>> GetSemesters()
        {
            try
            {
                var semesters = await _context.Semesters
                    .Select(s => new SemesterDTO
                    {
                        Semester_Name = s.Semester_Name
                    })
                    .ToListAsync();

                return Ok(semesters);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no se puede obtener los semestres"));
            }
        }

        // GET: api/Semester/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SemesterDTO>> GetSemester(int id)
        {
            try
            {
                var semester = await _context.Semesters
                    .Where(p => p.Semester_Id == id)
                    .Select(p => new SemesterDTO
                    {
                        Semester_Name = p.Semester_Name
                    })
                    .FirstOrDefaultAsync();

                if (semester == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Semestre no encontrado."));
                }

                return Ok(semester);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no se puede obtener el semestre"));
            }
        }
    }
}

