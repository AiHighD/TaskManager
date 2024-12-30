using System.ComponentModel.DataAnnotations;

namespace TasksManager.Data.Entities;

public class Attachments
{
    [Key]
    public int Attachments_Id { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? Description { get; set; }

    [Required]
    public string? File { get; set; }

    [Required]
    public string? Image { get; set; }

    [DataType(DataType.Date)]
    [Required]
    public DateTime Time_Create_Tep { get; set; }

    public int TaskId { get; set; }

    public Tasks? Task { get; set; }
}