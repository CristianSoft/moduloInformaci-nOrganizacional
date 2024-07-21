using backendfepon.Data;
using backendfepon.DTOs.RoleDTOs;
using backendfepon.DTOs.StateDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetRoles()
        {
            try
            {
                var roles = await _context.Roles
                    .Select(s => new RoleDTO
                    {
                        Role_Name = s.Role_Name
                    })
                    .ToListAsync();

                return Ok(roles);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener los roles"));
            }
        }

        // GET: api/Role/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StateDTO>> GetRole(int id)
        {
            try
            {
                var role = await _context.Roles
                    .Where(p => p.Role_Id == id)
                    .Select(p => new StateDTO
                    {
                        State_Name = p.Role_Name
                    })
                    .FirstOrDefaultAsync();

                if (role == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Rol no encontrado."));
                }

                return Ok(role);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el rol"));
            }
        }
    }

}
