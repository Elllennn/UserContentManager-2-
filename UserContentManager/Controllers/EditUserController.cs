using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using UserContentManager.Service;

namespace UserContentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class EditUserController : ControllerBase
    {
        private readonly ILogger<EditUserController> _logger;

        public EditUserController(ILogger<EditUserController> logger)
        {
            _logger = logger;
        }

        [HttpPut("userName")]
        public IActionResult EditUserName(string name)
        {
            if (name.Length < 3)
            {
                return BadRequest("UserName must contain at least 3 characters");
            }
            return Ok();
        }


        [HttpPut("email")]
        public IActionResult EditEmail([FromBody] string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
          
            var regex = new Regex(emailPattern);
            if (!regex.IsMatch(email))
            {
                return BadRequest("Email is not valid");
            }
            return Ok();
        }

        [HttpPut("password")]
        public IActionResult EditPassword([FromBody] string password, string userName)
        {
            if (!PasswordValidator.IsValidPassword(password, userName))
            {
                return BadRequest("Password must contain at least 6 characters");
            }
            return Ok();
        }

        [HttpPut("date")]
        public IActionResult EditDateofBirth([FromBody] DateTime date)
        {
            if (date > DateTime.Now)
            {
                return BadRequest("Date of birth must be in the past");
            }
            return Ok();
        }


        [HttpPut("quantity")]
        public IActionResult EditQuantity([FromBody] int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("Quantity must be a positive number");
            }
            return Ok();
        }


        [HttpPut("price")]
        public IActionResult EditPrice([FromBody] string price)
        {
            if (!decimal.TryParse(price, out decimal decPrice))
            {
                return BadRequest("Price must be a decimal value");
            }
            return Ok();
        }

        [HttpPut("amount")]
        public IActionResult EditAmount([FromBody] int amount)
        {
            if (amount >= 50)
            {
                return BadRequest("Amount must be less then 50");
            }
            return Ok();
        }

    }
}
