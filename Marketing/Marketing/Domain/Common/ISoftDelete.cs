﻿namespace YourBrand.Marketing.Domain.Common;

public interface ISoftDelete
{
    DateTime? Deleted { get; set; }

    string? DeletedById { get; set; }
}