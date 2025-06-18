using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db;

public class RestaurantReservationDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=RestaurantReservationCore;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Reservation entity configuration
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Customer)
            .WithMany(c => c.Reservations)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Cascade); // keep cascade for this one

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Restaurant)
            .WithMany(c => c.Reservations)
            .HasForeignKey(r => r.RestaurantId)
            .OnDelete(DeleteBehavior.Restrict); // restrict to avoid conflict

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Table)
            .WithMany(c => c.Reservations)
            .HasForeignKey(r => r.TableId)
            .OnDelete(DeleteBehavior.Restrict); // restrict this too

        // Order entity configuration
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade); // keep one cascade

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.MenuItem)
            .WithMany(mi => mi.OrderItems)
            .HasForeignKey(oi => oi.MenuItemId)
            .OnDelete(DeleteBehavior.Restrict); // prevent cycle

        // Dummy data seeding
        modelBuilder.Entity<Customer>().HasData(
            new Customer { CustomerId = 1, FirstName = "Ali", LastName = "Zain", Email = "ali@example.com", PhoneNumber = "1111111111" },
            new Customer { CustomerId = 2, FirstName = "Nora", LastName = "Salem", Email = "nora@example.com", PhoneNumber = "2222222222" },
            new Customer { CustomerId = 3, FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "3333333333" },
            new Customer { CustomerId = 4, FirstName = "Sara", LastName = "Lee", Email = "sara@example.com", PhoneNumber = "4444444444" },
            new Customer { CustomerId = 5, FirstName = "Mike", LastName = "Jordan", Email = "mike@example.com", PhoneNumber = "5555555555" }
        );

        modelBuilder.Entity<Restaurant>().HasData(
            new Restaurant { RestaurantId = 1, Name = "Mezza House", Address = "Downtown", PhoneNumber = "0123456789", OpeningHours = "10 AM - 10 PM" },
            new Restaurant { RestaurantId = 2, Name = "Zaatar W Zeit", Address = "Main Street", PhoneNumber = "0987654321", OpeningHours = "9 AM - 11 PM" },
            new Restaurant { RestaurantId = 3, Name = "Hummus Haven", Address = "City Center", PhoneNumber = "1111222233", OpeningHours = "11 AM - 9 PM" },
            new Restaurant { RestaurantId = 4, Name = "Falafel Fanatics", Address = "West Side", PhoneNumber = "4444333322", OpeningHours = "8 AM - 8 PM" },
            new Restaurant { RestaurantId = 5, Name = "Shawarma Station", Address = "East Gate", PhoneNumber = "9999888877", OpeningHours = "12 PM - 12 AM" }
        );

        modelBuilder.Entity<Employee>().HasData(
            new Employee { EmployeeId = 1, FirstName = "Ahmed", LastName = "Hassan", Position = "Manager", RestaurantId = 1 },
            new Employee { EmployeeId = 2, FirstName = "Leila", LastName = "Yousef", Position = "Waiter", RestaurantId = 1 },
            new Employee { EmployeeId = 3, FirstName = "Tariq", LastName = "Saleh", Position = "Manager", RestaurantId = 2 },
            new Employee { EmployeeId = 4, FirstName = "Jana", LastName = "Nabil", Position = "Chef", RestaurantId = 3 },
            new Employee { EmployeeId = 5, FirstName = "Kareem", LastName = "Ali", Position = "Waiter", RestaurantId = 4 }
        );

        modelBuilder.Entity<Table>().HasData(
            new Table { TableId = 1, RestaurantId = 1, Capacity = 4 },
            new Table { TableId = 2, RestaurantId = 1, Capacity = 6 },
            new Table { TableId = 3, RestaurantId = 2, Capacity = 2 },
            new Table { TableId = 4, RestaurantId = 3, Capacity = 5 },
            new Table { TableId = 5, RestaurantId = 4, Capacity = 3 }
        );

        modelBuilder.Entity<Reservation>().HasData(
            new Reservation { ReservationId = 1, CustomerId = 1, RestaurantId = 1, TableId = 1, ReservationDate = DateTime.Parse("2025-06-20 18:00"), PartySize = 2 },
            new Reservation { ReservationId = 2, CustomerId = 2, RestaurantId = 2, TableId = 3, ReservationDate = DateTime.Parse("2025-06-21 20:00"), PartySize = 4 },
            new Reservation { ReservationId = 3, CustomerId = 3, RestaurantId = 3, TableId = 4, ReservationDate = DateTime.Parse("2025-06-22 19:00"), PartySize = 3 },
            new Reservation { ReservationId = 4, CustomerId = 4, RestaurantId = 1, TableId = 2, ReservationDate = DateTime.Parse("2025-06-23 21:00"), PartySize = 5 },
            new Reservation { ReservationId = 5, CustomerId = 5, RestaurantId = 4, TableId = 5, ReservationDate = DateTime.Parse("2025-06-24 17:30"), PartySize = 2 }
        );

        modelBuilder.Entity<MenuItem>().HasData(
            new MenuItem { MenuItemId = 1, RestaurantId = 1, Name = "Hummus", Description = "Creamy chickpea dip", Price = 5.50m },
            new MenuItem { MenuItemId = 2, RestaurantId = 1, Name = "Falafel", Description = "Crispy chickpea balls", Price = 6.00m },
            new MenuItem { MenuItemId = 3, RestaurantId = 2, Name = "Shawarma", Description = "Spiced meat wrap", Price = 7.25m },
            new MenuItem { MenuItemId = 4, RestaurantId = 3, Name = "Tabbouleh", Description = "Parsley salad", Price = 4.75m },
            new MenuItem { MenuItemId = 5, RestaurantId = 4, Name = "Kebab", Description = "Grilled meat skewer", Price = 8.50m }
        );

        modelBuilder.Entity<Order>().HasData(
            new Order { OrderId = 1, ReservationId = 1, EmployeeId = 2, OrderDate = DateTime.Parse("2025-06-20 18:30"), TotalAmount = 11.50m },
            new Order { OrderId = 2, ReservationId = 2, EmployeeId = 3, OrderDate = DateTime.Parse("2025-06-21 20:10"), TotalAmount = 7.25m },
            new Order { OrderId = 3, ReservationId = 3, EmployeeId = 4, OrderDate = DateTime.Parse("2025-06-22 19:15"), TotalAmount = 4.75m },
            new Order { OrderId = 4, ReservationId = 4, EmployeeId = 1, OrderDate = DateTime.Parse("2025-06-23 21:20"), TotalAmount = 13.00m },
            new Order { OrderId = 5, ReservationId = 5, EmployeeId = 5, OrderDate = DateTime.Parse("2025-06-24 17:45"), TotalAmount = 8.50m }
        );

        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem { OrderItemId = 1, OrderId = 1, MenuItemId = 1, Quantity = 1 },
            new OrderItem { OrderItemId = 2, OrderId = 2, MenuItemId = 2, Quantity = 2 },
            new OrderItem { OrderItemId = 3, OrderId = 3, MenuItemId = 3, Quantity = 3 },
            new OrderItem { OrderItemId = 4, OrderId = 4, MenuItemId = 4, Quantity = 4 },
            new OrderItem { OrderItemId = 5, OrderId = 5, MenuItemId = 5, Quantity = 5 }
        );

    }
}
