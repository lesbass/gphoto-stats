using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PhotosLibrary.v1;
using Google.Apis.PhotosLibrary.v1.Data;
using Google.Apis.Services;
using GPhotoStats.BL.Interfaces;
using GPhotoStats.BL.Models;

namespace GPhotoStats.BL
{
    public class GPhotosClient : IGPhotosClient
    {
        private readonly PhotosLibraryService _client;
        private readonly IPhotoDataRepository _photoDataRepository;

        public GPhotosClient(IPhotoDataRepository photoDataRepository, IGlobalSettings globalSettings)
        {
            _photoDataRepository = photoDataRepository;
            _client = GetClient(new ClientSecrets
            {
                ClientId = globalSettings.GPhotosClientId,
                ClientSecret = globalSettings.GPhotosClientSecret
            });
        }

        public List<Album> RetrieveAlbums()
        {
            var albums = new List<Album>();
            var request = _client.Albums.List();
            var library = request.ExecuteAsync();
            library.Wait();
            albums.AddRange(library.Result.Albums);
            while (!string.IsNullOrEmpty(library.Result.NextPageToken))
            {
                request.PageToken = library.Result.NextPageToken;
                library = request.ExecuteAsync();
                library.Wait();
                albums.AddRange(library.Result.Albums);
            }

            return albums;
        }

        public Task<List<PhotoData>> RetrievePhotos()
        {
            return _photoDataRepository.FindAll();
        }

        public async Task<SyncResult> SyncPhotos()
        {
            var currentCollection = await _photoDataRepository.FindAll();

            return RetrievePhotos(false, currentCollection);
        }

        private SyncResult RetrievePhotos(bool overwriteExisting, IEnumerable<PhotoData> currentCollection)
        {
            var addedItems = 0;
            var skippedItems = 0;
            var errors = 0;
            var criteria = new SearchMediaItemsRequest {PageSize = 100};
            var currentIdList = currentCollection.Select(item => item.Id).ToImmutableHashSet();
            string nextToken = null;
            do
            {
                try
                {
                    criteria.PageToken = nextToken;
                    criteria.PageSize = 100;
                    var request = _client.MediaItems.Search(criteria);
                    var library = request.ExecuteAsync();
                    library.Wait();
                    nextToken = library.Result.NextPageToken;

                    foreach (var resultMediaItem in library.Result.MediaItems.Select(ConvertToPhotoData))
                    {
                        if (!overwriteExisting && currentIdList.Contains(resultMediaItem.Id))
                        {
                            skippedItems++;
                            continue;
                        }

                        _photoDataRepository.Insert(resultMediaItem);
                        addedItems++;
                    }
                }
                catch (Exception e)
                {
                    errors++;
                    Console.WriteLine(e);
                }
            } while (!string.IsNullOrEmpty(nextToken));

            return new SyncResult(addedItems, skippedItems, errors);
        }

        private PhotosLibraryService GetClient(ClientSecrets clientSecrets)
        {
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                clientSecrets,
                new[] {PhotosLibraryService.Scope.PhotoslibraryReadonly},
                "user", CancellationToken.None);

            credential.Wait();

            return new PhotosLibraryService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential.Result,
                ApplicationName = "Photos API Sample"
            });
        }

        private PhotoData ConvertToPhotoData(MediaItem item)
        {
            return new PhotoData
            {
                Id = item.Id,
                Url = item.ProductUrl,
                MimeType = item.MimeType,
                CreationTime = (DateTime?) item.MediaMetadata?.CreationTime,
                Maker = (item.MediaMetadata?.Photo?.CameraMake ?? "").ToUpper().Trim(),
                Model = (item.MediaMetadata?.Photo?.CameraModel ?? "").ToUpper().Trim()
            };
        }
    }
}