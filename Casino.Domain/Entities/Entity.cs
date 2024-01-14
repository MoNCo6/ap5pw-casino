using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Domain.Entities
{
    public abstract class Entity
    {
        [Key] [Required] public int Id { get; set; }
    }
}