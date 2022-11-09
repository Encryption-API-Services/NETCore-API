using Models.UserAuthentication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Validation.UserRegistration
{
    public class RegisterUserValidation
    {
        private Regex _userRegex { get; set; }
        private Regex _passwordRegex { get; set; }
        private Regex _emailRegex { get; set; }

        public RegisterUserValidation()
        {
            this._userRegex = new Regex(@"^(?=.*?[a-z]).{6,}$");
            this._passwordRegex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
            this._emailRegex = new Regex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
        }

        /// <summary>
        /// The username must be atlest 5 characters long
        /// The password must be atleast 8 chaaracters long with alaphanumeric with 2 special chaaracters.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool IsRegisterUserModelValid(RegisterUser model)
        {
            bool result = false;
            if (this._userRegex.IsMatch(model.username) && this._passwordRegex.IsMatch(model.password) && this._emailRegex.IsMatch(model.email))
            {
                result = true;
            }
            return result;
        }

        public bool IsEmailValid(string email)
        {
            bool result = false;
            if (this._emailRegex.IsMatch(email))
            {
                result = true;
            }
            return result;
        }
        public bool IsUserNameValid(string userName)
        {
            bool result = false;
            if (this._userRegex.IsMatch(userName))
            {
                result = true;
            }
            return result;
        }
    }
}