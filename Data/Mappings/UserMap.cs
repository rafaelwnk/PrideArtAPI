using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrideArtAPI.Models;

namespace PrideArtAPI.Data.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);

        builder.Property(x => x.Username)
            .IsRequired()
            .HasColumnName("Username")
            .HasColumnType("VARCHAR")
            .HasMaxLength(25);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasColumnName("Email")
            .HasColumnType("VARCHAR")
            .HasMaxLength(120);

        builder.Property(x => x.Password)
            .IsRequired()
            .HasColumnName("Password")
            .HasColumnType("VARCHAR")
            .HasMaxLength(50);

        builder.Property(x => x.Identity)
            .IsRequired()
            .HasColumnName("Identity")
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);

        builder.Property(x => x.Bio)
            .IsRequired(false)
            .HasColumnName("Bio")
            .HasColumnType("VARCHAR")
            .HasMaxLength(255);

        builder.Property(x => x.Image)
            .IsRequired(false)
            .HasColumnName("Image")
            .HasColumnType("VARCHAR");

        builder.HasMany(x => x.Following)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>
            (
                "UserFollows",
                user => user.HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .HasConstraintName("FK_UserFollows_UserId")
                    .OnDelete(DeleteBehavior.Cascade),
                followed => followed.HasOne<User>()
                    .WithMany()
                    .HasForeignKey("FollowedId")
                    .HasConstraintName("FK_UserFollows_FollowedId")
                    .OnDelete(DeleteBehavior.Cascade)
            )
            .Property<int>("UserId")
            .HasColumnOrder(0); 

        builder.HasIndex(x => x.Username, "IX_User_Username").IsUnique();
        builder.HasIndex(x => x.Email, "IX_User_Email").IsUnique();

    }
}