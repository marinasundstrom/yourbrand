using Skynet.Showroom.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Skynet.Showroom.Application.Common.Interfaces;

public interface IShowroomContext
{
    DbSet<ConsultantProfile> ConsultantProfiles { get; }

    DbSet<CompetenceArea> CompetenceAreas { get; }

    DbSet<Organization> Organizations { get; }

    DbSet<User> Users { get; }

    DbSet<Case> Cases { get; }

    DbSet<CaseConsultant> CaseConsultants { get; }

    DbSet<SkillGroup> SkillGroups { get; }

    DbSet<Skill> Skills { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<ITransaction> BeginTransactionAsync();
}

public interface ITransaction : IDisposable, IAsyncDisposable
{
    Task CommitAsync();
    Task RollbackAsync();
}