using EquipmentMicroservice.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace EquipmentMicroservice.DAL;

public class DataContext : DbContext
{
    public DbSet<Equipment> Equipments => Set<Equipment>();

    public DataContext(DbContextOptions options) : base(options)
    {
    }
}
