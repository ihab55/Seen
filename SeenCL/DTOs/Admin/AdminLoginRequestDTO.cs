namespace SeenCL.DTOs
{
    public class AdminLoginRequestDTO
    {
        public string Email { set; get; }
        public string Password { set; get; }
        public AdminLoginRequestDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
