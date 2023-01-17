using System.ComponentModel.DataAnnotations;

namespace EquipmentMicroservice.Models;

public class CreateEquipmentModel
{
    [MinLength(2)]
    public string Title { get; set; } = null!;

    [MaxLength(512)]
    public string? Description { get; set; }

    [MinLength(2)]
    public string ResponsibleName { get; set; } = null!;
}
