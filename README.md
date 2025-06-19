# TalentPro.Demo - Strategic Talent Management Platform

## 🚀 Overview

TalentPro is a comprehensive AI-powered talent management platform built with .NET 9 and MudBlazor, designed to optimize workforce performance through intelligent analytics, automated task allocation, and strategic planning tools. The platform empowers organizations to make data-driven decisions about their human resources while reducing administrative overhead and improving team productivity.


## ⚙️ Setup prerequisites

### 📜 appsettings.json
To obtain a developer key to configure the TalentFlow.Csharp.Core nuget, create an account at https://license.tiedapp.com/ and generate a 'Test' key
```bash
  "TalentFlowLicense": {
    "LicenseKey": "YOUR-LICENSE-KEY-HERE"
  },

  "AppSemanticKernel": {
    "ModelId": "gpt-4.1", //OR OTHER
    "ApiKey": "YOUR-API-KEY-HERE"
  },
```

### 🚗 Program.cs

#### ⚙️ Converters
Declare the converters you will use in your application to convert your classes into the format accepted by the TalentFlow.Csharp nuget suite

```bash
  services.AddScoped<IConverter<TaskCompleted, TaskFinished>, ConvertToTaskFinished>();
  services.AddScoped<IConverter<TaskDoTo, TaskAttribution>, ConvertToTaskAttribution>();
  ...
```

#### ⚙️ Services setup
Declare all services required for the proper functioning of:

##### 💻 TalentFlow.Csharp.Core
```bash
  services.AddSingleton<TaskAttributionManager>();
  services.AddSingleton<EmployeeManager>();
  services.AddSingleton<StrategyVisionManager>();
```

##### 💻 TalentFlow.Csharp.AI
```bash
  services.AddSingleton<AIManager>();
```

#### ⚙️ Services activation
After 'var app = builder.Build();' enable services for proper functioning of:

##### 💻 TalentFlow.Csharp.Core
```bash
  // Retrieve the configuration
  string? licenseKey = config["TalentFlowLicense:LicenseKey"];
  
  // Validate the key
  await LibraryConfiguration.InitializeAsync(licenseKey);
```

##### 💻 TalentFlow.Csharp.AI
```bash
  using (var scope = app.Services.CreateScope())
  {
      var aiManager = scope.ServiceProvider.GetRequiredService<AIManager>();
  
      // Retrieve the configuration
      string modelId = config["AppSemanticKernel:ModelId"]!;
      string apiKey = config["AppSemanticKernel:ApiKey"]!;
  
      // Method call
      await aiManager.SetSemanticConfig(modelId, apiKey); 
  }
```


## ✨ Key Features

