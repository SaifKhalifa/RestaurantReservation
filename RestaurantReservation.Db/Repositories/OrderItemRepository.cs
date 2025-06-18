using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories;

public class OrderItemRepository
{
    private readonly RestaurantReservationDbContext _context;
    public OrderItemRepository(RestaurantReservationDbContext context)
    {
        _context = context;
    }
    public async Task<List<OrderItem>> GetAllAsync()
    {
        return await _context.OrderItems
            .Include(oi => oi.Order)
            .Include(oi => oi.MenuItem)
            .ToListAsync();
    }
    public async Task<OrderItem> GetOrderItemByIdAsync(int orderItemId)
    {
        return await _context.OrderItems
            .Include(oi => oi.Order)
            .Include(oi => oi.MenuItem)
            .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId);
    }
    public async Task AddOrderItemAsync(OrderItem orderItem)
    {
        _context.OrderItems.Add(orderItem);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateOrderItemAsync(OrderItem orderItem)
    {
        _context.OrderItems.Update(orderItem);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteOrderItemAsync(int orderItemId)
    {
        var orderItem = await GetOrderItemByIdAsync(orderItemId);
        if (orderItem != null)
        {
            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
        }
    }
}
