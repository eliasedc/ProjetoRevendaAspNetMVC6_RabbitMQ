using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using ProjetoRevenda.Controllers;
using ProjetoRevenda.Models.DB;
using ProjetoRevenda.Tests.GeraFakes;
using Xunit;

namespace ProjetoRevenda.Tests.Controllers
{
    public class MarcasMVCControllerTests
    {
        private MarcasFake objGeraMarcasFake;
        private MarcasMVCController objMarcasController;

        public MarcasMVCControllerTests()
        {
            var objMockContext = new Mock<BDDesenvolContext>();
            objGeraMarcasFake = new MarcasFake();

            objMockContext.Setup(m => m.Marcas).Returns(objGeraMarcasFake.RetornarMockMarcaDbSet().Object);
            
            objMarcasController = new MarcasMVCController(objMockContext.Object);
        }

        [Fact]
        public void Create_MarcaMesmoNome()
        {
            var objActionResult = objMarcasController.Create(
                objGeraMarcasFake.GetMarca(0, "Marca de Teste")
            );
            objActionResult.Wait();
            string strViewDataValid = ((Microsoft.AspNetCore.Mvc.ViewResult)objActionResult.Result).ViewData.ModelState.IsValid.ToString();
            Assert.Equal(false.ToString(),strViewDataValid);
        }
        [Fact]
        public void Create_ChamaCreate()
        {
            var objActionResult = objMarcasController.Create();
            string strViewDataValid = ((Microsoft.AspNetCore.Mvc.ViewResult)objActionResult).ViewData.ModelState.IsValid.ToString();
            Assert.Equal(true.ToString(), strViewDataValid);
        }

        [Fact]
        public void Edit_MarcaMesmoNome()
        {
            var objActionResult = objMarcasController.Edit(2,
                objGeraMarcasFake.GetMarca(2, "Marca de Teste"));
            objActionResult.Wait();
            string strViewDataValid = ((Microsoft.AspNetCore.Mvc.ViewResult)objActionResult.Result).ViewData.ModelState.IsValid.ToString();
            Assert.Equal(false.ToString(), strViewDataValid);
        }

        [Fact]
        public void Edit_IdInvalido()
        {
            var objActionResult = objMarcasController.Edit(3);
            objActionResult.Wait();
            int? intStatusCode = (objActionResult as dynamic).Result.StatusCode;
            Assert.Equal(404, intStatusCode);
        }
    }
}
