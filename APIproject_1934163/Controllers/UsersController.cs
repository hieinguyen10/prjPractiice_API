using APIproject_1934163.Data;
using APIproject_1934163.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIproject_1934163.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            var users = _context.Users.ToList();

            if (users is null)
            {
                return NotFound();
            }

            return users;
        }

        [HttpPost]
        public ActionResult Post(User user)
        {
            _context.Add(user);

            var findUser = _context.Users.FirstOrDefault(x => x.Uid == user.Uid);
            var findUserByEmail = _context.Users.Where(x => x.Email == user.Email);

            if (findUser != null || findUserByEmail != null)
            {
                return BadRequest("This User already exists!");
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(user);
        }

        [HttpDelete("{uid}")]
        public ActionResult Delete(string uid)
        {
            var user= _context.Users.FirstOrDefault(x => x.Uid == uid);

            if (user == null)
            {
                return NotFound("Product not found!");
            }

            _context.Remove(user);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(string email, string pword)
        {
            var checkLogin = _context.Users.FirstOrDefault(x => x.Email == email && x.Password == pword);

            if (checkLogin == null)
            {
                return BadRequest("Wrong Email or Password");
            }
            else
            {
                Session session = new Session();
                session.Token = Guid.NewGuid().ToString();
                session.Email = email;

                _context.Add(session);
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                return Ok("Login successfully");
            }
        }
    }
}
