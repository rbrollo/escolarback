using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Escolar.Infrastructure.Persistence;

public class EscolarDbContextFactory : IDesignTimeDbContextFactory<EscolarDbContext>
{
    public EscolarDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EscolarDbContext>();
        var connectionString = "Server=localhost;Port=3307;Database=escolardb;User=root;Password=123456;";

        optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 4, 0)));

        return new EscolarDbContext(optionsBuilder.Options);
    }
}
