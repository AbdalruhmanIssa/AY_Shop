using AYShop.BLL.Services.Classes;
using AYShop.BLL.Services.Interface;
using AYShop.DAL.Data;
using AYShop.DAL.Models;
using AYShop.DAL.Repositories.Classes;
using AYShop.DAL.Repositories.Interface;
using AYShop.DAL.Utilites;
using AYShop.PL.Uti;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Stripe;
using System.Text;

namespace AYShop.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
          

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


            builder.Services.AddOpenApi();
           
            var userPolicy = "";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: userPolicy, policy =>
                {
                    policy.AllowAnyOrigin();
                });
            });

            builder.Services.AddDbContext<ApplicationDBContext>(options =>
options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<ICheckOutService,CheckOutService>();
            builder.Services.AddScoped<IUserService,UserService>();
            builder.Services.AddScoped<IOrderService,OrderService>();
            builder.Services.AddScoped<ReportService>();

            builder.Services.AddScoped<IReviewService,BLL.Services.Classes.ReviewService>();
            builder.Services.AddScoped<IFileService, BLL.Services.Classes.FileService>();
            builder.Services.AddScoped < IProductService, BLL.Services.Classes.ProductService>();
            builder.Services.AddScoped < IProductRepository, ProductRepository>();
            builder.Services.AddScoped < IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<ICheckOutRepository, CheckOutRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddScoped < ICartRepository, CartRepository>();
            builder.Services.AddScoped < IUserRepository, UserRepository>();
            builder.Services.AddScoped<ISeedData, SeedData>();
            builder.Services.AddScoped<IAuthenticationService,AuthenticationService>();
            builder.Services.AddScoped<IEmailSender,Emailsetting>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
.AddEntityFrameworkStores<ApplicationDBContext>()
.AddDefaultTokenProviders();



            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;// X cockies T JWT
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; //returns 401 unauth
            })

                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = true,//duration of token
                            ValidateIssuerSigningKey = true,//signiture
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("jwtOptions")["SecretKey"]))//encrypyion
                        ,
                            RoleClaimType = "Role"
                        };
                    });
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }
          var scope=  app.Services.CreateScope();
         
            var objseeddata= scope.ServiceProvider.GetRequiredService<ISeedData>();

            await objseeddata.DataSeeding();
            await objseeddata.IdentityDataSeedingAsync();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCors(userPolicy);

            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
