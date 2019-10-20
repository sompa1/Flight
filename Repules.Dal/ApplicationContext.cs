using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repules.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Repules.Dal
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<Airport> Airports { get; set; }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<GPSRecord> GPSRecords { get; set; }

        public DbSet<FlightLogFile> FlightLogFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            builder.Entity<Flight>().HasOne(f => f.DepartureLocation)
                                    .WithMany(dl => dl.DepartureFlights);

            builder.Entity<Flight>().HasOne(f => f.ArrivalLocation)
                                    .WithMany(al => al.ArrivalFlights);

            var cascadeFKs = builder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys())
                                                           .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser { Id = new Guid("bc9ecb96-a585-4c27-98b7-5ddad62cae63"), UserName = "panni", NormalizedUserName = "PANNI", Email = "panni@panni.hu", NormalizedEmail = "PANNI@PANNI.HU", EmailConfirmed = false, PasswordHash = "AQAAAAEAACcQAAAAELxIn43pMpHivyUzDkrTl2OAw/el6ZVGF2Mmw/z22dB1BbltY+M5tTFmHc8KbDdGcQ==", SecurityStamp = "L5GKNFULGY54QZWALF5YELHAHHZSEF5B", ConcurrencyStamp = "61104595-46bb-453a-be97-edcc7fa704e1", LockoutEnabled = true },
               new ApplicationUser { Id = new Guid("b89f5bf2-040d-4dd5-32df-08d74a8d31b7"), UserName = "admin", NormalizedUserName = "ADMIN", Email = "admin@admin.hu", NormalizedEmail = "ADMIN@ADMIN.HU", EmailConfirmed = false, PasswordHash = "AQAAAAEAACcQAAAAELxIn43pMpHivyUzDkrTl2OAw/el6ZVGF2Mmw/z22dB1BbltY+M5tTFmHc8KbDdGcQ==", SecurityStamp = "TARD737IV7D4UC3F2RAWPD7C574GXDEH", ConcurrencyStamp = "cd16e589-657a-4aa9-bed8-fe8c2f9e27a1", LockoutEnabled = true }
                );

            builder.Entity<ApplicationUserRole>().HasData(new ApplicationUserRole
            {
                UserId = new Guid("bc9ecb96-a585-4c27-98b7-5ddad62cae63"),
                RoleId = new Guid("a973cb5e-a900-4417-a1cb-08d74a8d3191")
            },
            new ApplicationUserRole
            {
                UserId = new Guid("b89f5bf2-040d-4dd5-32df-08d74a8d31b7"),
                RoleId = new Guid("6d801a95-fdc7-4f51-a1ca-08d74a8d3191")
            });

            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole
                {
                    Id = new Guid("a973cb5e-a900-4417-a1cb-08d74a8d3191"),
                    Name = "user",
                    NormalizedName = "USER"
                },
              new ApplicationRole
              {
                  Id = new Guid("6d801a95-fdc7-4f51-a1ca-08d74a8d3191"),
                  Name = "admin",
                  NormalizedName = "ADMIN"
              });

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
    }
}
