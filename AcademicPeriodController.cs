using backendfepon.Data;
using backendfepon.DTOs.AcademicPeriodDTOs;
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
    public class AcademicPeriodController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public AcademicPeriodController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AcademicPeriodDTO>>> GetAcademicPeriods()
        {
            try
            {
                var academicPeriods = await _context.AcademicPeriods
                    .Select(ap => new AcademicPeriodDTO
                    {
                        academicPeriod = ap.Academic_Period_Name
                    })
                    .ToListAsync();

                return Ok(academicPeriods);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "No es posible obtener los periodos académicos"));
            }
        }

        // GET: api/AcademicPeriod/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AcademicPeriodDTO>> GetAcademicPeriod(int id)
        {
            try
            {
                var academicPeriod = await _context.AcademicPeriods
                    .Where(p => p.Academic_Period_Id == id)
                    .Select(p => new AcademicPeriodDTO
                    {
                        academicPeriod = p.Academic_Period_Name
                    })
                    .FirstOrDefaultAsync();

                if (academicPeriod == null)
                {
                    return NotFound(GenerateErrorResponse(404, "No se encontró el periodo académico"));
                }

                return Ok(academicPeriod);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "No es posible obtener los periodos académicos"));
            }
        }
    }
}
