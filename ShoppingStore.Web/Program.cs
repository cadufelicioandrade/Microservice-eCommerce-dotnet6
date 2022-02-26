using Microsoft.AspNetCore.Authentication;
using ShoppingStore.Web.Services;
using ShoppingStore.Web.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddHttpClient<IProductService, ProductService>(httpClient => 
                        httpClient.BaseAddress = new Uri(configuration["ServiceUrls:ProductAPI"]));

builder.Services.AddHttpClient<ICartService, CartService>(httpClient =>
                        httpClient.BaseAddress = new Uri(configuration["ServiceUrls:CartAPI"]));

builder.Services.AddHttpClient<ICouponService, CouponService>(httpClient =>
                        httpClient.BaseAddress = new Uri(configuration["ServiceUrls:CouponAPI"]));

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";

}).AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
  .AddOpenIdConnect("oidc", options  =>
  {
      options.Authority = configuration["ServiceUrls:IdentityServer"];
      options.GetClaimsFromUserInfoEndpoint = true;
      options.ClientId = "shopping_store";
      options.ClientSecret = "my_super_secret";
      options.ResponseType = "code";
      options.ClaimActions.MapJsonKey("role", "role", "role");
      options.ClaimActions.MapJsonKey("sub", "sub", "sub");
      options.TokenValidationParameters.NameClaimType = "name";
      options.TokenValidationParameters.RoleClaimType = "role";
      options.Scope.Add("shopping_store");
      options.SaveTokens = true;

  });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
//following the order
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
