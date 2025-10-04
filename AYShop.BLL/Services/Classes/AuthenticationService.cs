using AYShop.BLL.Services.Interface;
using AYShop.DAL.DTO.Requests;
using AYShop.DAL.DTO.Responses;
using AYShop.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;


namespace AYShop.BLL.Services.Classes
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager,
            IConfiguration configuration,IEmailSender emailSender,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            this.configuration = configuration;
            this.emailSender = emailSender;
            this.signInManager = signInManager;
        }

        public async Task<UserResponse> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null)
            {
                throw new Exception("Invalid email or password");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, true);
            if (result.Succeeded)
            {
                return new UserResponse
                {
                    Token = await CreateTokenAsync(user)
                };
            }
            else if (result.IsLockedOut)
            {
                throw new Exception("Your account is locked");
            }
            else if (result.IsNotAllowed)
            {
                throw new Exception("Please confirm your email");
            }
            else
            {
                throw new Exception("Please confirm your email");
            }
        }

        public async Task<string> ConfirmEmail(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new Exception("user not found");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return "email confirmed successfully";
            }

            return "email confirmation failed";
        }


        public async Task<UserResponse> RegisterAsync(RegisterRequest registerRequest,HttpRequest request)
        {
            var user = new ApplicationUser()
            {
                FullName = registerRequest.FullName,
                Email = registerRequest.Email,
                PhoneNumber = registerRequest.PhoneNumber,
                UserName = registerRequest.UserName,
            };

            var Result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (Result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var escape=Uri.EscapeDataString(token);
                var emailUrl = $"{request.Scheme}://{request.Host}/api/identity/Account/ConfirmEmail?token={escape}&userId={user.Id}";
                await _userManager.AddToRoleAsync(user,"Customer");
                await emailSender.SendEmailAsync(user.Email, "welcome",
      $"<h1> Hello {user.UserName}</h1> " +
      $"<a href='{emailUrl}'> confirm </a>");

                return new UserResponse()
                {
                    Token = registerRequest.Email,
                };
            }
            else
            {
                throw new Exception($"{Result.Errors}");
            }
        }
        private async Task<string> CreateTokenAsync(ApplicationUser applicationUser)
        {
            var claims = new List<Claim>() { 
            new Claim("Name",applicationUser.UserName),
            new Claim("Email",applicationUser.Email),
            new Claim("Id",applicationUser.Id), 
           
            };
            var roles=await _userManager.GetRolesAsync(applicationUser);
            foreach (var role in roles) { claims.Add(new Claim("Role", role)); }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("jwtOptions")["SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(15),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> ForgotPassword(ForgetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null) throw new Exception("user not found");

            var random = new Random();
            var code = random.Next(1000, 9999).ToString();

            user.codeResetPassword = code;
            user.codeResetPasswordExpire = DateTime.UtcNow.AddMinutes(15);

            await _userManager.UpdateAsync(user);

            await emailSender.SendEmailAsync(
                request.Email,
                "reset password",
                $"<p>code is {code}</p>"
            );

            return true;
        }
        public async Task<bool> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null) throw new Exception("user not found");

            if (user.codeResetPassword != request.Code) return false;

            if (user.codeResetPasswordExpire < DateTime.UtcNow) return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            if (result.Succeeded)
            {
                await emailSender.SendEmailAsync(
                    request.Email,
                    "change password",
                    "<h1> your password is changed </h1>"
                );
            }

            return true;
        }

    }
}
