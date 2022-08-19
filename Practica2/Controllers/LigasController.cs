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
    public class LigasController : Controller
    {
        private readonly Context _context;
        private EntityMapper entityMapper;
        public LigasController(Context context, EntityMapper entityMapper)
        {
            _context = context;
            this.entityMapper = entityMapper;
        }

        // GET: Ligas
        public async Task<IActionResult> Index(string searchInput,ViewModelIndexLiga viewModelIndexLiga)
        {
            var ligas = await _context.Ligas.Where(e => e.IsDeleted == false).Select(x => new LigaEditDto{Id=x.Id,Country=x.Country,Name=x.Name} ).ToListAsync();
            List<LigaEditDto> list;
            if (searchInput != null)
            {



                list =  ligas.Where(x => x.Name.Contains(searchInput)).ToList();


            }
            else
            {
                list = ligas;
            }
            if(viewModelIndexLiga.Country!=null)
            {
                list=list.Where(x => x.Country.Equals(viewModelIndexLiga.Country)).ToList();
            }
         

            
            return View(new ViewModelIndexLiga { Ligas=list,Country=viewModelIndexLiga.Country});
                          
        }

        // GET: Ligas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ligas == null)
            {
                return NotFound();
            }

            var liga = await _context.Ligas.Where(x=>x.IsDeleted==false)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (liga == null)
            {
                return NotFound();
            }

            return View(new LigaEditDto {Id=liga.Id, Country=liga.Country,Name=liga.Name});
        }

        // GET: Ligas/Create
        public IActionResult Create()
        {
            return View(new LigaDto { }) ;
        }

        // POST: Ligas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LigaDto liga)
        {

            if (ModelState.IsValid)
            {

              
                _context.Add(entityMapper.LigaDtoToLiga(liga));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(liga);
        }

        // GET: Ligas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ligas == null)
            {
                return NotFound();
            }

            var liga = await _context.Ligas.Where(x=>x.IsDeleted==false).FirstOrDefaultAsync();
            if (liga == null)
            {
                return NotFound();
            }
            return View(new LigaEditDto { Id=(int)id,Country=liga.Country,Name=liga.Name});
        }

        // POST: Ligas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,LigaEditDto ligaEditDto)
        {
            if (id != ligaEditDto.Id)
            {
                return NotFound();
            }
            var liga = await _context.Ligas.Where(x=>x.IsDeleted==false).FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {
                try
                {
                    if (liga != null)
                    {
                        liga.LastUpdated = DateTime.Now;
                        liga.Name=ligaEditDto.Name;
                        liga.Country = ligaEditDto.Country;


                        _context.Update(liga);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound();
                    }
                 
                }
                catch (DbUpdateConcurrencyException)
                {
                   
                    
                        throw;
                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ligaEditDto);
        }

        // GET: Ligas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ligas == null)
            {
                return NotFound();
            }

            var liga = await _context.Ligas.Where(x=>x.IsDeleted==false)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (liga == null)
            {
                return NotFound();
            }

            return View(new LigaEditDto {Country=liga.Country,Name=liga.Name,Id=liga.Id});
        }

        // POST: Ligas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ligas == null)
            {
                return Problem("Entity set 'Context.Ligas'  is null.");
            }
            var liga = await _context.Ligas.Where(x=>x.IsDeleted==false).FirstOrDefaultAsync();
            if (liga != null)
            {
                liga.LastUpdated = DateTime.Now;
                liga.IsDeleted = true;
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LigaExists(int id)
        {
          return (_context.Ligas?.Any(e => e.Id == id && e.IsDeleted==false)).GetValueOrDefault();
        }
    }
}
