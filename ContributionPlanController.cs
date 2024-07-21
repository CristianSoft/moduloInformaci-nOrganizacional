using AutoMapper;
using backendfepon.Data;
using backendfepon.DTOs.AssociationDTOs;
using backendfepon.DTOs.ContributionPlanDTOs;
using backendfepon.DTOs.ProductDTOs;
using backendfepon.Models;
using backendfepon.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ContributionPlanController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ContributionPlanController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/ContributionPlan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContributionPlanDTO>>> GetContributionPlans()
        {
            try
            {
                var contributionPlans = await _context.ContributionPlans
                    .Include(p => p.State)
                    .Include(p => p.AcademicPeriod)
                    .Where(p => p.State_Id == Constants.DEFAULT_STATE)
                    .Select(p => new ContributionPlanDTO
                    {
                        id = p.Plan_Id,
                        academicPeriod = p.AcademicPeriod.Academic_Period_Name,
                        state_id = p.State_Id,
                        planName = p.Name,
                        price = p.Economic_Value,
                        benefits = p.Benefits
                    })
                    .ToListAsync();

                return Ok(contributionPlans);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el plan de aportación"));
            }
        }

        // GET: api/ContributionPlan/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContributionPlanDTO>> GetContributionPlan(int id)
        {
            try
            {
                var contributionPlan = await _context.ContributionPlans
                    .Include(p => p.State)
                    .Include(p => p.AcademicPeriod)
                    .Where(p => p.State_Id == Constants.DEFAULT_STATE && p.Plan_Id == id)
                    .Select(p => new ContributionPlanDTO
                    {
                        id = p.Plan_Id,
                        academicPeriod = p.AcademicPeriod.Academic_Period_Name,
                        state_id = p.State_Id,
                        planName = p.Name,
                        price = p.Economic_Value,
                        benefits = p.Benefits
                    })
                    .FirstOrDefaultAsync();

                if (contributionPlan == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Plan de contribución no encontrado."));
                }

                return Ok(contributionPlan);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el plan de aportación"));
            }
        }

        // POST: api/ContributionPlan
        [HttpPost]
        [Authorize(Policy = "OrganizationalOnly")]
        public async Task<ActionResult<ContributionPlanDTO>> PostContributionPlan(CreateUpdateContributionPlanDTO contributionPlanDTO)
        {
            try
            {

                var academicPeriod = await _context.AcademicPeriods.FirstOrDefaultAsync(a => a.Academic_Period_Name == contributionPlanDTO.Academic_Period_Name);
                if (academicPeriod == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre del período académico no válido."));
                }

                var contributionPlan = _mapper.Map<ContributionPlan>(contributionPlanDTO);
                contributionPlan.State_Id = Constants.DEFAULT_STATE;
                contributionPlan.Academic_Period_Id = academicPeriod.Academic_Period_Id;

                _context.ContributionPlans.Add(contributionPlan);
                await _context.SaveChangesAsync();

                var createdContributionPlanDTO = _mapper.Map<ContributionPlanDTO>(contributionPlan);

                return CreatedAtAction(nameof(GetContributionPlan), new { id = contributionPlan.Plan_Id }, createdContributionPlanDTO);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible crear el plan de aportación"));
            }
        }

        // PUT: api/ContributionPlan/5
        [HttpPut("{id}")]
        [Authorize(Policy = "OrganizationalOnly")]
        public async Task<IActionResult> PutContributionPlan(int id, CreateUpdateContributionPlanDTO updatedContributionPlan)
        {
            try
            {
                var contributionPlan = await _context.ContributionPlans.FindAsync(id);
                if (contributionPlan == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "ID del plan de contribución no válido."));
                }

                var academicPeriod = await _context.AcademicPeriods.FirstOrDefaultAsync(a => a.Academic_Period_Name == updatedContributionPlan.Academic_Period_Name);
                if (academicPeriod == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre del período académico no válido."));
                }

                _mapper.Map(updatedContributionPlan, contributionPlan);
                contributionPlan.Academic_Period_Id = academicPeriod.Academic_Period_Id;

                _context.Entry(contributionPlan).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContributionPlanExists(id))
                    {
                        return NotFound(GenerateErrorResponse(404, "Plan de contribución no encontrado."));
                    }
                    else
                    {
                        return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error de concurrencia."));
                    }
                }

                return NoContent();
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible actualizar el plan de aportación"));
            }
        }

        // PATCH: api/ContributionPlan/5
        [HttpPatch("{id}")]
        [Authorize(Policy = "OrganizationalOnly")]
        public async Task<IActionResult> PatchContributionPlanState(int id)
        {
            try
            {
                var contributionPlan = await _context.ContributionPlans.FindAsync(id);
                if (contributionPlan == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Plan de contribución no encontrado."));
                }

                contributionPlan.State_Id = Constants.STATE_INACTIVE;

                _context.Entry(contributionPlan).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContributionPlanExists(id))
                    {
                        return NotFound(GenerateErrorResponse(404, "Plan de contribución no encontrado."));
                    }
                    else
                    {
                        return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error de concurrencia."));
                    }
                }

                return NoContent();
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible actualizar el plan de aportación"));
            }
        }

        private bool ContributionPlanExists(int id)
        {
            return _context.ContributionPlans.Any(e => e.Plan_Id == id);
        }
    }
}
