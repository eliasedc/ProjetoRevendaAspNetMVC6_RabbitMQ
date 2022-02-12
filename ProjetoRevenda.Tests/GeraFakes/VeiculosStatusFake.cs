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
    public class VeiculosStatusFake
    {
        public Mock<DbSet<VeiculoStatus>> RetornarMockVeiculoStatusDbSet()
        {
            Mock<DbSet<VeiculoStatus>> objMockVeiculosDbSet = new Mock<DbSet<VeiculoStatus>>();

            var lstVeiculosStatusFake = new List<VeiculoStatus> {
                GetVeiculoStatus(1,"Disp","S"),
                GetVeiculoStatus(2,"Indisp","N")
            };

            objMockVeiculosDbSet.As<IQueryable<VeiculoStatus>>().Setup(m => m.Provider).Returns(lstVeiculosStatusFake.AsQueryable().Provider);
            objMockVeiculosDbSet.As<IQueryable<VeiculoStatus>>().Setup(m => m.Expression).Returns(lstVeiculosStatusFake.AsQueryable().Expression);
            objMockVeiculosDbSet.As<IQueryable<VeiculoStatus>>().Setup(m => m.ElementType).Returns(lstVeiculosStatusFake.AsQueryable().ElementType);
            objMockVeiculosDbSet.As<IQueryable<VeiculoStatus>>().Setup(m => m.GetEnumerator()).Returns(lstVeiculosStatusFake.AsQueryable().GetEnumerator());


            return objMockVeiculosDbSet;
        }

        /// <summary>
        /// Retorna Veiculos Fake para teste
        /// </summary>
        /// <param name="pId">Id do Veículo</param>
        /// <param name="pRenavam">Renavam do Veículo</param>
        /// <returns>Veículo preenchido</returns>
        public VeiculoStatus GetVeiculoStatus(int pId, string strDescricao, string strStatusInicial)
        {
            return new VeiculoStatus
            {
                Id = pId,
                DescricaoStatus = strDescricao,
                StatusInicial = strStatusInicial
            };
        }
    }
}
