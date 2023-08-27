using System.ComponentModel.DataAnnotations;

namespace Orion.Models
{
    public class ProjectData
    {
        public int Id { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public DateTime ProjectDueDate { get; set; }

        [Required]
        public bool ProjectStatus { get; set; }

        [Required]
        public bool ProjectCompleted { get; set; }
        [Required]
        public int TotalProjectTasks { get; set; }

        [Required]
        public int ProjectTasksCompleted { get; set; }

    }
}
