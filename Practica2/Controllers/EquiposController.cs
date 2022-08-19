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
    public class EquiposController : Controller
    {
        private readonly Context _context;
        private EntityMapper _entityMapper;
        public EquiposController(Context context, EntityMapper entityMapper)
        {
            _context = context;
            _entityMapper = entityMapper;
        }
      
        // GET: Equipoes
        public async Task<IActionResult> Index(string searchInput, int searchLiga)
        {
            var ligas = await _context.Ligas.Where(x=>x.IsDeleted==false).ToListAsync();
            Liga liga = new Liga { Id = 0, Name = "" };
            ligas.Insert(0, liga);
          

            ViewData["LigaId"] = new SelectList(ligas, "Id", "Name");

         
            List<EquipoMostrarDto> list;
            if (searchInput != null)
            {
               
              

                    list = await _context.Equipos.Where(x => x.IsDeleted == false && x.Name.Contains(searchInput)).Select(x => new EquipoMostrarDto { Id = x.Id, Name = x.Name, LigaId = x.LigaId }).ToListAsync();
               
             
            }
            else
            {
                list = await _context.Equipos.Where(x => x.IsDeleted == false).Select(x=>new EquipoMostrarDto { Id=x.Id,Name = x.Name, LigaId = x.LigaId }).ToListAsync();
            }
            if (searchLiga != 0)
            {
                list = list.Where(x => x.LigaId == searchLiga).Select(x => new EquipoMostrarDto { Id = x.Id, Name = x.Name, LigaId = x.LigaId }).ToList();
            }
           


           foreach(var i in list)
            {
                i.NameLiga =await _context.Ligas.Where(x=>x.Id==i.LigaId).Select(x=>x.Name).FirstOrDefaultAsync();
            }

          
     

          
            return View(new ViewModelIndex { equipos = list });
        }
     
       
    

        // GET: Equipoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Equipos == null)
            {
                return NotFound();
            }

            var equipo = await _context.Equipos.Where(x => x.IsDeleted == false)
                .Include(e => e.Liga)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipo == null)
            {
                return NotFound();
            }
      
                string nombre= await _context.Ligas.Where(x => x.Id == equipo.LigaId && x.IsDeleted==false).Select(x => x.Name).FirstOrDefaultAsync();
            
            return View(new EquipoMostrarDto { Id = equipo.Id, LigaId = equipo.LigaId,NameLiga=nombre ,Name = equipo.Name });
        }

        // GET: Equipoes/Create
        public IActionResult Create()
        {
            ViewData["LigaId"] = new SelectList(_context.Ligas.Where(x => x.IsDeleted == false), "Id", "Name");
            return View();
        }

        // POST: Equipoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EquipoDto equipoDto)
        {
            if (ModelState.IsValid)
            {
               var equipo= _entityMapper.EquipoDtoToEquipo(equipoDto);
                _context.Add(equipo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LigaId"] = new SelectList(_context.Ligas.Where(x => x.IsDeleted == false), "Id", "Name", equipoDto.LigaId);
            return View(equipoDto);
        }

        // GET: Equipoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Equipos == null)
            {
                return NotFound();
            }

            var equipo = await _context.Equipos.Where(x => x.IsDeleted == false).FirstOrDefaultAsync();
            if (equipo == null)
            {
                return NotFound();
            }
            ViewData["LigaId"] = new SelectList(_context.Ligas.Where(x => x.IsDeleted == false), "Id", "Name", equipo.LigaId);
            return View(new EquipoEditDto { Id = equipo.Id, LigaId = equipo.LigaId, Name = equipo.Name });
        }

        // POST: Equipoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EquipoEditDto equipoEditDto)
        {
           
            if (id != equipoEditDto.Id)
            {
                return NotFound();
            }
            var equipo = await _context.Equipos.Where(x => x.IsDeleted == false).FirstOrDefaultAsync();
            if (ModelState.IsValid)
            {
                try
                {
                    equipo.LigaId = equipoEditDto.LigaId;
                    equipo.Name = equipoEditDto.Name;
                    equipo.LastUpdated = DateTime.Now;
                    _context.Update(equipo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipoExists(equipoEditDto.Id))
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
            ViewData["LigaId"] = new SelectList(_context.Ligas, "Id", "Name", equipoEditDto.LigaId);
            return View(new EquipoEditDto { Id = equipo.Id, LigaId = equipo.LigaId, Name = equipo.Name });
        }

        // GET: Equipoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Equipos == null)
            {
                return NotFound();
            }

            var equipo = await _context.Equipos.Where(x => x.IsDeleted == false)
                .Include(e => e.Liga)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipo == null)
            {
                return NotFound();
            }

            string nombre = await _context.Ligas.Where(x => x.Id == equipo.LigaId && x.IsDeleted==false).Select(x => x.Name).FirstOrDefaultAsync();

            return View(new EquipoMostrarDto { Id = equipo.Id, LigaId = equipo.LigaId, NameLiga = nombre, Name = equipo.Name });
        }

        // POST: Equipoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Equipos == null)
            {
                return Problem("Entity set 'Context.Equipos'  is null.");
            }
            var equipo = await _context.Equipos.Where(x=>x.IsDeleted==false).FirstOrDefaultAsync();
            if (equipo != null)
            {
                equipo.IsDeleted = true;
                equipo.LastUpdated = DateTime.Now;

            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipoExists(int id)
        {
          return (_context.Equipos?.Any(e => e.Id == id && e.IsDeleted==false)).GetValueOrDefault();
        }
    }
}
