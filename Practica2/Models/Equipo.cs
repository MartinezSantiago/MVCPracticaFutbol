using System.ComponentModel.DataAnnotations;

namespace Practica2.Models
{
    public class Equipo:GenericModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public ICollection<Jugador> Jugadores { get; set; }
        [Required]
        public int LigaId { get; set; }
        public  virtual Liga Liga { get; set; }
  
    }
}
