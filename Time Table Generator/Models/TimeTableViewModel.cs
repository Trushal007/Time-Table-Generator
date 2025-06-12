namespace Time_Table_Generator.Models
{
    public class TimeTableViewModel
    {
        public int NoOfWorkingDays { get; set; }
        public int NoOfSubjectsPerDay { get; set; }
        public int TotalHoursOfWeek { get; set; }
        public List<SubjectHourDetail> SubjectDetails { get; set; } 
        public string[,] Timetable { get; set; }
    }
}
