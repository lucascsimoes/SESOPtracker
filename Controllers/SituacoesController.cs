using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SESOPtracker.Data;
using SESOPtracker.Models.Entities;

namespace SESOPtracker.Controllers
{
    public class SituacoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SituacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Situacoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Situacoes.ToListAsync());
        }

        // GET: Situacoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var situacao = await _context.Situacoes
               .Include(s => s.Equipamentos)!
               .ThenInclude(e => e.Sala)
               .FirstOrDefaultAsync(m => m.situacaoId == id);

            if (situacao == null)
            {
                return NotFound();
            }

            return View(situacao);
        }

        // GET: Situacoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Situacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("situacaoId,descricao,cor")] Situacao situacao)
        {
            if (_context.Situacoes.Any(s => s.descricao == situacao.descricao)) {
                ModelState.AddModelError("descricao", "Essa situação já foi cadastrada");
            }

            if (ModelState.IsValid)
            {
                _context.Add(situacao);
                await _context.SaveChangesAsync();
                TempData["AddStatus"] = true;
                return RedirectToAction(nameof(Index));
            }
            return View(situacao);
        }

        // GET: Situacoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var situacao = await _context.Situacoes.FindAsync(id);
            if (situacao == null)
            {
                return NotFound();
            }
            return View(situacao);
        }

        // POST: Situacoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("situacaoId,descricao,cor")] Situacao situacao)
        {
            if (id != situacao.situacaoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(situacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SituacaoExists(situacao.situacaoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["EditStatus"] = true;
                return RedirectToAction(nameof(Index));
            }
            return View(situacao);
        }

        // GET: Situacoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var situacao = await _context.Situacoes
                .FirstOrDefaultAsync(m => m.situacaoId == id);
            if (situacao == null)
            {
                return NotFound();
            }

            return View(situacao);
        }

        // POST: Situacoes/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var situacao = await _context.Situacoes.FindAsync(id);
            if (situacao != null)
            {
                var equipamentos = _context.Equipamentos.Where(h => h.situacao == id);
                _context.Equipamentos.RemoveRange(equipamentos);

                _context.Situacoes.Remove(situacao);
            }

            TempData["DeleteStatus"] = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SituacaoExists(int id)
        {
            return _context.Situacoes.Any(e => e.situacaoId == id);
        }
    }
}
