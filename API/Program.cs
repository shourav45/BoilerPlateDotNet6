using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Service;
using System.Text;


    var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapControllers();

app.Run();
