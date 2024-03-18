﻿namespace YourBrand.UserManagement.Domain.Common.Interfaces;

public interface ISoftDelete
{
    DateTime? Deleted { get; set; }

    string? DeletedBy { get; set; }
}