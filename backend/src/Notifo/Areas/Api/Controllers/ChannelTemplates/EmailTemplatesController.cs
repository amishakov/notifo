﻿// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Microsoft.AspNetCore.Mvc;
using Notifo.Areas.Api.Controllers.ChannelTemplates.Dtos;
using Notifo.Domain.Channels.Email;
using Notifo.Domain.ChannelTemplates;
using Notifo.Domain.Liquid;

namespace Notifo.Areas.Api.Controllers.ChannelTemplates;

[Route("api/apps/{appId:notEmpty}/email-templates")]
[ApiExplorerSettings(GroupName = "EmailTemplates")]
public sealed class EmailTemplatesController(IChannelTemplateStore<EmailTemplate> channelTemplateStore, LiquidPropertiesProvider propertiesProvider) : ChannelTemplatesController<EmailTemplate, EmailTemplateDto>(channelTemplateStore, propertiesProvider)
{
}
