using System.ComponentModel.DataAnnotations;

namespace TasksManager.ViewModels;

public class DocumentRequest
{

    [StringLength(500, MinimumLength = 3)]
    [Required]
    public string? Doc { get; set; }

    [DataType(DataType.Date)]
    [Required]
    public DateTime Time_Create_Doc { get; set; }

    public int TaskId { get; set; }


}

public class DocumentViewModel
{
    public int Documents_Id { get; set; }

    public string? Doc { get; set; }

    public DateTime Time_Create_Doc { get; set; }

    public int TaskId { get; set; }

    [Display(Name = "Task Name")]
    public int Task { get; set; }
}