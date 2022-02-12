using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProjetoRevenda.Models.DB;

namespace ProjetoRevenda.Tests.GeraFakes
{
    public class VeiculosFake
    {
        public Mock<DbSet<Veiculo>> RetornarMockVeiculoDbSet()
        {
            Mock<DbSet<Veiculo>> objMockVeiculosDbSet = new Mock<DbSet<Veiculo>>();

            var lstVeiculosFake = new List<Veiculo> {
                GetVeiculo(1,12345678),
                GetVeiculo(2,87654321)
            };

            objMockVeiculosDbSet.As<IQueryable<Veiculo>>().Setup(m => m.Provider).Returns(lstVeiculosFake.AsQueryable().Provider);
            objMockVeiculosDbSet.As<IQueryable<Veiculo>>().Setup(m => m.Expression).Returns(lstVeiculosFake.AsQueryable().Expression);
            objMockVeiculosDbSet.As<IQueryable<Veiculo>>().Setup(m => m.ElementType).Returns(lstVeiculosFake.AsQueryable().ElementType);
            objMockVeiculosDbSet.As<IQueryable<Veiculo>>().Setup(m => m.GetEnumerator()).Returns(lstVeiculosFake.AsQueryable().GetEnumerator());


            return objMockVeiculosDbSet;
        }

        /// <summary>
        /// Retorna Veiculos Fake para teste
        /// </summary>
        /// <param name="pId">Id do Veículo</param>
        /// <param name="pRenavam">Renavam do Veículo</param>
        /// <returns>Veículo preenchido</returns>
        public Veiculo GetVeiculo(int pId, int pRenavam)
        {
            return new Veiculo
            {
                Id = pId,
                AnoFabricacao = 2020,
                AnoModelo = 2020,
                MarcaId = 1,
                Modelo = "Modelo de Teste",
                ProprietarioId = 1,
                Quilometragem = 123,
                Renavam = pRenavam,
                StatusVeiculoId = 1,
                Valor = 100000
            };
        }
    }
}
