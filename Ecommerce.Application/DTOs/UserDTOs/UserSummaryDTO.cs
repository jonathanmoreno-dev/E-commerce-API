namespace Ecommerce.Application.DTOs.UserDTOs
{
    public class UserSummaryDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public bool IsAdmin { get; set; }
    }
}
