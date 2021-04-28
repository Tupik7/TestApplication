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
    public class PermissionsController : ControllerBase
    {
        ApplicationContext db;
        public PermissionsController(ApplicationContext context)
        {
            db = context;
            /*if (!db.Permissions.Any())
            {
                // Add data
                Permission p1 = new Permission { Name = "View Dashboards" };
                Permission p2 = new Permission { Name = "Edit Dashboards" };
                Permission p3 = new Permission { Name = "Manage Datasources" };
                Permission p4 = new Permission { Name = "Manage Users" };
                db.Permissions.AddRange(p1, p2, p3, p4);
                db.SaveChanges();
            }*/
        }
        
        // GET: api/<PermissionsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permission>>> Get()
        {
            return await db.Permissions.ToListAsync();
        }

        // GET api/<PermissionsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Permission>> Get(int id)
        {
            Permission permission = await db.Permissions.FirstOrDefaultAsync(x => x.Id == id);
            if (permission == null)
                return NotFound();
            return new ObjectResult(permission);
        }

        // POST api/<PermissionsController>
        [HttpPost]
        public async Task<ActionResult<Permission>> Post(Permission permission)
        {
            if (permission == null)
            {
                return BadRequest();
            }

            db.Permissions.Add(permission);
            await db.SaveChangesAsync();
            return Ok(permission);
        }

        // PUT api/<PermissionsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Permission>> Put(Permission permission)
        {
            if (permission == null)
            {
                return BadRequest();
            }
            if (!db.Permissions.Any(x => x.Id == permission.Id))
            {
                return NotFound();
            }

            db.Update(permission);
            await db.SaveChangesAsync();
            return Ok(permission);
        }

        // DELETE api/<PermissionsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Permission>> Delete(int id)
        {
            Permission permission = db.Permissions.FirstOrDefault(x => x.Id == id);
            if (permission == null)
            {
                return NotFound();
            }
            db.Permissions.Remove(permission);
            await db.SaveChangesAsync();
            return Ok(permission);
        }
    }
}
