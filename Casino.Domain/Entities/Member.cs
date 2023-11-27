using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Domain.Entities
{
    public class Member : Entity
    {
        [Required]
        [StringLength(70)]
        public string Forename { get; set; }
        [Required]
        [StringLength(70)]
        public string Surname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [StringLength(30)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
