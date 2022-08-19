using Practica2.Models;
using System.ComponentModel.DataAnnotations;

namespace Practica2.Mapper.DTOs
{
    public class JugadorMostrarDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
     
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }

        [Required]
        public int EquipoId { get; set; }
  
        public Equipo  Equipo { get; set; }
    }
}
