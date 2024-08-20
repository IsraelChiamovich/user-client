using user_client.ViewModel;

namespace user_client.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public byte[]? Image { get; set; }
        public List<UserDto> Friends { get; set; } = [];
    }
}
