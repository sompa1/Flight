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
                new ApplicationUser { Id= new Guid("bc9ecb96-a585-4c27-98b7-5ddad62cae63"), UserName= "panni", NormalizedUserName= "PANNI", Email= "panni@panni.hu", NormalizedEmail= "PANNI@PANNI.HU", EmailConfirmed= false, PasswordHash= "AQAAAAEAACcQAAAAELxIn43pMpHivyUzDkrTl2OAw/el6ZVGF2Mmw/z22dB1BbltY+M5tTFmHc8KbDdGcQ==", SecurityStamp= "L5GKNFULGY54QZWALF5YELHAHHZSEF5B", ConcurrencyStamp= "61104595-46bb-453a-be97-edcc7fa704e1", LockoutEnabled= true }
                );

            builder.Entity<ApplicationUserRole>().HasData(new ApplicationUserRole
            {
                UserId = new Guid("bc9ecb96-a585-4c27-98b7-5ddad62cae63"),
                RoleId = new Guid("a973cb5e-a900-4417-a1cb-08d74a8d3191")
            });

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {            
        }
    }
}
