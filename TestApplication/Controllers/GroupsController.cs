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
    public class GroupsController : ControllerBase
    {
        ApplicationContext db;
        public GroupsController(ApplicationContext context)
        {
            db = context;
            /*if (!db.Groups.Any())
            {
                // Add data
                Group g1 = new Group { Name = "Administrator" };
                Group g2 = new Group { Name = "User" };
                db.Groups.AddRange(g1, g2);
                db.SaveChanges();
            }*/
        }

        // GET: api/<GroupsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> Get()
        {
            return await db.Groups
                .Include(p => p.Permissions)
                .ToListAsync();
        }

        // GET api/<GroupsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> Get(int id)
        {
            Group group = await db.Groups
                                .Include(p => p.Permissions)
                                .FirstOrDefaultAsync(x => x.Id == id);
            if (group == null){
                return NotFound();
            }
            return new ObjectResult(group);
        }

        // POST api/<GroupsController>
        [HttpPost]
        public async Task<ActionResult<Group>> Post(Group group)
        {
            var new_group = new Group();
            if (group == null){
                return BadRequest();
            }
            new_group.Name = group.Name;
            db.Groups.Add(new_group);
            foreach(var p in group.Permissions){
                var db_perm = await db.Permissions.FindAsync(p.Id);
                new_group.Permissions.Add(db_perm);
            }
            await db.SaveChangesAsync();
            return Ok(new_group);
        }

        // PUT api/<GroupsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Group>> Put(Group group)
        {
            if (group == null){
                return BadRequest();
            }
            if (!db.Groups.Any(x => x.Id == group.Id)){
                return NotFound();
            }
            var db_group = await db.Groups
                .Include(p => p.Permissions)
                .FirstOrDefaultAsync(x => x.Id == group.Id);
            var permissions = await db.Permissions.ToListAsync();

            db.Entry(db_group).CurrentValues.SetValues(group);

            foreach(var p in permissions){
                if (group.Permissions.Any(x => x.Id == p.Id) && !db_group.Permissions.Any(x => x.Id == p.Id)){
                    db_group.Permissions.Add(p);
                }
                if (!group.Permissions.Any(x => x.Id == p.Id)){
                    db_group.Permissions.Remove(p);
                }
            }

            db.Update(db_group);
            await db.SaveChangesAsync();
            return Ok(group);
        }

        // DELETE api/<GroupsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Group>> Delete(int id)
        {
            Group group = db.Groups.FirstOrDefault(x => x.Id == id);
            if (group == null){
                return NotFound();
            }
            db.Entry(group)
                .Collection(c => c.Permissions)
                .Load();
            db.Groups.Remove(group);
            await db.SaveChangesAsync();
            return Ok(group);
        }
    }
}
