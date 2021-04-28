using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApplication.Models;

namespace TestApplication
{
    public static class SampleData
    {
        public static void Initialize(ApplicationContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Add data
            Permission p1 = new Permission { Name = "View Dashboards" };
            Permission p2 = new Permission { Name = "Edit Dashboards" };
            Permission p3 = new Permission { Name = "Manage Datasources" };
            Permission p4 = new Permission { Name = "Manage Users" };
            context.Permissions.AddRange(p1, p2, p3, p4);

            Group g1 = new Group { Name = "Administrator" };
            Group g2 = new Group { Name = "User" };
            context.Groups.AddRange(g1, g2);

            User u1 = new User { Name = "admin", Email = "admin@xx.com", Password = "Qwerty1" };
            User u2 = new User { Name = "user", Email = "user@xx.com", Password = "Asdfgh2" };
            context.Users.AddRange(u1, u2);

            g2.Permissions.Add(p2);
            g1.Permissions.Add(p3);
            g1.Permissions.Add(p4);

            u1.Permissions.Add(p1);
            u2.Permissions.Add(p1);

            u1.Groups.Add(g1);
            u1.Groups.Add(g2);
            u2.Groups.Add(g2);

            context.SaveChanges();
        }
    }
}
