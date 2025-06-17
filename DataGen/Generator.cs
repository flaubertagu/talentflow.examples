using TalentFlow.Demo.Models;

namespace TalentFlow.Demo.DataGen;
public static class Generator
{
    private static readonly Random _random = new();
    public static List<TaskCompleted> TasksCompleted { get; set; } = [];
    public static List<TaskDoTo> TasksToDo { get; set; } = [];
    public static List<TeamMember> TeamMembers { get; set; } = [];
    public static List<Skill> Skills { get; set; } = [];
    public static List<UserSkill> UserSkills { get; set; } = [];
    public static List<EmployeeUnavailability> Unavailabilities { get; set; } = [];

    private static readonly List<string> AvailableSkills =
    [
        "Python Programming", "C# Development", "Web API Design", "SQL Database Management", "Git Version Control",
        "Cloud Deployment", "Containerization with Docker", "Frontend Development with React", "Backend Architecture", "Unit Testing",
        "DevOps Automation", "RESTful Services", "Data Engineering", "Machine Learning", "Deep Learning",
        "Natural Language Processing", "Computer Vision", "Model Evaluation", "Prompt Engineering", "Data Annotation"
    ];

    private static readonly List<string> MemberNames =
    [
        "Jean Dupont", "Marie Martin", "Luc Lefèvre", "Sophie Bernard", "Pierre Moreau",
        "Nathalie Dubois", "Marc Simon", "Isabelle Girard", "David Robert", "Sandrine Lefebvre",
        "Jacques Petit", "Catherine Richard", "Michel Renaud", "Caroline Durand", "Alexandre Lemoine",
        "Chantal Morel", "Alain Fournier", "Brigitte Brun", "Philippe Leclerc", "Christine Blanc",
        "François Roux", "Valérie Perrot", "Hugo Vincent", "Émilie Charpentier", "Thierry Fontaine",
    ];

