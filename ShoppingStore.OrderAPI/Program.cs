using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShoppingStore.OrderAPI.MessageConsumer;
using ShoppingStore.OrderAPI.Model.Context;
using ShoppingStore.OrderAPI.Repository;
//using ShoppingStore.CarAPI.Model.Context;
//using ShoppingStore.CarAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuation = builder.Configuration;
var connection = configuation["MySQLConnection:MySQLConnectionString"];

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShoppingStore.CartAPI", Version = "v1" });
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Enter 'Bearer' [space] and your token!",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

});
builder.Services.AddDbContext<MySQLContext>(options =>
{
    options.UseMySql(connection, new MySqlServerVersion(new Version(5, 7, 36)));
});

//tornano o context singleton para salver apenas uma vez a ordem de pagamento.
var build = new DbContextOptionsBuilder<MySQLContext>();
build.UseMySql(connection, new MySqlServerVersion(new Version(5, 7, 36)));
builder.Services.AddSingleton(new OrderRepository(build.Options));

builder.Services.AddHostedService<RabbitMQCheckoutConsumer>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:4435/";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "shopping_store");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//seguir a ordem se não dá erro
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication(); // app.UseAuthentication() ficar entre UseRouting e UseAuthorization
app.UseAuthorization();
app.MapControllers();

app.Run();
