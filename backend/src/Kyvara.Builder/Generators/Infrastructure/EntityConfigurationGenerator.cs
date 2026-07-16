namespace Kyvara.Builder.Generators.Infrastructure;

public sealed class EntityConfigurationGenerator
{
    public string Generate()
    {
        return """
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace {{Namespace}}.Persistence.Configurations;

public sealed class {{Entity}}Configuration
    : IEntityTypeConfiguration<{{Entity}}>
{
    public void Configure(
        EntityTypeBuilder<{{Entity}}> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder.ToTable("{{Entity}}");
    }
}
""";
    }
}
