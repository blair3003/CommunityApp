namespace CommunityApp.Data.Models
{
    public class UserDto
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsManager { get; set; }
    }
}
