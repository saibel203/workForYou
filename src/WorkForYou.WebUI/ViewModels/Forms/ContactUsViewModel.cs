using System.ComponentModel.DataAnnotations;

namespace WorkForYou.WebUI.ViewModels.Forms;

public class ContactUsViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Subject { get; set; } = string.Empty;
    
    [Required]
    public string Message { get; set; } = string.Empty;
}
