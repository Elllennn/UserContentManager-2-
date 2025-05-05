using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UserContentManager.Controllers;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace UnitTest
{
    public class EditUserTestController 
    {
        [Fact]
        public void Test_BasicAssertion()
        {
            bool trueValue = true;
            Assert.True(trueValue);
        }
        //[Fact]
        //public void Test_EditUserName()
        //{
        //    //arrange
        //    var mock=new Mock<ILogger<EditUserController>>();
        //    EditUserController controller = new EditUserController(mock.Object);
        //    //act

        //    IActionResult badResult = controller.EditUserName("El");
        //    IActionResult okResult = controller.EditUserName("Elen");
        //    IActionResult emptyResult = controller.EditUserName(string.Empty);


        //   Assert.ty(okResult);
        //    //Assert.IsType<BadRequestObjectResult>(okResult);
        //    //Assert.IsType<BadRequestObjectResult>(emptyResult);
        //}
    }
}
