using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProjetoRevenda.Controllers;
using ProjetoRevenda.Models.DB;
using ProjetoRevenda.Tests.GeraFakes;
using Xunit;

namespace ProjetoRevenda.Tests.Controllers
{
    public class VeiculosMVCControllerTest
    {
        private VeiculosFake objVeiculosFake;
        private MarcasFake objMarcasFake;
        private ProprietariosFake objProprietariosFake;
        private VeiculosStatusFake objVeiculosStatusFake;
        private VeiculosMVCController objVeiculosController;
        //private BDDesenvolContext teste; 
        public VeiculosMVCControllerTest()
        {
            var objMockContext = new Mock<BDDesenvolContext>();
            objVeiculosFake = new VeiculosFake();
            objProprietariosFake = new ProprietariosFake();
            objMarcasFake = new MarcasFake();
            objVeiculosStatusFake = new VeiculosStatusFake();
                        
            objMockContext.Setup(m => m.Proprietarios).Returns(objProprietariosFake.RetornarMockProprietarioDbSet().Object);
            objMockContext.Setup(m => m.Marcas).Returns(objMarcasFake.RetornarMockMarcaDbSet().Object);
            objMockContext.Setup(m => m.VeiculoStatuses).Returns(objVeiculosStatusFake.RetornarMockVeiculoStatusDbSet().Object);
            objMockContext.Setup(m => m.Veiculos).Returns(objVeiculosFake.RetornarMockVeiculoDbSet().Object);

            objVeiculosController = new VeiculosMVCController(objMockContext.Object);
        }
        

        [Fact]
        public void Create_VeiculoMesmoRenavam()
        {
            var objVeiculoInsercao = objVeiculosFake.GetVeiculo(3, 12345678);

            var objActionResult = objVeiculosController.Create(objVeiculoInsercao);
            objActionResult.Wait();

            string strViewDataValid = ((Microsoft.AspNetCore.Mvc.ViewResult)objActionResult.Result).ViewData.ModelState.IsValid.ToString();
            Assert.Equal(false.ToString(), strViewDataValid);
        }
        [Fact]
        public void Create_ChamaCreate()
        {
            var objActionResult = objVeiculosController.Create();

            string strViewDataValid = ((Microsoft.AspNetCore.Mvc.ViewResult)objActionResult).ViewData.ModelState.IsValid.ToString();
            Assert.Equal(true.ToString(), strViewDataValid);
        }

        [Fact]
        public void Edit_VeiculoMesmoRenavam()
        {
            var objVeiculoEdicao = objVeiculosFake.GetVeiculo(2, 12345678);            

            var objActionResult = objVeiculosController.Edit(2, objVeiculoEdicao);
            objActionResult.Wait();

            string strViewDataValid = ((Microsoft.AspNetCore.Mvc.ViewResult)objActionResult.Result).ViewData.ModelState.IsValid.ToString();
            Assert.Equal(false.ToString(), strViewDataValid);
        }

        [Fact]
        public void Edit_IdInvalido()
        {
            var objActionResult = objVeiculosController.Edit(3);
            objActionResult.Wait();

            int? intStatusCode = (objActionResult as dynamic).Result.StatusCode;
            Assert.Equal(404, intStatusCode);
        }
    }
}
