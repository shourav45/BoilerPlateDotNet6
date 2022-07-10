using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service;
using System.Text;


    var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//SWAGGER DOC LINK
//https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio
//https://github.com/domaindrivendev/Swashbuckle.AspNetCore
//REGULAR SWAGGER
builder.Services.AddSwaggerGen(options =>
{
    //FOR IGNORING SAME API WITH DIFFERENT VERSION
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    //FOR BEARER TOKEN AUTHORIZATION IN SWAGGER
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with 'Bearer' into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme{
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
});


//SERVICES REGISTER
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TodoService>();

//JWT TOKEN AUTHENTICATION
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
         .AddJwtBearer(options =>
         {
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = true,
                 ValidateAudience = true,
                 ValidateIssuerSigningKey = true,
                 ValidAudience = builder.Configuration["Jwt:Audience"],
                 ValidIssuer = builder.Configuration["Jwt:Issuer"],
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
             };

             options.Events = new JwtBearerEvents
             {
                 OnAuthenticationFailed = context =>
                 {
                     if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                     {
                         context.Response.Headers.Add("Token-Expired", "true");
                     }
                     return Task.CompletedTask;
                 }
             };
         });



//API RATE LIMITING
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests = false;
    options.HttpStatusCode = 429;
    options.RealIpHeader = "X-Real-IP";
    options.ClientIdHeader = "X-ClientId";
    options.GeneralRules = new List<RateLimitRule>
        {
            new RateLimitRule
            {
                Endpoint = "GET:/get/to/do/item/list",
                Period = "10s",
                Limit = 2,
            }
        };
});

builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();


// ADDING API VERSIONING
builder.Services.AddApiVersioning(options =>
{
    //DEFAULT TAKING VERSION 1.O-- IF CONTROLLER IS NOT MAPPED THEN NO NEED TO MENSION VERSION IN THE REQUEST
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader =
    ApiVersionReader.Combine(
        //REQUEST HEADER KEY WITH VERSION
       new HeaderApiVersionReader("X-Api-Version"),
       //OR PUT API-VERSION AFTER REQUEST URL WITH ? CHARACTER
       new QueryStringApiVersionReader("api-version"));
    //IF A VERSION IS MAPPED THEN WITHOUT MENSION VERSION RETURN UNSUPPORTEDAPI VERSION ERROR
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//TOKEN AUTHENTICATION
app.UseAuthentication();
app.UseAuthorization();
//API RATE LIMIT
app.UseIpRateLimiting();

app.MapControllers();
//Enable Static File Middleware
app.UseStaticFiles();
app.Run();
