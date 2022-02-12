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
    public class ProprietariosMVCControllerTest
    {
        private ProprietariosFake objProprietariosFake;
        private ProprietariosMVCController objProprierariosController;

        public ProprietariosMVCControllerTest()
        {
            var objMockContext = new Mock<BDDesenvolContext>();
            objProprietariosFake = new ProprietariosFake();

            objMockContext.Setup(m => m.Proprietarios).Returns(objProprietariosFake.RetornarMockProprietarioDbSet().Object);

            objProprierariosController = new ProprietariosMVCController(objMockContext.Object);
        }        

        [Fact]
        public void Create_ProprietarioMesmoDocumento()
        {
            var objActionResult = objProprierariosController.Create(objProprietariosFake.GetProprietario(3, "81812469187"));
            objActionResult.Wait();
            string strViewDataValid = ((Microsoft.AspNetCore.Mvc.ViewResult)objActionResult.Result).ViewData.ModelState.IsValid.ToString();
            Assert.Equal(false.ToString(), strViewDataValid);
        }
        [Fact]
        public void Create_ChamaCreate()
        {
            var objActionResult = objProprierariosController.Create();
            string strViewDataValid = ((Microsoft.AspNetCore.Mvc.ViewResult)objActionResult).ViewData.ModelState.IsValid.ToString();
            Assert.Equal(true.ToString(), strViewDataValid);
        }

        [Fact]
        public void Edit_ProprietarioMesmoDocumento()
        {
            var objActionResult = objProprierariosController.Edit(2, objProprietariosFake.GetProprietario(2, "13452227774"));
            objActionResult.Wait();
            string strViewDataValid = ((Microsoft.AspNetCore.Mvc.ViewResult)objActionResult.Result).ViewData.ModelState.IsValid.ToString();
            Assert.Equal(false.ToString(), strViewDataValid);
        }

        [Fact]
        public void Edit_IdInvalido()
        {
            var objActionResult = objProprierariosController.Edit(3);
            objActionResult.Wait();
            int? intStatusCode = (objActionResult as dynamic).Result.StatusCode;
            Assert.Equal(404, intStatusCode);
        }
    }
}
