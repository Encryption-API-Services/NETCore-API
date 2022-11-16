using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Validation.Phone
{
    public class PhoneValidator
    {
        private Regex _phoneNumberRegex { get; set; }

        public PhoneValidator()
        {
            this._phoneNumberRegex = new Regex(@"^((\\+91-?)|0)?[0-9]{10}$");
        }

        public bool IsPhoneNumberValid(string phoneNumber)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                if (this._phoneNumberRegex.IsMatch(phoneNumber))
                {
                    result = true;
                }
            }
            else
            {
                throw new Exception("Please provide a string to to test for phone number validation");
            }
            return result;
        }
    }
}