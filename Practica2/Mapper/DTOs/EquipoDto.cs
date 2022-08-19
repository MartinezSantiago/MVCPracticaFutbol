using Practica2.Models;
using System.ComponentModel.DataAnnotations;

namespace Practica2.Mapper.DTOs
{
    public class EquipoDto
    {

        [Required]
        public string Name { get; set; }
        
        public int LigaId { get; set; }
      
    }
}
