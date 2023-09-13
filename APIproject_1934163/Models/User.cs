using System.ComponentModel.DataAnnotations;

namespace APIproject_1934163.Models
{
    public class User
    {
        [Key]
        public string Uid { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        
        public User()
        {
            Uid = Guid.NewGuid().ToString();
        }

        public string GetUidbyEmail(string Email)
        {
            return Uid;
        }
    }   
}
