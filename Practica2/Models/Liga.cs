using System.ComponentModel.DataAnnotations;

namespace Practica2.Models
{
    public class Liga : GenericModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
         public ICollection<Equipo> Equipos { get; set; }
    }
}
