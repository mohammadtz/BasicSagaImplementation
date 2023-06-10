using Domain.Contracts;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Domain.Base;
using Infrastructure;
using Newtonsoft.Json;

namespace MassTransitSagaImplementation.Services;

public class DbEntityChangeTracker : IEntityChangeTracker
{
    private readonly Func<MyContext> _contextFactory;

    public DbEntityChangeTracker(Func<MyContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public void LogChanges(IEnumerable<EntityEntry> entries)
    {
        using var context = _contextFactory();

        foreach (var entry in entries)
        {
            var entity = entry.Entity as BaseEntity;

            if(entity == null) return;

            Console.WriteLine(JsonConvert.SerializeObject(new LogEntity
            {
                EntityName = entity.GetType().FullName,
                EntityState = entry.State.ToString(),
                EntityData = entity
            }, Formatting.Indented));
        }
    }
}

public class LogEntity
{
    public string? EntityName { get; set; }
    public string EntityState { get; set; }
    public object EntityData { get; set; }
}