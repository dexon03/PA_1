using Lab3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Lab3.Models;

public static class SeedData
{
    public static void EnsurePopulated(IApplicationBuilder app)
    {
        var serviceScope = app.ApplicationServices.CreateScope();
        ApplicationDbContext dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        if (!dbContext.NodeValues.Any())
        {
            for (int i = 0; i < 10000; i++)
            {
                dbContext.NodeValues.Add(new NodeValue{Value = Guid.NewGuid().ToString()});
            }
        }

        dbContext.SaveChanges();
    }
}