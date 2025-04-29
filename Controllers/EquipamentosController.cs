using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using SESOPtracker.Data;
using SESOPtracker.Models.Entities;
using Microsoft.AspNetCore.Http;
using System.IO;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using Microsoft.SqlServer.Server;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SESOPtracker.Controllers
{
    public class EquipamentosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EquipamentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Equipamentos
        public async Task<IActionResult> Index(string viewBy) {
            if (viewBy != null) {
                viewBy = viewBy.Split("?")[0];
            } else {
                viewBy = "";
            }

            ViewData["StatusList"] = new SelectList(_context.Situacoes, "situacaoId", "descricao");
            ViewData["SalasList"] = new SelectList(_context.Salas, "salaId", "local");


            var equipamentos = from e in _context.Equipamentos.Include(e => e.Sala).Include(e => e.Situacao).Include(e => e.Historico)
                                   select e;

            var equipamentosList = await equipamentos.ToListAsync();

            switch (viewBy) {
                case "Categoria":
                    equipamentosList = equipamentosList.OrderBy(e => e.categoria).ToList();
                    break;
                case "Situacao":
                    equipamentosList = equipamentosList.OrderBy(e => e.Situacao.descricao).ToList();
                    break;
                case "Sala":
                    equipamentosList = equipamentosList.OrderBy(e => e.Sala.local).ToList();
                    break;
                case "Setor":
                    equipamentosList = equipamentosList.OrderBy(e => e.setor).ToList();
                    break;
                default:
                    equipamentosList = equipamentosList.OrderByDescending(e =>
                    {
                        if (long.TryParse(e.patrimonio, out long patrimonioNum))
                        {
                            return patrimonioNum;
                        }
                        return long.MinValue;
                    }).ToList();
                    break;
            }

            var equipamentosPorCategoria = equipamentosList.GroupBy(e => e.categoria).ToList();
            ViewData["EquipamentosPorCategoria"] = equipamentosPorCategoria;

            var equipamentosPorSituacao = equipamentosList.GroupBy(e => e.Situacao.descricao).ToList();
            ViewData["EquipamentosPorSituacao"] = equipamentosPorSituacao;

            var equipamentosPorSala = equipamentosList.GroupBy(e => e.Sala.local).ToList();
            ViewData["EquipamentosPorSala"] = equipamentosPorSala;

            var equipamentosPorSetor = equipamentosList.GroupBy(e => e.setor).ToList();
            ViewData["EquipamentosPorSetor"] = equipamentosPorSetor;

            ViewData["ViewBy"] = viewBy;
            return View(equipamentosList);
        }




        // GET: Equipamentos/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipamento = await _context.Equipamentos
                .Include(e => e.Sala)
                .Include(e => e.Situacao)
                .Include(e => e.Historico)
                    .ThenInclude(s => s.Situacao)
                .FirstOrDefaultAsync(m => m.patrimonio == id);

            if (equipamento == null)
            {
                return NotFound();
            }

            var todosPatrimonios = _context.Equipamentos
                .Select(e => e.patrimonio)
                .ToList();

            // Modifica as descrições do histórico para envolver patrimônios em <span>
            foreach (var historico in equipamento.Historico)
            {
                foreach (var patrimonio in todosPatrimonios)
                {
                    if (historico.observacao != null && historico.observacao.Contains(patrimonio))
                    {
                        var link = Url.Action("Details", "Equipamentos", new { id = patrimonio });
                        historico.observacao = historico.observacao.Replace(
                            patrimonio,
                            $"<a href='{link}' class='redirect-highlight'>{patrimonio}</a>"
                        );
                    }
                }
            }

            return View(equipamento);
        }

        // GET: Equipamentos/Create
        public IActionResult Create()
        {
            ViewData["sala"] = new SelectList(_context.Salas, "salaId", "local");
            ViewData["situacao"] = new SelectList(_context.Situacoes, "situacaoId", "descricao");
            return View(new Equipamento());
        }

        // POST: Equipamentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("patrimonio,item,nome,subCategoria,categoria,setor,dataCriacao,situacao,sala,tag")] Equipamento equipamento,
            [FromForm] string multiple, IFormFile file)
        {
            if (file != null && file.Length > 0) {
                return await Import(file);
            } else {
                ModelState.Remove("file");
            }

            if (_context.Equipamentos.Any(s => s.patrimonio == equipamento.patrimonio)) {
                ModelState.AddModelError("patrimonio", "Esse equipamento já foi cadastrado");
            } else {
                ModelState.Remove("patrimonio");
            }

            if (string.IsNullOrEmpty(equipamento.patrimonio)) {
                var lastEquipamento = await _context.Equipamentos
                    .Where(e => e.patrimonio.StartsWith("Não consta"))
                    .OrderByDescending(e => e.patrimonio)
                    .FirstOrDefaultAsync();

                int nextNumber = 1;
                if (lastEquipamento != null) {
                    var lastNumberStr = lastEquipamento.patrimonio.Replace("Não consta [", "").Replace("]", "");
                    if (int.TryParse(lastNumberStr, out int lastNumber)) {
                        nextNumber = lastNumber + 1;
                    }
                }

                equipamento.patrimonio = $"Não consta [{nextNumber:D3}]";
            }

            if (equipamento.setor != null) {
                equipamento.setor = equipamento.setor.ToUpper();
            }

            if (int.TryParse(equipamento.sala.ToString(), out int salaId)) {
                var salaFind = await _context.Salas.FirstOrDefaultAsync(s => s.salaId == salaId);
                if (salaFind == null) {
                    ModelState.AddModelError("sala", "A sala é obrigatória");
                } else {
                    equipamento.sala = salaId;
                    ModelState.Remove("sala");
                }
            } else {
                ModelState.AddModelError("sala", "Valor para sala inválido");
            }

            if (int.TryParse(equipamento.situacao.ToString(), out int situacaoId)) {
                equipamento.situacao = situacaoId;
                ModelState.Remove("situacao");
            } else {
                ModelState.AddModelError("situacao", "Situação não encontrada");
            }

            ModelState.Remove("multiple");
            ModelState.Remove("Historico");

            if (ModelState.IsValid)
            {
                _context.Add(equipamento);
                await _context.SaveChangesAsync();

                var historico = new Historico {
                    patrimonio = equipamento.patrimonio,
                    dataAlteracao = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                    situacaoAtual = equipamento.situacao,
                    descricao = "Equipamento adicionado",
                    observacao = null,
                    importante = false
                };

                _context.Historicos.Add(historico);
                await _context.SaveChangesAsync();

                TempData["AddEquipment"] = true;

                if (multiple == "on") {
                    TempData["Equipamento"] = new Dictionary<string, string>
                    {
                        { "subCategoria", equipamento.subCategoria },
                        { "categoria", equipamento.categoria },
                        { "setor", equipamento.setor ?? string.Empty },
                        { "situacao", equipamento.situacao.ToString() },
                        { "sala", equipamento.sala.ToString() },
                        { "dataCriacao", equipamento.dataCriacao }
                    };
                    return RedirectToAction("Create");
                } else {
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["sala"] = new SelectList(_context.Salas, "salaId", "local", equipamento.sala);
            ViewData["situacao"] = new SelectList(_context.Situacoes, "situacaoId", "descricao", equipamento.situacao);
            return View(equipamento);
        }

        // GET: Equipamentos/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipamento = await _context.Equipamentos.FindAsync(id);
            if (equipamento == null)
            {
                return NotFound();
            }

            ViewBag.Historico = new Historico();
            ViewBag.Situacao = new Situacao();

            ViewData["sala"] = new SelectList(_context.Salas, "salaId", "local", equipamento.sala);
            ViewData["situacao"] = new SelectList(_context.Situacoes, "situacaoId", "descricao", equipamento.situacao);
            return View(equipamento);
        }

        // POST: Equipamentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, 
            [Bind("patrimonio,item,nome,subCategoria,categoria,setor,dataCriacao,situacao,sala,tag")] Equipamento equipamento,
            [FromForm] string dataAlteracao,
            [FromForm] string descricao,
            [FromForm] string observacao,
            [FromForm] string importante) {

            if (equipamento.setor != null) {
                equipamento.setor = equipamento.setor.ToUpper();
            }

            if (id != equipamento.patrimonio)
            {
                return NotFound();
            }

            ModelState.Remove("sala");
            ModelState.Remove("situacao");
            ModelState.Remove("observacao");
            ModelState.Remove("importante");
            ModelState.Remove("Historico");

            if (ModelState.IsValid) {
                try {
                    _context.Update(equipamento);
                    await _context.SaveChangesAsync();

                    var historico = new Historico {
                        patrimonio = equipamento.patrimonio,
                        dataAlteracao = dataAlteracao,
                        situacaoAtual = equipamento.situacao,
                        descricao = descricao,
                        observacao = observacao,
                        importante = importante == "on" ? true : false
                    };

                    _context.Historicos.Add(historico);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "Equipamentos", new { id = equipamento.patrimonio });
                } catch (DbUpdateConcurrencyException) {
                    if (!EquipamentoExists(equipamento.patrimonio)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
            }

            if (descricao == null || descricao == "") {
                TempData["NoChanges"] = true;
                return RedirectToAction("Edit", new { id = equipamento.patrimonio });
            }

            return View(equipamento);
        }




        // GET: Equipamentos/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipamento = await _context.Equipamentos
                .Include(e => e.Sala)
                .Include(e => e.Situacao)
                .FirstOrDefaultAsync(m => m.patrimonio == id);
            if (equipamento == null)
            {
                return NotFound();
            }

            return View(equipamento);
        }

        // POST: Equipamentos/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var equipamento = await _context.Equipamentos.FindAsync(id);
            if (equipamento != null)
            {
                var historicos = _context.Historicos.Where(h => h.patrimonio == id);
                _context.Historicos.RemoveRange(historicos);
                _context.Equipamentos.Remove(equipamento);
            }

            await _context.SaveChangesAsync();
            TempData["DeletedEquipment"] = true;
            return RedirectToAction(nameof(Index));
        }

        private bool EquipamentoExists(string id)
        {
            return _context.Equipamentos.Any(e => e.patrimonio == id);
        }


        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file) {
            if (file == null || file.Length == 0) {
                ModelState.AddModelError("File", "Por favor, selecione um arquivo Excel.");
                return View("Create");
            }

            ExcelPackage.License.SetNonCommercialOrganization("ESAJ");

            var equipamentos = new List<Equipamento>();
            var historicos = new List<Historico>();
            var patrimonioSet = new HashSet<string>();

            using (var stream = new MemoryStream()) {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream)) {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++) {
                        if (worksheet.Cells[row, 1].Value == null &&
                            worksheet.Cells[row, 2].Value == null &&
                            worksheet.Cells[row, 3].Value == null &&
                            worksheet.Cells[row, 4].Value == null &&
                            worksheet.Cells[row, 5].Value == null &&
                            worksheet.Cells[row, 6].Value == null &&
                            worksheet.Cells[row, 7].Value == null &&
                            worksheet.Cells[row, 8].Value == null) {
                            break;
                        }

                        var patrimonio = worksheet.Cells[row, 1].Value?.ToString();

                        //if (_context.Equipamentos.FirstOrDefaultAsync(e => e.patrimonio == patrimonio) != null) {
                        //    await DeleteConfirmed(patrimonio);
                        //}

                        // Verificar duplicatas no arquivo Excel
                        if (!patrimonioSet.Add(patrimonio)) {
                            ModelState.AddModelError("patrimonio", $"O patrimônio {patrimonio} está duplicado no arquivo Excel.");
                            continue;
                        }

                        // Verificar se o patrimônio já está cadastrado no sistema
                        if (_context.Equipamentos.Any(e => e.patrimonio == patrimonio)) {
                            ModelState.AddModelError("patrimonio", $"O patrimônio {patrimonio} já está cadastrado no sistema.");
                            continue;
                        }

                        var situacaoDescricao = worksheet.Cells[row, 7].Value?.ToString();
                        var situacao = await _context.Situacoes.FirstOrDefaultAsync(s => s.descricao == situacaoDescricao);
                        if (situacao == null) {
                            ModelState.AddModelError("situacao", "Situação inválida");
                            continue;
                        }

                        var salaLocal = worksheet.Cells[row, 8].Value?.ToString();
                        var sala = await _context.Salas.FirstOrDefaultAsync(s => s.local == salaLocal);
                        if (sala == null) {
                            ModelState.AddModelError("sala", "Sala inválida");
                            continue;
                        }

                        var equipamento = new Equipamento {
                            patrimonio = worksheet.Cells[row, 1].Value?.ToString(),
                            item = worksheet.Cells[row, 2].Value?.ToString(),
                            nome = worksheet.Cells[row, 3].Value?.ToString(),
                            setor = worksheet.Cells[row, 4].Value?.ToString(),
                            subCategoria = worksheet.Cells[row, 5].Value?.ToString(),
                            categoria = worksheet.Cells[row, 6].Value?.ToString(),
                            situacao = situacao!.situacaoId,
                            sala = sala!.salaId,
                            dataCriacao = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
                        };

                        if (string.IsNullOrEmpty(equipamento.patrimonio)) {
                            var lastEquipamento = await _context.Equipamentos
                                .Where(e => e.patrimonio.StartsWith("Não consta"))
                                .OrderByDescending(e => e.patrimonio)
                                .FirstOrDefaultAsync();

                            int nextNumber = 1;
                            if (lastEquipamento != null) {
                                var lastNumberStr = lastEquipamento.patrimonio.Replace("Não consta [", "").Replace("]", "");
                                if (int.TryParse(lastNumberStr, out int lastNumber)) {
                                    nextNumber = lastNumber + 1;
                                }
                            }

                            equipamento.patrimonio = $"Não consta [{nextNumber:D3}]";
                        }

                        equipamentos.Add(equipamento);

                        var historico = new Historico {
                            patrimonio = equipamento.patrimonio,
                            dataAlteracao = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                            situacaoAtual = equipamento.situacao,
                            descricao = "Equipamento adicionado",
                            observacao = null,
                            importante = false
                        };

                        historicos.Add(historico);
                    }
                }
            }

            _context.Equipamentos.AddRange(equipamentos);
            await _context.SaveChangesAsync();

            _context.Historicos.AddRange(historicos);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditTag(string id, [FromBody] TagUpdateModel model) {
            var equipamento = _context.Equipamentos.FirstOrDefault(e => e.patrimonio == id);
            if (equipamento == null) {
                return NotFound("Equipamento não encontrado.");
            }

            equipamento.tag = model.Tag;
            _context.SaveChanges();

            if (equipamento.tag == null) {
                TempData["RemoveTag"] = true;
            }

            return Ok("Tag atualizada com sucesso.");
        }

        public class TagUpdateModel {
            public string? Tag { get; set; }
        }

        [HttpPost]
        public IActionResult BeforeEdit([FromBody] string subCategoria) {
            if (string.IsNullOrWhiteSpace(subCategoria)) {
                return BadRequest(new { error = "A subcategoria é obrigatória." });
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> EquipmentsPatrimonio() {
            try {
                // Consulta todos os equipamentos com os relacionamentos necessários
                var equipamentos = await _context.Equipamentos
                    .Select(e => e.patrimonio)
                    .ToListAsync();

                // Retorna os dados no formato JSON
                return Ok(equipamentos);
            } catch (Exception ex) {
                // Loga o erro para depuração
                Console.WriteLine($"Erro ao buscar equipamentos: {ex.Message}");
                return StatusCode(500, "Ocorreu um erro ao buscar os equipamentos.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> BindEquipmentsAsync([FromBody] BindEquipmentsModel model) {
            if (model == null || string.IsNullOrEmpty(model.ToBind) || string.IsNullOrEmpty(model.Equipment)) {
                return BadRequest("Os dados enviados são inválidos.");
            }

            var equipmentToBind = await _context.Equipamentos.FirstOrDefaultAsync(e => e.patrimonio == model.ToBind);
            var situacaoToBind = await _context.Situacoes
                .Where(s => s.descricao == "Transferido")
                .Select(s => s.situacaoId)
                .FirstOrDefaultAsync();

            if (situacaoToBind == 0) {
                return BadRequest("A situação 'Transferido' não foi encontrada.");
            }

            var historicoEquipmentToBind = new Historico {
                patrimonio = equipmentToBind!.patrimonio,
                dataAlteracao = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                situacaoAtual = situacaoToBind,
                descricao = "Vinculado ao equipamento '" + model.Equipment + "'",
                observacao = null,
                importante = false
            };

            equipmentToBind.situacao = situacaoToBind;

            _context.Equipamentos.Update(equipmentToBind);
            await _context.SaveChangesAsync();

            _context.Historicos.Add(historicoEquipmentToBind);
            await _context.SaveChangesAsync();



            var equipmentCurrent = await _context.Equipamentos.FirstOrDefaultAsync(e => e.patrimonio == model.Equipment);

            var historicoEquipmentCurrent = new Historico {
                patrimonio = equipmentCurrent!.patrimonio,
                dataAlteracao = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                situacaoAtual = equipmentCurrent.situacao,
                descricao = "Equipamento '" + model.ToBind + "' vinculado a este",
                observacao = null,
                importante = false
            };

            _context.Historicos.Add(historicoEquipmentCurrent);
            await _context.SaveChangesAsync();

            return Ok();
        }

        public class BindEquipmentsModel {
            public string ToBind { get; set; } // Equipamento a ser vinculado
            public string Equipment { get; set; } // Equipamento atual
        }

    }
}
