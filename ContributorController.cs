using AutoMapper;
using backendfepon.Data;
using backendfepon.DTOs.AssociationDTOs;
using backendfepon.DTOs.ContributorDTO;
using backendfepon.DTOs.ProductDTOs;
using backendfepon.Models;
using backendfepon.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContributorController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<EventController> _logger;

        public ContributorController(ApplicationDbContext context, IMapper mapper, ILogger<EventController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Contributor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContributorDTO>>> GetContributors()
        {
            try
            {
                var contributors = await _context.Contributors
                    .Include(p => p.State)
                    .Include(t => t.ContributionPlan)
                    .ThenInclude(cp => cp.AcademicPeriod)
                    .Where(p => p.State_Id == Constants.DEFAULT_STATE)
                    .Select(p => new ContributorDTO
                    {
                        id = p.Contributor_Id,
                        plan = p.ContributionPlan.Name,
                        state_id = p.State_Id,
                        price = p.ContributionPlan.Economic_Value,
                        date = p.Contributor_Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        name = p.Name,
                        career = p.Career.Career_Name,
                        faculty = p.Faculty.Faculty_Name,
                        email = p.Email,
                        academicPeriod = p.ContributionPlan.AcademicPeriod.Academic_Period_Name
                    })
                    .ToListAsync();

                return Ok(contributors);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener los aportantes"));
            }
        }

        // GET: api/Contributor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContributorDTO>> GetContributor(int id)
        {
            try
            {
                var contributor = await _context.Contributors
                    .Include(p => p.State)
                    .Include(t => t.ContributionPlan)
                     .ThenInclude(cp => cp.AcademicPeriod)
                    .Where(p => p.State_Id == Constants.DEFAULT_STATE && p.Contributor_Id == id)
                    .Select(p => new ContributorDTO
                    {
                        id = p.Contributor_Id,
                        plan = p.ContributionPlan.Name,
                        state_id = p.State_Id,
                        price = p.ContributionPlan.Economic_Value,
                        date = p.Contributor_Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        name = p.Name,
                        career = p.Career.Career_Name,
                        faculty = p.Faculty.Faculty_Name,
                        email = p.Email,
                        academicPeriod = p.ContributionPlan.AcademicPeriod.Academic_Period_Name
                    })
                    .FirstOrDefaultAsync();

                if (contributor == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Aportante no encontrado."));
                }

                return Ok(contributor);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener los aportantes"));
            }
        }

        [HttpPost]
        [Authorize(Policy = "OrganizationalOnly")]
        public async Task<ActionResult<ContributorDTO>> PostContributor(CreateUpdateContributorDTO contributorDTO)
        {
            try
            {
                var faculty = await _context.Faculties.FirstOrDefaultAsync(f => f.Faculty_Name == contributorDTO.Faculty);
                if (faculty == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Facultad no válida."));
                }

                var career = await _context.Careers.FirstOrDefaultAsync(c => c.Career_Name == contributorDTO.Career);
                if (career == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Carrera no válida."));
                }

                var plan = await _context.ContributionPlans
                    .Include(p => p.AcademicPeriod)  // Ensure AcademicPeriod is included
                    .FirstOrDefaultAsync(p => p.Name == contributorDTO.Plan);
                if (plan == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Plan no válido."));
                }

                // Log the retrieved ContributionPlan and AcademicPeriod
                _logger.LogInformation($"Retrieved ContributionPlan: {plan.Name}, AcademicPeriod: {plan.AcademicPeriod?.Academic_Period_Name}");

                var contributor = _mapper.Map<Contributor>(contributorDTO);
                contributor.Faculty_Id = faculty.Faculty_Id;
                contributor.Career_Id = career.Career_Id;
                contributor.Plan_Id = plan.Plan_Id;
                contributor.State_Id = Constants.DEFAULT_STATE;

                // Log the mapped Contributor for debugging
                _logger.LogInformation($"Mapped Contributor: {contributor.Name}, {contributor.Email}, {contributor.Contributor_Date}, Plan: {contributor.ContributionPlan?.Name}, AcademicPeriod: {contributor.ContributionPlan?.AcademicPeriod?.Academic_Period_Name}");

                _context.Contributors.Add(contributor);
                await _context.SaveChangesAsync();

                var createdContributorDTO = _mapper.Map<ContributorDTO>(contributor);

                // Ensure the AcademicPeriod is included in the DTO
                createdContributorDTO.academicPeriod = plan.AcademicPeriod?.Academic_Period_Name;

                return CreatedAtAction(nameof(GetContributor), new { id = contributor.Contributor_Id }, createdContributorDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating contributor");
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible crear el contribuyente."));
            }
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "OrganizationalOnly")]
        public async Task<IActionResult> PutContributor(int id, CreateUpdateContributorDTO contributorDTO)
        {
            try
            {
                var oldContributor = await _context.Contributors.FindAsync(id);
                if (oldContributor == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Contribuyente no encontrado."));
                }

                var faculty = await _context.Faculties.FirstOrDefaultAsync(f => f.Faculty_Name == contributorDTO.Faculty);
                if (faculty == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Facultad no válida."));
                }

                var career = await _context.Careers.FirstOrDefaultAsync(c => c.Career_Name == contributorDTO.Career);
                if (career == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Carrera no válida."));
                }

                var plan = await _context.ContributionPlans
                   .Include(p => p.AcademicPeriod)  // Ensure AcademicPeriod is included
                   .FirstOrDefaultAsync(p => p.Name == contributorDTO.Plan);
                if (plan == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Plan no válido."));
                }

                _mapper.Map(contributorDTO, oldContributor);
                oldContributor.Faculty_Id = faculty.Faculty_Id;
                oldContributor.Career_Id = career.Career_Id;
                oldContributor.Plan_Id = plan.Plan_Id;

                _context.Entry(oldContributor).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContributorExists(id))
                    {
                        return NotFound(GenerateErrorResponse(404, "Contribuyente no encontrado."));
                    }
                    else
                    {
                        return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error de concurrencia."));
                    }
                }

                // Map the updated contributor to a DTO
                var updatedContributorDTO = _mapper.Map<ContributorDTO>(oldContributor);
                updatedContributorDTO.academicPeriod = plan.AcademicPeriod?.Academic_Period_Name;

                return Ok(updatedContributorDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible actualizar el contribuyente."));
            }
        }

        // PATCH: api/Products/5
        [HttpPatch("{id}")]
        [Authorize(Policy = "OrganizationalOnly")]
        public async Task<IActionResult> PatchContributorState(int id)
        {
            try
            {
                var contributor = await _context.Contributors.FindAsync(id);
                if (contributor == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Aportante no encontrado."));
                }

                contributor.State_Id = Constants.STATE_INACTIVE;
                _context.Entry(contributor).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContributorExists(id))
                    {
                        return NotFound(GenerateErrorResponse(404, "Aportante no encontrado."));
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
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible actualizar el estado"));
            }
        }

        private bool ContributorExists(int id)
        {
            return _context.Contributors.Any(e => e.Contributor_Id == id);
        }
    }
}
