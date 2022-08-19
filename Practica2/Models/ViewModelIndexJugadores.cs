using Practica2.Mapper.DTOs;

namespace Practica2.Models
{
    public class ViewModelIndexJugadores
    {
        public IEnumerable<JugadorMostrarDto> Jugadores { get; set; }
        public int LigaId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
