using System.ComponentModel.DataAnnotations;

namespace APIproject_1934163.Models
{
    public class Session
    {
        [Key]
        public string Token { get; set; }
        [Required]
        public string Email { get; set; }

        public Session()
        {
        }
    }
}
