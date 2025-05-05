using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UserContentManager.Controllers;
using UserContentManager.Service;
using Xunit;

namespace UnitTest
{
    public class EditUserTestController
    {
        private readonly Mock<ILogger<EditUserController>> _mockLogger = new();

        [Fact]
        public void Test_EditUserName_ValidName_ReturnsOk()
        {
            
            var controller = new EditUserController(_mockLogger.Object);

            var result = controller.EditUserName("Elen"); 

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Test_EditUserName_ShortName_ReturnsBadRequest()
        {
            var controller = new EditUserController(_mockLogger.Object);

            var result = controller.EditUserName("El"); 

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Test_EditUserName_EmptyName_ReturnsBadRequest()
        {
            var controller = new EditUserController(_mockLogger.Object);

            var result = controller.EditUserName(string.Empty);

            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public void EditEmail_ValidEmail_ReturnsOk()
        {
            var controller = new EditUserController(_mockLogger.Object);
            var result = controller.EditEmail("test@example.com");
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void EditEmail_InvalidEmail_ReturnsBadRequest()
        {
            var controller = new EditUserController(_mockLogger.Object);
            var result = controller.EditEmail("invalid-email");
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public void IsValidPassword_ValidPassword_ReturnsTrue()
        {
            var password = "Arme123!ն";
            var username = "user1";
            var result = PasswordValidator.IsValidPassword(password, username);
            Assert.True(result);
        }

        [Fact]
        public void IsValidPassword_TooShort_ReturnsFalse()
        {
            var password = "Arm1!";
            var username = "user1";
            var result = PasswordValidator.IsValidPassword(password, username);
            Assert.False(result);
        }

        [Fact]
        public void IsValidPassword_MissingUppercase_ReturnsFalse()
        {
            var password = "armen123!ն";
            var username = "user1";
            var result = PasswordValidator.IsValidPassword(password, username);
            Assert.False(result);
        }

        

        [Fact]
        public void IsValidPassword_MissingDigit_ReturnsFalse()
        {
            var password = "Armenian!ն";
            var username = "user1";
            var result = PasswordValidator.IsValidPassword(password, username);
            Assert.False(result);
        }

        [Fact]
        public void IsValidPassword_MissingSpecialChar_ReturnsFalse()
        {
            var password = "Armen123ն";
            var username = "user1";
            var result = PasswordValidator.IsValidPassword(password, username);
            Assert.False(result);
        }

        [Fact]
        public void IsValidPassword_MissingArmenianLetter_ReturnsFalse()
        {
            var password = "Armen123!";
            var username = "user1";
            var result = PasswordValidator.IsValidPassword(password, username);
            Assert.False(result);
        }

        [Fact]
        public void IsValidPassword_ContainsUsername_ReturnsFalse()
        {
            var password = "User123!ն";
            var username = "user";
            var result = PasswordValidator.IsValidPassword(password, username);
            Assert.False(result);
        }

        [Fact]
        public void IsValidPassword_NullPassword_ReturnsFalse()
        {
            var username = "user1";
            var result = PasswordValidator.IsValidPassword(null, username);
            Assert.False(result);
        }

        [Fact]
        public void IsValidPassword_EmptyPassword_ReturnsFalse()
        {
            var username = "user1";
            var result = PasswordValidator.IsValidPassword("", username);
            Assert.False(result);
        }

        [Fact]
        public void IsValidPassword_WhitespacePassword_ReturnsFalse()
        {
            var username = "user1";
            var result = PasswordValidator.IsValidPassword("   ", username);
            Assert.False(result);
        }

        [Fact]
        public void EditDateofBirth_ValidDate_ReturnsOk()
        {
            var controller = new EditUserController(_mockLogger.Object);
            var result = controller.EditDateofBirth(DateTime.Now.AddDays(-20));
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void EditDateofBirth_FutureDate_ReturnsBadRequest()
        {
            var controller = new EditUserController(_mockLogger.Object);
            var result = controller.EditDateofBirth(DateTime.Now.AddDays(1));
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void EditQuantity_PositiveNumber_ReturnsOk()
        {
            var controller = new EditUserController(_mockLogger.Object);
            var result = controller.EditQuantity(1);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void EditQuantity_NonPositive_ReturnsBadRequest()
        {
            var controller = new EditUserController(_mockLogger.Object);
            var result = controller.EditQuantity(0);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void EditPrice_ValidDecimal_ReturnsOk()
        {
            var controller = new EditUserController(_mockLogger.Object);
            var result = controller.EditPrice("10.99");
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void EditPrice_InvalidDecimal_ReturnsBadRequest()
        {
            var controller = new EditUserController(_mockLogger.Object);
            var result = controller.EditPrice("not-a-number");
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void EditAmount_LessThan50_ReturnsOk()
        {
            var controller = new EditUserController(_mockLogger.Object);
            var result = controller.EditAmount(49);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void EditAmount_50OrMore_ReturnsBadRequest()
        {
            var controller = new EditUserController(_mockLogger.Object);
            var result = controller.EditAmount(50);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
