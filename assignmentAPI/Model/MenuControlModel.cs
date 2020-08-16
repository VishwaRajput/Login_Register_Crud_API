using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace assignmentAPI.Model
{
    public class MenuControlModel
    {
        [Key]
        public int Id { get; set; }

        public int ParentId { get; set; }
        public string Content { get; set; }
    }
}
