﻿namespace WorkForYou.Data.DtoModels;

public class UserRegisterDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string SelectedRole { get; set; } = string.Empty;
}
