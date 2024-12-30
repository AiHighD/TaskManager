using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TasksManager.Data.Entities;

public class Tasks
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

    public string? Status { get; set; }

    public ICollection<Progress>? Progress { get; set; }

    public ICollection<Documents>? Documents { get; set; }

    public ICollection<Attachments>? Attachments { get; set; }

}
public class TasksConfiguration : IEntityTypeConfiguration<Tasks>
{
    public void Configure(EntityTypeBuilder<Tasks> builder)
    {
        builder.HasMany(e => e.Progress)
        .WithOne(e => e.Task)
        .HasForeignKey(e => e.TaskId)
        .HasPrincipalKey(e => e.Task_Id);

        builder.HasMany(e => e.Documents)
        .WithOne(e => e.Task)
        .HasForeignKey(e => e.TaskId)
        .HasPrincipalKey(e => e.Task_Id);

        builder.HasMany(e => e.Attachments)
        .WithOne(e => e.Task)
        .HasForeignKey(e => e.TaskId)
        .HasPrincipalKey(e => e.Task_Id);
    }
}


