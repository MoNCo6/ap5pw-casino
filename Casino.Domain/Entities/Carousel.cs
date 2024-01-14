using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Domain.Entities
{
    public class Carousel : Entity
    {
        [Required] public string? ImageSrc { get; set; }
        public string? ImageAlt { get; set; }
    }
}