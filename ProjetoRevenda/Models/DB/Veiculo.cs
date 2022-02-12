using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjetoRevenda.Models.DB
{
    public partial class Veiculo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Proprietário é um campo obrigatório")]
        [Display(Name ="Proprietário")]
        [Range(0,int.MaxValue, ErrorMessage = "Número inválido")]
        public int ProprietarioId { get; set; }

        [Required(ErrorMessage = "O Renavam é um campo obrigatório")]
        [Range(0, 99999999999, ErrorMessage = "Renavam deve conter entre 1 e 11 dígitos")]
        public long Renavam { get; set; }

        [Required(ErrorMessage = "A Marca é um campo obrigatório")]
        [Display(Name ="Marca")]
        [Range(0, int.MaxValue, ErrorMessage = "Número inválido")]
        public int MarcaId { get; set; }

        [Required(ErrorMessage = "O Modelo é um campo obrigatório")]
        [MaxLength(100, ErrorMessage = "Número máximo de caracteres superior ao permitido")]
        public string Modelo { get; set; } = null!;

        [Required(ErrorMessage = "O Ano Fabricação é um campo obrigatório")]
        [Display(Name ="Ano Fabricação")]
        [VeiculoAnoValido(1800)]
        [Range(0, int.MaxValue, ErrorMessage = "Número inválido")]
        public int AnoFabricacao { get; set; }

        [Required(ErrorMessage = "O Ano Modelo é um campo obrigatório")]
        [Display(Name = "Ano Modelo")]
        [VeiculoAnoValido(1800)]        
        public int AnoModelo { get; set; }

        [Required(ErrorMessage = "A Quilometragem é um campo obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Valor deve ser maior que 0 e menor que 2147483647")]
        public int Quilometragem { get; set; }

        [Required(ErrorMessage = "O Valor é um campo obrigatório")]
        // removido daqui devido ao formato mostrado na tela. Não consegue entender os "."
        // adicionado validação no controller
        // [Range(0.01, 99999999.99, ErrorMessage = "O Valor deve estar entre 0,01 e 99.999.999,99")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$", ErrorMessage = "O campo deve ser numérico")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O Status é um campo obrigatório")]
        [Display(Name ="Status")]
        [Range(0, int.MaxValue, ErrorMessage = "Número inválido")]
        public int StatusVeiculoId { get; set; }

        public virtual Marca Marca { get; set; } = null!;

        [Display(Name = "Proprietário")]
        public virtual Proprietario Proprietario { get; set; } = null!;

        [Display(Name = "Status")]
        public virtual VeiculoStatus StatusVeiculo { get; set; } = null!;
    }
}
