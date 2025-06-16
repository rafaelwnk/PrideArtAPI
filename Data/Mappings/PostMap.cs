using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrideArtAPI.Models;

namespace PrideArtAPI.Data.Mappings;

public class PostMap : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Post");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasColumnName("Title")
            .HasColumnType("VARCHAR")
            .HasMaxLength(120);

        builder.Property(x => x.Image)
            .IsRequired(false)
            .HasColumnName("Image")
            .HasColumnType("VARCHAR");

        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasColumnName("Description")
            .HasColumnType("VARCHAR")
            .HasMaxLength(355);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Posts)
            .HasConstraintName("FK_Post_User")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.UsersLiked)
            .WithMany(x => x.LikedPosts)
            .UsingEntity<Dictionary<string, object>>(
                "PostLikes",
                user => user.HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .HasConstraintName("FK_PostLikes_UserId")
                    .OnDelete(DeleteBehavior.Cascade),
                post => post.HasOne<Post>()
                    .WithMany()
                    .HasForeignKey("PostId")
                    .HasConstraintName("FK_PostLikes_PostId")
                    .OnDelete(DeleteBehavior.Cascade)
            );

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.LastUpdate)
            .IsRequired()
            .HasColumnName("LastUpdate")
            .HasColumnType("DATETIME")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(x => x.Title, "IX_Post_Title");
    }
}