using Audit.EntityFramework;
using Bridgenext.Models.Constant.DataAccess;
using Bridgenext.Models.Enums;
using Bridgenext.Models.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Bridgenext.DataAccess
{
    public class UserSystemContext : DbContext
    {
        private readonly DbContextHelper _helper = new();
        private readonly IAuditDbContext _auditContext;
        private readonly Guid IdAdministrator;

        public UserSystemContext(DbContextOptions<UserSystemContext> options, IConfigurationRoot _configuration) : base(options)
        {
            _auditContext = new DefaultAuditContext(this)
            {
                IncludeEntityObjects = true,
                AuditEventType = "{context}:{database}"
            };

            _helper.SetConfig(_auditContext);
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            Guid.TryParse(_configuration["IdUserAdmin"],out IdAdministrator);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await _helper.SaveChangesAsync(_auditContext, () => base.SaveChangesAsync(cancellationToken));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable(DBTables.Users);
                entity.HasIndex(x => x.Email).IsUnique();
                entity.HasIndex(x => x.IdUserType);
                entity.HasMany(e => e.Addreesses)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.IdUser);
                entity.HasOne(e => e.UserTypes)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.IdUserType);
                entity.HasMany(e => e.Documents)
                .WithOne(e => e.Users)
                .HasForeignKey(e => e.IdUser);
                entity.HasMany(e => e.Comments)
                .WithOne(e => e.Users)
                .HasForeignKey(e => e.IdUser);
                entity.Property(e => e.CreateDate)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );
                entity.Property(e => e.ModifyDate)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );
                entity.HasData(
                new { Id = IdAdministrator, FirstName = "Administrator", LastName = "Administrator", Email = "admin@admin.admin", IdUserType = (int)UsersTypeEnum.Administrator, CreateUser = "Administrator", CreateDate = DateTime.Now, ModifyUser = "Administrator", ModifyDate = DateTime.Now });

            });

            modelBuilder.Entity<Users_History>(entity => { 
                entity.ToTable($"{DBTables.Users}_History");
                entity.Property(e => e.CreateDate)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );
                entity.Property(e => e.ModifyDate)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );
                entity.Property(e => e.AuditDate)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );
            });

            modelBuilder.Entity<UsersTypes>(entity =>
            {
                entity.ToTable(DBTables.UsersType);
                entity.HasData(
                new { Id = 1, Type = "Administrator" },
                new { Id = 2, Type = "AMBContext"});
            });


            modelBuilder.Entity<Addreesses>(entity => {
                entity.ToTable(DBTables.Addresses);
                entity.HasOne(e => e.User)
               .WithMany(e => e.Addreesses)
               .HasForeignKey(e => e.IdUser);
                entity.HasIndex(x => x.City);
                entity.HasIndex(x => x.Country);
                entity.HasIndex(x => x.Zip);
                entity.Property(e => e.CreateDate)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );
                entity.Property(e => e.ModifyDate)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );
            });

            modelBuilder.Entity<Addreesses_History>(entity =>
            {
                entity.ToTable($"{DBTables.Addresses}_History");
                entity.Property(e => e.CreateDate)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );
                entity.Property(e => e.ModifyDate)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );
                entity.Property(e => e.AuditDate)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );
            });
                

            modelBuilder.Entity<Documents>(entity =>
            {
                entity.ToTable(DBTables.Documents);
                entity.HasIndex(e => e.IdDocumentType);
                entity.HasOne(e => e.DocumentType)
                .WithMany(e => e.Documents)
                .HasForeignKey(e => e.IdDocumentType);
                entity.HasOne(e => e.Users)
                .WithMany(e => e.Documents);
                entity.HasMany(e => e.Comments)
                .WithOne(e => e.Documents)
                .HasForeignKey(e => e.IdDocumnet);
                entity.Property(e => e.CreateDate)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );
                entity.Property(e => e.ModifyDate)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );
            });

            modelBuilder.Entity<Documents_History>(entity =>
            {
                entity.ToTable($"{DBTables.Documents}_History");
                entity.Property(e => e.CreateDate)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );
                entity.Property(e => e.ModifyDate)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );
                entity.Property(e => e.AuditDate)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                );
            });
                

            modelBuilder.Entity<DocumentsType>(entity =>
            {
                entity.ToTable(DBTables.DocumentsType);
                entity.HasData(
                new { Id = 1, Type = "Text" },
                new { Id = 2, Type = "Doc" },
                new { Id = 3, Type = "Image" },
                new { Id = 4, Type = "Video" });
            });

            modelBuilder.Entity<Comments>(entity => {
                entity.ToTable(DBTables.Comments);
                entity.HasOne(e => e.Documents)
               .WithMany(e => e.Comments)
               .HasForeignKey(e => e.IdDocumnet);
                entity.HasOne(e => e.Users)
               .WithMany(e => e.Comments)
               .HasForeignKey(e => e.IdUser);
                entity.HasIndex(x => x.IdUser);
                entity.HasIndex(x => x.IdDocumnet);
                entity.Property(e => e.Date)
                .HasConversion
                (
                    src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                    dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
                ); ;
            });

        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UsersTypes> UsersTypes { get; set; }
        public virtual DbSet<Users_History> Users_History { get; set; }
        public virtual DbSet<Addreesses> Addreesses { get; set; }
        public virtual DbSet<Addreesses_History> Addreesses_History { get; set; }
        public virtual DbSet<Documents> Documents { get; set; }
        public virtual DbSet<DocumentsType> DocumentsTypes { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
    }
}
