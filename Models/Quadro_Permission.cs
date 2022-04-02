using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Areas.Identity.Data;

namespace ToDo.Models
{
    public class Quadro_Permission
    {
        
        public int Id { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
        public int QuadroId { get; set; }
        public Quadro? Quadro { get; set; }
        public int PermissionTypeId { get; set; }
        public Permission_Type? PermissionType { get; set; }
    }
}