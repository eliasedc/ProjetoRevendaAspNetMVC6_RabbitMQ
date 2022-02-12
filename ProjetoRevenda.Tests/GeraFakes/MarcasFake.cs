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
    public class MarcasFake
    {
        public Mock<DbSet<Marca>> RetornarMockMarcaDbSet()
        {
            Mock<DbSet<Marca>> objMockMarcasDbSet = new Mock<DbSet<Marca>>();

            var lstMarcasFake = new List<Marca> {
                new Marca { Id =1, Nome = "Marca de Teste", Status = "A" },
                new Marca { Id =2, Nome = "Marca de Teste2", Status = "A"}
            };

            objMockMarcasDbSet.As<IQueryable<Marca>>().Setup(m => m.Provider).Returns(lstMarcasFake.AsQueryable().Provider);
            objMockMarcasDbSet.As<IQueryable<Marca>>().Setup(m => m.Expression).Returns(lstMarcasFake.AsQueryable().Expression);
            objMockMarcasDbSet.As<IQueryable<Marca>>().Setup(m => m.ElementType).Returns(lstMarcasFake.AsQueryable().ElementType);
            objMockMarcasDbSet.As<IQueryable<Marca>>().Setup(m => m.GetEnumerator()).Returns(lstMarcasFake.AsQueryable().GetEnumerator());

            return objMockMarcasDbSet;
        }

        public Marca GetMarca(int pId, string pNome)
        {
            return new Marca
            {
                Id = pId,
                Nome = pNome,
                Status = "A"
            };
        }
    }
}
