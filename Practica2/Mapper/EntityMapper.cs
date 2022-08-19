using Practica2.Mapper.DTOs;
using Practica2.Models;

namespace Practica2.Mapper
{
    public class EntityMapper
    {
        public Liga LigaDtoToLiga(LigaDto ligadto)
        {
            return new Liga { Country = ligadto.Country,
                Name = ligadto.Name,
                
                LastUpdated = DateTime.Now,
                IsDeleted = false
            };
        }
        public Equipo EquipoDtoToEquipo(EquipoDto equipoDto)
        {
            return new Equipo { IsDeleted = false, LastUpdated = DateTime.Now, LigaId = equipoDto.LigaId,Name=equipoDto.Name};
        }
        public Jugador JugadorCreateDtoJugador(JugadorCreateDto jugadorCreateDto)
        {
            return new Jugador { IsDeleted = false,LastUpdated=DateTime.Now, EquipoId=jugadorCreateDto.EquipoId,BirthDate=jugadorCreateDto.BirthDate,LastName=jugadorCreateDto.LastName,Name=jugadorCreateDto.Name};
           
        }
    }
}
