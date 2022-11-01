namespace SELLit.Data;

internal static class EntityIndexesConfiguration
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        // IDeletableEntity.IsDeleted index
        var deletableEntityTypes = modelBuilder.Model
            .GetEntityTypes()
            .Where(et => typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
        foreach (var deletableEntityType in deletableEntityTypes)
        {
            modelBuilder.Entity(deletableEntityType.ClrType).HasIndex(nameof(IDeletableEntity.IsDeleted));
        }
    }
}