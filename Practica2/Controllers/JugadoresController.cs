using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practica2.Contexts;
using Practica2.Mapper;
using Practica2.Mapper.DTOs;
using Practica2.Models;

namespace Practica2.Controllers
{
    [Authorize]
    public class JugadoresController : Controller
    {
        private readonly Context _context;
        private EntityMapper _entityMapper;
        public JugadoresController(Context context, EntityMapper entityMapper)
        {
            _context = context;
            _entityMapper = entityMapper;
        }

        // GET: Jugadors
        public async Task<IActionResult> Index(ViewModelIndexJugadores viewModelIndexJugadores)
        {

            var ligas = await _context.Ligas.Where(x => x.IsDeleted == false).ToListAsync();
            var equipos = await _context.Equipos.Where(x => x.IsDeleted == false).ToListAsync();
            Liga liga1 = new Liga { Id = 0, Name = "" };
            ligas.Insert(0, liga1);


            ViewData["LigaId"] = new SelectList(ligas, "Id", "Name");

            var jugadores = await _context.Jugadores.Where(x => x.IsDeleted == false && (x.LastName.Contains(viewModelIndexJugadores.LastName) || viewModelIndexJugadores.LastName == null) && (x.Name.Contains(viewModelIndexJugadores.Name) || viewModelIndexJugadores.Name == null)).ToListAsync();


          

            foreach (var item in jugadores)
            {
                var equipo = equipos.FirstOrDefault(x => x.Id == item.EquipoId);
                var liga = ligas.FirstOrDefault(x => x.Id == equipo.LigaId);
                equipo.Liga = liga;
                item.Equipo = equipo;
                

            }
           var jugadoresDto=jugadores.Select(X => new JugadorMostrarDto { Equipo = X.Equipo, BirthDate = X.BirthDate, Id = X.Id, EquipoId = X.EquipoId, LastName = X.LastName, Name = X.Name }).ToList();
         if (viewModelIndexJugadores.LigaId != 0)
            {
                jugadoresDto = jugadoresDto.Where(x => x.Equipo.Liga.Id == viewModelIndexJugadores.LigaId).ToList();
            }
          
              
          
            
            


            return View(new ViewModelIndexJugadores { Jugadores = jugadoresDto, LastName= viewModelIndexJugadores .LastName,Name= viewModelIndexJugadores.Name, LigaId = viewModelIndexJugadores.LigaId });
        }

        // GET: Jugadors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Jugadores == null)
            {
                return NotFound();
            }

            var jugador = await _context.Jugadores.Where(x => x.IsDeleted == false)
                .Include(j => j.Equipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jugador == null)
            {
                return NotFound();
            }

            return View(jugador);
        }

        // GET: Jugadors/Create
        public IActionResult Create()
        {
            ViewData["EquipoId"] = new SelectList(_context.Equipos.Where(x => x.IsDeleted == false), "Id", "Name");
            return View();
        }

        // POST: Jugadors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JugadorCreateDto jugadorCreateDto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(_entityMapper.JugadorCreateDtoJugador(jugadorCreateDto));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["EquipoId"] = new SelectList(_context.Equipos.Where(x => x.IsDeleted == false), "Id", "Name", jugadorCreateDto.EquipoId);

            return View(jugadorCreateDto);
        }

        // GET: Jugadors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Jugadores == null)
            {
                return NotFound();
            }

            var jugador = await _context.Jugadores.Where(x => x.IsDeleted == false && x.Id == id).Select(x=>new JugadorDto { BirthDate = x.BirthDate, EquipoId = x.EquipoId, Name = x.Name, LastName = x.LastName, Id = x.Id, Equipo = x.Equipo }).FirstOrDefaultAsync();

            if (jugador == null)
            {
                return NotFound();
            }
            ViewData["EquipoId"] = new SelectList(_context.Equipos.Where(x => x.IsDeleted == false), "Id", "Name", jugador.EquipoId);
            return View(jugador);
        }

        // POST: Jugadors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JugadorDto jugadorDto)
        {
            if (id != jugadorDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Jugador jugador = await _context.Jugadores.Where(x => x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
                    jugador.LastUpdated = DateTime.Now;
                    jugador.EquipoId = jugadorDto.EquipoId;
                    jugador.BirthDate = jugadorDto.BirthDate;
                    jugador.LastName = jugadorDto.LastName;
                    jugador.Name = jugadorDto.Name;

                    _context.Update(jugador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JugadorExists(jugadorDto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EquipoId"] = new SelectList(_context.Equipos.Where(x => x.IsDeleted == false), "Id", "Name", jugadorDto.EquipoId);
            return View(jugadorDto);
        }

        // GET: Jugadors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Jugadores == null)
            {
                return NotFound();
            }

            var jugador = await _context.Jugadores.Where(x => x.IsDeleted == false)
                .Include(j => j.Equipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            jugador.Equipo =await _context.Equipos.Where(x => x.IsDeleted == false && x.Id == jugador.EquipoId).FirstOrDefaultAsync();
            if (jugador == null)
            {
                return NotFound();
            }

            return View(new JugadorDto { BirthDate = jugador.BirthDate, EquipoId = jugador.EquipoId, Name = jugador.Name, LastName = jugador.LastName, Id = jugador.Id,Equipo=jugador.Equipo });
        }

        // POST: Jugadors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Jugadores == null)
            {
                return Problem("Entity set 'Context.Jugadores'  is null.");
            }
            var jugador = await _context.Jugadores.Where(x => x.IsDeleted == false).FirstOrDefaultAsync();
            if (jugador != null)
            {
                jugador.IsDeleted = true;
                jugador.LastUpdated = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JugadorExists(int id)
        {
            return (_context.Jugadores?.Any(e => e.Id == id && e.IsDeleted == false)).GetValueOrDefault();
        }
    }
}
