using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class AddRoleToUserDTO
    {
        public string Username { get; set; }
        public string RoleName { get; set; }
    }
}