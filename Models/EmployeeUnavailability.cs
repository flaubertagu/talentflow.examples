namespace TalentFlow.Demo.Models
{
    public class EmployeeUnavailability
    {
        public required string EmployeeName { get; set; }
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
    }
}
