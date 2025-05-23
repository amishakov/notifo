﻿// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Notifo.Domain.Channels.Email;
using Notifo.Domain.Integrations;
using Notifo.Domain.UserNotifications;
using Squidex.Hosting;

namespace Notifo.Areas.Api.Utils;

public sealed class UrlBuilder(IUrlGenerator urlGenerator) : IEmailUrl, IIntegrationUrl, IUserNotificationUrl
{
    public string EmailPreferences(string apiKey, string language)
    {
        return urlGenerator.BuildUrl($"api/email-preferences?access_token={apiKey}&amp;culture={language}");
    }

    public string TrackConfirmed(Guid notificationId, string language)
    {
        return urlGenerator.BuildUrl($"api/tracking/notifications/{notificationId}/confirm?culture={language}");
    }

    public string TrackDelivered(Guid notificationId, string language)
    {
        return urlGenerator.BuildUrl($"api/tracking/notifications/{notificationId}/delivered?culture={language}");
    }

    public string TrackSeen(Guid notificationId, string language)
    {
        return urlGenerator.BuildUrl($"api/tracking/notifications/{notificationId}/seen?culture={language}");
    }

    public string WebhookUrl(string appId, string integrationId)
    {
        return urlGenerator.BuildCallbackUrl($"api/callback?appId={appId}&integrationId={integrationId}");
    }

    public string CallbackUrl()
    {
        return urlGenerator.BuildUrl("api");
    }
}
