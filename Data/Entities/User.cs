using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TasksManager.Data.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(50)]
        [Required]
        public string? FullName { get; set; }
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
    }
}