using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories;

public class MenuItemRepository
{
    private readonly RestaurantReservationDbContext _context;
    public MenuItemRepository(RestaurantReservationDbContext context)
    {
        _context = context;
    }
    public async Task<List<MenuItem>> GetAllAsync()
    {
        return await _context.MenuItems.ToListAsync();
    }
    public async Task<MenuItem> GetMenuItemByIdAsync(int menuItemId)
    {
        return await _context.MenuItems.FindAsync(menuItemId);
    }
    public async Task AddMenuItemAsync(MenuItem menuItem)
    {
        _context.MenuItems.Add(menuItem);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateMenuItemAsync(MenuItem menuItem)
    {
        _context.MenuItems.Update(menuItem);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteMenuItemAsync(int menuItemId)
    {
        var menuItem = await GetMenuItemByIdAsync(menuItemId);
        if (menuItem != null)
        {
            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
        }
    }
}
