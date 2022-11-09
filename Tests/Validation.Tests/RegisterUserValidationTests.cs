using Validation.UserRegistration;
using Xunit;

namespace Validation.Tests
{
    public class RegisterUserValidationTests
    {
       private RegisterUserValidation _validation { get; set; }

        public RegisterUserValidationTests()
        {
            _validation = new RegisterUserValidation();
        }

        [Fact] 
        public void EmailIsValid()
        {
            Assert.Equal(true, this._validation.IsEmailValid("mtmulch@gmail.com"));
        }

        [Fact]
        public void EmailIsNotValid()
        {
            Assert.Equal(false, this._validation.IsEmailValid("mtmulch"));
        }
    }
}
