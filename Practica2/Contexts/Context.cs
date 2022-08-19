using Microsoft.EntityFrameworkCore;
using Practica2.Models;
using Practica2.Mapper.DTOs;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Practica2.Contexts
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> dbContextOptions) : base(dbContextOptions)
        { }


        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Jugador> Jugadores { get; set; }
        public DbSet<Liga> Ligas { get; set; }
        public DbSet<User> Users { get; set; }
    }

}
