using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace TestApplication.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Permission> Permissions { get; set; } = new List<Permission>();
        [JsonIgnore]
        public List<User> Users { get; set; } = new List<User>();
    }
}
