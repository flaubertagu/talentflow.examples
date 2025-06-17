using TalentFlow.Demo.Models;

namespace TalentFlow.Demo.DataGen;
public static class UnavailabilityGenerator
{
    private static readonly Random _random = new();
    private const double MAX_CONCURRENT_UNAVAILABILITY_PERCENTAGE = 0.15; // 15%
    private const double MAX_INDIVIDUAL_UNAVAILABILITY_PERCENTAGE = 0.10; // 10%
    private const int MIN_UNAVAILABILITY_DAYS = 1;
    private const int MAX_UNAVAILABILITY_DAYS = 5;

    public static void Generate()
    {
        Generator.Unavailabilities.Clear();
        var teamMembers = Generator.TeamMembers;
        var tasks = Generator.TasksToDo;
        if (!teamMembers.Any() || !tasks.Any()) 
            return;

        // Determine the overall period
        var minStartDate = tasks.Min(t => t.StartDate);
        var maxEndDate = tasks.Max(t => t.EndDate);
        var totalPeriodDays = (maxEndDate - minStartDate).Days + 1;

        var unavailabilities = new List<EmployeeUnavailability>();
        var maxConcurrentEmployees = Math.Max(1, (int)(teamMembers.Count * MAX_CONCURRENT_UNAVAILABILITY_PERCENTAGE));
        var maxIndividualDays = Math.Max(1, (int)(totalPeriodDays * MAX_INDIVIDUAL_UNAVAILABILITY_PERCENTAGE));

        // Tracker for each employee how many days of unavailability he already has
        var employeeUnavailabilityDays = teamMembers.ToDictionary(tm => tm.Name, tm => 0);

        // Generate unavailability
        var attempts = 0;
        var maxAttempts = teamMembers.Count * 10; // Limit to avoid an infinite loop

        while (attempts < maxAttempts)
        {
            attempts++;

            // Select a random employee who may still have unavailability
            var availableEmployees = teamMembers
                .Where(tm => employeeUnavailabilityDays[tm.Name] < maxIndividualDays)
                .ToList();

            if (!availableEmployees.Any())
                break; // More employees available for unavailability

            var selectedEmployee = availableEmployees[_random.Next(availableEmployees.Count)];

            // Generate a random downtime period
            var unavailabilityStart = GenerateRandomDate(minStartDate, maxEndDate.AddDays(-MIN_UNAVAILABILITY_DAYS));
            var remainingDaysForEmployee = maxIndividualDays - employeeUnavailabilityDays[selectedEmployee.Name];
            var maxDaysForThisUnavailability = Math.Min(MAX_UNAVAILABILITY_DAYS, remainingDaysForEmployee);

            if (maxDaysForThisUnavailability < MIN_UNAVAILABILITY_DAYS)
                continue;

            var unavailabilityDuration = _random.Next(MIN_UNAVAILABILITY_DAYS, maxDaysForThisUnavailability + 1);
            var unavailabilityEnd = unavailabilityStart.AddDays(unavailabilityDuration - 1);

            // Check that the period does not exceed the overall period
            if (unavailabilityEnd > maxEndDate)
                unavailabilityEnd = maxEndDate;

            // Check the competition constraint (max 15% of employees at the same time)
            if (!IsWithinConcurrencyLimits(unavailabilities, unavailabilityStart, unavailabilityEnd, maxConcurrentEmployees))
                continue;

            // Create unavailability
            var newUnavailability = new EmployeeUnavailability
            {
                EmployeeName = selectedEmployee.Name,
                Start = unavailabilityStart,
                End = unavailabilityEnd
            };

            unavailabilities.Add(newUnavailability);
            employeeUnavailabilityDays[selectedEmployee.Name] += (unavailabilityEnd - unavailabilityStart).Days + 1;

            // Check if we can continue to generate unavailability
            if (!CanGenerateMoreUnavailabilities(teamMembers, employeeUnavailabilityDays, maxIndividualDays))
                break;
        }

        Generator.Unavailabilities = unavailabilities.OrderBy(u => u.Start).ThenBy(u => u.EmployeeName).ToList();
    }

    private static DateTime GenerateRandomDate(DateTime minDate, DateTime maxDate)
    {
        var range = (maxDate - minDate).Days;
        if (range <= 0) return minDate;

        var randomDays = _random.Next(0, range + 1);
        return minDate.AddDays(randomDays);
    }

    private static bool IsWithinConcurrencyLimits(
        List<EmployeeUnavailability> existingUnavailabilities,
        DateTime newStart,
        DateTime newEnd,
        int maxConcurrentEmployees)
    {
        // Check every day of the new unavailability period
        for (var date = newStart; date <= newEnd; date = date.AddDays(1))
        {
            var concurrentCount = existingUnavailabilities
                .Count(u => date >= u.Start && date <= u.End);

            if (concurrentCount >= maxConcurrentEmployees)
                return false;
        }

        return true;
    }

    private static bool CanGenerateMoreUnavailabilities(
        List<TeamMember> teamMembers,
        Dictionary<string, int> employeeUnavailabilityDays,
        int maxIndividualDays)
    {
        return teamMembers.Any(tm => employeeUnavailabilityDays[tm.Name] < maxIndividualDays);
    }

    public static void PrintUnavailabilityReport(
        List<EmployeeUnavailability> unavailabilities,
        List<TeamMember> teamMembers,
        List<TaskDoTo> tasks)
    {
        if (!tasks.Any())
        {
            Console.WriteLine("Aucune tâche fournie.");
            return;
        }

        var minStartDate = tasks.Min(t => t.StartDate);
        var maxEndDate = tasks.Max(t => t.EndDate);
        var totalPeriodDays = (maxEndDate - minStartDate).Days + 1;

        Console.WriteLine("=== UNAVAILABILITY REPORT ===");
        Console.WriteLine($"Period: {minStartDate:dd/MM/yyyy} - {maxEndDate:dd/MM/yyyy} ({totalPeriodDays} days)");
        Console.WriteLine($"Number of employees: {teamMembers.Count}");
        Console.WriteLine($"Number of unavailabilities generated: {unavailabilities.Count}");
        Console.WriteLine();

        // Statistics by employee
        Console.WriteLine("--- Statistics by employee ---");
        foreach (var employee in teamMembers.OrderBy(tm => tm.Name))
        {
            var employeeUnavailabilities = unavailabilities.Where(u => u.EmployeeName == employee.Name).ToList();
            var totalDays = employeeUnavailabilities.Sum(u => (u.End - u.Start).Days + 1);
            var percentage = totalPeriodDays > 0 ? (double)totalDays / totalPeriodDays * 100 : 0;

            Console.WriteLine($"{employee.Name}: {employeeUnavailabilities.Count} period(s), {totalDays} day(s) ({percentage:F1}%)");
        }

        Console.WriteLine();

        // Detailed list of unavailability
        Console.WriteLine("--- Details of unavailability ---");
        foreach (var unavailability in unavailabilities)
        {
            var duration = (unavailability.End - unavailability.Start).Days + 1;
            Console.WriteLine($"{unavailability.EmployeeName}: {unavailability.Start:dd/MM/yyyy} - {unavailability.End:dd/MM/yyyy} ({duration} day(s))");
        }
    }
}
