using API.Models.Domains;
using API.Models.DTOS;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly ITokenService tokenService;

        public AuthService(UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IConfiguration configuration,
            ITokenService tokenService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.tokenService = tokenService;
        }

        public async Task<object> Login(LoginVM model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            var password = await userManager.CheckPasswordAsync(user, model.Password);
            if (user != null && password)
            {
                string token = await tokenService.CreateAccessToken(user);

                return (new
                {
                    EM = "login successfully",
                    EC = 0,
                    DT = token
                });
            }

            return (new
            {
                EM = "email or password not right",
                EC = 1,
                DT = ""
            });
        }


        public async Task<object?> Register(RegisterVM model)
        {
            var check = await userManager.FindByEmailAsync(model.Email);
            if (check == null)
            {
                var user = new User
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    ProfilePic = "",
                };

                var result = await userManager.CreateAsync(user, model.Password);


                if (result.Succeeded)
                {
                    if(! await roleManager.RoleExistsAsync(RoleVM.Guest))
                    {
                        await roleManager.CreateAsync(new IdentityRole(RoleVM.Guest));

                    }

                    await userManager.AddToRoleAsync(user, RoleVM.Guest);

                    return (new {
                        EM = "register successfully",
                        EC = 0,
                        DT = ""
                    });
                }

                return null;
            }

            return (new
            {
                EM = "email have exits",
                EC = 1,
                DT = ""
            });
        }
    }
}
