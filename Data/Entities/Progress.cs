using System.ComponentModel.DataAnnotations;

namespace TasksManager.Data.Entities;

public class Progress
{
    [Key]
    public int Progress_Id { get; set; }

    [Required]
    public int? Progress_Percentage { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? Note { get; set; }

    public int TaskId { get; set; }

    public Tasks? Task { get; set; }

}