    public static readonly List<MyStrategy> Strategies =
    [
        new MyStrategy
        {
            ActivityArea = "Healthcare",
            Title = "Data-Driven Healthcare Optimization",
            Type = "Strategic_vision",
            Strategy = "Our vision is to become a leader in predictive healthcare solutions by leveraging advanced data engineering and machine learning models. We aim to create a platform that assists medical professionals in diagnosing diseases, predicting patient outcomes, and optimizing treatment plans using real-time data streams and AI insights.\n\nTo realize this, we focus on building a strong foundation in Data Engineering and Machine Learning, ensuring seamless handling of sensitive medical data, and implementing ethical AI algorithms for actionable insights.\n\nOur approach is to develop modular pipelines capable of ingesting, cleaning, and analyzing vast amounts of structured and unstructured health data, while adhering to strict security and compliance requirements.",
            StrategicSkills =
            [
                new StrategicSkill
                {
                    NameAI = "Medical Data Engineering",
                    Equivalent = "Data Engineering",
                    Complexity = "Complex",
                    ComplementarySkills = ["SQL Database Management", "Cloud Deployment", "DevOps Automation"]
                },
                new StrategicSkill
                {
                    NameAI = "Predictive Diagnosis with AI",
                    Equivalent = "Machine Learning",
                    Complexity = "Complex",
                    ComplementarySkills = ["Model Evaluation", "Deep Learning", "Data Annotation"]
                }
            ]
        },
        new MyStrategy
        {
            ActivityArea = "Smart Cities",
            Title = "Smart Infrastructure for Urban Mobility",
            Type = "Strategic_vision",
            Strategy = "We envision a connected and intelligent urban transport system powered by real-time data processing, cloud computing, and AI. Our strategy focuses on building backend architectures that collect and process data from IoT devices, with RESTful services enabling seamless interconnectivity across mobility platforms.\n\nWe aim to reduce traffic congestion and pollution while improving commuter experience through accurate, AI-driven predictive models and reactive control mechanisms.",
            StrategicSkills =
            [
                new StrategicSkill
                {
                    NameAI = "Cloud-based Infrastructure",
                    Equivalent = "Cloud Deployment",
                    Complexity = "Moderate",
                    ComplementarySkills = ["Containerization with Docker", "DevOps Automation", "Backend Architecture"]
                },
                new StrategicSkill
                {
                    NameAI = "Real-time API Orchestration",
                    Equivalent = "Web API Design",
                    Complexity = "Complex",
                    ComplementarySkills = ["RESTful Services", "C# Development", "Git Version Control"]
                }
            ]
        },
        new MyStrategy
        {
            ActivityArea = "Customer Support AI",
            Title = "Human-Like Conversational AI Assistants",
            Type = "Strategic_vision",
            Strategy = "Our goal is to design natural language AI agents that simulate human interaction for customer support, education, and accessibility services. The focus is on advancing Natural Language Processing and Prompt Engineering to build intelligent agents capable of handling nuanced, multilingual, and context-sensitive conversations.\n\nWe will prioritize the development of contextual understanding, memory-aware dialogues, and real-time adaptation to user intent, enhancing the user experience while reducing operational costs for enterprises.",
            StrategicSkills =
            [
                new StrategicSkill
                {
                    NameAI = "Conversational Language Modeling",
                    Equivalent = "Natural Language Processing",
                    Complexity = "Complex",
                    ComplementarySkills = ["Deep Learning", "Data Annotation", "Model Evaluation"]
                },
                new StrategicSkill
                {
                    NameAI = "Instruction Fine-tuning",
                    Equivalent = "Prompt Engineering",
                    Complexity = "Moderate",
                    ComplementarySkills = ["Python Programming", "Machine Learning"]
                }
            ]
        },
        new MyStrategy
        {
            ActivityArea = "Manufacturing",
            Title = "AI-Enhanced Industrial Quality Control",
            Type = "Strategic_vision",
            Strategy = "We aim to revolutionize quality assurance in industrial environments by integrating computer vision and automated model evaluation in production lines. Our strategy revolves around real-time image analysis and anomaly detection to identify defective parts with high accuracy.\n\nBy leveraging computer vision systems combined with backend automation, we seek to decrease error margins and downtime while increasing product consistency and traceability.",
            StrategicSkills =
            [
                new StrategicSkill
                {
                    NameAI = "Industrial Image Recognition",
                    Equivalent = "Computer Vision",
                    Complexity = "Moderate",
                    ComplementarySkills = ["Model Evaluation", "Deep Learning", "Data Annotation"]
                },
                new StrategicSkill
                {
                    NameAI = "Automated Model Validation",
                    Equivalent = "Model Evaluation",
                    Complexity = "Moderate",
                    ComplementarySkills = ["Unit Testing", "Machine Learning", "Backend Architecture"]
                }
            ]
        },
        new MyStrategy
        {
            ActivityArea = "Software Engineering",
            Title = "Developer-Centric DevOps Acceleration",
            Type = "Strategic_vision",
            Strategy = "To boost developer efficiency and delivery speed, our strategy focuses on enhancing DevOps pipelines with a developer-first mindset. This includes standardizing automation scripts, improving test coverage, and integrating containerized environments using Docker.\n\nWe strive for a frictionless CI/CD workflow where infrastructure becomes code and every deployment is safe, repeatable, and rollback-ready.",
            StrategicSkills =
            [
                new StrategicSkill
                {
                    NameAI = "Automated DevOps Pipelines",
                    Equivalent = "DevOps Automation",
                    Complexity = "Moderate",
                    ComplementarySkills = ["Unit Testing", "Git Version Control", "Cloud Deployment"]
                },
                new StrategicSkill
                {
                    NameAI = "Microservice Containerization",
                    Equivalent = "Containerization with Docker",
                    Complexity = "Moderate",
                    ComplementarySkills = ["Web API Design", "Backend Architecture", "Cloud Deployment"]
                }
            ]
        },
    ];

    private static readonly string[] ComplexityLevels = ["Basic", "Moderate", "Complex"];

    public static void GetRandomSkills()
    {
        Skills.Clear();
        UserSkills.Clear();
        foreach (var skillName in AvailableSkills)
        {
            UserSkills.Add(new UserSkill()
            {
                Name = skillName,
                Complexity = ComplexityLevels[_random.Next(ComplexityLevels.Count())],
            });
        }

        // Transform to Skill list without duplicates on Name property
        Skills = UserSkills
            .GroupBy(us => us.Name)
            .Select(g => new Skill
            {
                Name = g.Key,
                Complexity = g.First().Complexity // We take the complexity of the first element of the group
            })
            .ToList();
    }

    public static void GetList()
    {
        TeamMembers.Clear();
        foreach (var member in MemberNames)
        {
            string name = member;
            string email = $"{member.ToLower().Replace(" ", "_")}@company.com";
            TeamMembers.Add(new TeamMember()
            {
                Name = name,
                Email = email,
            });
        }
    }

    public static void GenerateEmployeeSkills()
    {
        GetRandomSkills();
        GetList();
        SkillAssigner.Assign();
    }

    public static void GenerateTaskCompleted()
    {
        GenerateEmployeeSkills();
        TaskCompletedGenerator.Generate();
    }

    public static void GenerateTaskToDo()
    {
        GenerateTaskCompleted();
        TaskToDoGenerator.Generate();
        UnavailabilityGenerator.Generate();
    }
}