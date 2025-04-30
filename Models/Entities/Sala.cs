using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SESOPtracker.Models.Entities {
    public class Sala {
        [Key]
        [Column("SALAID")]
        public int salaId { get; set; }

        [Display(Name = "Sala")]
        [Required(ErrorMessage = "A sala é obrigatória")]
        [Column("lugar")]
        public string local { get; set; } = string.Empty;

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Selecione uma descrição válida")]
        [Column("descricao")]
        public string descricao { get; set; } = string.Empty;

        public virtual ICollection<Equipamento>? Equipamentos { get; set; }
        public int EquipamentosCount => Equipamentos?.Count() ?? 0;
    }
}