using Casino.Domain.Implementation.Validations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Domain.Entities
{
    public class BaseGame : Entity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Rules { get; set; }
    }

    public class GameCreate : BaseGame
    {
        [Required]
        [FileContent("image")]
        public IFormFile Image { get; set; }
    }

    public class GameEdit : BaseGame
    {
        [FileContent("image")]
        public IFormFile? Image { get; set; }
    }

    public class Game : BaseGame
    {
        [Required]
        public string ImageSrc { get; set; }
    }
}
