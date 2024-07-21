using AutoMapper;
using backendfepon.Data;
using backendfepon.DTOs.AdmnistrativeMemberDTOs;
using backendfepon.DTOs.AssociationDTOs;
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
    public class AdministrativeMembersController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<EventController> _logger;

        public AdministrativeMembersController(ApplicationDbContext context, IMapper mapper, ILogger<EventController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/AdministrativeMembers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdministrativeMemberDTO>>> GetAdministrativesMembers()
        {
            try
            {
                var administrativeMembers = await _context.AdministrativeMembers
                    //.Include(e => e.Student)
                    .Where(p => p.State_Id == Constants.DEFAULT_STATE)
                    .Select(p => new AdministrativeMemberDTO
                    {
                        id = p.Administrative_Member_Id,
                        state_id = p.State_Id,
                        firstName = p.Name,
                        lastName = p.Last_Name,
                        birthDate = p.Birth_Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        cellphone = p.Phone,
                        faculty = p.Faculty.Faculty_Name,
                        career = p.Career.Career_Name,
                        semester = p.Semester.Semester_Name,
                        email = p.Email,
                        position = p.Role.Role_Name
                    })
                    .ToListAsync();

                return Ok(administrativeMembers);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor."));
            }
        }

        // GET: api/AdministrativeMembers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdministrativeMemberDTO>> GetAdministrativeMember(int id)
        {
            try
            {
                var administrativeMember = await _context.AdministrativeMembers
                    //.Include(e => e.Student)
                    .Where(p => p.State_Id == Constants.DEFAULT_STATE && p.Administrative_Member_Id == id)
                    .Select(p => new AdministrativeMemberDTO
                    {
                        id = p.Administrative_Member_Id,
                        firstName = p.Name,
                        lastName = p.Last_Name,
                        state_id = p.State_Id,
                        birthDate = p.Birth_Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        cellphone = p.Phone,
                        faculty = p.Faculty.Faculty_Name,
                        career = p.Career.Career_Name,
                        semester = p.Semester.Semester_Name,
                        email = p.Email,
                        position = p.Role.Role_Name,
                    })
                    .FirstOrDefaultAsync();

                if (administrativeMember == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Miembro administrativo no encontrado."));
                }

                return Ok(administrativeMember);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "\"Ocurrió un error interno del servidor, no es posible obtenet el miembro administrativo"));
            }
        }
 

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<AdministrativeMemberDTO>> PostAdministrativeMember(CreateUpdateAdministrativeMemberDTO administrativeMemberDTO)
        {
            try
            {
                // Verificar si el Faculty existe
                var faculty = await _context.Faculties.FirstOrDefaultAsync(f => f.Faculty_Name == administrativeMemberDTO.Faculty);
                if (faculty == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre de facultad no válido."));
                }

                // Verificar si el Career existe
                var career = await _context.Careers.FirstOrDefaultAsync(c => c.Career_Name == administrativeMemberDTO.Career);
                if (career == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre de carrera no válido."));
                }

                // Verificar si el Semester existe
                var semester = await _context.Semesters.FirstOrDefaultAsync(s => s.Semester_Name == administrativeMemberDTO.Semester);
                if (semester == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre de semestre no válido."));
                }
                
                

                // Verificar si el Role existe
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Role_Name == administrativeMemberDTO.Position);
                if (role == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Rol no válido."));
                }

                // Mapear el DTO a la entidad del modelo
                var administrativeMember = _mapper.Map<AdministrativeMember>(administrativeMemberDTO);
                //administrativeMember.Birth_Date = DateTime.Now;
                administrativeMember.Faculty_Id = faculty.Faculty_Id;
                administrativeMember.Career_Id = career.Career_Id;
                administrativeMember.Semester_Id = semester.Semester_Id;
                administrativeMember.Role_Id = role.Role_Id;

                administrativeMember.State_Id = Constants.DEFAULT_STATE;
                

                // Guardar la nueva entidad en la base de datos
                _context.AdministrativeMembers.Add(administrativeMember);
                await _context.SaveChangesAsync();
                
                // Mapear la entidad creada de vuelta al DTO
                var createdAdministrativeMemberDTO = _mapper.Map<AdministrativeMemberDTO>(administrativeMember);
                

                // Devolver la respuesta con el nuevo miembro administrativo creado
                return CreatedAtAction(nameof(GetAdministrativeMember), new { id = administrativeMember.Administrative_Member_Id }, createdAdministrativeMemberDTO);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible crear el miembro administrativo."));
            }
        }


        // PUT: api/AdministrativeMembers/5
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> PutAdministrativeMember(int id, CreateUpdateAdministrativeMemberDTO administrativeMemberDTO)
        {
            try
            {
                var existingMember = await _context.AdministrativeMembers.FindAsync(id);
                if (existingMember == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Miembro administrativo no encontrado."));
                }

                var faculty = await _context.Faculties.FirstOrDefaultAsync(f => f.Faculty_Name == administrativeMemberDTO.Faculty);
                if (faculty == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre de facultad no válido."));
                }

                var career = await _context.Careers.FirstOrDefaultAsync(c => c.Career_Name == administrativeMemberDTO.Career);
                if (career == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre de carrera no válido."));
                }

                var semester = await _context.Semesters.FirstOrDefaultAsync(s => s.Semester_Name == administrativeMemberDTO.Semester);
                if (semester == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre de semestre no válido."));
                }

                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Role_Name == administrativeMemberDTO.Position);
                if (role == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Rol no válido."));
                }

                _mapper.Map(administrativeMemberDTO, existingMember);
                existingMember.Faculty_Id = faculty.Faculty_Id;
                existingMember.Career_Id = career.Career_Id;
                existingMember.Semester_Id = semester.Semester_Id;
                existingMember.Role_Id = role.Role_Id;

                _context.Entry(existingMember).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdministrativeMemberExists(id))
                    {
                        return NotFound(GenerateErrorResponse(404, "Miembro administrativo no encontrado."));
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
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible actualizar el miembro administrativo."));
            }
        }

    


        // DELETE: api/AdministrativeMembers/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteAdministrativeMember(int id)
        {
            try
            {
                var administrativeMember = await _context.AdministrativeMembers.FindAsync(id);
                if (administrativeMember == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Miembro administrativo no encontrado."));
                }

                _context.AdministrativeMembers.Remove(administrativeMember);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible eliminar el miembro administrativo"));
            }
        }


        // PATCH: api/Products/5
        [HttpPatch("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> PatchProductState(int id)
        {
            try
            {
                var administrativeMember = await _context.AdministrativeMembers.FindAsync(id);
                if (administrativeMember == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Miembro admnistrativo no encontrado."));
                }

                administrativeMember.State_Id = Constants.STATE_INACTIVE;
                _context.Entry(administrativeMember).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdministrativeMemberExists(id))
                    {
                        return NotFound(GenerateErrorResponse(404, "Miembro administrativo no encontrado."));
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

        private bool AdministrativeMemberExists(int id)
        {
            return _context.AdministrativeMembers.Any(e => e.Administrative_Member_Id == id);
        }
    }
}
