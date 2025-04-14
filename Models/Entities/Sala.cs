using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SESOPtracker.Models.Entities {
    public class Sala {
        [Key]
        public int salaId { get; set; }

        [Display(Name = "Sala")]
        [Required(ErrorMessage = "A sala é obrigatória")]
        public string local { get; set; } = string.Empty;

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Selecione uma descrição válida")]
        public string descricao { get; set; } = string.Empty;

        public virtual ICollection<Equipamento>? Equipamentos { get; set; }
        public int EquipamentosCount => Equipamentos?.Count() ?? 0;
    }
}