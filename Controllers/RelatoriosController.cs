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
using OfficeOpenXml;
using System.Text.Json;
using System.Text;

namespace SESOPtracker.Controllers
{
    public class RelatoriosController : Controller {
        private readonly ApplicationDbContext _context;

        public RelatoriosController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: Relatorios
        public async Task<IActionResult> Index() {
            var applicationDbContext = _context.Equipamentos.Include(e => e.Sala).Include(e => e.Situacao);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpGet]
        public IActionResult GetEquipamentos() {
            var equipamentos = _context.Equipamentos
                .Select(e => new {
                    e.patrimonio,
                    e.item,
                    e.nome,
                    e.setor,
                    e.subCategoria,
                    e.categoria,
                    Situacao = e.Situacao != null ? e.Situacao.descricao : null,
                    Sala = e.Sala != null ? e.Sala.local : null,
                    e.tag
                })
                .ToList();

            return Json(equipamentos);
        }

        [HttpPost]
        public IActionResult ExportToExcel([FromBody] List<Dictionary<string, object>> equipamentos) {
            if (equipamentos == null || !equipamentos.Any()) {
                // Pode retornar para a view com mensagem de erro
                TempData["Erro"] = "Nenhum dado para exportar";
                return RedirectToAction("Index");
            }

            using (var package = new ExcelPackage()) {
                var worksheet = package.Workbook.Worksheets.Add("Dados");

                if (equipamentos.Count > 0) {
                    var headers = equipamentos[0].Keys.ToList();

                    for (int i = 0; i < headers.Count; i++) {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                    }

                    // Adiciona dados
                    for (int i = 0; i < equipamentos.Count; i++) {
                        var rowData = equipamentos[i];
                        for (int j = 0; j < headers.Count; j++) {
                            worksheet.Cells[i + 2, j + 1].Value = rowData[headers[j]]?.ToString();
                        }
                    }
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream,
                           "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                           "equipamentos.xlsx");
            }
        }

        [HttpPost]
        public IActionResult ExportToCsv([FromBody] List<Dictionary<string, object>> equipamentos) {
            if (equipamentos == null || !equipamentos.Any()) {
                TempData["Erro"] = "Nenhum dado para exportar";
                return RedirectToAction("Index");
            }

            var csv = new StringBuilder();

            // Adicionar cabeçalhos
            var headers = equipamentos[0].Keys.ToList();
            csv.AppendLine(string.Join(",", headers));

            // Adicionar dados
            foreach (var equipamento in equipamentos) {
                var row = headers.Select(header => equipamento[header]?.ToString()?.Replace(",", " ")); // Evitar problemas com vírgulas
                csv.AppendLine(string.Join(",", row));
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            var stream = new MemoryStream(bytes);

            return File(stream, "text/csv", "equipamentos.csv");
        }

    }
}
