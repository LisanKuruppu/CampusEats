namespace Core.Application.DTOs
{
    public class RegisterUserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UpdateProfileDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

}

