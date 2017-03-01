using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KostenBatenTool.Models;
using Microsoft.AspNetCore.Identity;

namespace KostenBatenTool.Data
{
    public class KostenBatenInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public KostenBatenInitializer(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task InitializeData()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                await InitializeUsers();

            }
        }

        private async Task InitializeUsers()
        {
            string email = "test@test.be";
            ApplicationUser user = new ApplicationUser {UserName = email, Email = email};
            await _userManager.CreateAsync(user, "P@ssword1");
            
        }

        }
    
}
