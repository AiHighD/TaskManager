using System.ComponentModel.DataAnnotations;

namespace TasksManager.ViewModels;

public class AttachmentRequest
{
    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? Description { get; set; }

    [Required]
    public IFormFile? FileUpload { get; set; }

    [Required]
    public IFormFile? ImageUpload { get; set; }

    [DataType(DataType.Date)]
    [Required]
    public DateTime Time_Create_Tep { get; set; }

    public int TaskId { get; set; }
}

public class AttachmentViewModel
{
    public int Attachments_Id { get; set; }

    public string? Description { get; set; }

    public string? File { get; set; }

    public IFormFile? FileUpload { get; set; }

    public string? Image { get; set; }

    public IFormFile? ImageUpload { get; set; }

    [Display(Name = "Time Created")]
    [DataType(DataType.Date)]
    public DateTime Time_Create_Tep { get; set; }

    public int TaskId { get; set; }

    [Display(Name = "Task Name")]
    public int Task { get; set; }
}