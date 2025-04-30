using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SESOPtracker.Models.Entities {
    public class Historico {
        [Key]
        public int historicoId { get; set; }

        [Display(Name = "Patrimônio")]
        [Required(ErrorMessage = "O patrimônio é obrigatório")]
        public string patrimonio { get; set; } = string.Empty;

        [Display(Name = "Data e alteração")]
        public string dataAlteracao { get; set; } = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

        [Display(Name = "Situação atual")]
        [Required(ErrorMessage = "A situacão é obrigatória")]
        public virtual int situacaoAtual { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Informe uma descrição para a alteração")]
        public string descricao { get; set; } = string.Empty;

        [Display(Name = "Observação")]
        public string? observacao { get; set; }

        [Display(Name = "Importante")]
        public int importante { get; set; } = 0;



        [ForeignKey("situacaoAtual")]
        public virtual Situacao Situacao { get; set; } = null!;

        [ForeignKey("patrimonio")]
        public virtual Equipamento Equipamento { get; set; } = null!;
    }
}
