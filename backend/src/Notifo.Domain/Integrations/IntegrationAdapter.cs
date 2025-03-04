﻿// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Notifo.Domain.Users;
using Notifo.Infrastructure.Mediator;

namespace Notifo.Domain.Integrations;

internal sealed class IntegrationAdapter(IUserStore userStore, IMediator mediator) : IIntegrationAdapter
{
    public async Task<UserInfo?> FindUserAsync(string appId, string id,
        CancellationToken ct)
    {
        var user = await userStore.GetAsync(appId, id, ct);

        return user?.ToContext();
    }

    public async Task<UserInfo?> FindUserByPropertyAsync(string appId, string key, string value,
        CancellationToken ct)
    {
        var user = await userStore.GetByPropertyAsync(appId, key, value, ct);

        return user?.ToContext();
    }

    public async Task UpdateUserAsync(string appId, string id, string key, string value,
        CancellationToken ct)
    {
        var command = new SetUserSystemProperty
        {
            PropertyKey = key,
            PropertyValue = value
        };

        await mediator.SendAsync(command.With(appId, id), ct);
    }
}
