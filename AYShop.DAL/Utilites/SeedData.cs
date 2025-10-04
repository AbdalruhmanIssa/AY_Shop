using AYShop.DAL.Data;
using AYShop.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYShop.DAL.Utilites
{
    public class SeedData : ISeedData
    {
       
        private readonly ApplicationDBContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public SeedData(
            ApplicationDBContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task DataSeeding()
        {
            if ((await _context.Database.GetPendingMigrationsAsync()).Any()) {await _context.Database.MigrateAsync(); }
            if (! await _context.Categories.AnyAsync())
            {
              await  _context.Categories.AddRangeAsync(
                    new Category { Name = "Clothes" },
                    new Category { Name = "Mobiles" }
                );
            }

            if (!await _context.Brands.AnyAsync())
            {
                await _context.Brands.AddRangeAsync(
                    new Brand { Name = "Samsung", MainImage = "brands/samsung.png" },
                    new Brand { Name = "Apple", MainImage = "brands/apple.png" },
                    new Brand { Name = "Nike", MainImage = "brands/nike.png" }
                );
            }
            await _context.SaveChangesAsync();


        }
        public async Task IdentityDataSeedingAsync()
        {
            if (!await _roleManager.Roles.AnyAsync())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole("Customer"));
            }

            if (!await _userManager.Users.AnyAsync())
            {
                var user1 = new ApplicationUser()
                {
                    Email = "tariq@gmail.com",
                    FullName = "tariq shreem",
                    PhoneNumber = "0592100103",
                    UserName = "tshreem",
                    EmailConfirmed = true,
                };

                var user2 = new ApplicationUser()
                {
                    Email = "example@gmail.com",
                    FullName = "example user",
                    PhoneNumber = "0592100104",
                    UserName = "euser",
                    EmailConfirmed = true,
                };

                var user3 = new ApplicationUser()
                {
                    Email = "aboody9068@gmail.com",
                    FullName = "abood issa",
                    PhoneNumber = "0592100105",
                    UserName = "ashreem",
                    EmailConfirmed = true,
                };

                await _userManager.CreateAsync(user1, "Pass@1212");
                await _userManager.CreateAsync(user2, "Pass@1212");
                await _userManager.CreateAsync(user3, "Pass@1212");

                await _userManager.AddToRoleAsync(user1, "Admin");
                await _userManager.AddToRoleAsync(user2, "Admin");
                await _userManager.AddToRoleAsync(user3, "Admin");
                await _context.SaveChangesAsync();
            }
        }

    }
}
