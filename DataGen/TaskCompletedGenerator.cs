using TalentFlow.Demo.Models;

namespace TalentFlow.Demo.DataGen
{
    public class TaskCompletedGenerator
    {
        private static readonly Random _random = new();

        public static void Generate()
        {
            Generator.TasksCompleted.Clear();
            var allSkills = Generator.UserSkills;
            var allEmployees = Generator.TeamMembers;
            var tasks = new List<TaskCompleted>();

            // List of 50 tasks with titles, descriptions and skills used
            var predefinedTasks = new List<(string TaskName, string Description, List<string> RequiredSkills)>
            {
                ("Build REST API", "Develop and test a RESTful service.", ["Web API Design", "C# Development", "Unit Testing"]),
                ("Data Cleaning Pipeline", "Set up a data cleaning pipeline using Python.", ["Python Programming", "Data Engineering"]),
                ("Dockerize ML App", "Containerize a machine learning application.", ["Machine Learning", "Docker"]),
                ("React Frontend Dev", "Create frontend with React.", ["Frontend Development with React"]),
                ("CI/CD Integration", "Set up continuous integration pipeline.", ["DevOps Automation"]),
                ("Build SQL Schema", "Design SQL schema for user data.", ["SQL Database Management"]),
                ("Git Workflow", "Establish Git flow for team.", ["Git Version Control"]),
                ("Cloud Deployment", "Deploy app to cloud environment.", ["Cloud Deployment"]),
                ("Prompt Tuning", "Optimize prompts for language model.", ["Prompt Engineering"]),
                ("Vision Module", "Implement object detection pipeline.", ["Computer Vision"]),
                ("API Authentication", "Add OAuth2 to web API.", ["Web API Design", "Backend Architecture"]),
                ("Test Suite Dev", "Create automated unit tests.", ["Unit Testing", "C# Development"]),
                ("SQL Data Migration", "Migrate old schema to new.", ["SQL Database Management"]),
                ("Language Model Eval", "Evaluate large language model outputs.", ["Model Evaluation"]),
                ("Frontend Refactor", "Update UI to latest standards.", ["Frontend Development with React"]),
                ("Build DevOps Pipeline", "Automate deployment with DevOps tools.", ["DevOps Automation", "Cloud Deployment"]),
                ("Setup Docker Compose", "Manage multi-container app.", ["Containerization with Docker"]),
                ("Deep Learning Tuning", "Train a deep learning model.", ["Deep Learning"]),
                ("Semantic Search", "Add semantic search to platform.", ["Natural Language Processing"]),
                ("Annotate Dataset", "Label dataset for image classification.", ["Data Annotation"]),
                ("Microservice Design", "Create microservices for modules.", ["Backend Architecture"]),
                ("C# Refactoring", "Refactor legacy C# code.", ["C# Development"]),
                ("Setup Git Hooks", "Enforce coding standards.", ["Git Version Control"]),
                ("Chatbot Intent Parsing", "Train chatbot on intents.", ["Natural Language Processing"]),
                ("Document REST APIs", "Add Swagger documentation.", ["RESTful Services"]),
                ("Time Series Forecasting", "Forecast trends using ML.", ["Machine Learning"]),
                ("Deploy ML Model", "Push model to production.", ["Cloud Deployment", "Model Evaluation"]),
                ("Container Healthcheck", "Add health monitoring.", ["Containerization with Docker"]),
                ("Feature Store Setup", "Centralize ML features.", ["Data Engineering"]),
                ("UI Theming", "Style components.", ["Frontend Development with React"]),
                ("NLP Model Training", "Fine-tune language model.", ["Natural Language Processing"]),
                ("Database Indexing", "Optimize SQL queries.", ["SQL Database Management"]),
                ("ML Model Metrics", "Track model accuracy.", ["Model Evaluation"]),
                ("Unit Test Refactor", "Improve coverage.", ["Unit Testing"]),
                ("Model Packaging", "Prepare model for inference.", ["Machine Learning"]),
                ("Error Logging", "Implement global error handling.", ["Backend Architecture"]),
                ("Container Registry Setup", "Push Docker images.", ["Containerization with Docker"]),
                ("OAuth2 Login", "Enable secure login.", ["Web API Design"]),
                ("Data Label QA", "Quality control on labels.", ["Data Annotation"]),
                ("Prompt Chaining", "Link multiple prompts.", ["Prompt Engineering"]),
                ("CI for ML", "Integrate ML into CI/CD.", ["DevOps Automation"]),
                ("Project Dashboard", "Track project metrics.", ["Frontend Development with React"]),
                ("Hybrid Cloud Setup", "Deploy across clouds.", ["Cloud Deployment"]),
                ("Syntax Parser", "Build NLP parser.", ["Natural Language Processing"]),
                ("SQL Report Automation", "Automate reporting.", ["SQL Database Management"]),
                ("Gitlab CI", "Set Gitlab pipelines.", ["DevOps Automation"]),
                ("Docker Secrets", "Handle secrets securely.", ["Containerization with Docker"]),
                ("API Gateway", "Configure request routing.", ["RESTful Services"]),
                ("Language Detection", "Detect language of text.", ["Natural Language Processing"]),
                ("Debug ML Workflow", "Troubleshoot training pipeline.", ["Machine Learning"]),
            };

            var currentDate = DateTime.Today.AddDays(-5);

            int id = predefinedTasks.Count;
            foreach (var (taskName, description, requiredSkillNames) in predefinedTasks)
            {
                var requiredSkills = allSkills.Where(s => requiredSkillNames.Contains(s.Name)).ToList();

                var candidates = allEmployees.Where(e => e.UserSkills.Count(s => requiredSkillNames.Contains(s.Name)) >= requiredSkills.Count / 2.0).ToList();
                if (!candidates.Any())
                    continue;

                var assignedEmployee = candidates[_random.Next(candidates.Count)];

                var start = currentDate;
                var end = start.AddDays(_random.Next(3, 5));
                var delivery = end.AddDays(_random.Next(-2, 2));

                tasks.Add(new TaskCompleted
                {
                    Id = id--,
                    TaskName = taskName,
                    TaskDescription = description,
                    AssignedTo = assignedEmployee.Name,
                    TaskSkills = requiredSkills.Select(x => new Skill() { Name = x.Name, Complexity = x.Complexity}).ToList(),
                    StartDate = start,
                    EndDate = end,
                    DeliveryDate = delivery,
                    Occurence = _random.Next(1, 3),
                });

                currentDate = currentDate.AddDays(-_random.Next(3, 5));
            }

            Generator.TasksCompleted = tasks;

            //Updates the LastUsed value in the team members list
            var teamMembers = Generator.TeamMembers;
            var tasksFiltered = tasks.OrderBy(x => x.DeliveryDate).ToList();
            foreach (var task in tasksFiltered)
            {
                var member = teamMembers.FirstOrDefault(x => x.Name == task.AssignedTo);
                if (member == null) continue;
                foreach (var skill in task.TaskSkills)
                {
                    var userSkill = member.UserSkills.FirstOrDefault(x => x.Name == skill.Name);
                    if (userSkill == null) continue;

                }
            }

            foreach (var member in teamMembers)
            {
                var memberTasks = tasksFiltered.Where(x => x.AssignedTo == member.Name).ToList();
                foreach (var task in memberTasks)
                {
                    foreach (var skill in task.TaskSkills)
                    {
                        var userSkill = member.UserSkills.FirstOrDefault(x => x.Name == skill.Name);
                        if (userSkill == null) continue;
                        if (task.DeliveryDate < userSkill.LastUse) continue;
                        userSkill.LastUse = task.DeliveryDate;
                    }
                }
            }
            Generator.TeamMembers = teamMembers;
        }
    }
}
