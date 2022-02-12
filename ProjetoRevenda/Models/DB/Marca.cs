using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjetoRevenda.Models.DB
{
    public partial class Marca
    {
        public Marca()
        {
            Veiculos = new HashSet<Veiculo>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Nome da Marca é um campo obrigatório")]
        [MaxLength(100, ErrorMessage = "Número máximo de caracteres superior ao permitido")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "o Status é um campo obrigatório")]
        [RegularExpression(@"^(A|C)$", ErrorMessage = "Valor do Status Incorreto, favor abrir a página novamente.")]
        public string Status { get; set; } = null!;

        public virtual ICollection<Veiculo> Veiculos { get; set; }
    }
}
