namespace E_commerce_API.src.Application.DTOs.UserDTOs
{
    public class UserListDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public bool IsAdmin { get; set; }
    }
}
