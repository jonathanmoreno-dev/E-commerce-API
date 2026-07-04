namespace Ecommerce.Application.DTOs.UserDTOs
{
    public class UserListDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = "";
        public bool IsAdmin { get; set; }
    }
}