### 📊 Employee Metrics & Analytics
- **Performance Scoring**: Calculate comprehensive employee scores based on customizable parameters and time periods
- **Competency Assessment**: Analyze technical skill mastery levels and identify training opportunities
- **Productivity Metrics**: Track deadline compliance, task recurrence patterns, and overall productivity indicators
- **Targeted Training**: Generate personalized training recommendations based on skill gaps and business needs
![](https://github.com/flaubertagu/talentflow.examples/tree/master/wwwroot/images/1-Employee-metrics.gif)

### 🎯 Smart Candidate Selection
- **Strategic Skill Prioritization**: Define and prioritize critical technical competencies aligned with business strategy
- **Top Performer Identification**: Automatically identify top X candidates based on specific skill requirements
- **Project-Specific Matching**: Match talent to projects beyond general scoring, focusing on specialized competencies
![](https://github.com/flaubertagu/talentflow.examples/tree/master/wwwroot/images/2-Perfect-candidate.gif)

### 🤖 Intelligent Task Attribution
- **Automated Distribution**: Smart task allocation based on availability, competencies, and workload balance
- **Burnout Prevention**: Intelligent workload management to prevent employee overload
- **Manual Override**: Flexible manual task reassignment capabilities when needed
- **Manager Efficiency**: Reduce management overhead by up to 40% through automation
![](https://github.com/flaubertagu/talentflow.examples/tree/master/wwwroot/images/3-Automatic-tsk-attribution.gif)

### 🎯 Strategic Planning & Analysis
- **Competency Gap Analysis**: Identify required technical skills for strategic initiatives and detect workforce gaps
- **Vision Alignment**: Analyze strategic projects against current team capabilities
- **Rapid Assessment**: Quickly evaluate skill requirements for new business domains and projects
![](https://github.com/flaubertagu/talentflow.examples/tree/master/wwwroot/images/4-Strategic-vision.gif)

### 📈 AI-Powered Action Planning
- **Success Probability Dashboard**: Real-time analysis of strategic initiative success likelihood
- **OpenAI Integration**: Generate comprehensive action plans and recommendations
- **Internal vs External Hiring**: Intelligent insights on whether to recruit externally or develop internal talent
- **Decision Optimization**: Reduce strategic meeting time and accelerate decision-making processes
![](https://github.com/flaubertagu/talentflow.examples/tree/master/wwwroot/images/5-Action-plan.gif)

### 📄 CV Analysis & Integration
- **Automated Skill Extraction**: Parse candidate resumes to identify technical competencies
- **Profile Generation**: Create comprehensive candidate profiles for comparison
- **Seamless Integration**: Connect external candidate analysis with internal workforce planning
![](https://github.com/flaubertagu/talentflow.examples/tree/master/wwwroot/images/6-CV-Skill-analytics.gif)

## 🛠️ Technology Stack

- **Backend**: .NET 9, C#
- **Frontend**: Blazor Server/WebAssembly with MudBlazor UI components
- **AI Integration**: OpenAI API for intelligent recommendations
- **Architecture**: Modern, scalable, and maintainable codebase

## 📈 Business Impact

- **85%** reduction in task assignment errors
- **60%** faster strategic decision-making
- **40%** improvement in overall team productivity
- **40%** reduction in manager administrative time

## 🎯 Target Users

- **HR Managers**: Comprehensive talent analytics and strategic workforce planning
- **Team Leaders**: Automated task distribution and team performance insights
- **C-Level Executives**: Strategic planning tools and success probability analysis
- **Project Managers**: Optimal resource allocation and competency matching

## 🔧 Core Modules

| Module | Purpose | Key Benefits |
|--------|---------|--------------|
| Employee Metrics | Performance analytics and scoring | Data-driven performance management |
| Candidate Selection | Strategic talent identification | Optimal team composition |
| Task Attribution | Intelligent workload distribution | Reduced errors, balanced workload |
| Strategic Planning | Competency requirement analysis | Proactive skill gap management |
| Action Planning | AI-powered strategic recommendations | Accelerated decision-making |
| CV Analysis | Automated candidate profiling | Streamlined recruitment process |

## 🚀 Getting Started

1. Clone the repository
2. Configure your OpenAI API keys
3. Set up your database connection
4. Run the application using .NET 9
5. Access the modern web interface to start optimizing your talent management

## 🎯 Use Cases

- **Project Staffing**: Automatically assign the best-fit employees to new projects
- **Skills Development**: Identify training needs and create targeted development programs
- **Strategic Planning**: Evaluate workforce readiness for business initiatives
- **Recruitment**: Compare internal candidates with external prospects
- **Performance Management**: Continuous monitoring and improvement of team performance

## 🏆 Why Choose TalentPro?

TalentPro transforms traditional HR processes into intelligent, data-driven operations. By combining advanced analytics with practical automation, organizations can optimize their most valuable asset—their people—while reducing administrative burden and improving strategic outcomes.
![](https://github.com/flaubertagu/talentflow.examples/tree/master/wwwroot/images/homepage-screenshot.jpeg)

---

*Built with modern web technologies and AI integration, TalentPro represents the future of intelligent talent management.*


## Authors

- [@flaubertagu](https://github.com/flaubertagu)


## Support

For support, email suppport@tiedapp.com

