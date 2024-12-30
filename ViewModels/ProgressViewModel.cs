using System.ComponentModel.DataAnnotations;
using TasksManager.Data.Entities;

namespace TasksManager.ViewModels;
public class ProgressRequest
{
    [Required]
    public int? Progress_Percentage { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? Note { get; set; }

    public int TaskId { get; set; }

}

public class ProgressViewModel
{
    public int Progress_Id { get; set; }

    public int? Progress_Percentage { get; set; }

    public string? Note { get; set; }

    [Display(Name = "Taski")]
    public int TaskId { get; set; }

    [Display(Name = "Task Name")]
    public int Task { get; set; }
}