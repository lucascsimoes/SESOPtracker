using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SESOPtracker.Data;
using SESOPtracker.Models.Entities;
using System.Text;

namespace SESOPtracker.Controllers
{
    public class HistoricosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HistoricosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Historicos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Equipamentos.Include(e => e.Sala).Include(e => e.Situacao);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Historicos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historico = await _context.Historicos.FindAsync(id);
            if (historico == null)
            {
                return NotFound();
            }
            return View(historico);
        }

        private bool HistoricoExists(int id)
        {
            return _context.Historicos.Any(e => e.historicoId == id);
        }

        // POST: Historicos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("historicoId,patrimonio,dataAlteracao,situacaoAtual,descricao,observacao,importante")] Historico historico)
        {

            if (id != historico.historicoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Historicos.Update(historico);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "Equipamentos", new { id = historico.patrimonio });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistoricoExists(historico.historicoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToAction("Details", "Equipamentos", new { id = historico.patrimonio });
        }

    }
}
