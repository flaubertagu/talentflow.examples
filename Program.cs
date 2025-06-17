using TalentFlow.Demo.Components;
using TalentFlow.Demo.Converters;
using TalentFlow.Demo.Models;
using TalentFlow.Demo.Services;
using MudBlazor.Services;
using TalentFlow.Csharp.Core.Services;
using TalentFlow.Csharp.AI;
using TalentFlow.Csharp.Models.Objects;
using TalentFlow.Csharp.Core.Licensing;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var services = builder.Services;

// Add MudBlazor services
services.AddMudServices();

// Add services to the container.
services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Web app services
services.AddScoped<Snacks>();
services.AddScoped<CvService>();

// Add TalentFlow.Csharp services
services.AddSingleton<AIManager>();
services.AddSingleton<TaskAttributionManager>();
services.AddSingleton<EmployeeManager>();
services.AddSingleton<StrategyVisionManager>();

// Add converter to convert data in TalentFlow.Csharp classes
services.AddScoped<IConverter<TaskCompleted, TaskFinished>, ConvertToTaskFinished>();
services.AddScoped<IConverter<TaskDoTo, TaskAttribution>, ConvertToTaskAttribution>();
services.AddScoped<IConverter<TeamMember, Employee>, ConvertToEmployee>();
services.AddScoped<IConverter<EmployeeUnavailability, EmployeeUnavailabilities>, ConvertToEmployeeUnavailabilities>();
services.AddScoped<IConverter<UserSkill, EmployeeSkill>, ConvertToEmployeeSkill>();
services.AddScoped<IConverter<TaskSetting, AttributionSetting>, ConvertToAttributionSetting>();
services.AddScoped<IConverter<TaskAttributionSetting, TaskAttributionInput>, ConvertToTaskAttributionInput>();
services.AddScoped<IConverter<AIStrategicSkill, StrategicSkill>, ConvertToStrategicSkill>();
services.AddScoped<IConverter<TaskAttribution, TaskDoTo>, ConvertToTaskDoTo>();
services.AddScoped<IConverter<EmployeeSkill, UserSkill>, ConvertToUserSkill>();
services.AddScoped<IConverter<StrategicSkill, AIStrategicSkill>, ConvertToAIStrategicSkill>();
services.AddScoped<IConverter<Skill, AISkill>, ConvertToAISkill>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    #region For TalentFlow.Csharp.Core
    // Retrieve the configuration
    string? licenseKey = config["TalentFlowLicense:LicenseKey"];

    // Validate the key
    bool activated = await LibraryConfiguration.InitializeAsync(licenseKey);
    #endregion

    #region For TalentFlow.Csharp.AI
    var aiManager = scope.ServiceProvider.GetRequiredService<AIManager>();

    // Retrieve the configuration
    string modelId = config["AppSemanticKernel:ModelId"]!;
    string apiKey = config["AppSemanticKernel:ApiKey"]!;

    // Method call
    await aiManager.SetSemanticConfig(modelId, apiKey); 
    #endregion
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
