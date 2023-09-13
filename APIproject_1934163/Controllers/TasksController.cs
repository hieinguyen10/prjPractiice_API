using APIproject_1934163.Data;
using APIproject_1934163.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIproject_1934163.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<DoingTask>> Get()
        {
            var tasks = _context.DoingTasks.ToList();

            if (tasks is null)
            {
                return NotFound();
            }

            return tasks;
        }

        // List Tasks by User UID created
        [HttpGet("createdby/{createdByUid}")]
        public ActionResult<IEnumerable<DoingTask>> GetTaskByCreatedUser(string createdUserUid) {
            var tasks = _context.DoingTasks.Where(x => x.CreatedByUid == createdUserUid).ToList();

            if (tasks is null)
            {
                return NotFound();
            }

            return tasks;
        }
        
        // List Tasks by User UID assigned
        [HttpGet("assignedto/{assignedToUid}")]
        public ActionResult<IEnumerable<DoingTask>> GetTaskByAssignedUer(string assignedToUserUid)
        {
            var tasks = _context.DoingTasks.Where(x => x.AssignedToUid == assignedToUserUid).ToList();

            if (tasks is null)
            {
                return NotFound();
            }

            return tasks;
        }

        // Delete Task
        [HttpDelete("{tid}")]
        public ActionResult Delete(string tid, string token)
        {
            // Find current User login
            var session = _context.Sessions.FirstOrDefault(x => x.Token == token);

            if (session == null)
            {
                return NotFound("Session not found!");
            }

            var currentUser = _context.Users.FirstOrDefault(x => x.Email == session.Email);

            if (currentUser == null)
            {
                return NotFound("User not found!");
            }
            
            // Find Task
            var task= _context.DoingTasks.FirstOrDefault(x => x.TaskUid == tid);

            if (task == null)
            {
                return NotFound("Task not found!");
            }

            if (currentUser.Uid == task.CreatedByUid)
            {
                _context.Remove(task);

                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                return Ok(task);
            }
            else
            {
                return BadRequest("This User cannot delete tasks");
            }
        }


        [HttpPut("{tid}")]
        public ActionResult Put(string tid, string token)
        {
            // Find current User login
            var session = _context.Sessions.FirstOrDefault(x => x.Token == token);

            if (session == null)
            {
                return NotFound("Session not found!");
            }

            var currentUser = _context.Users.FirstOrDefault(x => x.Email == session.Email);

            if (currentUser == null)
            {
                return NotFound("User not found!");
            }

            // Find Task
            var task = _context.DoingTasks.FirstOrDefault(x => x.TaskUid == tid);

            if (task == null)
            {
                return NotFound("Task not found!");
            }

            if (currentUser.Uid == task.AssignedToUid)
            {
                _context.Entry(task).State = EntityState.Modified;

                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                return Ok(task);
            }
            else
            {
                return BadRequest("This User cannot modify tasks");
            }
        }

        // Add Task
        [HttpPost]
        public ActionResult Post(string token, string description, string assignedToId)
        {
            DoingTask task = new DoingTask();
            task.Description = description;
            task.AssignedToUid= assignedToId;

            // Find created by User
            var session = _context.Sessions.FirstOrDefault(x => x.Token == token);

            if (session == null)
            {
                return NotFound("Session not found!");
            }
            var createdUser = _context.Users.FirstOrDefault(x => x.Email == session.Email);

            if (createdUser == null)
            {
                return NotFound("User created not found!");
            }
            task.CreatedByUid = createdUser.Uid;
            task.CreatedByName = createdUser.Name;

            // Find assigned to User
            var assignedUser = _context.Users.FirstOrDefault(x => x.Uid == assignedToId);
            if (assignedUser == null)
            {
                return NotFound("User assigned not found!");
            }
           
            task.AssignedToUid = assignedUser.Uid;
            task.AssignedToName = assignedUser.Name;
            task.Done = false;

            _context.Add(task);

            var findTask = _context.DoingTasks.FirstOrDefault(x => x.TaskUid == task.TaskUid);

            if (findTask != null)
            {
                return BadRequest("This Task already exists!");
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(task);
        }
    }
}
