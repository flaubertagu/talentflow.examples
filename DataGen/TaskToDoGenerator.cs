using TalentFlow.Demo.Models;

namespace TalentFlow.Demo.DataGen
{
    public static class TaskToDoGenerator
    {
        private static readonly Random _random = new();
        public static void Generate()
        {
            Generator.TasksToDo.Clear();
            var tasks = new List<TaskDoTo>();
            var skillList = Generator.UserSkills.ToList();

            DateTime currentStart = DateTime.Today.AddDays(_random.Next(0, 3));

            for (int i = 1; i <= 50; i++)
            {
                int skillCount = _random.Next(3, 7);
                var taskSkills = skillList.OrderBy(_ => _random.Next()).Take(skillCount).ToList();

                string taskName = $"Task {i}: {_taskTitles[_random.Next(_taskTitles.Count)]}";
                string taskDesc = $"This task involves: {string.Join(", ", taskSkills.Select(s => s.Name))}.";

                DateTime endDate = currentStart.AddDays(_random.Next(2, 6));

                tasks.Add(new TaskDoTo
                {
                    Id = i,
                    TaskName = taskName,
                    TaskDescription = taskDesc,
                    StartDate = currentStart,
                    EndDate = endDate,
                    UserSkills = taskSkills,
                });

                currentStart = currentStart.AddDays(_random.Next(0, 3));
            }

            Generator.TasksToDo = tasks;
        }

        private static readonly List<string> _taskTitles =
        [
            "Implement REST API", 
            "Develop Machine Learning Model", 
            "Refactor Legacy Code",
            "Optimize SQL Queries", 
            "Deploy to Cloud", 
            "Create Docker Containers",
            "Build CI/CD Pipeline", 
            "Conduct Unit Testing", 
            "Integrate Frontend with Backend",
            "Analyze Image Dataset", 
            "Tune Hyperparameters", 
            "Label Data for NLP",
            "Design Database Schema", 
            "Implement Role-Based Auth", 
            "Build Interactive Dashboard",
            "Train Deep Learning Network", 
            "Generate Prompt Templates", "Evaluate Model Accuracy",
            "Automate Data Ingestion", 
            "Enhance React UI",
            "React Frontend Dev",
            "CI/CD Integration",
            "Build SQL Schema",
            "Git Workflow",
            "Cloud Deployment",
            "Prompt Tuning",
            "Setup Docker Compose",
            "Microservice Design",
            "C# Refactoring",
            "Setup Git Hooks",
            "Chatbot Intent Parsing",
            "Unit Test Refactor",
            "Model Packaging",
            "Error Logging",
            "Container Registry Setup",
            "Hybrid Cloud Setup",
            "Syntax Parser",
            "SQL Report Automation",
            "Gitlab CI",
            "Docker Secrets",
            "API Gateway",
            "Language Detection",
            "Debug ML Workflow",
        ];
    }
}
