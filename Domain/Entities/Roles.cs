using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Roles : BaseEntity
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<UserRoles> UserRoles { get; set; }
    }
}