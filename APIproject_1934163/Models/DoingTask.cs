using System.ComponentModel.DataAnnotations;

namespace APIproject_1934163.Models
{
    public class DoingTask
    {
        [Key]
        public string TaskUid { get; set; }

        [Required]
        public string CreatedByUid { get; set; }
        [Required]
        public string CreatedByName { get; set; }
        [Required]
        public string AssignedToUid { get; set; }
        [Required]
        public string AssignedToName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool Done { get; set; }

        public DoingTask() 
        {
            Done = false;
            TaskUid = Guid.NewGuid().ToString();
        }
    }
}
