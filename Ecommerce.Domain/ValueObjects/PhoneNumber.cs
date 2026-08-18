using Ecommerce.Domain.Exceptions;
using PhoneNumbers;

namespace Ecommerce.Domain.ValueObjects
{
    public record PhoneNumber
    {
        public string Value { get; } = null!;
        private PhoneNumber() { }
        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException("Phone number cannot be empty");

            value = value.Trim();

            if (value.Length > 50)
                throw new DomainValidationException("Phone number exceed 50 characters");

            if(!value.StartsWith('+'))
                throw new DomainValidationException("Phone number need to starts with +");

            var phoneUtils = PhoneNumberUtil.GetInstance();

            try
            {
                var number = phoneUtils.Parse(value, null);
                if (!phoneUtils.IsValidNumber(number))
                    throw new DomainValidationException("Invalid phone number");

                Value = phoneUtils.Format(number, PhoneNumberFormat.E164);
            }
            catch (NumberParseException)
            {
                throw new DomainValidationException("Invalid phone number");
            }
        }
    }
}
