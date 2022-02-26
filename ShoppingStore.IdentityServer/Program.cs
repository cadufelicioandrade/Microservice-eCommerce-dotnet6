
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingStore.IdentityServer.Configuration;
using ShoppingStore.IdentityServer.Initializer;
using ShoppingStore.IdentityServer.Model.Context;
using ShoppingStore.IdentityServer.Services;

var builder = WebApplication.CreateBuilder(args);


//Configurando banco de dados
IConfiguration configuation = builder.Configuration;
var connetion = configuation["MySQLConnection:MySQLConnectionString"];
builder.Services.AddDbContext<MySQLContext>(options =>
{
    options.UseMySql(connetion, new MySqlServerVersion(new Version(5, 7, 36)));
});

//configuração identity

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MySQLContext>()
    .AddDefaultTokenProviders();

var builderIdentity = builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true;
}).AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
  .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
  .AddInMemoryClients(IdentityConfiguration.Clients)
  .AddAspNetIdentity<ApplicationUser>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IProfileService, ProfileService>();

builderIdentity.AddDeveloperSigningCredential();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

//seguir a ordem se não dá erro
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

var scope = app.Services.CreateScope();
IDbInitializer initializer = scope.ServiceProvider.GetService<IDbInitializer>();

//app.Services.GetRequiredService<DbInitializer>();

initializer.Initializer();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
