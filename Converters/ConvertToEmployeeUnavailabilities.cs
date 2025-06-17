using TalentFlow.Demo.Models;
using TalentFlow.Csharp.Models.Objects;

namespace TalentFlow.Demo.Converters
{
    public class ConvertToEmployeeUnavailabilities : IConverter<EmployeeUnavailability, EmployeeUnavailabilities>
    {
        public EmployeeUnavailabilities Convert(EmployeeUnavailability source)
        {
            return new EmployeeUnavailabilities()
            {
                EmployeeName = source.EmployeeName,
                Start = source.Start,
                End = source.End,
            };
        }

        public EmployeeUnavailabilities? ConvertNull(EmployeeUnavailability? source)
        {
            if (source == null) return null;
            return Convert(source);
        }

        public List<EmployeeUnavailabilities> Convert(List<EmployeeUnavailability> sourceList)
        {
            return sourceList?.Select(Convert).ToList() ?? [];
        }
    }
}
