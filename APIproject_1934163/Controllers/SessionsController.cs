using APIproject_1934163.Data;
using APIproject_1934163.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIproject_1934163.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Session>> Get()
        {
            var sessions = _context.Sessions.ToList();

            if (sessions is null)
            {
                return NotFound();
            }

            return sessions;
        }

        [HttpPost(Name = "PostSession")]
        public ActionResult Post(Session session)
        {
            _context.Add(session);

            var findSession = _context.Sessions.FirstOrDefault(x => x.Token == session.Token);

            if (findSession != null)
            {
                return BadRequest("This Session already exists!");
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(session);
        }
    }
}
