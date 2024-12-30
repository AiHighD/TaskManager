using System.ComponentModel.DataAnnotations;

namespace TasksManager.Data.Entities;

public class Documents
{
    [Key]
    public int Documents_Id { get; set; }

    [StringLength(500, MinimumLength = 3)]
    [Required]
    public string? Doc { get; set; }

    [DataType(DataType.Date)]
    [Required]
    public DateTime Time_Create_Doc { get; set; }

    public int TaskId { get; set; }

    public Tasks? Task { get; set; }
}