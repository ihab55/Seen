using SeenBLL.Services;
using SeenCL.Repositories;
using SeenCL.Services;
using SeenDAL.Infrastructure;
using SeenDAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Senior Backend: Register DatabaseHelper as Singleton
builder.Services.AddSingleton<IDatabaseHelper, DatabaseHelper>();

// Register Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<ICoachApprovalRepository, CoachApprovalRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IProgramCommentRepository, ProgramCommentRepository>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<ISensorDataRepository, SensorDataRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
builder.Services.AddScoped<ITrainingProgramRepository, TrainingProgramRepository>();

// Register Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<ICoachApprovalService, CoachApprovalService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IProgramCommentService, ProgramCommentService>();
builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddScoped<ISensorDataService, SensorDataService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITeamMemberService, TeamMemberService>();
builder.Services.AddScoped<ITrainingProgramService, TrainingProgramService>();
builder.Services.AddScoped<ICoachService, CoachService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
