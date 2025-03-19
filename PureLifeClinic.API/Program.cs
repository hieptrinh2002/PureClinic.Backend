using Asp.Versioning;
using DinkToPdf;
using DinkToPdf.Contracts;
using FluentValidation;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PureLifeClinic.API.Extensions;
using PureLifeClinic.API.Helpers;
using PureLifeClinic.API.Middlewares;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Interfaces.IServices;
using PureLifeClinic.Infrastructure.Caching;
using PureLifeClinic.Infrastructure.Persistence.Data;
using PureLifeClinic.Infrastructure.SignalR.Hubs;
using Swashbuckle.AspNetCore.SwaggerGen;
using RecurringJobScheduler = PureLifeClinic.Infrastructure.BackgroundServices.Schedulers.RecurringJobScheduler;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PrimaryDbConnection"))); //.UseLazyLoadingProxies()

// Add hangfire 
builder.Services.AddHangfire(
    config => config.UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireDbConnection"))
);
builder.Services.AddHangfireServer();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ConnectionStrings"));

// Add caching services
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<MemoryCacheService>();
builder.Services.AddSingleton<RedisCacheService>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("RedisConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Redis connection string is missing or empty.");
    }
    return new RedisCacheService(connectionString);
});
builder.Services.AddSingleton<IFileValidator, FileValidator>(); 

// Add CacheServiceFactory
builder.Services.AddSingleton<ICacheServiceFactory, CacheServiceFactory>();

// add fluent validations
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

//builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);   

// Register ILogger service
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSeq(builder.Configuration.GetSection("SeqConfig"));
});
//Log.Logger = new LoggerConfiguration()
//    .ReadFrom.Configuration(builder.Configuration) // Đọc config từ appsettings.json
//    .Enrich.FromLogContext()
//    .WriteTo.Console() // Ghi log ra console
//    .WriteTo.Seq("http://localhost:5341") // Ghi log lên Seq
//    .CreateLogger();

//builder.Host.UseSerilog(); // Sử dụng Serilog thay vì logging mặc định

// Register Services
builder.Services.RegisterSecurityService(builder.Configuration);
builder.Services.RegisterService();
builder.Services.AddSignalR();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
// API Versioning
builder.Services
    .AddApiVersioning()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers(
    //options =>
    //{
    //    options.Filters.Add<ApiLoggingFilter>();
    //}
);

// Swagger Settings
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<SwaggerDefaultValues>();

    // Define Bearer token security scheme
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    // Add Bearer token as a requirement for all operations
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder.WithOrigins("http://127.0.0.1:3000/") // URL frontend
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials()
               .SetIsOriginAllowed(host => true);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Database seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        // Seed the database
        await ApplicationDbContextSeed.SeedAsync(services, loggerFactory);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DefaultModelsExpandDepth(-1);
        var descriptions = app.DescribeApiVersions();

        // Build a swagger endpoint for each discovered API version
        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
}

app.UseStaticFiles();
app.UseRouting(); // Add this line to configure routing

app.UseAuthentication();
app.UseMiddleware<PermissionsMiddleware>();
app.UseAuthorization();

app.UseHangfireDashboard();

using (var scope = app.Services.CreateScope())
{
    RecurringJobScheduler.ConfigureJobs(scope.ServiceProvider);
}

app.UseCors("AllowReactApp");

#region Custom Middleware
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseMiddleware<RequestResponseLoggingMiddleware>();
#endregion

app.MapControllers(); // API controllers
app.MapHub<NotificationHub>("/NotificationHub"); // SignalR hub
app.MapHub<UserAuthHub>("/UserAuthHub");

app.Run();

