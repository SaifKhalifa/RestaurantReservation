using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Models;

public class Restaurant
{
    public int RestaurantId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string OpeningHours { get; set; }

    public ICollection<Employee> Employees { get; set; }
    public ICollection<Table> Tables { get; set; }
    public ICollection<MenuItem> MenuItems { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
}
