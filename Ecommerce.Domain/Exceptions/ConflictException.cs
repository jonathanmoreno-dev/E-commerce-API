namespace Ecommerce.Domain.Exceptions
{
    public class ConflictException : Exception
    {
        public string Resource { get; }
        public ConflictException(string resource) : base($"{resource} already exists")
        {
            Resource = resource;
        }
        public ConflictException(string resource, string developerMessage) : base(developerMessage)
        {
            Resource = resource;
        }
    }
}
