using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.PhotosLibrary.v1.Data;
using GPhotoStats.BL.Models;

namespace GPhotoStats.BL.Interfaces
{
    public interface IGPhotosClient
    {
        List<Album> RetrieveAlbums();
        Task<List<PhotoData>> RetrievePhotos();
        Task<SyncResult> SyncPhotos();
    }
}