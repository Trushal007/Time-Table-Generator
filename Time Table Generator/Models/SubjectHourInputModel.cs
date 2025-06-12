using System.ComponentModel.DataAnnotations;

namespace Time_Table_Generator.Models
{
    public class SubjectHourInputModel
    {
        public int TotalHoursOfWeek { get; set; }

        [Required(ErrorMessage = "Please enter the name for each subject.")]
        public List<SubjectHourDetail> SubjectDetails { get; set; } = new List<SubjectHourDetail>();
    }
    public class SubjectHourDetail
    {
        [Required(ErrorMessage = "Please enter subject name.")]
        public string SubjectName { get; set; }

        [Required(ErrorMessage = "Please enter hours for this subject.")]
        [Range(1, int.MaxValue, ErrorMessage = "Hours must be a positive number.")]
        public int Hours { get; set; }
    }
}
