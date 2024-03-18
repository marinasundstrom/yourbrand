﻿using MassTransit;

using MediatR;

using YourBrand.UserManagement.Application.Organizations.Commands;
using YourBrand.UserManagement.Contracts;
using YourBrand.Identity;

namespace YourBrand.UserManagement.Consumers;

public class CreateOrganizationConsumer : IConsumer<CreateOrganization>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public CreateOrganizationConsumer(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public async Task Consume(ConsumeContext<CreateOrganization> context)
    {
        var message = context.Message;

        var organization = await _mediator.Send(new CreateOrganizationCommand(message.Name, message.FriendlyName));

        await context.RespondAsync(new CreateOrganizationResponse(organization.Id, organization.Name, organization.FriendlyName));
    }
}