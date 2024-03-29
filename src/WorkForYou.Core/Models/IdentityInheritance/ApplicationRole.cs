﻿using Microsoft.AspNetCore.Identity;

namespace WorkForYou.Core.Models.IdentityInheritance;

public class ApplicationRole : IdentityRole
{
    public string? Description { get; set; }

    public ApplicationRole(string name, string description)
        : base(name)
    {
        Description = description;
    }
}
