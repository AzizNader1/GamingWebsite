using GameZone.Models;

namespace GameZone.Data;
/// <summary>
/// this class will represent the connection line between the tables inside your database and how you can access them through a class by using the simple way of doing this
/// inside this class you will add all the entities that you need to represent their own table in the shape of class to make the access and the operation over these tables (class) more easier
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Game> Games { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<GameDevice> GameDevices { get; set; }

    /// <summary>
    /// this method will be invoked when your application runs and make a connection to your own database and everything inside this method will be applied on your tables inside your database such as fluenapi and also the database seeding
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasData(new Category[]
            {
                new Category { Id = 1, Name = "Sports" },
                new Category { Id = 2, Name = "Action" },
                new Category { Id = 3, Name = "Adventure" },
                new Category { Id = 4, Name = "Racing" },
                new Category { Id = 5, Name = "Fighting" },
                new Category { Id = 6, Name = "Film" }
            });

        modelBuilder.Entity<Device>()
            .HasData(new Device[]
            {
                new Device { Id = 1, Name = "PlayStation", Icon = "bi bi-playstation" },
                new Device { Id = 2, Name = "xbox", Icon = "bi bi-xbox" },
                new Device { Id = 3, Name = "Nintendo Switch", Icon = "bi bi-nintendo-switch" },
                new Device { Id = 4, Name = "PC", Icon = "bi bi-pc-display" }
            });

        // this will add a candanite key (more than one attribute will be the key of this tabel)
        modelBuilder.Entity<GameDevice>()
            .HasKey(e => new { e.GameId, e.DeviceId });

        base.OnModelCreating(modelBuilder);
    }
}