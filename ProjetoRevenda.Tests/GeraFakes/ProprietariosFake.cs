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
    public class ProprietariosFake
    {
        public Mock<DbSet<Proprietario>> RetornarMockProprietarioDbSet()
        {
            Mock<DbSet<Proprietario>> objMockProprietariosDbSet = new Mock<DbSet<Proprietario>>();

            var lstProprietariosFake = new List<Proprietario> {
                GetProprietario(1,"13452227774"),
                GetProprietario(2,"81812469187")
            };

            objMockProprietariosDbSet.As<IQueryable<Proprietario>>().Setup(m => m.Provider).Returns(lstProprietariosFake.AsQueryable().Provider);
            objMockProprietariosDbSet.As<IQueryable<Proprietario>>().Setup(m => m.Expression).Returns(lstProprietariosFake.AsQueryable().Expression);
            objMockProprietariosDbSet.As<IQueryable<Proprietario>>().Setup(m => m.ElementType).Returns(lstProprietariosFake.AsQueryable().ElementType);
            objMockProprietariosDbSet.As<IQueryable<Proprietario>>().Setup(m => m.GetEnumerator()).Returns(lstProprietariosFake.AsQueryable().GetEnumerator());

            return objMockProprietariosDbSet;
        }

        /// <summary>
        /// Retorna Proprietarios Fake para teste
        /// </summary>
        /// <param name="pId">Id do Proprietário</param>
        /// <param name="pDocumento">Documento do Proprietário</param>
        /// <returns>Proprietario Preenchido</returns>
        public Proprietario GetProprietario(int pId, string pDocumento)
        {
            return new Proprietario
            {
                Id = pId,
                Nome = "João da Silva",
                Cep = 12345678,
                Complemento = "123, bl 1 ap 111",
                Documento = pDocumento,
                Email = "teste@teste.com",
                Endereco = "Rua de Teste",
                Status = "A"
            };
        }
    }
}
