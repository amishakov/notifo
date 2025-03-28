﻿// ==========================================================================
//  Notifo.io
// ==========================================================================
//  Copyright (c) Sebastian Stehle
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using NodaTime;
using Notifo.Infrastructure;
using Squidex.Assets;

namespace Notifo.Domain.Media;

public sealed class MediaStore(
    IMediaFileStore mediaFileStore,
    IMediaRepository mediaRepository,
    IEnumerable<IMediaMetadataSource> mediaMetadataSources,
    IClock clock)
    : IMediaStore
{
    public Task<IResultList<Media>> QueryAsync(string appId, MediaQuery query,
        CancellationToken ct = default)
    {
        Guard.NotNullOrEmpty(appId);
        Guard.NotNull(query);

        return mediaRepository.QueryAsync(appId, query, ct);
    }

    public async Task<Media> UploadAsync(string appId, IAssetFile file,
        CancellationToken ct = default)
    {
        Guard.NotNullOrEmpty(appId);
        Guard.NotNull(file);

        // Calculate once to have some timestamp for created and updated when new entity is created.
        var now = clock.GetCurrentInstant();

        var request = new MetadataRequest
        {
            File = file
        };

        foreach (var metadataSource in mediaMetadataSources)
        {
            await metadataSource.EnhanceAsync(request);
        }

        var infos = new List<string>();

        foreach (var metadataSource in mediaMetadataSources)
        {
            infos.AddRange(metadataSource.Format(request));
        }

        var fileInfo = string.Join(", ", infos);

        var media = new Media(appId, file.FileName, now)
        {
            FileInfo = fileInfo,
            FileSize = file.FileSize,
            LastUpdate = now,
            Metadata = request.Metadata,
            MimeType = file.MimeType,
            Type = request.Type
        };

        await using (var stream = request.File.OpenRead())
        {
            await mediaFileStore.UploadAsync(appId, media, stream, ct);
        }

        await mediaRepository.UpsertAsync(media, ct);

        return media;
    }

    public Task<Media?> GetAsync(string appId, string fileName,
        CancellationToken ct = default)
    {
        Guard.NotNullOrEmpty(appId);
        Guard.NotNullOrEmpty(fileName);

        return mediaRepository.GetAsync(appId, fileName, ct);
    }

    public Task DeleteAsync(string appId, string fileName,
        CancellationToken ct = default)
    {
        Guard.NotNullOrEmpty(appId);
        Guard.NotNullOrEmpty(fileName);

        return mediaRepository.DeleteAsync(appId, fileName, ct);
    }
}
