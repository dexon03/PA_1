using Lab3.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab3.Models;

public static class Generator
{
    public static void GenerateDataToDb(IApplicationBuilder app)
    {
        ApplicationDbContext dbContext = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        if (dbContext.NodeValues.Count() != 0)
        {
            for (int i = 0; i < 10000; i++)
            {
                dbContext.NodeValues.Add(new NodeValue{Value = Guid.NewGuid().ToString()});
            }
        }
    }
}