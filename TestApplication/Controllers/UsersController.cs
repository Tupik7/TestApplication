using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApplication.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        ApplicationContext db;
        public UsersController(ApplicationContext context)
        {
            db = context;
            /*if (!db.Users.Any())
            {
                // Add data
                User u1 = new User { Name = "Administrator" };
                User u2 = new User { Name = "User" };
                db.Users.AddRange(u1, u2);
                db.SaveChanges();
            }*/
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await db.Users
                            .Include("Groups.Permissions")
                            .Include(p => p.Permissions)
                            .ToListAsync();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            User user = await db.Users
                                .Include("Groups.Permissions")
                                .Include(p => p.Permissions)
                                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null){
                return NotFound();
            }
            return new ObjectResult(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            if (user == null){
                return BadRequest();
            }
            var new_user = new User();
            new_user.Name = user.Name;
            new_user.Email = user.Email;
            new_user.Password = user.Password;
            db.Users.Add(new_user);
            foreach (var p in user.Permissions){
                var db_perm = await db.Permissions.FindAsync(p.Id);
                new_user.Permissions.Add(db_perm);
            }
            foreach (var g in user.Groups)
            {
                var db_gr = await db.Groups.FindAsync(g.Id);
                new_user.Groups.Add(db_gr);
            }
            await db.SaveChangesAsync();
            return Ok(new_user);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Put(User user)
        {
            if (user == null){
                return BadRequest();
            }
            if (!db.Users.Any(x => x.Id == user.Id)){
                return NotFound();
            }

            var db_user = await db.Users
                .Include(p => p.Permissions)
                .Include(g => g.Groups)
                .FirstOrDefaultAsync(x => x.Id == user.Id);
            var permissions = await db.Permissions.ToListAsync();
            var groups = await db.Groups.ToListAsync();

            db.Entry(db_user).CurrentValues.SetValues(user);

            foreach (var p in groups){
                if (user.Groups.Any(x => x.Id == p.Id) && !db_user.Groups.Any(x => x.Id == p.Id))
                {
                    db_user.Groups.Add(p);
                }
                if (!user.Groups.Any(x => x.Id == p.Id))
                {
                    db_user.Groups.Remove(p);
                }
            }

            foreach (var p in permissions){
                if (user.Permissions.Any(x => x.Id == p.Id) && !db_user.Permissions.Any(x => x.Id == p.Id))
                {
                    db_user.Permissions.Add(p);
                }
                if (!user.Permissions.Any(x => x.Id == p.Id))
                {
                    db_user.Permissions.Remove(p);
                }
            }

            db.Update(db_user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int id)
        {
            User user = db.Users.FirstOrDefault(x => x.Id == id);
            if (user == null){
                return NotFound();
            }
            db.Entry(user)
                .Collection(c => c.Permissions)
                .Load();
            db.Entry(user)
                .Collection(c => c.Groups)
                .Load();
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }
    }
}
