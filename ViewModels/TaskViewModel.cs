using System.ComponentModel.DataAnnotations;

namespace TasksManager.ViewModels;

public class TaskRequest
{
    [Key]
    public int Task_Id { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? Task_Name { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? Topic { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? Description { get; set; }

    [DataType(DataType.Date)]
    [Required]
    public DateTime StartTime { get; set; }

    [DataType(DataType.Date)]
    [Required]
    public DateTime EndTime { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? Status { get; set; }
}

public class TaskViewModel
{
    [Key]
    public int Task_Id { get; set; }

    public string? Task_Name { get; set; }

    public string? Topic { get; set; }

    public string? Description { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string? Status { get; set; }
}