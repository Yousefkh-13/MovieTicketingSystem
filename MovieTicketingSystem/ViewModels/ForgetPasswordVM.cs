using System.ComponentModel.DataAnnotations;

namespace MovieTicketingSystem.ViewModels;

public class ForgetPasswordVM
{
    [Required]
    [Display(Name = "Email Or UserName")]
    public string EmailOrUserName { get; set; } = string.Empty;
}
