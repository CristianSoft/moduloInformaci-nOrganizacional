using backendfepon.Data;
using backendfepon.DTOs.AcademicPeriodDTOs;
using backendfepon.DTOs.CareerDTOs;
using backendfepon.DTOs.CategoryDTOs;
using backendfepon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CareerController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public CareerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CareerDTO>>> GetCareers()
        {
            try
            {
                var careers = await _context.Careers
                    .Select(c => new CareerDTO
                    {
                        Career_Name = c.Career_Name
                    })
                    .ToListAsync();

                return Ok(careers);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener la carrera"));
            }
        }

        // GET: api/Career/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CareerDTO>> GetCareer(int id)
        {
            try
            {
                var career = await _context.Careers
                    .Where(p => p.Career_Id == id)
                    .Select(p => new CareerDTO
                    {
                        Career_Name = p.Career_Name
                    })
                    .FirstOrDefaultAsync();

                if (career == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Carrera no encontrada."));
                }

                return Ok(career);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener la carrera"));
            }
        }
    }
}
