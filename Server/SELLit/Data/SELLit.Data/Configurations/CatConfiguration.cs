using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SELLit.Data.Configurations;
internal sealed class CatConfiguration : IEntityTypeConfiguration<Cat>
{
    public void Configure(EntityTypeBuilder<Cat> cat)
    {
        cat
            .HasOne(x => x.User)
            .WithMany(u => u.Cats)
            .HasForeignKey(x => x.UserId);
    }

}

