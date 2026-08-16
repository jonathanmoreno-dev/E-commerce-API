using System.Diagnostics;
using System.Net.Mail;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.ValueObjects
{
    public record Email
    {
        public string Value { get; } = null!;
        private Email() { }
        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException("Email cannot be empty");

            value = value.Trim().ToLowerInvariant();

            if (value.Length > 255)
                throw new DomainValidationException("Email cannot exceed 255 characters");
            if (!IsValid(value))
                throw new DomainValidationException("Invalid email");

            Value = value;
        }
        private static bool IsValid(string emailAddress)
        {
            try
            {
                var email = new MailAddress(emailAddress);
                return email.Address == emailAddress;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
