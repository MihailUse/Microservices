namespace EquipmentMicroservice.DAL.Entities;

public class Equipment
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string ResponsibleName { get; set; } = null!;
}
