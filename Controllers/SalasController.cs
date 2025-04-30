using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using SCCWEB.Data;
using SESOPtracker.Data;
using SESOPtracker.Models.Entities;

namespace SESOPtracker.Controllers
{
    public class SalasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalasController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool SalaExists(string id)
        {
            bool rec = false;

            try
            {
                OracleConnection conn = Conexao.GetConexao();
                string sql = $"select * from LUCAS_TRACKER_SALAS where lugar like '{id}' ";
                OracleCommand cmd = new OracleCommand(sql, conn);
                conn.Open();
                OracleDataReader rd = cmd.ExecuteReader();

                rd.Read();

                if (rd.HasRows)
                {
                    rec = true;
                }
                rd.Close();

                if (conn != null)
                    conn.Close();
            }
            catch (Exception ex)
            {
                TempData["Erro"] = "Erro de SQL: " + ex.Message;
                throw new Exception("Erro de SQL: " + ex.Message);
            }

            return rec;
        }

        // GET: Salas
        public IActionResult Index()
        {
            List<Sala> salas = new List<Sala>();

            try
            {
                OracleConnection conn = Conexao.GetConexao();
                string sql = "SELECT * FROM LUCAS_TRACKER_SALAS";
                OracleCommand cmd = new OracleCommand(sql, conn);
                conn.Open();
                OracleDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    Sala sala = new Sala()
                    {
                        salaId = Convert.ToInt32(rd["salaId"]),
                        local = rd["lugar"].ToString(),
                        descricao = rd["descricao"].ToString()
                    };

                    salas.Add(sala);
                }

                rd.Close();

                if (conn != null)
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro de SQL: " + ex.Message);
            }

            return View(salas);
        }

        // GET: Salas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sala = await _context.Salas
               .Include(s => s.Equipamentos)!
               .ThenInclude(e => e.Situacao)
               .FirstOrDefaultAsync(m => m.salaId == id);

            if (sala == null)
            {
                return NotFound();
            }

            var equipamentos = _context.Equipamentos.Where(h => h.sala == id).ToList();

            if (equipamentos.Count() > 0)
            {
                TempData["DeleteSalaError"] = true;
            }

            return View(sala);
        }

        // GET: Salas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Salas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("salaId,local,descricao")] Sala sala)
        {
            if (SalaExists(sala.local)) {
                ModelState.AddModelError("local", "Essa sala já foi cadastrada");
            }

            if (ModelState.IsValid)
            {
                sala.local = sala.local.ToUpper();

                _context.Add(sala);
                await _context.SaveChangesAsync();

                TempData["AddSala"] = true;

                return RedirectToAction(nameof(Index));
            }
            return View(sala);
        }

        // GET: Salas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sala = await _context.Salas.FindAsync(id);
            if (sala == null)
            {
                return NotFound();
            }
            return View(sala);
        }

        // POST: Salas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("salaId,local,descricao")] Sala sala)
        {
            if (id != sala.salaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sala);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (SalaExists(sala.local))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["EditSala"] = true;
                return RedirectToAction(nameof(Index));
            }
            return View(sala);
        }

        // GET: Salas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sala = await _context.Salas
                .FirstOrDefaultAsync(m => m.salaId == id);
            if (sala == null)
            {
                return NotFound();
            }

            return View(sala);
        }

        // POST: Salas/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sala = await _context.Salas.FindAsync(id);
            if (sala != null)
            {
                _context.Salas.Remove(sala);
                TempData["DeleteSala"] = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
