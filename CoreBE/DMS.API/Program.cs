using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NLog;
using DMS.API.AppCode.Extensions;
using NLog.Extensions.Logging;
using DMS.BUSINESS;
using DMS.API.Middleware;
using DMS.BUSINESS.Services.HUB;
using Microsoft.Extensions.FileProviders;
using Minio;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var logger = LogManager.Setup()
    .LoadConfiguration(new NLogLoggingConfiguration(config.GetSection("NLog")))
    .GetCurrentClassLogger();

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddDIServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMvc();
builder.Services.AddMemoryCache();

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

builder.Services.AddSingleton<IMinioClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new MinioClient()
        .WithEndpoint(configuration["Minio:Endpoint"], int.Parse(configuration["Minio:Port"]))
        .WithCredentials(configuration["Minio:AccessKey"], configuration["Minio:SecretKey"])
        .WithSSL(bool.Parse(configuration["Minio:UseSSL"]))
        .Build();
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("V1", new OpenApiInfo
    {
        Version = "V1",
        Title = "WebAPI",
        Description = "<a href='/log' target='_blank'>Bấm vào đây để xem log file</a>",
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });

    options.MapType<TimeSpan>(() => new OpenApiSchema
    {
        Type = "string",
        Example = new OpenApiString("00:00:00")
    });
});

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["JWT:Issuer"],
        ValidAudience = config["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"])),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
     builder =>
     {
         builder.SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("Accept-Ranges", "Content-Range", "Content-Length", "Content-Disposition");
     }));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/V1/swagger.json", "PROJECT WebAPI");
});

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ActionLoggingMiddleware>();
app.MapControllers();

app.MapHub<RefreshServiceHub>("/Refresh");

app.MapHub<NotificationHub>("/NotificationHub");

TransferObjectExtension.SetHttpContextAccessor(app.Services.GetRequiredService<IHttpContextAccessor>());
app.EnableRequestBodyRewind();

app.Run();



