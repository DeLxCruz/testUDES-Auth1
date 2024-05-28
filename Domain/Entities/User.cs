using System.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string TypeIdentification { get; set; }
        public string DocumentNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ICollection<Roles> Roles { get; set; } = new HashSet<Roles>();
        public ICollection<UserRoles> UserRoles { get; set; }

    }
}