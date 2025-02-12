using LoginAPI.Data;
using LoginAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace LoginAPI.Controllers
{
    [Route("api/Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<LoginDTO>> GetUsers()
        {
            return Ok(LoginStore.LoginList);
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<LoginDTO> GetUser(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var user = LoginStore.LoginList.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<LoginDTO> CreateLog([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(loginDTO.Username) || string.IsNullOrEmpty(loginDTO.Password))
            {
                ModelState.AddModelError("CustomError", "Username and password are required.");
                return BadRequest(ModelState);
            }

            if (LoginStore.LoginList.FirstOrDefault(u => u.Username.ToLower() == loginDTO.Username.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Username already exists!");
                return BadRequest(ModelState);
            }

            if (loginDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // Generate a new ID
            loginDTO.Id = LoginStore.LoginList.OrderByDescending(u => u.Id).FirstOrDefault()?.Id + 1 ?? 1;
            LoginStore.LoginList.Add(loginDTO);

            return CreatedAtRoute("GetUser", new { id = loginDTO.Id }, loginDTO);
        }
    }
}