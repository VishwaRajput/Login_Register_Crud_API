using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using assignmentAPI.Context;
using assignmentAPI.Model;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Options;

namespace assignmentAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        private readonly DataContext _context;
        IService service;
        private readonly AppSetting _appSettings;

        public UserDetailsController(IOptions<AppSetting> appSettings, IService _service, DataContext context)
        {
            _appSettings = appSettings.Value;
            service = _service;
            _context = context;
        }

        [AllowAnonymous]
        [Route("~authenticate")]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]LoginModel user)
        {
            var validUser = service.Authenticate(user.Username, user.Password);

            if (validUser == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, validUser.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            
            return Ok(new
            {
                Id = validUser.Id,
                Username = validUser.Username,
                Email = validUser.Email,
                Token = tokenString
            });
        }

        // GET: api/UserDetails
        [HttpGet]
        public IEnumerable<UserDetailsModel> GetUsers()
        {
            return _context.Users;
        }

        // GET: api/UserDetails/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDetailsModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userDetailsModel = await _context.Users.FindAsync(id);

            if (userDetailsModel == null)
            {
                return NotFound();
            }

            return Ok(userDetailsModel);
        }

        // GET: api/UserDetails/GetUserByEmail?email=email@gmail.com
        [AllowAnonymous]
        [HttpGet("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users.SingleOrDefault(x => x.Email == email);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> PutPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }

            var user = _context.Users.SingleOrDefault(x => x.Email == model.email);
            
            if(user != null)
            {
                user.Password = model.newPassword;
                _context.SaveChanges();

                return Ok();
            }

            return BadRequest();
        }

        // PUT: api/UserDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserDetailsModel([FromRoute] int id, [FromBody] UserDetailsModel userDetailsModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userDetailsModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(userDetailsModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDetailsModelExists(id))
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

        // POST: api/UserDetails
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostUserDetailsModel([FromBody] UserDetailsModel userDetailsModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(service.Create(userDetailsModel))
            {
                _context.Users.Add(userDetailsModel);
                await _context.SaveChangesAsync();

                //return CreatedAtAction("GetUserDetailsModel", new { id = userDetailsModel.Id }, userDetailsModel);
                return Ok();
            }

            return BadRequest(ModelState);

        }

        // DELETE: api/UserDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserDetailsModel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userDetailsModel = await _context.Users.FindAsync(id);
            if (userDetailsModel == null)
            {
                return NotFound();
            }

            _context.Users.Remove(userDetailsModel);
            await _context.SaveChangesAsync();

            return Ok(userDetailsModel);
        }

        private bool UserDetailsModelExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}