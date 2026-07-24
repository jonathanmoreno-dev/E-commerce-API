namespace Ecommerce.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public string Resource { get; }
        public NotFoundException(string resource) : base($"{resource} was not found")
        {
            Resource = resource;
        }
        public NotFoundException(string resource, string developerMessage) : base(developerMessage)
        {
            Resource = resource;
        }
    }
}
