using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestApplication.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<Group> Groups { get; set; } = new List<Group>();
        [JsonIgnore]
        public List<User> Users { get; set; } = new List<User>();
    }
}
