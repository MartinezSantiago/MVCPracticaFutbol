using Practica2.Models;
using System.ComponentModel.DataAnnotations;

namespace Practica2.Mapper.DTOs
{
    public class LigaDto
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public string Country { get; set; }
   
    }
}
