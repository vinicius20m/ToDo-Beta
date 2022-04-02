using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Areas.Identity.Data;

namespace ToDo.Models
{
    public class Item
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool Done { get; set; }
        public int QuadroId { get; set; }
        public Quadro? Quadro { get; set; }
        public string AuthorId { get; set; }
    }
}