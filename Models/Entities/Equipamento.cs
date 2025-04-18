using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SESOPtracker.Models.Entities {
    public class Equipamento {
        [Key]
        [Display(Name = "Patrimônio")]
        [Required(ErrorMessage = "O patrimônio é obrigatório")]
        public string patrimonio { get; set; } = string.Empty;

        [Display(Name = "Item")]
        public string? item { get; set; }

        [Display(Name = "Nome")]
        public string? nome { get; set; }

        [Display(Name = "Subcategoria", Prompt = "Cadeira, Projetor, Monitor...")]
        [Required(ErrorMessage = "A subcategoria é obrigatória")]
        public string subCategoria { get; set; } = string.Empty;

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "A categoria é obrigatória")]
        public string categoria { get; set; } = string.Empty; // Informática, Mobília, Telefonia, Audiovisual

        [Display(Name = "Setor")]
        public string? setor { get; set; }

        [Display(Name = "Data de criação")]
        public string dataCriacao { get; set; } = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

        [Display(Name = "Situação")]
        [Required(ErrorMessage = "A situacão é obrigatória")]
        public int situacao { get; set; }

        [Display(Name = "Sala")]
        [Required(ErrorMessage = "A sala é obrigatória")]
        public int sala { get; set; }

        [Display(Name = "Tag")]
        public string? tag { get; set; }




        [ForeignKey("situacao")]
        public virtual Situacao Situacao { get; set; } = null!;

        [ForeignKey("sala")]
        public virtual Sala Sala { get; set; } = null!;


        public virtual ICollection<Historico> Historico { get; set; } = new List<Historico>();
    }
}
