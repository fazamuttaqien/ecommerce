using Microsoft.EntityFrameworkCore;
using AuthenticationApi.Domain.Entities;

namespace AuthenticationApi.Infrastructure.Data {
    public class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : DbContext(options) {
        public DbSet<AppUser> Users { get; set; }
    }
}