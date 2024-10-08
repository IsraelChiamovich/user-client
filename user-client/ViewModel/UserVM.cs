﻿namespace user_client.ViewModel
{
    public class UserVM
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public IFormFile? Image { get; set; }
        public List<UserVM> Friends { get; set; } = [];
    }
}
