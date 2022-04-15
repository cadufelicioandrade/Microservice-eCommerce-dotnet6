using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShoppingStore.CartAPI.Config;
using ShoppingStore.CartAPI.Model.Context;
using ShoppingStore.CartAPI.RabbitMQSender;
using ShoppingStore.CartAPI.Repository;
//using ShoppingStore.CarAPI.Model.Context;
//using ShoppingStore.CarAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuation = builder.Configuration;
var connetion = configuation["MySQLConnection:MySQLConnectionString"];

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
    options.UseMySql(connetion, new MySqlServerVersion(new Version(5, 7, 36)));
});

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();

builder.Services.AddHttpClient<ICouponRepository, CouponRepository>(s => 
                            s.BaseAddress = new Uri(configuation["ServiceUrls:CouponAPI"]));
builder.Services.AddSingleton<IRabbitMQMessageSender, RabbitMQMessageSender>();

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
