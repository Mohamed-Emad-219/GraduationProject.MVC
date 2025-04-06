using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GP.DAL.Models
{
    public class PetitionCourse
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PetitionRequestId { get; set; }  // Foreign Key

        [Required]
        public string CourseCode { get; set; }

        [ForeignKey("PetitionRequestId")]
        public PetitionRequest? PetitionRequest { get; set; }

        [ForeignKey("CourseCode")]
        public Course? Course { get; set; }
    }
}