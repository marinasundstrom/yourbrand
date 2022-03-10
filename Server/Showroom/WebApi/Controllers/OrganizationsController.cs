﻿
using MediatR;

using Microsoft.AspNetCore.Mvc;

using Skynet.Showroom.Application.Common.Models;
using Skynet.Showroom.Application.Organizations;
using Skynet.Showroom.Application.Organizations.Commands;
using Skynet.Showroom.Application.Organizations.Queries;

namespace Skynet.Showroom.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class OrganizationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrganizationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<Results<OrganizationDto>> GetOrganizations(int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, Application.Common.Models.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GetOrganizationsQuery(page - 1, pageSize, searchString, sortBy, sortDirection), cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<OrganizationDto?> GetOrganization(string id, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetOrganizationQuery(id), cancellationToken);
    }

    [HttpPost]
    public async Task CreateOrganization(CreateOrganizationDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CreateOrganizationCommand(dto.Name), cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task UpdateOrganization(string id, UpdateOrganizationDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateOrganizationCommand(id, dto.Name), cancellationToken);
    }


    [HttpDelete("{id}")]
    public async Task DeleteOrganization(string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteOrganizationCommand(id), cancellationToken);
    }
}

public record CreateOrganizationDto(string Name);

public record UpdateOrganizationDto(string Name);
