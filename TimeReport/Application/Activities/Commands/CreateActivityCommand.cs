﻿
using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.TimeReport.Application.Common.Interfaces;
using YourBrand.TimeReport.Application.Projects;
using YourBrand.TimeReport.Domain.Entities;

namespace YourBrand.TimeReport.Application.Activities.Commands;

public class CreateActivityCommand : IRequest<ActivityDto>
{
    public CreateActivityCommand(string projectId, string name, string activityTypeId, string? description, decimal? hourlyRate)
    {
        ProjectId = projectId;
        Name = name;
        ActivityTypeId = activityTypeId;
        Description = description;
        HourlyRate = hourlyRate;
    }

    public string ProjectId { get; }
    public string Name { get; }
    public string ActivityTypeId { get; }
    public string? Description { get; }
    public decimal? HourlyRate { get; }

    public class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand, ActivityDto>
    {
        private readonly ITimeReportContext _context;

        public CreateActivityCommandHandler(ITimeReportContext context)
        {
            _context = context;
        }

        public async Task<ActivityDto> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects
               .AsSplitQuery()
               .FirstOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken);

            if (project is null)
            {
                throw new Exception();
            }

            var activity = new Activity
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                ActivityType = await _context.ActivityTypes.FirstAsync(at => at.Id == request.ActivityTypeId),
                Description = request.Description,
                Project = project,
                HourlyRate = request.HourlyRate
            };

            _context.Activities.Add(activity);

            await _context.SaveChangesAsync(cancellationToken);

            return activity.ToDto();
        }
    }
}