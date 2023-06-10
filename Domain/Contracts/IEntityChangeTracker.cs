using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Domain.Contracts;

public interface IEntityChangeTracker
{
    void LogChanges(IEnumerable<EntityEntry> entries);
}