using GPhotoStats.BL.Models;

namespace GPhotoStats.BL.Interfaces
{
    public interface IGlobalSettings
    {
        string GPhotosClientId { get; }
        string GPhotosClientSecret { get; }
        string FireStoreProjectId { get; }
        string FireStoreCollection { get; }
        FireStoreJsonCredentialsData FireStoreJsonCredentials { get; }
    }
}