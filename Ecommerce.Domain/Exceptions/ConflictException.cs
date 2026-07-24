namespace Ecommerce.Domain.Exceptions
{
    public class ConflictException : Exception
    {
        public string ClientMessage { get; }
        public ConflictException(string clientMessage, string developerMessage) : base(developerMessage)
        {
            ClientMessage = clientMessage;
        }
    }
}
