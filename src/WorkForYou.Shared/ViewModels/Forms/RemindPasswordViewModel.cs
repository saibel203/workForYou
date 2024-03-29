﻿using System.ComponentModel.DataAnnotations;

namespace WorkForYou.Shared.ViewModels.Forms;

public class RemindPasswordViewModel
{
    [Required(ErrorMessage = "Email поле є обов'язвовим")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}