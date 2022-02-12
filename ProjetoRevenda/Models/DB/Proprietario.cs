using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjetoRevenda.Models.DB
{
    public partial class Proprietario
    {
        private string _documento;
        public Proprietario()
        {
            Veiculos = new HashSet<Veiculo>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Nome é um campo obrigatório")]
        [MaxLength(100, ErrorMessage = "Número máximo de caracteres superior ao permitido")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O Documento é um campo obrigatório")]
        [MaxLength(18, ErrorMessage = "Número máximo de caracteres superior ao permitido")]
        [Display(Name = "CPF / CNPJ")]
        public string Documento
        {
            get { return _documento; }
            set
            {
                //Remove tudo o que não for número
                _documento = string.Join("", System.Text.RegularExpressions.Regex.Split(value, @"[^\d]"));
            }
        }

        [Required(ErrorMessage = "O Email é um campo obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [MaxLength(100, ErrorMessage = "Número máximo de caracteres superior ao permitido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "O Cep é um campo obrigatório")]
        [Range(0, 99999999, ErrorMessage = "o CEP deve conter 8 dígitos")]
        public int Cep { get; set; }

        [Required(ErrorMessage = "O Endereco é um campo obrigatório")]
        [MaxLength(250, ErrorMessage = "Número máximo de caracteres superior ao permitido")]
        public string Endereco { get; set; } = null!;

        [Required(ErrorMessage = "O Complemento é um campo obrigatório")]
        [Display(Name = "Nº / Complemento")]
        [MaxLength(100, ErrorMessage = "Número máximo de caracteres superior ao permitido")]
        public string Complemento { get; set; } = null!;

        [Required(ErrorMessage = "o Status é um campo obrigatório")]
        [RegularExpression(@"^(A|C)$", ErrorMessage = "Valor do Status Incorreto, favor abrir a página novamente.")]
        public string Status { get; set; } = null!;

        public virtual ICollection<Veiculo> Veiculos { get; set; }
    }
}
