using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SESOPtracker.Models.Entities {
    public class Situacao {
        [Key]
        public int situacaoId { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "A descrição é obrigatória")]
        public string descricao { get; set; } = string.Empty;

        [Display(Name = "Cor")]
        [Required(ErrorMessage = "Selecione uma cor válida")]
        public string cor { get; set; } = "#000000";

        public virtual ICollection<Equipamento>? Equipamentos { get; set; }
        public int EquipamentosCount => Equipamentos?.Count() ?? 0;

        public string ToRgb(string color) {
            color = color.TrimStart('#');

            if (color.Length != 3 && color.Length != 6 && color.Length != 8) {
                throw new ArgumentException("Formato hexadecimal inválido. Use #RGB, #RRGGBB ou #RRGGBBAA.");
            }

            if (color.Length == 3) {
                color = $"{color[0]}{color[0]}{color[1]}{color[1]}{color[2]}{color[2]}";
            }

            byte red = Convert.ToByte(color.Substring(0, 2), 16);
            byte green = Convert.ToByte(color.Substring(2, 2), 16);
            byte blue = Convert.ToByte(color.Substring(4, 2), 16);

            if (color.Length == 8) {
                byte alpha = Convert.ToByte(color.Substring(6, 2), 16);
                return $"rgba({red}, {green}, {blue}, 0.{alpha})";
            }

            return $"rgb({red}, {green}, {blue})";
        }
    }
}
