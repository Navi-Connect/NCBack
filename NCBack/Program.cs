using System.Reflection;
using System.Text;
using CorePush.Apple;
using CorePush.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NCBack.Data;
using NCBack.Models;
using NCBack.NotificationModels;
using NCBack.Services;
using NCBack.Spaces;
using SendGrid.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;



var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, config) =>
{
    /*ConfigureLogs();*/
    var connections = context.Configuration.GetConnectionString("SecondConnection");
    config.WriteTo.PostgreSQL(connections, "DbLoggerOptions", needAutoCreateTable: true)
        .MinimumLevel.Information()
        .WriteTo.Console();
});

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession(options =>  
{  
    options.IdleTimeout = TimeSpan.FromMinutes(10);  
    options.Cookie.HttpOnly = true;  
    options.Cookie.IsEssential = true;  
});  

builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache(); 

// URI for EventsSearch
builder.Services.AddSingleton<IUriService>(o =>
{
    var accessor = o.GetRequiredService<IHttpContextAccessor>();
    var request = accessor.HttpContext?.Request;
    var uri = string.Concat(request?.Scheme, "://", request?.Host.ToUriComponent());
    return new UriService(uri);
});




builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddHttpClient<FcmSender>();
builder.Services.AddHttpClient<ApnSender>();
builder.Services.AddControllers();
builder.Services.AddTransient<UploadFileService>();
builder.Services.AddTransient<PushSms>();
builder.Services.AddTransient<EmailService>();
builder.Services.AddTransient<PasswordGeneratorService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


// Configure strongly typed settings objects
var appSettingsSection = builder.Configuration.GetSection("FcmNotification");
builder.Services.Configure<FcmNotificationSetting>(appSettingsSection);

builder.Services.Configure<MobizonNotificationSMS>(builder.Configuration.GetSection("MobizonNotification"));
builder.Services.Configure<SpacesSettings>(builder.Configuration.GetSection("SpacesKeys"));

builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT" // Optional
    });

    c.OperationFilter<SecurityRequirementsOperationFilter>();


});
builder.Services.AddAutoMapper(typeof(Program).Assembly);



/*builder.Services.AddScoped<ICharacterService, CharacterService>();*/



builder.Services.AddScoped<IAuthRepository, AuthRepository>();




builder.Services.Configure<TokenSetting>(builder.Configuration.GetSection("TokenSetting"));

/*builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("TokenSetting").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });*/


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var tokenSettings = builder.Configuration.GetSection("TokenSetting")
            .Get<TokenSetting>();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = tokenSettings.Issuer,
            ValidateIssuer = true,
            
            ValidAudience = tokenSettings.Audience,
            ValidateAudience = true,
            
            ValidateIssuerSigningKey = true,
            
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(tokenSettings.SecretKey)),
            
            ClockSkew = TimeSpan.Zero
        };
    });
    

builder.Services.AddSendGrid(option =>
{
    option.ApiKey = builder.Configuration.GetSection("SendGrindEmailSettings")
        .GetValue<string>("APIkey");
});

builder.Services.AddHttpContextAccessor();


/*builder.Services.AddScoped<IWeaponService, WeaponService>();
builder.Services.AddScoped<IFightService, FightService>();*/

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}


app.UseSession();
//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

#region helperLogs
void ConfigureLogs()
{
    
    //GET the environment which the app is running on  
    var evn = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    // Get the configuration
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    //CREATE Logger
    if (evn != null)
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails() //Add details exception
            .WriteTo.Debug()
            .WriteTo.Console()
            .MinimumLevel.Information()
            
            /*.WriteTo.File("Logs/Example.txt",
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")*/
            .WriteTo.Elasticsearch(ConfigureEls(configuration, evn))
            .CreateLogger();
}

ElasticsearchSinkOptions ConfigureEls(IConfigurationRoot configuration, string env)
{
    return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
    {
      
        AutoRegisterTemplate = true,
        IndexFormat =
            $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower()}-{env.ToLower().Replace(".", "-")}-{DateTime.UtcNow:YYYY-MM}"
    };
    
}


#endregion


/*
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NCBack;
using NCBack.Data;
using NCBack.Helpers;
using NCBack.Interfaces;
using NCBack.Services;

var builder = WebApplication.CreateBuilder(args);

const string AllowAllHeadersPolicy = "AllowAllHeadersPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowAllHeadersPolicy,
        builder =>
        {
            builder.WithOrigins(" ")
                    .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionString")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = TokenHelper.Issuer,
                ValidAudience = TokenHelper.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(TokenHelper.Secret)),
                ClockSkew = TimeSpan.Zero
            };

        });

builder.Services.AddAuthorization();

builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IUserService, UserService>();
/*builder.Services.AddTransient<ITaskService, TaskService>();#1#

var app = builder.Build();
app.UseCors(AllowAllHeadersPolicy);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
*/













/*
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using NCBack.Data;
using NCBack.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<UploadFileService>();
builder.Services.AddDbContext<DataContext>();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NgOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
*/



/*using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NCBack.Data;
using NCBack.Models;
using NCBack.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<UploadFileService>();
builder.Services.AddDbContext<DataContext>();



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.WebHost.UseKestrel(options =>
{
    options.Limits.MaxRequestBodySize = Int64.MaxValue;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
//    app.UseHttpsRedirection();
}


//app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.UseStaticFiles();

app.MapControllers();

app.Run();*/