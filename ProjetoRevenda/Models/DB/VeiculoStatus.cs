using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjetoRevenda.Models.DB
{
    public partial class VeiculoStatus
    {
        public VeiculoStatus()
        {
            Veiculos = new HashSet<Veiculo>();
        }

        [Key]
        public int Id { get; set; }
        public string DescricaoStatus { get; set; } = null!;
        public string StatusInicial { get; set; } = null!;

        public virtual ICollection<Veiculo> Veiculos { get; set; }
    }
}
