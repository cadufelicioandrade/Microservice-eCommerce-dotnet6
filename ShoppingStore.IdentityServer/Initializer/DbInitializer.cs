using IdentityModel;
using Microsoft.AspNetCore.Identity;
using ShoppingStore.IdentityServer.Configuration;
using ShoppingStore.IdentityServer.Model.Context;
using System.Security.Claims;

namespace ShoppingStore.IdentityServer.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly MySQLContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole> _role;

        public DbInitializer(MySQLContext context,
            UserManager<ApplicationUser> user,
            RoleManager<IdentityRole> role)
        {
            _context = context;
            _user = user;
            _role = role;
        }

        public void Initializer()
        {
            if (_role.FindByIdAsync(IdentityConfiguration.Admin).Result != null) return;

            //Cadastrando as roles
            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Admin)).GetAwaiter().GetResult();
            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Client)).GetAwaiter().GetResult();

            ApplicationUser admin = new ApplicationUser()
            {
                UserName = "admin.admin",
                Email = "admin-admin@admin.com",
                EmailConfirmed = true,
                PhoneNumber = "+55 11 12233-4455",
                FirstName = "admin",
                LastName = "admin"
            };

            //Cadastrando o usuário.
            _user.CreateAsync(admin, "Admin@123").GetAwaiter().GetResult();
            //Vinculando a role de admin ao usuário admin
            _user.AddToRoleAsync(admin, IdentityConfiguration.Admin).GetAwaiter().GetResult();

            //add as claims do user admin
            var adminClaims = _user.AddClaimsAsync(admin, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin)

            }).Result;

            ApplicationUser client = new ApplicationUser()
            {
                UserName = "Client",
                Email = "client-client@client.com",
                EmailConfirmed = true,
                PhoneNumber = "+55 11 12233-4455",
                FirstName = "client",
                LastName = "client"
            };

            //Cadastrando o usuário.
            _user.CreateAsync(client, "Client@123").GetAwaiter().GetResult();
            //Vinculando a role de client ao usuário client
            _user.AddToRoleAsync(client, IdentityConfiguration.Client).GetAwaiter().GetResult();

            //add as claims do user client
            var clientClaims = _user.AddClaimsAsync(client, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                new Claim(JwtClaimTypes.GivenName, client.FirstName),
                new Claim(JwtClaimTypes.FamilyName, client.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client)

            }).Result;
        }
    }
}
