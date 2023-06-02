using Repository.Enums;

namespace Repository.Models
{
    public class User
    {
        public UserRole Role { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public List<Test> Tests { get; set; }
    }
}
