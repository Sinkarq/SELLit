namespace SELLit.Data.Infrastructure;

public static class ModelBuilderExtensions
{
    public static void ConfigureRelations(this ModelBuilder builder)
    {
        builder.Entity<Product>()
            .HasOne(x => x.Category)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CategoryId);

        builder.Entity<Product>()
            .HasOne(x => x.User)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.UserId);
    }
}